namespace Geocrest.Web.Tokens
{
    using System;
    using System.Collections.Specialized;
    using System.Configuration;
    using System.Web;
    using Geocrest.Web.Infrastructure;
    using Geocrest.Web.Infrastructure.Security;

    /// <summary>
    /// Provides an encrypted token for authorizing secure web services
    /// </summary>
    public class Token
    {
        #region Private Fields
        private const string tokenName = "token";
        private const string tokenKeyName = "TokenKey";
        private static string cookieKey = "geotoken";
        private static string sslCookieKey = "geotoken2";
        private string token = "";       
        //private string password = "";
        //private string roles = "";
        private string serverKey = "";
        private static double tokenTime = Convert.ToDouble(ConfigurationManager.AppSettings["TokenTime"]);
        private string referer;
        #endregion

        #region Properties
        /// <summary>
        /// Gets the name of the group for which this token is authorized.
        /// </summary>
        /// <value>
        /// The name of the group.
        /// </value>
        public string GroupName{get;private set;}

        /// <summary>
        /// Gets the client id.
        /// </summary>
        /// <value>
        /// The client id.
        /// </value>
        public string ClientId { get; private set; }

        /// <summary>
        /// Gets the expiration date.
        /// </summary>
        /// <value>
        /// The expiration date.
        /// </value>
        public DateTime ExpirationDate { get; private set; }
        
        /// <summary>
        /// Gets the cookie key.
        /// </summary>
        /// <value>
        /// The cookie key.
        /// </value>
        public static string CookieKey
        {
            get
            {
                return cookieKey;
            }
        }
        
        /// <summary>
        /// Gets the SSL cookie key.
        /// </summary>
        /// <value>
        /// The SSL cookie key.
        /// </value>
        public static string SSLCookieKey
        {
            get
            {
                return sslCookieKey;
            }
        }         
        
        ///// <summary>
        ///// Gets the roles for the user.
        ///// </summary>
        ///// <value>
        ///// The roles.
        ///// </value>
        //public string Roles
        //{
        //    get
        //    {
        //        return this.GetRoles();
        //    }
        //}
        
