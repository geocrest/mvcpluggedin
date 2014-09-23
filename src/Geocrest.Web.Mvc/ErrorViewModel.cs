namespace Geocrest.Web.Mvc
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Security.Principal;
    using System.Web;
    using System.Web.Http;
    using System.Web.Http.Hosting;
    using System.Web.Security;
    using Geocrest.Web.Infrastructure;
    /// <summary>
    /// View model for sending exceptions from client to server. Based on the JavaScript Error object.
    /// </summary>
    public class ErrorViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Web.Mvc.ErrorViewModel" /> class.
        /// </summary>
        public ErrorViewModel() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Web.Mvc.ErrorViewModel" /> class.
        /// </summary>
        /// <param name="request">The current HTTP request.</param>
        /// <param name="user">The currently logged on user.</param>
        /// <param name="exception">The exception object used to define this model.</param>
        public ErrorViewModel(HttpRequestBase request, IPrincipal user, Exception exception)
        {
            HttpMethod method = new HttpMethod(request.HttpMethod);
            var requestMessage = new HttpRequestMessage(method, request.Url.ToString());
            requestMessage.Properties.Add(HttpPropertyKeys.IncludeErrorDetailKey,
                new Lazy<bool>(() => !request.RequestContext.HttpContext.IsCustomErrorEnabled));
            bool includeDetail = request == null || user == null ? false :
                (Roles.Enabled && user.IsInRole(BaseApplication.AdminRole)) ||
                GlobalConfiguration.Configuration.ShouldIncludeErrorDetail(requestMessage) ?
                true : false;
            if (exception == null)
            {
                this.details = new string[] { };
                this.message = string.Empty;
                this.code = 500;
            }
            else
            {
                List<string> details = new List<string>();
                if (exception.InnerException != null) details.Add(exception.InnerException.GetExceptionMessages());
                this.stackTrace = includeDetail ? exception.StackTrace : "";
                this.message = includeDetail ? exception.Message :
                    @"Unfortunately, something went wrong during your request. The issue has been logged and 
we will fix it as soon as possible.";

                this.details = includeDetail ? details.ToArray() : new string[] { };
                this.code = exception is HttpException ?
                    (exception as HttpException).GetHttpCode() :
                    exception is System.Web.Http.HttpResponseException ?
                    (int)(exception as System.Web.Http.HttpResponseException)
                    .Response.StatusCode : 500;
            }
        }
        /// <summary>
        /// Gets or sets the number.
        /// </summary>
        /// <value>
        /// The number.
        /// </value>
        public int number { get; set; }
        
        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string description { get; set; }
        
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string name { get; set; }
        
        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        /// <value>
        /// The code.
        /// </value>
        public int code { get; set; }
        
        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        public string message { get; set; }
        
        /// <summary>
        /// Gets or sets a value indicating whether to log this <see cref="T:Geocrest.Web.Mvc.ErrorViewModel"/>.
        /// </summary>
        /// <value>
        /// <b>true</b>, to log the error; otherwise, <b>false</b>.
        /// </value>
        public bool log { get; set; }

        /// <summary>
        /// Gets or sets the details.
        /// </summary>
        /// <value>
        /// The details.
        /// </value>
        public string[] details { get; set; }
        /// <summary>
        /// Gets or sets the stack trace.
        /// </summary>
        /// <value>
        /// The stack trace.
        /// </value>
        public string stackTrace { get; set; }
    }
}
