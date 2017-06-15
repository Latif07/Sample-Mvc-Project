using System;
using System.Collections.Generic;
using System.Linq;

namespace SampleWebProject.Toast {
    [Serializable]
    public class Toastr {
        public bool ShowNewestOnTop { get; set; }
        public bool ShowCloseButton { get; set; }
        public List<ToastMessage> ToastMessages { get; set; }

        public ToastMessage AddToastMessage(string title, string message, ToastType toastType) {
            var toast = new ToastMessage {
                Title = title,
                Message = message,
                ToastType = toastType
            };
            ToastMessages.Add(toast);
            return toast;
        }

        public void AddToastMessages(string title, string[] messages, ToastType toastType)
        {
            var result = messages.Select(message => new ToastMessage
            {
                Title = title, 
                ToastType = toastType, 
                Message = message
            }).ToList();
            ToastMessages.AddRange(result);
        }


        public Toastr() {
            ToastMessages = new List<ToastMessage>();
            ShowNewestOnTop = false;
            ShowCloseButton = false;
        }
    }
}