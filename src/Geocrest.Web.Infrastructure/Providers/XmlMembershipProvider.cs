namespace Geocrest.Web.Infrastructure.Providers
{
    #region Using

    using System;
    using System.Xml;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Configuration.Provider;
    using System.Web.Security;
    using System.Web.Hosting;
    using System.Web.Management;
    using System.Security.Permissions;
    using System.Web;
    using System.Text;
    using System.Security.Cryptography;
    using System.IO;
    using System.Threading.Tasks;

    #endregion

    /// <summary>
    /// Provides a membership provider implementation based on users stored in an XML file.
    /// </summary>
    public class XmlMembershipProvider : MembershipProvider
    {       
        private Dictionary<string, MembershipUser> _Users;
        private XmlDocument doc = null;
        private string _XmlFileName;

        #region Properties

        // MembershipProvider Properties
        /// <summary>
        /// The name of the application using the custom membership provider.
        /// </summary>
        /// <returns>The name of the application using the custom membership provider.</returns>
        /// <exception cref="T:System.NotSupportedException">
        /// </exception>
        public override string ApplicationName
        {
            get { throw new NotSupportedException(); }
            set { throw new NotSupportedException(); }
        }

        /// <summary>
        /// Indicates whether the membership provider is configured to allow users to retrieve their passwords.
        /// </summary>
        /// <returns>true if the membership provider is configured to support password retrieval; otherwise, false. The default is false.</returns>
        public override bool EnablePasswordRetrieval
        {
            get { return false; }
        }

        /// <summary>
        /// Indicates whether the membership provider is configured to allow users to reset their passwords.
        /// </summary>
        /// <returns>true if the membership provider supports password reset; otherwise, false. The default is true.</returns>
        public override bool EnablePasswordReset
        {
            get { return false; }
        }

        /// <summary>
        /// Gets the number of invalid password or password-answer attempts allowed before the membership user is locked out.
        /// </summary>
        /// <returns>The number of invalid password or password-answer attempts allowed before the membership user is locked out.</returns>
        public override int MaxInvalidPasswordAttempts
        {
            get { return 5; }
        }

        /// <summary>
        /// Gets the minimum number of special characters that must be present in a valid password.
        /// </summary>
        /// <returns>The minimum number of special characters that must be present in a valid password.</returns>
        public override int MinRequiredNonAlphanumericCharacters
        {
            get { return 0; }
        }

        /// <summary>
        /// Gets the minimum length required for a password.
        /// </summary>
        /// <returns>The minimum length required for a password. </returns>
        public override int MinRequiredPasswordLength
        {
            get { return 0; }
        }

        /// <summary>
        /// Gets the number of minutes in which a maximum number of invalid password or password-answer attempts are allowed before the membership user is locked out.
        /// </summary>
        /// <returns>The number of minutes in which a maximum number of invalid password or password-answer attempts are allowed before the membership user is locked out.</returns>
        /// <exception cref="T:System.NotSupportedException"></exception>
        public override int PasswordAttemptWindow
        {
            get { throw new NotSupportedException(); }
        }

        /// <summary>
        /// Gets a value indicating the format for storing passwords in the membership data store.
        /// </summary>
        /// <returns>One of the <see cref="T:System.Web.Security.MembershipPasswordFormat" /> values indicating the format for storing passwords in the data store.</returns>
        public override MembershipPasswordFormat PasswordFormat
        {
            get { return MembershipPasswordFormat.Clear; }
        }

        /// <summary>
        /// Gets the regular expression used to evaluate a password.
        /// </summary>
        /// <returns>A regular expression used to evaluate a password.</returns>
        /// <exception cref="T:System.NotSupportedException"></exception>
        public override string PasswordStrengthRegularExpression
        {
            get { throw new NotSupportedException(); }
        }

        /// <summary>
        /// Gets a value indicating whether the membership provider is configured to require the user to answer a password question for password reset and retrieval.
        /// </summary>
        /// <returns>true if a password answer is required for password reset and retrieval; otherwise, false. The default is true.</returns>
        public override bool RequiresQuestionAndAnswer
        {
            get { return false; }
        }

        /// <summary>
        /// Gets a value indicating whether the membership provider is configured to require a unique e-mail address for each user name.
        /// </summary>
        /// <returns>true if the membership provider requires a unique e-mail address; otherwise, false. The default is true.</returns>
        public override bool RequiresUniqueEmail
        {
            get { return false; }
        }

        #endregion

        #region Supported methods

        /// <summary>
        /// Initializes the provider.
        /// </summary>
        /// <param name="name">The friendly name of the provider.</param>
        /// <param name="config">A collection of the name/value pairs representing the provider-specific attributes specified in the configuration for this provider.</param>
        /// <exception cref="T:System.ArgumentNullException">config</exception>
        /// <exception cref="T:System.Configuration.Provider.ProviderException">Unrecognized attribute:  + attr</exception>
        public override void Initialize(string name, NameValueCollection config)
        {
            Throw.IfArgumentNull(config, "config");
            
            if (String.IsNullOrEmpty(name))
                name = "XmlMembershipProvider";

            if (string.IsNullOrEmpty(config["description"]))
            {
                config.Remove("description");
                config.Add("description", "XML membership provider");
            }

            base.Initialize(name, config);

            // Initialize _XmlFileName and make sure the path
            // is app-relative
            string path = config["xmlFileName"];

            if (String.IsNullOrEmpty(path))
                path = "~/App_Data/Users.xml";
            string fullyQualifiedPath = "";
            if (!VirtualPathUtility.IsAppRelative(path))
                _XmlFileName = path;
            //throw new ArgumentException
            //    ("xmlFileName must be app-relative");
            else
            {
                fullyQualifiedPath = VirtualPathUtility.Combine
                   (VirtualPathUtility.AppendTrailingSlash
                   (HttpRuntime.AppDomainAppVirtualPath != null ? HttpRuntime.AppDomainAppVirtualPath
                   : "/"), path);
                _XmlFileName = HostingEnvironment.MapPath(fullyQualifiedPath);
            }

            if (string.IsNullOrEmpty(_XmlFileName)) _XmlFileName = path;
            config.Remove("xmlFileName");

            // Make sure file exists
            FileInfo fi = new FileInfo(_XmlFileName);
            if (!fi.Exists)
            {                
                using (StreamWriter sw = new StreamWriter(fi.Create()))
                {
                    sw.WriteLine("<Users></Users>");
                    //sw.Close();
                }
            }

            // Make sure we have permission to read the XML data source and
            // throw an exception if we don't
            FileIOPermission permission = new FileIOPermission(FileIOPermissionAccess.Write, _XmlFileName);
            permission.Demand();

            // Throw an exception if unrecognized attributes remain
            if (config.Count > 0)
            {
                string attr = config.GetKey(0);
                if (!String.IsNullOrEmpty(attr))
                    throw new ProviderException("Unrecognized attribute: " + attr);
            }
        }

        /// <summary>
        /// Returns true if the username and password match an exsisting user.
        /// </summary>
        /// <param name="username">The name of the user to validate.</param>
        /// <param name="password">The password for the specified user.</param>
        /// <returns>
        /// true if the specified username and password are valid; otherwise, false.
        /// </returns>
        public override bool ValidateUser(string username, string password)
        {
            ReadDataStore();
            if (String.IsNullOrEmpty(username) || String.IsNullOrEmpty(password))
                return false;

            try
            {
                // Validate the user name and password
                MembershipUser user;
                if (_Users.TryGetValue(username, out user))
                {
                    if (user.Comment == Encrypt(password)) // Case-sensitive
                    {
                        user.LastLoginDate = DateTime.Now;
                        UpdateUser(user);
                        return true;
                    }
                }

                return false;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Retrieves a user based on his/hers username.
        /// the userIsOnline parameter is ignored.
        /// </summary>
        /// <param name="username">The name of the user to get information for.</param>
        /// <param name="userIsOnline">true to update the last-activity date/time stamp for the user; false to return user information without updating the last-activity date/time stamp for the user.</param>
        /// <returns>
        /// A <see cref="T:System.Web.Security.MembershipUser" /> object populated with the specified user's information from the data source.
        /// </returns>
        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            ReadDataStore();
            if (String.IsNullOrEmpty(username))
                return null;

            // Retrieve the user from the data source
            MembershipUser user;
            if (_Users.TryGetValue(username, out user))
                return user;

            return null;
        }

        /// <summary>
        /// Retrieves a collection of all the users.
        /// This implementation ignores pageIndex and pageSize,
        /// and it doesn't sort the MembershipUser objects returned.
        /// </summary>
        /// <param name="pageIndex">The index of the page of results to return. <paramref name="pageIndex" /> is zero-based.</param>
        /// <param name="pageSize">The size of the page of results to return.</param>
        /// <param name="totalRecords">The total number of matched users.</param>
        /// <returns>
        /// A <see cref="T:System.Web.Security.MembershipUserCollection" /> collection that contains a page of <paramref name="pageSize" /><see cref="T:System.Web.Security.MembershipUser" /> objects beginning at the page specified by <paramref name="pageIndex" />.
        /// </returns>
        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            ReadDataStore();
            MembershipUserCollection users = new MembershipUserCollection();

            foreach (KeyValuePair<string, MembershipUser> pair in _Users)
            {
                users.Add(pair.Value);
            }

            totalRecords = users.Count;
            return users;
        }

        /// <summary>
        /// Changes a users password.
        /// </summary>
        /// <param name="username">The user to update the password for.</param>
        /// <param name="oldPassword">The current password for the specified user.</param>
        /// <param name="newPassword">The new password for the specified user.</param>
        /// <returns>
        /// true if the password was updated successfully; otherwise, false.
        /// </returns>
        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            XmlNodeList nodes = doc.GetElementsByTagName("User");
            foreach (XmlNode node in nodes)
            {
                if (node["UserName"].InnerText.Equals(username, StringComparison.OrdinalIgnoreCase)
                  || node["Password"].InnerText.Equals(Encrypt(oldPassword), StringComparison.OrdinalIgnoreCase))
                {
                    node["Password"].InnerText = Encrypt(newPassword);
                    doc.Save(_XmlFileName);
                    return true;
                }
            }
            ReadDataStore();
            return false;
        }
        /// <summary>
        /// Creates the user with the specified properties.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <param name="email">The email.</param>
        /// <param name="isApproved">if set to <c>true</c> is approved.</param>
        /// <param name="providerUserKey">The provider user key.</param>
        /// <param name="passwordQuestion">The password question.</param>
        /// <param name="passwordAnswer">The password answer.</param>
        /// <returns>
        /// Returns an instance of <see cref="T:System.Web.Security.MembershipUser"/>.
        /// </returns>
        public static MembershipUser CreateUser(string username, string password, string email,            
            bool isApproved = true, object providerUserKey = null,
            string passwordQuestion = "", string passwordAnswer="")
        {
            XmlMembershipProvider xml = (XmlMembershipProvider)Membership.Provider;
            MembershipCreateStatus status;
            return xml.CreateUser(username, password, email,
                passwordQuestion, passwordAnswer, isApproved, providerUserKey,out status);
        }
        /// <summary>
        /// Creates a new user store in the XML file
        /// </summary>
        /// <param name="username">The user name for the new user.</param>
        /// <param name="password">The password for the new user.</param>
        /// <param name="email">The e-mail address for the new user.</param>
        /// <param name="passwordQuestion">The password question for the new user.</param>
        /// <param name="passwordAnswer">The password answer for the new user</param>
        /// <param name="isApproved">Whether or not the new user is approved to be validated.</param>
        /// <param name="providerUserKey">The unique identifier from the membership data source for the user.</param>
        /// <param name="status">A <see cref="T:System.Web.Security.MembershipCreateStatus" /> enumeration value indicating whether the user was created successfully.</param>
        /// <returns>
        /// A <see cref="T:System.Web.Security.MembershipUser" /> object populated with the information for the newly created user.
        /// </returns>
        /// <exception cref="System.Web.Security.MembershipCreateUserException">User already exists</exception>
        public override MembershipUser CreateUser(string username, string password, string email, 
            string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, 
            out MembershipCreateStatus status)
        {
            var user = this.GetUser(username, false);
            if (user != null)
                throw new MembershipCreateUserException(MembershipCreateStatus.DuplicateUserName);
           
            XmlNode xmlUserRoot = doc.CreateElement("User");
            XmlNode xmlUserName = doc.CreateElement("UserName");
            XmlNode xmlPassword = doc.CreateElement("Password");
            XmlNode xmlEmail = doc.CreateElement("Email");
            XmlNode xmlLastLoginTime = doc.CreateElement("LastLoginTime");

            xmlUserName.InnerText = username;
            xmlPassword.InnerText = Encrypt(password);
            xmlEmail.InnerText = email;
            xmlLastLoginTime.InnerText = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            xmlUserRoot.AppendChild(xmlUserName);
            xmlUserRoot.AppendChild(xmlPassword);
            xmlUserRoot.AppendChild(xmlEmail);
            xmlUserRoot.AppendChild(xmlLastLoginTime);

            doc.SelectSingleNode("Users").AppendChild(xmlUserRoot);
            doc.Save(_XmlFileName);

            status = MembershipCreateStatus.Success;
            user = new MembershipUser(Name, username, username, email, passwordQuestion, Encrypt(password), isApproved, false, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.MaxValue);

            ReadDataStore();
            return user;
        }

        /// <summary>
        /// Deletes the user from the XML file and
        /// removes him/her from the internal cache.
        /// </summary>
        /// <param name="username">The name of the user to delete.</param>
        /// <param name="deleteAllRelatedData">true to delete data related to the user from the database; false to leave data related to the user in the database.</param>
        /// <returns>
        /// true if the user was successfully deleted; otherwise, false.
        /// </returns>
        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {           
            if (deleteAllRelatedData)
                Roles.RemoveUserFromRoles(username,Roles.GetRolesForUser(username));
            foreach (XmlNode node in doc.GetElementsByTagName("User"))
            {
                if (node.ChildNodes[0].InnerText.Equals(username, StringComparison.OrdinalIgnoreCase))
                {
                    var users = doc.SelectSingleNode("Users");
                    users.RemoveChild(node);                   
                    doc.Save(_XmlFileName);
                    _Users.Remove(username);
                    ReadDataStore();
                    return true;
                }
            }
            ReadDataStore();
            return false;
        }

        /// <summary>
        /// Get a user based on the username parameter.
        /// the userIsOnline parameter is ignored.
        /// </summary>
        /// <param name="providerUserKey">The unique identifier for the membership user to get information for.</param>
        /// <param name="userIsOnline">true to update the last-activity date/time stamp for the user; false to return user information without updating the last-activity date/time stamp for the user.</param>
        /// <returns>
        /// A <see cref="T:System.Web.Security.MembershipUser" /> object populated with the specified user's information from the data source.
        /// </returns>
        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            ReadDataStore();
            foreach (XmlNode node in doc.SelectNodes("//User"))
            {
                if (node.ChildNodes[0].InnerText.Equals(providerUserKey.ToString(), StringComparison.OrdinalIgnoreCase))
                {
                    string userName = node.ChildNodes[0].InnerText;
                    string password = node.ChildNodes[1].InnerText;
                    string email = node.ChildNodes[2].InnerText;
                    DateTime lastLoginTime = DateTime.Parse(node.ChildNodes[3].InnerText);
                    return new MembershipUser(Name, providerUserKey.ToString(), providerUserKey, email, string.Empty, password, true, false, DateTime.Now, lastLoginTime, DateTime.Now, DateTime.Now, DateTime.MaxValue);
                }
            }

            return default(MembershipUser);
        }

        /// <summary>
        /// Retrieves a username based on a matching email.
        /// </summary>
        /// <param name="email">The e-mail address to search for.</param>
        /// <returns>
        /// The user name associated with the specified e-mail address. If no match is found, return null.
        /// </returns>
        public override string GetUserNameByEmail(string email)
        {
            ReadDataStore();
            foreach (XmlNode node in doc.GetElementsByTagName("User"))
            {
                if (node.ChildNodes[2].InnerText.Equals(email.Trim(), StringComparison.OrdinalIgnoreCase))
                {
                    return node.ChildNodes[0].InnerText;
                }
            }

            return null;
        }

        /// <summary>
        /// Updates a user. The username will not be changed.
        /// </summary>
        /// <param name="user">A <see cref="T:System.Web.Security.MembershipUser" /> object that represents the user to update and the updated information for the user.</param>
        public override void UpdateUser(MembershipUser user)
        {
            foreach (XmlNode node in doc.GetElementsByTagName("User"))
            {
                if (node.ChildNodes[0].InnerText.Equals(user.UserName, StringComparison.OrdinalIgnoreCase))
                {
                    //if (user.Comment.Length > 30)
                    //{
                    //    node.ChildNodes[1].InnerText = Encrypt(user.Comment);
                    //}
                    node.ChildNodes[2].InnerText = user.Email;
                    node.ChildNodes[3].InnerText = user.LastLoginDate.ToString("yyyy-MM-dd HH:mm:ss");
                    doc.Save(_XmlFileName);
                    _Users[user.UserName] = user;
                }
            }
            ReadDataStore();
        }

        #endregion

        #region Helper methods

        /// <summary>
        /// Builds the internal cache of users.
        /// </summary>
        private void ReadDataStore()
        {
            lock (this)
            {                
                    _Users = new Dictionary<string, MembershipUser>(16, StringComparer.InvariantCultureIgnoreCase);
                    this.doc = new XmlDocument();
                    doc.Load(_XmlFileName);
                    XmlNodeList nodes = doc.GetElementsByTagName("User");

                    foreach (XmlNode node in nodes)
                    {
                        MembershipUser user = new MembershipUser(
                            Name,                       // Provider name
                            node["UserName"].InnerText, // Username
                            node["UserName"].InnerText, // providerUserKey
                            node["Email"].InnerText,    // Email
                            String.Empty,               // passwordQuestion
                            node["Password"].InnerText, // Comment
                            true,                       // isApproved
                            false,                      // isLockedOut
                            DateTime.Now,               // creationDate
                            DateTime.Parse(node["LastLoginTime"].InnerText), // lastLoginDate
                            DateTime.Now,               // lastActivityDate
                            DateTime.Now, // lastPasswordChangedDate
                            new DateTime(1980, 1, 1)    // lastLockoutDate
                        );

                        _Users.Add(user.UserName, user);
                    }
            }
        }
        /// <summary>
        /// Encrypts a string using the SHA256 algorithm.
        /// </summary>
        /// <param name="plainMessage">The string to encrypt.</param>
        private static string Encrypt(string plainMessage)
        {
            byte[] data = Encoding.UTF8.GetBytes(plainMessage);
            using (HashAlgorithm sha = new SHA256Managed())
            {
                byte[] encryptedBytes = sha.TransformFinalBlock(data, 0, data.Length);
                string hash = Convert.ToBase64String(sha.Hash);
                return hash;
            }
        }

        #endregion

        #region Unsupported methods

        /// <summary>
        /// Resets a user's password to a new, automatically generated password.
        /// </summary>
        /// <param name="username">The user to reset the password for.</param>
        /// <param name="answer">The password answer for the specified user.</param>
        /// <returns>
        /// The new password for the specified user.
        /// </returns>
        /// <exception cref="T:System.NotSupportedException"></exception>
        public override string ResetPassword(string username, string answer)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Clears a lock so that the membership user can be validated.
        /// </summary>
        /// <param name="userName">The membership user whose lock status you want to clear.</param>
        /// <returns>
        /// true if the membership user was successfully unlocked; otherwise, false.
        /// </returns>
        /// <exception cref="T:System.NotSupportedException"></exception>
        public override bool UnlockUser(string userName)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Gets a collection of membership users where the e-mail address contains the specified e-mail address to match.
        /// </summary>
        /// <param name="emailToMatch">The e-mail address to search for.</param>
        /// <param name="pageIndex">The index of the page of results to return. <paramref name="pageIndex" /> is zero-based.</param>
        /// <param name="pageSize">The size of the page of results to return.</param>
        /// <param name="totalRecords">The total number of matched users.</param>
        /// <returns>
        /// A <see cref="T:System.Web.Security.MembershipUserCollection" /> collection that contains a page of <paramref name="pageSize" /><see cref="T:System.Web.Security.MembershipUser" /> objects beginning at the page specified by <paramref name="pageIndex" />.
        /// </returns>
        /// <exception cref="T:System.NotSupportedException"></exception>
        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Gets a collection of membership users where the user name contains the specified user name to match.
        /// </summary>
        /// <param name="usernameToMatch">The user name to search for.</param>
        /// <param name="pageIndex">The index of the page of results to return. <paramref name="pageIndex" /> is zero-based.</param>
        /// <param name="pageSize">The size of the page of results to return.</param>
        /// <param name="totalRecords">The total number of matched users.</param>
        /// <returns>
        /// A <see cref="T:System.Web.Security.MembershipUserCollection" /> collection that contains a page of <paramref name="pageSize" /><see cref="T:System.Web.Security.MembershipUser" /> objects beginning at the page specified by <paramref name="pageIndex" />.
        /// </returns>
        /// <exception cref="T:System.NotSupportedException"></exception>
        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Gets the number of users currently accessing the application.
        /// </summary>
        /// <returns>
        /// The number of users currently accessing the application.
        /// </returns>
        /// <exception cref="T:System.NotSupportedException"></exception>
        public override int GetNumberOfUsersOnline()
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Processes a request to update the password question and answer for a membership user.
        /// </summary>
        /// <param name="username">The user to change the password question and answer for.</param>
        /// <param name="password">The password for the specified user.</param>
        /// <param name="newPasswordQuestion">The new password question for the specified user.</param>
        /// <param name="newPasswordAnswer">The new password answer for the specified user.</param>
        /// <returns>
        /// true if the password question and answer are updated successfully; otherwise, false.
        /// </returns>
        /// <exception cref="T:System.NotSupportedException"></exception>
        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Gets the password for the specified user name from the data source.
        /// </summary>
        /// <param name="username">The user to retrieve the password for.</param>
        /// <param name="answer">The password answer for the user.</param>
        /// <returns>
        /// The password for the specified user name.
        /// </returns>
        /// <exception cref="T:System.NotSupportedException"></exception>
        public override string GetPassword(string username, string answer)
        {
            throw new NotSupportedException();
        }

        #endregion

    }
}