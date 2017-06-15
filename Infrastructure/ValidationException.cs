using System;

namespace SampleWebProject.Infrastructure {
    public class ValidationException : Exception {
        public ValidationException(string message) : base(message) { }
    }
}