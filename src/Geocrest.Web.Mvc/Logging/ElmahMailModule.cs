namespace Geocrest.Web.Mvc.Logging
{
    using Elmah;
    using Geocrest.Web.Infrastructure;
    using System.Web;
    /// <summary>
    /// Provides email notifications using an Exchange email server.
    /// </summary>
    public sealed class ElmahMailModule : ErrorMailModule
    {        
        /// <summary>
        /// Sends the mail.
        /// </summary>
        /// <param name="mail">The mail.</param>
        /// <exception cref="T:System.ArgumentNullException">mail</exception>
        protected override void SendMail(System.Net.Mail.MailMessage mail)
        {
            Throw.IfArgumentNull(mail, "mail");            
            using (var client = Geocrest.Web.Infrastructure.Mail.MailSender.LoadSettings())
            {
                client.Send(mail);
            }
        }
        /// <summary>
        /// Reports the error.
        /// </summary>
        /// <param name="error">The error.</param>
        /// <exception cref="T:System.ArgumentNullException">error</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", 
            MessageId = "0", Justification="Throw class handles validation")]
        protected override void ReportError(Error error)
        {
            Throw.IfArgumentNull(error, "error");
            if (error.Exception != null && error.Exception is HttpException
                && (error.Exception as HttpException).GetHttpCode() == 404
                && error.Exception.Message.StartsWith("MVC Error:", System.StringComparison.CurrentCultureIgnoreCase))
                return;
            else base.ReportError(error);
        }
    }
}
