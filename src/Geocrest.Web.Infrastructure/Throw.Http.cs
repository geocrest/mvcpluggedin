namespace Geocrest.Web.Infrastructure
{
    using System;
    using System.Diagnostics;
    using System.Net;
    using System.Net.Http;
    using System.ServiceModel.Web;
    using System.Web.Http;

    /// <summary>
    /// Throws web response exceptions.
    /// </summary>
    public static partial class Throw
    {
        /// <summary>
        /// Throws an HTTP response exception to a client.
        /// </summary>
        /// <param name="code">The status code to return to the client.</param>
        /// <param name="modifier">A modifier delegate used to modify the exception before being thrown.</param>
        [DebuggerStepThrough]
        public static void HttpResponse(HttpStatusCode code, Func<Exception, Exception> modifier = null)
        {
            ThrowInternal(new HttpResponseException(new HttpResponseMessage(code)), modifier);
        }

        /// <summary>
        /// Throws an HTTP response exception to a client.
        /// </summary>
        /// <param name="code">The status code to return to the client.</param>
        /// <param name="reasonphrase">The reason phrase.</param>
        /// <param name="content">The message detail to return.</param>
        /// <param name="modifier">A modifier delegate used to modify the exception before being thrown.</param>
        [DebuggerStepThrough]
        public static void HttpResponse(HttpStatusCode code, string reasonphrase, string content,
             Func<Exception, Exception> modifier = null)
        {
            ThrowInternal(new HttpResponseException(new HttpResponseMessage(code)
            {
                Content = new StringContent(content),
                ReasonPhrase = reasonphrase
            }), modifier);
        }

        /// <summary>
        /// Throws an HTTP response exception to a client.
        /// </summary>
        /// <param name="message">The response message to return to the client.</param>
        /// <param name="modifier">A modifier delegate used to modify the exception before being thrown.</param>
        [DebuggerStepThrough]
        public static void HttpResponse(HttpResponseMessage message,
             Func<Exception, Exception> modifier = null)
        {
            ThrowInternal(new HttpResponseException(message), modifier);
        }
        
        /// <summary>
        /// Throws an HTTP Bad Request exception to a client if an input argument is null or empty.
        /// </summary>
        /// <param name="argument">The argument to check.</param>
        /// <param name="modifier">A modifier delegate used to modify the exception before being thrown.</param>
        [DebuggerStepThrough]
        public static void BadRequestIfNullOrEmpty(string argument, Func<Exception, Exception> modifier = null)
        {
            if (string.IsNullOrEmpty(argument))
                Throw.HttpResponse(HttpStatusCode.BadRequest, modifier);
        }
        
        /// <summary>
        /// Throws an HTTP Bad Request exception to a client if an input argument is null.
        /// </summary>
        /// <param name="argument">The argument to check.</param>
        /// <param name="modifier">A modifier delegate used to modify the exception before being thrown.</param>
        [DebuggerStepThrough]
        public static void BadRequestIfNull(object argument, Func<Exception, Exception> modifier = null)
        {
            if (argument == null)
                Throw.HttpResponse(HttpStatusCode.BadRequest, modifier);
        }
        
        /// <summary>
        /// Throws an HTTP Not Found exception to a client if the argument is null.
        /// </summary>
        /// <param name="argument">The argument to check.</param>
        /// <param name="modifier">A modifier delegate used to modify the exception before being thrown.</param>
        [DebuggerStepThrough]
        public static void NotFoundIfNull(object argument,Func<Exception, Exception> modifier = null)
        {
            if (argument == null)
                Throw.HttpResponse(HttpStatusCode.NotFound, modifier);
        }
        
        /// <summary>
        /// Passes a WebException received from an external source to an internal handling mechanism in order
        /// to be logged. The original exception will continue to be thrown.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Net.WebException">WebException</see> received from a call to 
        /// an external source.</param>
        [DebuggerStepThrough]
        public static void WebException(WebException e)
        {
            ThrowInternal(e);
        }

        /// <summary>
        /// Throws an HTTP exception from a <see cref="T:System.ServiceModel.Web.WebFaultException`1" />.
        /// </summary>
        /// <param name="webFaultException">A web service WebFaultException.</param>
        public static void WebFaultToHttpException(this WebFaultException<string> webFaultException)
        {
            if (webFaultException != null)
            {
                Throw.HttpResponse(
                                   webFaultException.StatusCode,
                                   "Web service threw a WebFaultException.",
                                   webFaultException.Detail);
            }
        }
    }
}
