namespace Geocrest.Web.Mvc.Controllers
{
    using System.Collections.Generic;
    using System.Configuration;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Web;
    using System.Web.Helpers;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Newtonsoft.Json;
    using Ninject;
    using Geocrest.Web.Infrastructure;

    /// <summary>
    /// Provides some common properties to be output to the rendered view.
    /// </summary>
    /// <remarks>
    /// The properties available in the view are as follows:
    /// <para>
    /// <ul>
    /// <li>CurrentEnvironment</li>
    /// <li>IsDebug</li>
    /// <li>IsAuthenticated</li>
    /// </ul>
    /// </para>
    /// </remarks>
    public abstract class BaseController : Controller
    {
        /// <summary>
        /// Called after the action method is invoked.
        /// </summary>
        /// <param name="filterContext">Information about the current request and action.</param>
        /// <remarks>This override allows the rendering of some general information used in markup.</remarks>
        /// <exception cref="T:System.InvalidOperationException">If the web.config is missing one of the following
        /// AppSettings properties:
        /// <list type="bullet">
        /// <item>Environment</item>
        /// <item>DebugVersions</item>
        /// </list>
        /// </exception>
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            try
            {
                Environment env = BaseApplication.CurrentEnvironment;
                Environment[] debugs = BaseApplication.DebugVersions;
                ViewBag.CurrentEnvironment = BaseApplication.CurrentEnvironment.ToString();
                ViewBag.IsDebug = debugs.Contains(env) ? true : false;
                ViewBag.IsAuthenticated = Request.IsAuthenticated.ToString().ToLower();
            }
            catch (SettingsPropertyNotFoundException ex)
            {
                ViewBag.IsDebug = false; // treat as production if missing values
                Throw.InvalidOperation("",
                    x => new System.InvalidOperationException("Missing an AppSettings value", ex));
            }
            base.OnActionExecuted(filterContext);
            
        }
        
