namespace Geocrest.Web.Mvc.Controllers
{
    using System;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Ninject;
    /// <summary>
    /// Provides redirection to the pre-compiled unauthorized view when authorization fails.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class AuthorizeAttribute : System.Web.Mvc.AuthorizeAttribute
    {
        /// <summary>
        /// Processes HTTP requests that fail authorization.
        /// </summary>
        /// <param name="filterContext">Encapsulates the information for using <see cref="T:System.Web.Mvc.AuthorizeAttribute" />. The <paramref name="filterContext" /> object contains the controller, HTTP context, request context, action result, and route data.</param>
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                ErrorController errorController = BaseApplication.Kernel.Get<ErrorController>() ??
                    new ErrorController();
                filterContext.Result = errorController.Unauthorized();
            }
            else
            {
                base.HandleUnauthorizedRequest(filterContext);
            }              
        }
    }
}
