namespace Geocrest.Web.Infrastructure
{
    using System;

    /// <summary>
    /// Provides extension methods for <see cref="T:System.Exception"/> objects.
    /// </summary>
    public static class ExceptionExtensions
    {
        /// <summary>
        /// Gets the exception messages including all inner exception messages.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <returns>
        /// Returns an instance of <see cref="T:System.String">System.String</see>.
        /// </returns>
        public static string GetExceptionMessages(this Exception exception)
        {
            Throw.IfArgumentNull(exception, "exception");
            string message = exception.Message;
            if (exception.InnerException != null)
            {
                message += " Inner exception: " + exception.InnerException.Message + " ";
                message += GetInnerExceptionMessages(exception.InnerException);
            }
            return message;
        }
        /// <summary>
        /// Gets the inner exception messages.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <returns>
        /// Returns an instance of <see cref="T:System.String">System.String</see>.
        /// </returns>
        public static string GetInnerExceptionMessages(this Exception exception)
        {
            Throw.IfArgumentNull(exception, "exception");
            string message = "";
            if (exception.InnerException != null)
            {
                message += "Inner exception: " + exception.InnerException.Message + " ";
                message += GetInnerExceptionMessages(exception.InnerException);
            }
            return message;
        }
    }
}
