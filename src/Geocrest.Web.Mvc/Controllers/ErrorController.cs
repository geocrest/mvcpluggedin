namespace Geocrest.Web.Mvc.Controllers
{
    using System;
    using System.Net;
    using System.Web;
    using System.Web.Mvc;
    using Geocrest.Web.Infrastructure;
    using Geocrest.Web.Mvc.Resources;
    /// <summary>
    /// Allows handling of application errors
    /// </summary>
    public class ErrorController : BaseController
    {
       
        #region Http404

        /// <summary>
        /// An action that returns 404 responses and logs an ELMAH error to the configured storage mechanism.
        /// </summary>
        /// <param name="url">The url of the original request.</param>
        /// <returns>The view for 404 errors.</returns>
        /// <remarks>Override this method to render your own 404 page.</remarks>
        [RedirectMobileDevicesToMobileArea]
        public virtual ViewResult Http404(string url)
        {            
            Response.StatusCode = (int)System.Net.HttpStatusCode.NotFound;
            var model = new NotFoundViewModel();
            // If the url is relative ('NotFound' route) then replace with Requested path
            model.RequestedUrl = !string.IsNullOrEmpty(url) ? 
                Request.Url.OriginalString.Contains(url) & Request.Url.OriginalString != url ?
                Request.Url.OriginalString : url : "";
            // Dont get the user stuck in a 'retry loop' by
            // allowing the Referrer to be the same as the Request
            model.ReferrerUrl = Request.UrlReferrer != null &&
                Request.UrlReferrer.OriginalString != model.RequestedUrl ?
                Request.UrlReferrer.OriginalString : null;
            
            // Log error           
            //Elmah.ErrorSignal.FromCurrentContext().Raise(new HttpException(404,
            //    string.Format(ExceptionStrings.MVC404, model.RequestedUrl)));           
            return View(model);
        }
        /// <summary>
        /// An action that returns a view indicating that the resource is for authorized viewing only.
        /// </summary>
        /// <returns></returns>
        [RedirectMobileDevicesToMobileArea]
        public virtual ViewResult Unauthorized()
        {
            return View("Http401");
        }
        /// <summary>
        /// An action that returns generic information about an error and logs an ELMAH error to the configured storage mechanism.
        /// </summary>
        /// <param name="exception">The exception to display.</param>
        /// <returns>A view containing the error information.</returns>
        [RedirectMobileDevicesToMobileArea]
        public virtual ViewResult Error(Exception exception)
        {
            ViewBag.Messages = exception.GetExceptionMessages();
            ViewBag.ErrorCode = exception is HttpException ? 
                (exception as HttpException).GetHttpCode().ToString() : 
                exception is System.Web.Http.HttpResponseException ? 
                (exception as System.Web.Http.HttpResponseException).Response.StatusCode.ToString() : "500";

            // Log error
            Elmah.ErrorSignal.FromCurrentContext().Raise(exception);

            return View("~/Views/Shared/Error.cshtml");
        }
        
        /// <summary>
        /// An action that returns information about a 500 error.
        /// </summary>
        /// <param name="exception">The exception to display.</param>
        /// <returns>A view containing the error information.</returns>
        [RedirectMobileDevicesToMobileArea]
        public virtual ViewResult Http500(Exception exception)
        {
            return View();
        }
        /// <summary>
        /// Logs an error in the database.
        /// </summary>
        /// <param name="errormessage">An object containing information about the error.</param>
        /// <returns>
        /// Returns an <see cref="T:System.String"/> indicated the error was logged.
        /// </returns>
        [HttpPost]
        public virtual JsonResult LogError(ErrorViewModel errormessage)
        {
            string message = errormessage.message;
            if (errormessage.details != null)
            {
                foreach (string detail in errormessage.details)
                    message += "<br />" + detail;
            }
            if (string.IsNullOrEmpty(message)) message = "Error message was empty.";
            Elmah.ErrorSignal.FromCurrentContext().Raise(new ApplicationException(message));
            return Json("Error logged.");
        }
        #endregion
    }
}
