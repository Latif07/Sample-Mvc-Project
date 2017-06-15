using System.Linq;
using Models;
using SampleWebProject.Infrastructure.Interface;

namespace SampleWebProject.Infrastructure.Validator {
    public class MenuValidator : IMenuValidator {
        public ValidationResult Validate(Menu entity) {
            var result = new ValidationResult();
            if (string.IsNullOrWhiteSpace(entity.Name)) result.AddValidationError("Name must be provided");
            if (string.IsNullOrWhiteSpace(entity.Target)) result.AddValidationError("Target must be provided");
            if (string.IsNullOrWhiteSpace(entity.Value_enUS)) result.AddValidationError("Name must be provided");
            if (string.IsNullOrWhiteSpace(entity.Value_trTR)) result.AddValidationError("Name must be provided");
            if (string.IsNullOrWhiteSpace(entity.Value_ruRU)) result.AddValidationError("Name must be provided");

            using (var ctx = new SampleEntities()) {
                var alreadyDefined = ctx.Menus.FirstOrDefault(r => r.Id != entity.Id && r.Name == entity.Name && r.Target == entity.Target) != null;
                if (alreadyDefined) result.AddValidationError("There is already a menu defined");
            }
            return result;
        }
    }
}