
namespace Geocrest.Web.Infrastructure.Mail
{
    using System.ComponentModel;

    /// <summary>
    /// Describes the result of email operations.
    /// </summary>
    public enum emailResult
    {
        /// <summary>
        /// Email was successfully sent
        /// </summary>
        [Description("Email sent successfully.")]
        Success = 0,
        /// <summary>
        /// SMTP user was not found
        /// </summary>
        [Description("Target email user could not be found.")]
        UserNotFound = 1,
        /// <summary>
        /// Generic failure
        /// </summary>
        [Description("A general error occurred while sending mail.")]
        Failure = 2
    }
}
