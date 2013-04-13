using System.Web.Mvc;
using System.Web.Security;
using Elmah.Everywhere.Models;
using Elmah.Everywhere.Web;
using System;


namespace Elmah.Everywhere.Controllers
{
    //[CustomRequireHttps]
    public class AccountController : Controller
    {
        public ActionResult LogOn()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LogOn(LogOnModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (ValidateUser(model.UserName, model.Password))
                {
                    SetAuthCookie(model.UserName, model.RememberMe);
                    if (LocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    return RedirectToAction("", "Elmah");
                }
                ModelState.AddModelError("", "The user name or password provided is incorrect.");
            }
            return View(model);
        }

        public ActionResult LogOff()
        {
            this.SignOutInternal();
            return RedirectToAction("", "Elmah");
        }

        private bool LocalUrl(string returnUrl)
        {
            if(string.IsNullOrWhiteSpace(returnUrl))
            {
                return false;
            }
            return Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && 
                                                returnUrl.StartsWith("/", StringComparison.OrdinalIgnoreCase) && !
                                                returnUrl.StartsWith("//", StringComparison.OrdinalIgnoreCase) &&
                                                returnUrl.StartsWith("/\\", StringComparison.OrdinalIgnoreCase) == false;
        }

        protected virtual void SignOutInternal()
        {
            FormsAuthentication.SignOut();
        }

        protected virtual bool ValidateUser(string userName, string passowrd)
        {
            return Membership.ValidateUser(userName, passowrd);
        }

        protected virtual void SetAuthCookie(string userName, bool remember)
        {
            FormsAuthentication.SetAuthCookie(userName, remember);
        }
    }
}
