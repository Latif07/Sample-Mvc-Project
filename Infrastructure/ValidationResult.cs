using System.Collections.Generic;

namespace SampleWebProject.Infrastructure {
    public class ValidationResult {
        private readonly Dictionary<string, string> _validationErrors = new Dictionary<string, string>();
        private readonly Dictionary<string, string> _validationWarnings = new Dictionary<string, string>();
        private readonly Dictionary<string, string> _validationInfo = new Dictionary<string, string>();

        public Dictionary<string, string> Errors {
            get {
                return _validationErrors;
            }
        }

        public Dictionary<string, string> Warnings {
            get {
                return _validationWarnings;
            }
        }

        public Dictionary<string, string> Info {
            get {
                return _validationInfo;
            }
        }

        public void AddValidationError(string error, string key = "")
        {
            if (string.IsNullOrWhiteSpace(key)) key = _validationErrors.Count.ToString();
            _validationErrors.Add(key, error);
        }

        public void AddValidationWarning(string warning, string key = "") {
            if (string.IsNullOrWhiteSpace(key)) key = _validationWarnings.Count.ToString();
            _validationWarnings.Add(key, warning);
        }

        public void AddValidationInfo(string info, string key = "") {
            if (string.IsNullOrWhiteSpace(key)) key = _validationInfo.Count.ToString();
            _validationInfo.Add(key, info);
        }

        public void Set(ValidationResult validationResult)
        {
            if (validationResult == null) validationResult = new ValidationResult();
            foreach (var error in validationResult.Errors) _validationErrors.Add(error.Key, error.Value);
            foreach (var info in validationResult.Info) _validationInfo.Add(info.Key, info.Value);
            foreach (var warning in validationResult.Warnings) _validationWarnings.Add(warning.Key, warning.Value);
        }
    }
}