using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Security.Authentication;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;

namespace SampleWebProject.Infrastructure.Security {

    public static class SecurityManager {
        private const HashAlgorithmType HashAlgorithm = HashAlgorithmType.Sha1;
        public static string ConnectionString { get; set; }

        static SecurityManager() {
            ConnectionString = ConfigurationManager.AppSettings["SecurityConnectionString"];
        }

        public static string Login(string username, string password) {
            using (var con = new SqlConnection(ConnectionString)) {
                con.Open();

                int userId;
                int companyId;
                string passwordHash;
                int tokenValidity;
                DateTime userExpireDate;
                bool isLocked;
                using (var cmdCheck = con.CreateCommand()) {
                    cmdCheck.CommandText = "select Id, CompanyId, PasswordHash, TokenValidity, ExpireDate, IsLocked from [User] where Username = @Username";
                    cmdCheck.Parameters.AddWithValue("Username", username);

                    using (var dr = cmdCheck.ExecuteReader()) {
                        var b = dr.Read();
                        if (!b) {
                            CreateLoginHistory(con, username, false);
                            throw new Exception("User not found!");
                        }

                        userId = dr.GetInt32(0);
                        companyId = dr.GetInt32(1);
                        passwordHash = dr.GetString(2);
                        tokenValidity = dr.GetInt32(3);
                        userExpireDate = dr.GetDateTime(4);
                        isLocked = dr.GetBoolean(5);
                    }
                }

                if (isLocked) {
                    CreateLoginHistory(con, username, false);
                    throw new Exception("User is locked!");
                }

                if (DateTime.Now > userExpireDate) {
                    CreateLoginHistory(con, username, false);
                    throw new Exception("User is expired!");
                }

                if (!HashHelper.VerifyHash(password, HashAlgorithm, passwordHash)) {
                    CreateLoginHistory(con, username, false);
                    throw new Exception("Invalid password!");
                }

                CreateLoginHistory(con, username, true);

                var expireDate = DateTime.Now.AddMinutes(tokenValidity);
                var identity = new HubIdentity(userId, username, companyId, expireDate);

                return GenerateToken(identity);
            }
        }

        public static void Authenticate(string token) {
            var tokenBytes = Convert.FromBase64String(token);
            var decryptedBytes = MachineKey.Unprotect(tokenBytes);
            Debug.Assert(decryptedBytes != null, "decryptedBytes != null");
            var identityStr = Encoding.UTF8.GetString(decryptedBytes);
            var iv = identityStr.Split(';');

            var expireDate = DateTime.Parse(iv[3]);
            if (DateTime.Now > expireDate)
                throw new Exception("Token is expired!");

            var identity = new HubIdentity(int.Parse(iv[0]), iv[1], int.Parse(iv[2]), expireDate);
        }

        public static Task<bool> IsInRole(string role) {
            return IsInRole(Thread.CurrentPrincipal.Identity.Name, role);
        }

        public static async Task<bool> IsInRole(string name, string role) {
            using (var con = new SqlConnection(ConnectionString)) {
                await con.OpenAsync();

                using (var cmd = con.CreateCommand()) {
                    cmd.CommandText = "select 1 " +
                                      "from [User] u" +
                                      " inner join [UserRole] ur on u.Id = ur.UserId" +
                                      " inner join [Role] r on ur.RoleId = r.Id" +
                                      "where u.Username = @username and r.Name = @rolename";
                    cmd.Parameters.AddWithValue("username", name);
                    cmd.Parameters.AddWithValue("rolename", role);

                    using (var dr = await cmd.ExecuteReaderAsync()) {
                        return await dr.ReadAsync();
                    }
                }
            }
        }

