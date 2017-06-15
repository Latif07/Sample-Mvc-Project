using System;
using System.Security.Authentication;
using System.Security.Cryptography;
using System.Text;

namespace SampleWebProject.Infrastructure {

    /// <summary>
    /// We save passwords as hashed values for security reasons.
    /// This helper class can hash password and compare given password with previously hashed password.
    /// </summary>
    internal static class HashHelper {

        /// <summary>
        /// Computes the hash for given plaintext for given hashAlgorithm.
        /// </summary>
        /// <param name="plainText">The plain text.</param>
        /// <param name="hashAlgorithm">The hash algorithm.</param>
        /// <param name="saltBytes">The salt bytes.</param>
        /// <returns>Hashed password.</returns>
        public static string ComputeHash(string plainText, HashAlgorithmType hashAlgorithm, byte[] saltBytes = null) {
            if (plainText == null)
                plainText = string.Empty;

            if (saltBytes == null) {
                const int minSaltSize = 4;
                const int maxSaltSize = 8;

                var random = new Random();
                var saltSize = random.Next(minSaltSize, maxSaltSize);
                saltBytes = new byte[saltSize];
                using (var rng = new RNGCryptoServiceProvider()) {
                    rng.GetNonZeroBytes(saltBytes);
                }
            }

            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            var plainTextWithSaltBytes = new byte[plainTextBytes.Length + saltBytes.Length];
            for (var i = 0; i < plainTextBytes.Length; i++)
                plainTextWithSaltBytes[i] = plainTextBytes[i];

            for (var i = 0; i < saltBytes.Length; i++)
                plainTextWithSaltBytes[plainTextBytes.Length + i] = saltBytes[i];

            string hashValue;
            using (var hash = CreateHasher(hashAlgorithm)) {
                var hashBytes = hash.ComputeHash(plainTextWithSaltBytes);
                var hashWithSaltBytes = new byte[hashBytes.Length + saltBytes.Length];
                for (var i = 0; i < hashBytes.Length; i++)
                    hashWithSaltBytes[i] = hashBytes[i];

                for (var i = 0; i < saltBytes.Length; i++)
                    hashWithSaltBytes[hashBytes.Length + i] = saltBytes[i];

                hashValue = Convert.ToBase64String(hashWithSaltBytes);
            }

            return hashValue;
        }

        private static HashAlgorithm CreateHasher(HashAlgorithmType hashAlgorithm) {
            HashAlgorithm hash;
            switch (hashAlgorithm) {
                case HashAlgorithmType.Sha1:
                    hash = new SHA1Managed();
                    break;
                default:
                    hash = new MD5CryptoServiceProvider();
                    break;
            }
            return hash;
        }

        /// <summary>
        /// Hashed password can not be retrieved again. So we hash the given password to compare with previously hashed value.
        /// </summary>
        /// <param name="plainText">The plain text.</param>
        /// <param name="hashAlgorithm">The hash algorithm.</param>
        /// <param name="hashValue">The hash value.</param>
        /// <returns>
        ///   <c>true</c> if the given password is correct; otherwise, <c>false</c>.
        /// </returns>
        public static bool VerifyHash(string plainText, HashAlgorithmType hashAlgorithm, string hashValue) {
            var hashWithSaltBytes = Convert.FromBase64String(hashValue);
            int hashSizeInBits;

            switch (hashAlgorithm) {
                case HashAlgorithmType.Sha1:
                    hashSizeInBits = 160;
                    break;
                default:
                    hashSizeInBits = 128;
                    break;
            }

            var hashSizeInBytes = hashSizeInBits / 8;
            if (hashWithSaltBytes.Length < hashSizeInBytes)
                return false;

            var saltBytes = new byte[hashWithSaltBytes.Length - hashSizeInBytes];
            for (var i = 0; i < saltBytes.Length; i++)
                saltBytes[i] = hashWithSaltBytes[hashSizeInBytes + i];

            var expectedHashString = ComputeHash(plainText, hashAlgorithm, saltBytes);
            return (hashValue == expectedHashString);
        }
    }
}
