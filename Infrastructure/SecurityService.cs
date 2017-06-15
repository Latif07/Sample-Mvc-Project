using System;
using System.Linq;
using System.Threading;
using System.Web;
using Models;

namespace SampleWebProject.Infrastructure {
    public class SecurityService {

        public static void CheckPermission(string menuName, System.Enum permission, string userName = null) {
            var result = HasPermission(menuName, permission, userName);
            if (!result)
                throw new ValidationException("You have no permission " + permission + " " + menuName);
        }

        public static bool HasPermission(string menuName, System.Enum permission, string userName = null) {
            if (userName == null)
                userName = UserName;

            using (var context = new SampleEntities()) {
                var result = context.sp_HasPermission(userName, menuName, Convert.ToInt32(permission)).SingleOrDefault();
                return result.HasValue && result.Value;
            }
        }

        public static string UserName {
            get {
                return HttpContext.Current == null || HttpContext.Current.User == null
                    ? Thread.CurrentPrincipal.Identity.Name
                    : HttpContext.Current.User.Identity.Name;
            }
        }
    }
}