using System.Linq;
using Models;
using SampleWebProject.Infrastructure.Interface;

namespace SampleWebProject.Infrastructure.Validator {
    public class UserValidator : IUserValidator {
        public ValidationResult Validate(User entity) {
            var result = new ValidationResult();
            if (string.IsNullOrWhiteSpace(entity.Username)) result.AddValidationError("Name must be provided");

            using (var ctx = new SampleEntities()) {
                var alreadyDefined = ctx.Users.FirstOrDefault(r => r.Id != entity.Id && r.Username == entity.Username) != null;
                if (alreadyDefined) result.AddValidationError("There is already a user defined");
            }
            return result;
        }
    }
}