namespace Geocrest.Web.Mvc.Controllers
{
    using System.Text.RegularExpressions;
    using System.Web.Mvc;
    using System.Web.Routing;
    /// <summary>
    /// Redirects users on mobile devices to an MVC area called "mobile"
    /// </summary>
    public class RedirectMobileDevicesToMobileAreaAttribute : AuthorizeAttribute
    {
        /// <summary>
        /// When overridden, provides an entry point for custom authorization checks.
        /// </summary>
        /// <param name="httpContext">The HTTP context, which encapsulates all HTTP-specific information about an individual HTTP request.</param>
        /// <returns>
        /// true if the user is authorized; otherwise, false.
        /// </returns>
        protected override bool AuthorizeCore(System.Web.HttpContextBase httpContext)
        {
            // Only redirect on the first request in a session
            if (httpContext.Session != null && !httpContext.Session.IsNewSession)
                return true;

            // Don't redirect non-mobile browsers
            if (!httpContext.Request.Browser.IsMobileDevice)
                return true;

            // Don't redirect requests for the Mobile area
            if (Regex.IsMatch(httpContext.Request.Url.PathAndQuery, "/mobile($|/)"))
                return true;
                                    
            return false;
        }

        /// <summary>
        /// Processes HTTP requests that fail authorization.
        /// </summary>
        /// <param name="filterContext">Encapsulates the information for using <see cref="T:System.Web.Mvc.AuthorizeAttribute" />. The <paramref name="filterContext" /> object contains the controller, HTTP context, request context, action result, and route data.</param>
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            var redirectionRouteValues = GetRedirectionRouteValues(filterContext.RequestContext);
            filterContext.Result = new RedirectToRouteResult(redirectionRouteValues);
        }

        // Override this method if you want to customize the controller/action/parameters to which
        // mobile users would be redirected. This lets you redirect users to the mobile equivalent
        // of whatever resource they originally requested.
        /// <summary>
        /// Gets the customized redirection route values to which mobile users are redirected.
        /// </summary>
        /// <param name="requestContext">The request context.</param>
        /// <returns>
        /// Returns an instance of <see cref="T:System.Web.Routing.RouteValueDictionary"/>.
        /// </returns>
        protected virtual RouteValueDictionary GetRedirectionRouteValues(RequestContext requestContext)
        {
            return new RouteValueDictionary(new
            {
                area = "mobile",
                controller = requestContext.RouteData.Values["controller"],
                action = requestContext.RouteData.Values["action"]
            });
        }
    }
}
