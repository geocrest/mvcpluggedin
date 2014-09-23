
namespace Geocrest.Web.Mvc
{
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using Geocrest.Web.Infrastructure;
    /// <summary>
    /// Provides extension methods on <see cref="T:System.Net.Http.HttpRequestMessage"/> objects.
    /// </summary>
    public static class HttpRequestMessageExtensions
    {
        /// <summary>
        /// Helper method that performs content negotiation and creates a <see cref="T:System.Net.Http.HttpResponseMessage"/>
        /// representing an error with an instance of <see cref="T:System.Net.Http.ObjectContent`1"/> wrapping an 
        /// <see cref="T:System.Web.Http.HttpError"/> with message <paramref name="message"/> and message detail 
        /// <paramref name="messageDetail"/>. If no formatter is found, this method returns a response with 
        /// status 406 NotAcceptable.
        /// </summary>
        /// <remarks>
        /// This method requires that <paramref name="request"/> has been associated with an instance of 
        /// <see cref="T:System.Web.Http.HttpConfiguration"/>.
        /// </remarks>
        /// <param name="request">The request.</param>
        /// <param name="statusCode">The status code of the created response.</param>
        /// <param name="message">The error message. This message will always be seen by clients.</param>
        /// <param name="messageDetail">The error message detail. This message will only be seen by clients if we should include error detail.</param>
        /// <returns>An error response with error message <paramref name="message"/> and message detail <paramref name="messageDetail"/>
        /// and status code <paramref name="statusCode"/>.</returns>
        /// <exception cref="T:System.ArgumentNullException">request
        /// or
        /// message</exception>
        public static HttpResponseMessage CreateErrorResponse(this HttpRequestMessage request, HttpStatusCode statusCode, string message, string messageDetail)
        {
            Throw.IfArgumentNull(request, "request");
            Throw.IfArgumentNullOrEmpty(message, "message");
            HttpConfiguration configuration = request.GetConfiguration();

            // CreateErrorResponse should never fail, even if there is no configuration associated with the request
            // In that case, use the default HttpConfiguration to con-neg the response media type
            if (configuration == null) configuration = new HttpConfiguration();
            bool includeDetail = configuration != null ? configuration.ShouldIncludeErrorDetail(request)
                : new HttpConfiguration().ShouldIncludeErrorDetail(request);
            HttpError error = new HttpError(message);
            if (includeDetail) error.Add("MessageDetail", messageDetail);
            return request.CreateResponse<HttpError>(statusCode, error, configuration);           
        }       
    }
}