        // Summary:
        //     Creates a System.Web.Mvc.JsonResult object that serializes the specified
        //     object to JavaScript Object Notation (JSON).
        //
        // Parameters:
        //   data:
        //     The JavaScript object graph to serialize.
        //
        // Returns:
        //     The JSON result object that serializes the specified object to JSON format.
        //     The result object that is prepared by this method is written to the response
        //     by the ASP.NET MVC framework when the object is executed.
        /// <summary>
        /// Creates a System.Web.Mvc.JsonResult object that serializes the specified 
        /// object to JavaScript Object Notation (JSON).
        /// </summary>
        /// <param name="data">The JavaScript object graph to serialize.</param>
        /// <param name="behavior">Behavior type for JSON requests.</param>
        /// <param name="formatting">The formatting type of the output json.</param>
        /// <returns>
        /// The JSON result object that serializes the specified object to JSON format. 
        /// The result object that is prepared by this method is written to the response 
        /// by the ASP.NET MVC framework when the object is executed.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">data</exception>
        protected virtual JsonResult Json(object data, JsonRequestBehavior behavior, Formatting formatting)
        {
            Throw.IfArgumentNull(data, "data");
            if (behavior != JsonRequestBehavior.AllowGet)
                Throw.InvalidOperation(@"This request has been blocked because sensitive information could be disclosed to third
            party web sites when this is used in a GET request. To allow GET requests, set JsonRequestBehavior to AllowGet.");

            return new BaseJsonResult()
            {
                Data = data,
                JsonRequestBehavior = behavior,
                Formatting = formatting
            };
        }
        /// <summary>
        /// Returns an <see cref="T:System.Web.Mvc.HttpStatusCodeResult"/> representing the specified exception.
        /// </summary>
        /// <param name="statusCode">The status code to return with the response.</param>
        /// <param name="exception">An exception that occurred during a controller action.</param>
        /// <returns>The status code and description representing the error that occurred.</returns>
        public ActionResult HttpStatusCode(System.Net.HttpStatusCode statusCode, System.Exception exception)
        {
            var error = new ErrorViewModel(Request, User, exception);
            return new HttpStatusCodeResult(statusCode, error.message.Replace("\r\n","") + 
                (error.details.Length > 0 ? "<p>" +
                string.Join("<br />", error.details).Replace("\r\n", "") + "</p>" : ""));
        }
        /// <summary>
        /// Returns an <see cref="T:System.Web.Mvc.HttpStatusCodeResult"/> representing the specified status code.
        /// </summary>
        /// <param name="statusCode">The status code.</param>
        /// <returns>The status code and description representing the error that occurred.</returns>
        public ActionResult HttpStatusCode(System.Net.HttpStatusCode statusCode)
        {
            return new HttpStatusCodeResult(statusCode);
        }
        /// <summary>
        /// Validates the request header to verify the anti-forgery token is present.
        /// </summary>
        /// <param name="request">The request.</param>
        [DebuggerStepThrough]
        protected void ValidateRequestHeader(HttpRequestBase request)
        {
            string cookieToken = "";
            string formToken = "";

            IEnumerable<string> tokenHeaders = request.Headers.GetValues("RequestVerificationToken");
            if (tokenHeaders != null)
            {
                string[] tokens = tokenHeaders.First().Split(':');
                if (tokens.Length == 2)
                {
                    cookieToken = tokens[0].Trim();
                    formToken = tokens[1].Trim();
                }
            }
            AntiForgery.Validate(cookieToken, formToken);
        }

        /// <summary>
        /// Renders the partial view as a string.
        /// </summary>
        /// <param name="viewName">Name of the view.</param>
        /// <param name="model">The model.</param>
        /// <returns>A string containing the rendered HTML.</returns>
        public string RenderPartialViewToString(string viewName, object model)
        {
            this.ViewData.Model = model;
            try
            {
                using (StringWriter sw = new StringWriter())
                {
                    ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(this.ControllerContext, viewName);
                    ViewContext viewContext = new ViewContext(this.ControllerContext, viewResult.View, 
                        this.ViewData, this.TempData, sw);
                    viewResult.View.Render(viewContext, sw);

                    return sw.GetStringBuilder().ToString();
                }
            }
            catch (System.Exception ex)
            {
                return ex.ToString();
            }
        }
        /// <summary>
        /// Returns the contents of an embedded resource.
        /// </summary>
        /// <param name="id">The name of the resource.</param>
        /// <returns>
        /// Returns the file content as a stream.
        /// </returns>
        public virtual FileStreamResult ContentFile(string id)
        {            
            string resourceName = Assembly.GetExecutingAssembly().GetManifestResourceNames().ToList().FirstOrDefault(f => f.EndsWith(id));
            return new FileStreamResult(Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName), GetMIMEType(id));
        }

        #region Http404 handling
        /// <summary>
        /// Called when a request matches this controller, but no method with the specified action name is found in the controller.
        /// </summary>
        /// <param name="actionName">The name of the attempted action.</param>
        protected override void HandleUnknownAction(string actionName)
        {
            // If controller is ErrorController dont 'nest' exceptions
            if (this.GetType() != typeof(ErrorController))
                this.InvokeHttp404(HttpContext);
        }

        /// <summary>
        /// Invokes the HTTP404 action on the <see cref="T:Geocrest.Web.Mvc.Controllers.ErrorController"/>.
        /// </summary>
        /// <param name="httpContext">The context object.</param>
        /// <returns>
        /// Returns an <see cref="T:System.Web.Mvc.EmptyResult"/>
        /// </returns>
        public ActionResult InvokeHttp404(HttpContextBase httpContext)
        {
            IController errorController = BaseApplication.Kernel.Get<ErrorController>();
            var errorRoute = new RouteData();
            errorRoute.Values.Add("controller", "Error");
            errorRoute.Values.Add("action", "Http404");
            errorRoute.Values.Add("url", httpContext.Request.Url.OriginalString);
            errorController.Execute(new RequestContext(httpContext, errorRoute));
            return new EmptyResult();
        }
        #endregion

        private string GetMIMEType(string fileId)
        {
            if (fileId.EndsWith(".js"))
            {
                return "text/javascript";
            }
            else if (fileId.EndsWith(".css"))
            {
                return "text/css";
            }
            else if (fileId.EndsWith(".jpg"))
            {
                return "image/jpeg";
            }
            return "text";
        }
    }
}