        /// <summary>
        /// Gets or sets the referer.
        /// </summary>
        /// <value>
        /// The referer.
        /// </value>
        public string Referer
        {
            get
            {
                if (this.referer == null)
                    this.referer = HttpContext.Current.Request.ServerVariables["HTTP_REFERER"];
                //if (this.referer == null)
                //{
                //    Uri url = HttpContext.Current.Request.Url;
                //    string host = GetHost(HttpContext.Current);
                //    this.referer = !(url.Host == host) ? string.Format("{0}://{1}{2}", url.Scheme, host, url.PathAndQuery) 
                //        : HttpContext.Current.Request.Url.ToString();
                //}
                return this.referer;
            }
            set
            {
                this.referer = value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is valid.
        /// </summary>
        /// <value>
        /// <b>true</b>, if this instance is valid; otherwise, <b>false</b>.
        /// </value>
        public bool IsValid { get; private set; }
        #endregion

        #region Ctors       
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Web.Tokens.Token" /> class.
        /// </summary>
        /// <param name="groupname">The groupname.</param>
        /// <param name="clientID">The client ID.</param>
        public Token(string groupname, string clientID) : this(groupname,clientID,tokenTime)
        {                           
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Web.Tokens.Token" /> class.
        /// </summary>
        /// <param name="groupname">The groupname.</param>
        /// <param name="clientID">The client ID.</param>
        /// <param name="tokenTime">The token expiration time in seconds.</param>
        /// <exception cref="System.ArgumentNullException">groupname</exception>
        public Token(string groupname, string clientID, double tokenTime)
        {
            Throw.IfArgumentNullOrEmpty(groupname, "groupname");
            this.GroupName = groupname;
            //this.password = password;
            this.ClientId = clientID;
            this.ExpirationDate = DateTime.Now.AddSeconds(tokenTime);
            this.IsValid = true;
            this.LoadApplicationSettings();  
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Web.Tokens.Token" /> class.
        /// </summary>
        public Token()
        {
            this.LoadApplicationSettings();
            this.ExtractTokenFromRequest();
            if (string.IsNullOrEmpty(this.token))
            {
                this.GroupName = "?";
                this.ExpirationDate = new DateTime(1970, 1, 1, 0, 0, 0);
                //this.roles = "?";
                this.IsValid = false;
            }
            else
                this.Validate();
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Web.Tokens.Token" /> class.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <exception cref="System.ArgumentNullException">token</exception>
        public Token(string token)
        {            
            if (string.IsNullOrEmpty(token))
            {
                this.GroupName = "?";
                //this.roles = "?";
                this.ExpirationDate = new DateTime(1970, 1, 1, 0, 0, 0);
                this.IsValid = false;
                throw new ArgumentNullException("token");
            }
            this.LoadApplicationSettings();
            this.token = token;
            this.Validate();
        }
        #endregion
        /// <summary>
        /// Gets the host name from the server variables.
        /// </summary>
        /// <param name="context">The http context.</param>
        /// <returns>
        /// Returns the host name of the request.
        /// </returns>
        internal static string GetHost(HttpContext context)
        {
            string str = context.Request.ServerVariables["SERVER_NAME"];
            if (context.Request.Headers["X-Forwarded-Host"] != null)
                str = context.Request.Headers["X-Forwarded-Host"];
            else if (context.Request.Headers["Host"] != null)
                str = context.Request.Headers["Host"];
            return str;
        }

        /// <summary>
        /// Validates this instance.
        /// </summary>
        public void Validate()
        {
            this.IsValid = false;
            if (string.IsNullOrEmpty(this.token)) return;
            string unencryptedtoken;
            try
            {
                unencryptedtoken = this.Decrypt(Utility.UrlDecodeToken(this.token));
            }
            catch
            {
                return;
            }
            string[] values = unencryptedtoken.Split(":".ToCharArray());          
            if (values == null || values.Length < 2  || 
                this.GetEpochTime(DateTime.Now) > Convert.ToInt64(values[1]))
                return;
            // set expiration date
            this.ExpirationDate = this.FromEpochTime(Convert.ToInt64(values[1]));

            this.GroupName = values[0];
            //if (values.Length >= 3)
            //    this.roles = values[2] == " " ? string.Empty : values[2];
            if (values.Length >= 4)
            {
                string client = this.ClientId = values[3];
                if (!string.IsNullOrEmpty(client))
                {
                    if (client.StartsWith("ip."))
                    {
                        //this.ClientId = client.Substring(2);
                        if (HttpContext.Current == null || (!HttpContext.Current.Request.IsLocal 
                            && client.ToLower() != "ip." + 
                            HttpContext.Current.Request.UserHostAddress.ToLower()))
                            return;                        
                    }
                    else
                    {
                        if (!client.StartsWith("ref."))
                            return;
                        string referer = this.Referer;
                        if (string.IsNullOrEmpty(referer) || 
                            !referer.ToLower().Contains(client.TrimStart("ref.".ToCharArray()).ToLower()))
                            return;
                    }
                }
            }
            this.IsValid = true;            
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        /// <exception cref="System.ArgumentException">
        /// if the current value of <see cref="P:Geocrest.Web.Tokens.Token.GroupName"/> is null or emtpy.
        /// </exception>
        public override string ToString()
        {
            Throw.IfArgumentNullOrEmpty(this.GroupName, "GroupName");            
            this.token = Utility.UrlEncodeToken(this.Encrypt(this.GroupName + ":" + 
                this.GetDateString(this.ExpirationDate) + ": :" + this.ClientId));
            return this.token;            
        }

        /// <summary>
        /// Encrypts the specified plain text argument.
        /// </summary>
        /// <param name="toEncrypt">The text to encrypt.</param>
        /// <returns>An encrypted string</returns>
        private string Encrypt(string toEncrypt)
        {
            return Encryption.Encrypt(toEncrypt, this.serverKey);
        }

        /// <summary>
        /// Decrypts the specified argument to plain text.
        /// </summary>
        /// <param name="toDecrypt">The encrypted text to decrypt.</param>
        /// <returns>The plain text.</returns>
        private string Decrypt(string toDecrypt)
        {
            return Encryption.Decrypt(toDecrypt, this.serverKey);
        }

        //private string GetRoles()
        //{
        //    if (string.IsNullOrEmpty(this.roles))
        //    {
        //        string[] strArray = Utility.GetUserInfo(this.UserName).Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
        //        if (strArray.Length == 2)
        //            this.roles = strArray[1];
        //    }
        //    return this.roles;
        //}

        /// <summary>
        /// Extracts the token from request.
        /// </summary>
        private void ExtractTokenFromRequest()
        {
            if (this.ExtractTokenFromQuery())
                return;
            else if (this.ExtractTokenFromHeader())
                return;
            else this.ExtractTokenFromPost();
        }

        /// <summary>
        /// Extracts the token from header.
        /// </summary>
        private bool ExtractTokenFromHeader()
        {
            this.token = HttpContext.Current.Request.Headers[tokenName];
            if (string.IsNullOrEmpty(this.token))
                return false;
            return true;
        }

        /// <summary>
        /// Extracts the token from post.
        /// </summary>
        private bool ExtractTokenFromPost()
        {
            this.token = HttpContext.Current.Request.Form[tokenName];
            if (string.IsNullOrEmpty(this.token))
                return false;
            return true;
        }

        /// <summary>
        /// Extracts the token from query.
        /// </summary>
        private bool ExtractTokenFromQuery()
        {
            this.token = HttpContext.Current.Request.QueryString[tokenName];
            if (string.IsNullOrEmpty(this.token))
                return false;
            return true;
        }

        /// <summary>
        /// Loads the application settings.
        /// </summary>
        private void LoadApplicationSettings()
        {
            NameValueCollection appSettings = ConfigurationManager.AppSettings;            
            if (appSettings[tokenKeyName] == null)
                return;
            this.serverKey = Convert.ToString(appSettings[tokenKeyName]);
        }

        /// <summary>
        /// Gets the date string.
        /// </summary>
        /// <param name="date">The date.</param>
        private string GetDateString(DateTime date)
        {
            return Convert.ToString(this.GetEpochTime(date));
        }

        //private string GetDateString()
        //{
        //    return GetDateString(DateTime.Now.AddMinutes(this.tokenTime));           
        //}

        /// <summary>
        /// Gets the epoch time.
        /// </summary>
        /// <param name="dt">A datetime object.</param>
        private long GetEpochTime(DateTime dt)
        {
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0);
            return (dt.ToUniversalTime() - dateTime).Ticks / 10000L;
        }

        /// <summary>
        /// Froms the epoch time.
        /// </summary>
        /// <param name="epoch">The epoch.</param>
        private DateTime FromEpochTime(long epoch)
        {
            var ticks = epoch * 10000L;
            DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0);
            return (dt + new TimeSpan(ticks)).ToLocalTime();
        }
    }
}
