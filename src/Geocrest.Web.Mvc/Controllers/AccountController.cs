
namespace Geocrest.Web.Mvc.Controllers
{
    using Geocrest.Web.Mvc.Models.Account;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
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
        /// <summary>
        /// Returns the main page for account activities. By default, this page allows editing of account information.
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult Index()
        {
            BaseProfile profile = BaseApplication.Profile;
            return View(profile);
        }
        /// <summary>
        /// Updates the current user's account information.
        /// </summary>
        /// <param name="model">The new information about the user including FirstName, LastName, and Email.</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(FormCollection model)
        {
            if (ModelState.IsValid)
            {
                var profile = BaseApplication.Profile;
                profile.FirstName = model["FirstName"];
                profile.LastName = model["LastName"];
                profile.Email = model["Email"];
                profile.Save();
                return RedirectToAction("index");
            }
            return View(model);
        }

        /// <summary>
        /// Returns the login page.
        /// </summary>
        /// <param name="returnUrl">The return URL to redirect after logging in.</param>
        /// <returns></returns>
        [AllowAnonymous]
        public virtual ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
        /// <summary>
        /// Returns a partial view for a login form. No embedded view is provided.
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public virtual ActionResult LoginForm()
        {
            return PartialView(new Login());
        }
        /// <summary>
        /// Logs in with the provided username/password combination.
        /// </summary>
        /// <param name="model">The login information.</param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult LoginForm(Login model)
        {
            return Login(model, string.Empty);
        }

        /// <summary>
        /// Logs in with the provided username/password combination.
        /// </summary>
        /// <param name="model">The login information.</param>
        /// <param name="returnUrl">The return URL to redirect after loggin in.</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Login model, string returnUrl)
        {
            return LoginUser(model, returnUrl);
        }

        /// <summary>
        /// Logs off the current user.
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult LogOff()
        {
            if (simpleMembership)
                WebSecurity.Logout();
            else
                FormsAuthentication.SignOut();
            return RedirectToAction("index", "home");
        }


        /// <summary>
        /// Returns the account registration page.
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public virtual ActionResult Register()
        {
            return View();
        }

        /// <summary>
        /// Registers the user with the application and logs them in.
        /// </summary>
        /// <param name="model">The account information.</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(Register model)
        {
            if (ModelState.IsValid)
            {
                // Attempt to register the user
                if (simpleMembership)
                {
                    WebSecurity.CreateUserAndAccount(model.UserName, model.Password, new { 
                        Email = model.Email,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Application = ConfigurationManager.AppSettings["applicationName"]
                    });
                    if (!User.Identity.IsAuthenticated)
                        WebSecurity.Login(model.UserName, model.Password);
                }
                else
                {
                    try
                    {
                        Membership.CreateUser(model.UserName, model.Password, model.Email);
                    }
                    catch (MembershipCreateUserException ex)
                    {
                        ModelState.AddModelError("", ErrorCodeToString(ex.StatusCode));
                        return View(model);
                    }
                }
                return RedirectToAction("Index", "Home", new { area=""});              
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        /// <summary>
        /// Returns a view for changing a user's password.
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult ChangePassword()
        {
            return View();
        }


        /// <summary>
        /// Changes the password of the current user.
        /// </summary>
        /// <param name="model">The new password information.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Returns a view indicating aa successful password change.
        /// </summary>
        /// <returns></returns>
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
