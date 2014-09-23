namespace Geocrest.Web.Mvc.Controllers
{
    using System;
    using System.IO.Compression;
    using System.Web.Mvc;

    /// <summary>
    /// Provides compression to outgoing responses from controller actions.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class CompressAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// Called by the ASP.NET MVC framework before the action method executes.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var encodingsAccepted = filterContext.HttpContext.Request.Headers["Accept-Encoding"];
            if (string.IsNullOrEmpty(encodingsAccepted) || 
                filterContext.HttpContext.Response.Filter == null) return;

            encodingsAccepted = encodingsAccepted.ToLowerInvariant();
            var response = filterContext.HttpContext.Response;

            if (encodingsAccepted.Contains("gzip"))
            {
                response.AppendHeader("Content-encoding", "gzip");
                response.Filter = new GZipStream(response.Filter, CompressionMode.Compress);
            }
            else if (encodingsAccepted.Contains("deflate"))
            {
                response.AppendHeader("Content-encoding", "deflate");
                response.Filter = new DeflateStream(response.Filter, CompressionMode.Compress);
            }            
        }
    }
}
