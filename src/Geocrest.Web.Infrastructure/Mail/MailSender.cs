
namespace Geocrest.Web.Infrastructure.Mail
{
    using System.Configuration;
    using System.Net;
    using System.Net.Configuration;
    using System.Net.Mail;
    using System.Web.Configuration;
    using System.Web.Security;

    /// <summary>
    /// Sends mail notifications
    /// </summary>
    public class MailSender : IMailSender
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Web.Infrastructure.Mail.MailSender"/> class.
        /// </summary>
        public MailSender()
        {
            this.LoadSettings("", "");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Web.Infrastructure.Mail.MailSender">class</see>.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        public MailSender(string username, string password)
        {
            this.LoadSettings(username, password);
        }
        private void LoadSettings(string username = "", string password = "")
        {
            try
            {
                Configuration configurationFile = WebConfigurationManager.OpenWebConfiguration("~/web.config");
                MailSettingsSectionGroup mailSettings = configurationFile.GetSectionGroup("system.net/mailSettings")
                    as MailSettingsSectionGroup;
                if (mailSettings != null)
                {
                    this.Port = mailSettings.Smtp.Network.Port;
                    this.Host = mailSettings.Smtp.Network.Host;
                    this.Username = string.IsNullOrEmpty(username) ? mailSettings.Smtp.Network.UserName : username;
                    this.EnableSsl = mailSettings.Smtp.Network.EnableSsl;
                    this.UseDefaultCredentials = mailSettings.Smtp.Network.DefaultCredentials;
                    this.FromAddress = new MailAddress(mailSettings.Smtp.From);
                    this.Client = new SmtpClient
                    {
                        Host = this.Host,
                        Port = this.Port,
                        EnableSsl = this.EnableSsl,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = this.UseDefaultCredentials,
                        Credentials = new NetworkCredential(this.Username, string.IsNullOrEmpty(password) ? mailSettings.Smtp.Network.Password : password),
                    };
                }
            }
            catch
            {
            }
        }
        /// <summary>
        /// Loads the SMTP settings from the configuration file.
        /// </summary>
        /// <returns>An SMTP client with settings from the configuration file.</returns>
        /// <exception cref="T:System.Configuration.SettingsPropertyNotFoundException">if no SMTP section is found in the web.config</exception>
        public static SmtpClient LoadSettings()
        {
            SmtpClient client = new SmtpClient();
            try
            {
                Configuration configurationFile = WebConfigurationManager.OpenWebConfiguration("~/web.config");
                MailSettingsSectionGroup mailSettings = configurationFile.GetSectionGroup("system.net/mailSettings")
                    as MailSettingsSectionGroup;
                if (mailSettings != null)
                {
                    client.Host = mailSettings.Smtp.Network.Host;
                    client.Port = mailSettings.Smtp.Network.Port;
                    client.EnableSsl = mailSettings.Smtp.Network.EnableSsl;
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    client.UseDefaultCredentials = mailSettings.Smtp.Network.DefaultCredentials;
                    client.Credentials = new NetworkCredential(mailSettings.Smtp.Network.UserName, mailSettings.Smtp.Network.Password);
                }
                else
                {
                    Throw.SettingsPropertyNotFound("Must have a 'mailSettings' setting in web.config");
                }
                return client;
            }
            catch
            {
                return client;
            }
        }
        /// <summary>
        /// Gets or sets the port.
        /// </summary>
        /// <value>
        /// The port.
        /// </value>
        public int Port { get; private set; }
        /// <summary>
        /// Gets or sets the host.
        /// </summary>
        /// <value>
        /// The host.
        /// </value>
        public string Host { get; private set; }
        /// <summary>
        /// Gets the username.
        /// </summary>
        /// <value>
        /// The username.
        /// </value>
        public string Username { get; private set; }
        /// <summary>
        /// Gets or sets a value indicating whether to use default credentials.
        /// </summary>
        /// <value>
        ///   <c>true</c> to use default credentials.
        /// </value>
        public bool UseDefaultCredentials { get; private set; }
        /// <summary>
        /// Gets or sets a value indicating whether to enable SSL.
        /// </summary>
        /// <value>
        ///   <c>true</c> to enable SSL.
        /// </value>
        public bool EnableSsl { get; private set; }
        /// <summary>
        /// Gets or sets the address from which to send mail.
        /// </summary>
        /// <value>
        /// From address.
        /// </value>
        public MailAddress FromAddress { get; private set; }
        /// <summary>
        /// Gets the SMTP client used for sending mail.
        /// </summary>
        public SmtpClient Client { get; private set; }

        /// <summary>
        /// Sends mail to the specified user.
        /// </summary>
        /// <param name="to">The address to which the mail should be sent. Can also be a comma-separated list of addresses.</param>
        /// <param name="subject">The subject of the email.</param>
        /// <param name="body">The body of the email.</param>
        /// <param name="result">The result of the operation.</param>
        /// <returns></returns>
        public bool Send(string to, string subject, string body, out emailResult result)
        {
            if (this.Client == null) Throw.InvalidOperation("Mail client is not initialized. Make sure there are mail settings defined in the web.config.");
            Throw.IfArgumentNullOrEmpty(to, "to");
            using (var message = new MailMessage()
            {
                From = this.FromAddress,
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
            {
                try
                {
                    message.To.Add(to);
                    this.Client.Send(message);
                    result = emailResult.Success;
                    return true;
                }
                catch (SmtpFailedRecipientException ex)
                {
                    result = emailResult.UserNotFound;
                    return false;
                }
                catch (SmtpException ex)
                {
                    result = emailResult.Failure;
                    return false;
                }
            }
        }
    }
}
