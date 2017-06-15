
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace SampleWebProject.Infrastructure {
    public class ValidationItem
    {
        public ValidationItem()
        {
            Key = string.Empty;
            Message = string.Empty;
        }

        public ValidationItem(string key, string message)
        {
            Key = key ?? string.Empty;
            Message = message ?? string.Empty;
        }
        public string Key { get; set; }
        public string Message { get; set; }
    }

    public class ViewModelValidator {
        public ViewModelValidator()
        {
            ValidationErrors=new List<ValidationItem>();
        }

        public List<ValidationItem> ValidationErrors { get; set; }

        public void Add(string message) {
            ValidationErrors.Add(new ValidationItem(string.Empty, message));
        }

        public void AddRange(List<string> messages) {
            ValidationErrors.AddRange(messages.Select(m => new ValidationItem{Key = string.Empty, Message = m}));
        }

        public void Add(string key, string message)
        {
            ValidationErrors.Add(new ValidationItem(key, message));
        }

        public void Add(ValidationItem validationItem)
        {
            ValidationErrors.Add(validationItem);
        }

        public void AddRange(List<ValidationItem> validationItems) {
            ValidationErrors.AddRange(validationItems);
        }

        public bool IsValid
        {
            get
            {
                return ValidationErrors.Count == 0;
            }
        }

        public void GenerateModelError(ModelStateDictionary modelState)
        {
            ValidationErrors.ForEach(e => {
                modelState.AddModelError(e.Key, e.Message);
            });
        }
    
        public ViewResult GenerateModelErrorAndRedirect(ModelStateDictionary modelState, ViewResult view) {
            GenerateModelError(modelState);
            return view;
        }
    }
}