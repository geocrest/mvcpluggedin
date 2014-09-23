namespace Geocrest.Web.Infrastructure
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.Net;
    using System.Net.Http;
    using System.ServiceModel;
    using System.ServiceModel.Web;

    /// <summary>
    /// Throws service fault exceptions.
    /// </summary>
    public static partial class Throw
    {
        /// <summary>
        /// Throws <see cref="T:System.ServiceModel.Web.WebFaultException`1" /> with
        /// the requested HTTP status code.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="httpStatusCode">The HTTP status code.</param>
        /// <exception cref="T:System.ServiceModel.Web.WebFaultException`1"></exception>
        [DebuggerStepThrough]
        public static void Fault(string message, HttpStatusCode httpStatusCode)
        {
            throw new WebFaultException<string>(message, httpStatusCode);
        }

        /// <summary>
        /// Throws <see cref="T:System.ServiceModel.Web.WebFaultException`1"/> with an HTTP Bad Request status code if the
        /// provided <paramref name="argument"/> value is null, empty, or consists only of Unicode-defined whitepace characters.
        /// </summary>
        /// <param name="argument">A string to test.</param>
        /// <param name="argumentName">The name of the argument holding the test string. 
        /// This is only used in the error message if the argument is null, empty, or whitepace.</param>
        /// <exception cref="T:System.ServiceModel.Web.WebFaultException`1">The supplied <paramref name="argument"/> 
        /// value is null, empty, or consists only of Unicode-defined whitepace characters. 
        /// The fault response message will include <b>HTTP 400 Bad Request</b>.</exception>
        [DebuggerStepThrough]
        public static void FaultIfArgumentNullOrWhiteSpace(string argument, string argumentName)
        {
            if (string.IsNullOrWhiteSpace(argument))
            {
                throw new WebFaultException<string>(
                                    string.Format(CultureInfo.CurrentUICulture,
                                    Resources.Exceptions.ArgumentNullOrWhitespace, argumentName),
                                    HttpStatusCode.BadRequest);
            }
        }

        /// <summary>
        /// Throws <see cref="T:System.ServiceModel.Web.WebFaultException`1" /> with
        /// the requested HTTP status code if the argument is null.
        /// </summary>
        /// <param name="argument">The argument.</param>
        /// <param name="message">The message.</param>
        /// <param name="httpStatusCode">The HTTP status code.</param>
        /// <exception cref="T:System.ServiceModel.Web.WebFaultException`1">The supplied <paramref name="argument"/> 
        /// value is null. The fault response message will include <b>HTTP 400 Bad Request</b>.</exception>
        [DebuggerStepThrough]
        public static void FaultIfNull(object argument, string message, HttpStatusCode httpStatusCode)
        {
            if (argument == null)
            {
                throw new WebFaultException<string>(message, httpStatusCode);
            }
        }

        /// <summary>
        /// Faults if resource not found.
        /// </summary>
        /// <param name="resource">The resource.</param>
        /// <exception cref="T:System.ServiceModel.Web.WebFaultException`1">The supplied <paramref name="resource"/> 
        /// value is null or empty. The fault response message will include <b>HTTP 404 Not Found</b>.</exception>
        [DebuggerStepThrough]
        public static void FaultIfResourceNotFound(this object resource)
        {
            if (resource == null)
            {
                throw new WebFaultException<string>("Resource not found.", HttpStatusCode.NotFound);
            }
        }
    }
}
