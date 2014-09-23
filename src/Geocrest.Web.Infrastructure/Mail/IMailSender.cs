namespace Geocrest.Web.Infrastructure.Mail
{
    using System.Net.Mail;
    /// <summary>
    /// Provides methods for sending mail notifications
    /// </summary>
    public interface IMailSender
    {
        /// <summary>
        /// Gets the SMTP client used for sending mail.
        /// </summary>
        SmtpClient Client { get; }
        /// <summary>
        /// Gets the port.
        /// </summary>
        /// <value>
        /// The port.
        /// </value>
        int Port { get; }
        /// <summary>
        /// Gets the SMTP host name.
        /// </summary>
        /// <value>
        /// The host.
        /// </value>
        string Host { get; }
        /// <summary>
        /// Gets the username.
        /// </summary>
        /// <value>
        /// The username.
        /// </value>
        string Username { get; }
        /// <summary>
        /// Gets or sets a value indicating whether to use default credentials.
        /// </summary>
        /// <value>
        /// 	<c>true</c> to use default credentials.
        /// </value>
        bool UseDefaultCredentials { get; }
        /// <summary>
        /// Gets or sets a value indicating whether to enable SSL.
        /// </summary>
        /// <value>
        ///   <c>true</c> to enable SSL.
        /// </value>
        bool EnableSsl { get; }
        /// <summary>
        /// Gets the address from which to send mail.
        /// </summary>
        /// <value>
        /// From address.
        /// </value>
        MailAddress FromAddress { get; }
        /// <summary>
        /// Sends mail to the specified user.
        /// </summary>
        /// <param name="to">The address to which the mail should be sent.</param>
        /// <param name="subject">The subject of the email.</param>
        /// <param name="body">The body of the email.</param>
        /// <param name="result">The result of the operation.</param>
        bool Send(string to, string subject, string body, out emailResult result);
    }
}
