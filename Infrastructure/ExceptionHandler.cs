
using System;
using System.Web.Mvc;

namespace SampleWebProject.Infrastructure {
    /// <summary>
    /// OtiHubBackOffice Exception Handler
    /// </summary>
    public static class ExceptionHandler {
        public static ViewResult Handle(Exception exception, ModelStateDictionary modelState, ViewResult view)
        {
            modelState.AddModelError(string.Empty, exception.Message);
            return view;
        }

        public static ViewResult Handle(string exceptionMessage, ModelStateDictionary modelState, ViewResult view)
        {
            return Handle(new Exception(exceptionMessage), modelState, view);
        }
    }
}