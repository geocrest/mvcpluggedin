namespace Geocrest.Web.Mvc
{
    using System;
    using System.Collections.Generic;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Geocrest.Web.Infrastructure;

    /// <summary>
    /// Provides extension methods for generating Urls
    /// </summary>
    public static class UrlHelperExtensions
    {
        /// <summary>
        /// Generates a fully qualified URL for the specified route values. 
        /// </summary>
        /// <param name="urlHelper">The URL helper.</param>
        /// <param name="routeValues">The route values. Note that the values must include a 'routename' entry.</param>
        /// <returns>
        /// Returns a fully-qualified URL.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        /// urlHelper
        /// or
        /// routeValues</exception>
        /// <exception cref="T:System.NotSupportedException">if <paramref name="routeValues"/>
        /// does not contain an entry for 'routename'</exception>
        public static string HttpRouteUrl(this UrlHelper urlHelper, RouteValueDictionary routeValues)
        {
            Throw.IfArgumentNull(urlHelper, "urlHelper");
            Throw.IfArgumentNull(routeValues, "routeValues");
            Throw.If<IDictionary<string, object>>(routeValues, x => !x.ContainsKey("routename"), 
                "No entry found for 'routename'.");
            var route = routeValues["routename"].ToString();
            var newRouteValues = new RouteValueDictionary(routeValues);
            if (newRouteValues.ContainsKey("routename")) newRouteValues.Remove("routename");
            return urlHelper.HttpRouteUrl(route, newRouteValues);
        }
        /// <summary>
        /// Generates a fully qualified URL for the specified route values. 
        /// </summary>
        /// <param name="urlHelper">The URL helper.</param>
        /// <param name="routeValues">The route values. Note that the values must include a 'routename' entry.</param>
        /// <returns>
        /// Returns an instance of <see cref="System.String">System.String</see>.
        /// </returns>
        public static string HttpRouteUrl(this UrlHelper urlHelper, object routeValues)
        {
            return HttpRouteUrl(urlHelper, new RouteValueDictionary(routeValues));
        }
        /// <summary>
        /// Generates a fully qualified URL for the specified route values.
        /// </summary>
        /// <param name="urlHelper">The URL helper.</param>
        /// <param name="routeName">Name of the route.</param>
        /// <param name="controller">The controller.</param>
        /// <param name="mainArgument">The main argument.</param>
        /// <returns>
        /// Returns a fully-qualified URL.
        /// </returns>
        public static string Link(this System.Web.Http.Routing.UrlHelper urlHelper,string routeName,
            string controller, string mainArgument = null)
        {
            Throw.IfArgumentNull(urlHelper, "urlHelper");
            Throw.IfArgumentNullOrEmpty(routeName, "routeName");
            Throw.IfArgumentNullOrEmpty(controller, "controller");
            return urlHelper.Link(routeName, new { controller = controller, mainArgument = mainArgument});
        }
        /// <summary>
        /// Converts the provided app-relative path into an absolute Url containing the full host name
        /// </summary>
        /// <param name="relativeUrl">App-Relative path</param>
        /// <returns>Provided relativeUrl parameter as fully qualified Url</returns>
        /// <example>~/path/to/foo to http://www.web.com/path/to/foo</example>
        public static string ToAbsoluteUrl(this string relativeUrl)
        {
            if (string.IsNullOrEmpty(relativeUrl))
                return relativeUrl;

            if (HttpContext.Current == null)
                return relativeUrl;

            if (relativeUrl.ToLower().StartsWith("http"))
                return relativeUrl;

            if (relativeUrl.StartsWith("/"))
                relativeUrl = relativeUrl.Insert(0, "~");
            if (!relativeUrl.StartsWith("~/"))
                relativeUrl = relativeUrl.Insert(0, "~/");

            var url = HttpContext.Current.Request.Url;
            var port = url.Port != 80 ? (":" + url.Port) : String.Empty;

            return String.Format("{0}://{1}{2}{3}",
                   url.Scheme, url.Host, port, VirtualPathUtility.ToAbsolute(relativeUrl));
        }
    }
}
