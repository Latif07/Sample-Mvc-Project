using System.Linq;
using Models;
using SampleWebProject.Infrastructure.Interface;

namespace SampleWebProject.Infrastructure.Validator {
    public class RoleValidator : IRoleValidator {
        public ValidationResult Validate(Role entity) {
            var result = new ValidationResult();
            if (string.IsNullOrWhiteSpace(entity.Name)) result.AddValidationError("Name must be provided");

            using (var ctx = new SampleEntities()) {
                var alreadyDefined = ctx.Roles.FirstOrDefault(r => r.Id != entity.Id && r.Name == entity.Name) != null;
                if (alreadyDefined) result.AddValidationError("There is already a currency rate defined for this date");
            }
            return result;
        }
    }
}