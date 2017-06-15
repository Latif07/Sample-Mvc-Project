using System;
using System.Collections.Generic;
using System.Web.Mvc;
using SampleWebProject.Toast;

namespace SampleWebProject.Infrastructure {

    /// <summary>
    /// Base Controller class for editable HubEntity classes
    /// </summary>
    /// <typeparam name="T">Type of HubEntity class</typeparam>
    [Authorize]
    public abstract class EditControllerBase<T> : MessageControllerBase
    {
        protected EditControllerBase()
        {
            Validator = new ViewModelValidator();
        }
        public virtual ActionResult Index() {
            try {
                CheckPermission(CRUDPermissions.Read);
                return View();
            }
            catch (ValidationException ex) {
                AddToastMessage("", ex.Message, ToastType.Warning);
                return RedirectToAction("Index", "Home");
            }
        }
        [HttpGet]
        public abstract ActionResult Edit(int? id = Constants.NewItemId);

        [HttpPost]
        [ValidateAntiForgeryToken]
        public abstract ActionResult Edit(T model);
        public abstract ActionResult Delete(int? id = Constants.NewItemId);

        /// <summary>
        /// Validation and verification method for the database entity
        /// </summary>
        /// <param name="obj">Entity to be validate and verified</param>
        /// <returns>Returns list of validation errors</returns>
        public abstract List<string> Validate(ref T obj);
        public ViewModelValidator Validator { get; set; }
        public ActionResult RedirectToIndexPage() {
            return RedirectToAction("Index");
        }
        public void CheckPermissionBeforeSave(int id) {
            CheckPermission(id == Constants.NewItemId ? CRUDPermissions.Insert : CRUDPermissions.Update);
        }

       public void CheckPermission(Enum permission) {
            var menuName = MenuName;
            if (string.IsNullOrEmpty(menuName)) return;
            SecurityService.CheckPermission(menuName, permission, UserName);
        }
       public virtual string MenuName {
           get {
               var rd = HttpContext.Request.RequestContext.RouteData;
               return rd.GetRequiredString("controller");
           }
       }
        public string UserName {
            get { return User.Identity.Name; }
        }

        public bool CanSave(int? id) {

            var canUpdate = false;
            var canInsert = false;
            if (id == Constants.NewItemId)
                canInsert = SecurityService.HasPermission(MenuName, CRUDPermissions.Insert, UserName);
            else
                canUpdate = SecurityService.HasPermission(MenuName, CRUDPermissions.Update, UserName);
            return canInsert | canUpdate;
        }
    }
}