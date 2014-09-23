
namespace Geocrest.Web.Mvc
{
    using System.Web;
    using System.Web.Mvc;
    using Newtonsoft.Json;
    using Geocrest.Web.Infrastructure;

    /// <summary>
    /// Provides a custom JSON action result for specifying Json.Net serialization properties.
    /// </summary>
    public class BaseJsonResult : JsonResult
    {
        private Newtonsoft.Json.Formatting _format = Newtonsoft.Json.Formatting.Indented;
        /// <summary>
        /// Gets or sets the formatting.
        /// </summary>
        /// <value>
        /// The formatting.
        /// </value>
        public Newtonsoft.Json.Formatting Formatting
        {
            get { return this._format; }
            set { this._format = value; }
        }
        /// <summary>
        /// Enables processing of the result of an action method by a custom type that inherits from the 
        /// <see cref="T:System.Web.Mvc.ActionResult" /> class.
        /// </summary>
        /// <param name="context">The context within which the result is executed.</param>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="context" /> parameter is null.</exception>
        public override void ExecuteResult(ControllerContext context)
        {
            Throw.IfArgumentNull(context, "context");
            HttpResponseBase response = context.HttpContext.Response;
            if (!string.IsNullOrEmpty(ContentType))
                response.ContentType = this.ContentType;
            else
                response.ContentType = "application/json";
            if (ContentEncoding != null)
                response.ContentEncoding = ContentEncoding;
            if (Data != null)
            {
                response.Write(JsonConvert.SerializeObject(Data, Formatting));
            }
        }
    }
}