
namespace Geocrest.Web.Mvc.Controllers
{
    using Geocrest.Web.Mvc.Models.Account;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using System.Web.Security;
    using WebMatrix.WebData;
    /// <summary>
    /// Provides account management actions for the entire application.
    /// </summary>
    [Geocrest.Web.Mvc.Controllers.Authorize]
    public class AccountController : BaseController
    {
        private bool simpleMembership = BaseApplication.IsSimpleMembershipProviderConfigured();
        //
        // GET: /Account/Login
        [AllowAnonymous]
        public virtual ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
        [AllowAnonymous]
        public virtual ActionResult LoginForm()
        {
            return PartialView(new Login());
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult LoginForm(Login model)
        {
            return Login(model, string.Empty);
        }
        //
        // POST: /Account/Login

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Login model, string returnUrl)
        {
            return LoginUser(model, returnUrl);
        }

        //
        // GET: /Account/LogOff
        public virtual ActionResult LogOff()
        {
            if (simpleMembership)
                WebSecurity.Logout();
            else
                FormsAuthentication.SignOut();
            return RedirectToAction("index", "home");
        }

        //
        // GET: /Account/Register

        [AllowAnonymous]
        public virtual ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(Register model)
        {
            if (ModelState.IsValid)
            {
                // Attempt to register the user
                WebSecurity.CreateUserAndAccount(model.UserName, model.Password, new { Email=model.Email });
                if (!User.Identity.IsAuthenticated)
                    WebSecurity.Login(model.UserName, model.Password);
                return RedirectToAction("Index", "Home", new { area=""});              
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ChangePassword

        public virtual ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Account/ChangePassword

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(ChangePassword model)
        {
            if (ModelState.IsValid)
            {

                // ChangePassword will throw an exception rather
                // than return false in certain failure scenarios.
                bool changePasswordSucceeded;
                try
                {
                    MembershipUser currentUser = Membership.GetUser(User.Identity.Name, userIsOnline: true);
                    changePasswordSucceeded = currentUser.ChangePassword(model.OldPassword, model.NewPassword);
                }
                catch (Exception)
                {
                    changePasswordSucceeded = false;
                }

                if (changePasswordSucceeded)
                {
                    return RedirectToAction("ChangePasswordSuccess");
                }
                else
                {
                    ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ChangePasswordSuccess

        public virtual ActionResult ChangePasswordSuccess()
        {
            return View();
        }
        private ActionResult LoginUser(Login model, string returnUrl)
        {            
            bool partial = Request.IsAjaxRequest();           
            if (ModelState.IsValid)
            {
                if (this.simpleMembership)
                {
                    if (WebSecurity.Login(model.UserName, model.Password, model.RememberMe))
                    {
                        if (partial)                        
                            return Json(new { success = true });                        
                        else if (Url.IsLocalUrl(returnUrl))                        
                            return Redirect(returnUrl);                        
                        else                        
                            return RedirectToAction("index", "home");                        
                    }
                    else
                    {
                        var user = Membership.GetUser(model.UserName);
                        if (user != null)
                        {
                            if (user.IsLockedOut)
                                ModelState.AddModelError("", "Your account is currently locked out. Please contact an administrator.");
                            else
                                ModelState.AddModelError("", "The user name or password provided is incorrect.");
                        }
                        else
                        {
                            ModelState.AddModelError("", "No such user exists.");
                        }
                        return partial ? (ActionResult)PartialView(model) : View(model);
                    }
                }
                else
                {
                    if (Membership.ValidateUser(model.UserName, model.Password))
                    {
                        FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
                        if (partial)
                            return Json(new { success = true });
                        else if (Url.IsLocalUrl(returnUrl))
                            return Redirect(returnUrl);
                        else
                            return RedirectToAction("index", "home");
                    }
                    else
                    {
                        var user = Membership.GetUser(model.UserName);
                        if (user != null)
                        {
                            if (user.IsLockedOut)
                                ModelState.AddModelError("", "Your account is currently locked out. Please contact an administrator.");
                            else
                                ModelState.AddModelError("", "The user name or password provided is incorrect.");
                        }
                        else
                        {
                            ModelState.AddModelError("", "No such user exists.");
                        }
                        return partial ? (ActionResult)PartialView(model) : View(model);
                    }
                }
            }
            return partial ? (ActionResult)PartialView(model) : View(model);
        }
        private IEnumerable<string> GetErrorsFromModelState()
        {
            return ModelState.SelectMany(x => x.Value.Errors.Select(error => error.ErrorMessage));
        }

        #region Status Codes
        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion
    }
}