        public static bool CreateUser(string username, string password, int companyId, DateTime? expireDate = null, int tokenValidity = 10080) {
            using (var con = new SqlConnection(ConnectionString)) {
                 con.Open();

                if (expireDate == null)
                    expireDate = DateTime.Now.AddYears(1);
                var passwordHash = HashHelper.ComputeHash(password, HashAlgorithm);

                using (var cmd = con.CreateCommand()) {
                    cmd.CommandText = @"insert into [User]
                                               (CompanyId,  Username,  PasswordHash, TokenValidity, ExpireDate, PasswordAttemptCount, IsLocked, CreateDate, IsCanceled, IsAdmin)
                                        values (@CompanyId, @Username, @PasswordHash,@TokenValidity, @ExpireDate, @PasswordAttemptCount, @IsLocked, @CreateDate, @IsCanceled, @IsAdmin)";
                    cmd.Parameters.AddWithValue("CompanyId", companyId);
                    cmd.Parameters.AddWithValue("Username", username);
                    cmd.Parameters.AddWithValue("PasswordHash", passwordHash);
                    cmd.Parameters.AddWithValue("TokenValidity", tokenValidity);
                    cmd.Parameters.AddWithValue("ExpireDate", expireDate);
                    cmd.Parameters.AddWithValue("PasswordAttemptCount", 0);
                    cmd.Parameters.AddWithValue("IsLocked", false);
                    cmd.Parameters.AddWithValue("CreateDate", DateTime.Now);
                    cmd.Parameters.AddWithValue("IsCanceled", false);
                    cmd.Parameters.AddWithValue("IsAdmin", false);

                   var result= cmd.ExecuteNonQuery();
                    return result>0;
                }
            }
        }

        public static async Task ChangePassword(string username, string oldPassword, string newPassword) {
            using (var con = new SqlConnection(ConnectionString)) {
                await con.OpenAsync();

                string passwordHash;
                using (var cmdCheck = con.CreateCommand()) {
                    cmdCheck.CommandText = "select PasswordHash from [User] where Username = @Username";
                    cmdCheck.Parameters.AddWithValue("Username", username);

                    using (var dr = await cmdCheck.ExecuteReaderAsync()) {
                        var b = await dr.ReadAsync();
                        if (!b)
                            throw new Exception("User not found!");

                        passwordHash = dr.GetString(0);
                    }
                }

                if (!HashHelper.VerifyHash(oldPassword, HashAlgorithm, passwordHash))
                    throw new Exception("Invalid old password!");

                using (var cmdUpdate = con.CreateCommand()) {
                    var newPasswordHash = HashHelper.ComputeHash(newPassword, HashAlgorithm);
                    cmdUpdate.CommandText = "update [User] set PasswordHash = @PasswordHash where Username = @Username";
                    cmdUpdate.Parameters.AddWithValue("PasswordHash", newPasswordHash);
                    await cmdUpdate.ExecuteNonQueryAsync();
                }
            }
        }

        private static string GenerateToken(HubIdentity identity) {
            var identityStr = string.Join(";", identity.UserId, identity.Name, identity.CompanyId, identity.ExpireDate);
            var identityBytes = Encoding.UTF8.GetBytes(identityStr);
            var encryptedBytes = MachineKey.Protect(identityBytes);
            Debug.Assert(encryptedBytes != null, "encryptedBytes != null");
            var encryptedStr = Convert.ToBase64String(encryptedBytes);

            return encryptedStr;
        }
       private static void CreateLoginHistory(SqlConnection con, string username, bool isSuccessful) {
            string clientIp = null;
            var http = HttpContext.Current;
            if (http != null)
                clientIp = http.Request.UserHostAddress;

            using (var cmd = con.CreateCommand()) {
                cmd.CommandText = "insert into UserLoginHistory (Username, LoginDate, ClientIP, IsSuccessful) " +
                                  "values (@Username, @LoginDate, @ClientIP, @IsSuccessful)";
                cmd.Parameters.AddWithValue("Username", username);
                cmd.Parameters.AddWithValue("LoginDate", DateTime.Now);
                cmd.Parameters.AddWithValue("ClientIP", (object)clientIp ?? DBNull.Value);
                cmd.Parameters.AddWithValue("IsSuccessful", isSuccessful);

                cmd.ExecuteNonQuery();
            }

            UpdatePasswordAttemptCount(con, username, isSuccessful);
        }

        private static void UpdatePasswordAttemptCount(SqlConnection con, string username, bool isSuccessful) {
            using (var cmd = con.CreateCommand()) {
                cmd.CommandText = isSuccessful
                    ? "update [User] set PasswordAttemptCount = 0 where Username = @Username"
                    : "update [User] set PasswordAttemptCount = PasswordAttemptCount + 1, " +
                      "IsLocked = case PasswordAttemptCount when 4 then 1 else 0 end where Username = @Username";
                cmd.Parameters.AddWithValue("Username", username);

                cmd.ExecuteNonQuery();
            }
        }
        
    }
}
