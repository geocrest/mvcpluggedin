namespace Geocrest.Web.Mvc.Controllers
{
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Http;
    using System.Xml.Serialization;
    using Elmah;
    using Newtonsoft.Json;
    using Geocrest.Web.Infrastructure;
    using Geocrest.Web.Infrastructure.Resources;

    /// <summary>
    /// Provides a base class for all Web API controllers
    /// </summary>
    public abstract class BaseApiController : ApiController
    {
        /// <summary>
        /// Writes the image to the response stream.
        /// </summary>
        /// <param name="image">An image <see cref="System.IO.Stream"/>.</param>
        /// <returns>
        /// Returns the input <see cref="System.IO.Stream"/>.
        /// </returns>
        protected virtual Stream WriteOutputImage(Stream image)
        {
            HttpContext ctx = HttpContext.Current;
            if (image != null)
            {
                ctx.Response.ClearHeaders();
                ctx.Response.Clear();
                ctx.Response.CacheControl = "no-cache";
                ctx.Response.ContentType = "image/png";
                ((MemoryStream)image).WriteTo(ctx.Response.OutputStream);
                ctx.Response.Flush();
                ctx.ApplicationInstance.CompleteRequest();
            }
            else
            {
                WriteError(new System.ArgumentNullException("image is null"));
            }

            return image;
        }

        /// <summary>
        /// Writes the image to the response as a base 64 string.
        /// </summary>
        /// <param name="image">An image <see cref="System.IO.Stream" />.</param>
        protected virtual void writeOutputImageString(Stream image)
        {
            HttpContext ctx = HttpContext.Current;
            if (image != null)
            {
                ctx.Response.ClearHeaders();
                ctx.Response.Clear();
                ctx.Response.CacheControl = "no-cache";
                ctx.Response.ContentType = "text/plain";
                ctx.Response.Write(image.ToBase64());
                ctx.Response.Flush();
                ctx.Response.End();
            }
            else
            {
                WriteError(new System.ArgumentNullException("image is null"));
            }
        }

        /// <summary>
        /// Writes the exception information to the output response.
        /// </summary>
        /// <param name="ex">An exception to write to the stream.</param>
        protected virtual void WriteError(System.Exception ex)
        {
            string type = GetResponseType();
            string content = string.Empty;
            bool includeDetail = ControllerContext.Configuration.ShouldIncludeErrorDetail(Request);
            if (type == "application/xml")
            {
                XmlSerializer xml = new XmlSerializer(typeof(HttpError));
                StringWriter writer = new StringWriter();
                xml.Serialize(writer, includeDetail ? new HttpError(ex, includeDetail) :
                    new HttpError(ex.Message));
                content = writer.ToString();
            }
            else
            {
                content = JsonConvert.SerializeObject(includeDetail ?
                    new HttpError(ex, includeDetail) : new HttpError(ex.Message));
            }
            HttpStatusCode status = ex is HttpException ?
                    (HttpStatusCode)((HttpException)ex).GetHttpCode() : HttpStatusCode.InternalServerError;

            Throw.HttpResponse(new HttpResponseMessage(status)
            {
                ReasonPhrase = "Critical Error",
                Content = new StringContent(content, Encoding.UTF8, type)
            });
        }

        /// <summary>
        /// Executes asynchronously a single HTTP operation.
        /// </summary>
        /// <param name="controllerContext">The controller context for a single HTTP operation.</param>
        /// <param name="cancellationToken">The cancellation token assigned for the HTTP operation.</param>
        /// <returns>
        /// The newly started task.
        /// </returns>
        /// <remarks>This override is used to control any outgoing error response so as not to expose the
        /// entire stack trace.</remarks>
        public override Task<HttpResponseMessage> ExecuteAsync(System.Web.Http.Controllers.HttpControllerContext controllerContext, System.Threading.CancellationToken cancellationToken)
        {
            Task<HttpResponseMessage> task = null;

            task = base.ExecuteAsync(controllerContext, cancellationToken);
            if (task != null && task.Exception != null && task.Exception.InnerException != null)
            {
                System.Exception inner = task.Exception.InnerException;

                string type = GetResponseType();
                string content = string.Empty;
                bool includeDetail = controllerContext.Configuration.ShouldIncludeErrorDetail(Request);
                if (type == "application/xml")
                {
                    XmlSerializer xml = new XmlSerializer(typeof(HttpError));
                    StringWriter writer = new StringWriter();
                    xml.Serialize(writer, includeDetail ? new HttpError(inner, includeDetail) :
                        new HttpError(inner.Message));
                    content = writer.ToString();
                }
                else 
                {
                    content = JsonConvert.SerializeObject(includeDetail ?
                        new HttpError(inner, includeDetail) : new HttpError(inner.Message));
                }

                ErrorSignal.FromCurrentContext().Raise(inner);
                HttpStatusCode status = inner is HttpException ?
                    (HttpStatusCode)((HttpException)inner).GetHttpCode() : HttpStatusCode.InternalServerError;

                Throw.HttpResponse(new HttpResponseMessage(status)
                    {
                        ReasonPhrase = "Critical Error",
                        Content = new StringContent(content, Encoding.UTF8, type)
                    });
            }

            return task;
        }
        private string GetResponseType()
        {
            string type = BaseApplication.QueryStringResponseFormats["json"]; // default to json
            var qs = Request.RequestUri.ParseQueryString();
            var qsValues = qs.GetValues(BaseApplication.FormatParameter);
            // check query string override first
            if (qs.AllKeys.Contains(BaseApplication.FormatParameter) &&
                BaseApplication.QueryStringResponseFormats.Keys.Any(f => { return qsValues.Contains(f); }))
            {
                type = BaseApplication.QueryStringResponseFormats[qsValues.First()];
            }
            else
            {
                if (Request.Headers.Accept.Any(a =>
                {
                    return
                        a.MediaType.Contains("application/xml") ||
                        a.MediaType.Contains("text/xml") ||
                        a.MediaType.Contains("application/hal+xml");
                }))
                {
                    type = "application/xml";
                }
                else if (Request.Headers.Accept.Any(a =>
                {
                    return
                        a.MediaType.Contains("application/json") ||
                        a.MediaType.Contains("text/json") ||
                        a.MediaType.Contains("application/hal+json");
                }))
                {
                    type = "application/json";
                }
            }
            return type;
        }
    }
}
