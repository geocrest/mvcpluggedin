namespace Geocrest.Web.Infrastructure
{
    using System;
    using System.Diagnostics;

    /// <summary>
    /// Throws Web API-specific exceptions
    /// </summary>
    public partial class Throw
    {
        /// <summary>
        /// Throws a <see cref="T:Geocrest.Web.Infrastructure.DataSourceUnavailableException"/>
        /// exception.
        /// </summary>
        /// <param name="url">The Url that was requested.</param>
        /// <param name="modifier">A modifier delegate used to modify the exception before being thrown.</param>
        [DebuggerStepThrough]
        public static void HttpResponse(string url, Func<Exception, Exception> modifier = null)
        {
            ThrowInternal(new DataSourceUnavailableException(url), modifier);
        }
    }
}
