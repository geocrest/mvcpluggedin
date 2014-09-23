
namespace Geocrest.Web.Infrastructure.Providers
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Configuration.Provider;
    using System.IO;
    using System.Linq;
    using System.Security.Permissions;
    using System.Web;
    using System.Web.Hosting;
    using System.Web.Security;
    using System.Xml;
    using Geocrest.Web.Infrastructure;
    /// <summary>
    /// Provides a role provider based on roles stored in an XML file.
    /// </summary>
    public class XmlRoleProvider : RoleProvider
    {
        private Dictionary<string, string[]> _UsersAndRoles = new Dictionary<string, string[]>();
        private Dictionary<string, string[]> _RolesAndusers = new Dictionary<string, string[]>();
        private string _XmlFileName;
        /// <summary>
        /// Adds the specified user names to the specified roles for the configured applicationName.
        /// </summary>
        /// <param name="usernames">A string array of user names to be added to the specified roles.</param>
        /// <param name="roleNames">A string array of the role names to add the specified user names to.</param>
        /// <exception cref="T:System.NotSupportedException">
        /// One or more users do not exist
        /// or
        /// One or mroe roles do not exist</exception>
        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            MembershipProvider provider = Membership.Provider;
            Throw.If<string[]>(usernames, x => x.Any(s => provider.GetUser(s, false) == null), "One or more users do not exist");
            Throw.If<string[]>(roleNames, x => x.Any(s => !RoleExists(s)), "One or more roles do not exist");
            var doc = new XmlDocument();
            doc.Load(_XmlFileName);
            var roles = doc.DocumentElement;
            foreach (var role in roleNames)
            {
                foreach (string user in usernames)
                {
                    var usernode = roles.SelectSingleNode(
                        string.Format("descendant::Role[@name='{0}' and Users/UserName='{1}']", role, user));
                    if (usernode == null)
                    {
                        var users = roles.SelectSingleNode(string.Format("descendant::Role[@name='{0}']/Users", role));
                        var newuser = doc.CreateElement("UserName");
                        newuser.InnerText = user;
                        users.AppendChild(newuser);
                    }
                }
            }
            doc.Save(_XmlFileName);
            ReadRoleDataStore();
        }

        /// <summary>
        /// Gets or sets the name of the application to store and retrieve role information for.
        /// </summary>
        /// <returns>The name of the application to store and retrieve role information for.</returns>       
        public override string ApplicationName { get; set; }
        /// <summary>
        /// Adds a new role to the data source for the configured applicationName.
        /// </summary>
        /// <param name="roleName">The name of the role to create.</param>
        /// <exception cref="T:System.ArgumentNullException">roleName</exception>
        /// <exception cref="T:System.ArgumentException">Role already exists</exception>
        public override void CreateRole(string roleName)
        {
            Throw.IfArgumentNullOrEmpty(roleName, "roleName");
            if (RoleExists(roleName)) Throw.ArgumentException(string.Format("Role '{0}' already exists", roleName));
            var doc = new XmlDocument();
            doc.Load(_XmlFileName);
            var roles = doc.DocumentElement;
            var newrole = doc.CreateElement("Role");
            newrole.Attributes.Append(doc.CreateAttribute("name"));
            newrole.Attributes["name"].Value = roleName;
            newrole.AppendChild(doc.CreateElement("Users"));
            roles.AppendChild(newrole);
            doc.Save(_XmlFileName);
            ReadRoleDataStore();
        }

        /// <summary>
        /// Removes a role from the data source for the configured applicationName.
        /// </summary>
        /// <param name="roleName">The name of the role to delete.</param>
        /// <param name="throwOnPopulatedRole">If true, throw an exception if <paramref name="roleName" /> has one or more members and do not delete <paramref name="roleName" />.</param>
        /// <returns>
        /// true if the role was successfully deleted; otherwise, false.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">roleName</exception>
        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            Throw.IfArgumentNullOrEmpty(roleName, "roleName");
            var doc = new XmlDocument();
            doc.Load(_XmlFileName);
            var roles = doc.DocumentElement;
            var rolenode = roles.SelectSingleNode(string.Format("descendant::Role[@name='{0}']", roleName));
            if (rolenode == null) return true;
            var users = rolenode.SelectNodes("descendant::Users/UserName");
            if (users.Count >= 0 && (!throwOnPopulatedRole))
                roles.RemoveChild(rolenode);
            else if (users.Count > 0 && throwOnPopulatedRole)
                Throw.ProviderException("Role contains users.");
            doc.Save(_XmlFileName);
            ReadRoleDataStore();
            return true;
        }

        /// <summary>
        /// Gets an array of user names in a role where the user name contains the specified user name to match.
        /// </summary>
        /// <param name="roleName">The role to search in.</param>
        /// <param name="usernameToMatch">The user name to search for.</param>
        /// <returns>
        /// A string array containing the names of all the users where the user name matches <paramref name="usernameToMatch" /> and the user is a member of the specified role.
        /// </returns>
        /// <exception cref="T:System.NotImplementedException"></exception>
        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            Throw.NotImplemented("This method is not implemented.");
            return null;
        }

        /// <summary>
        /// Gets a list of all the roles for the configured applicationName.
        /// </summary>
        /// <returns>
        /// A string array containing the names of all the roles stored in the data source for the configured applicationName.
        /// </returns>
        public override string[] GetAllRoles()
        {
            return _RolesAndusers.Select(x => x.Key).Distinct().ToArray();
        }

        /// <summary>
        /// Gets a list of the roles that a specified user is in for the configured applicationName.
        /// </summary>
        /// <param name="username">The user to return a list of roles for.</param>
        /// <returns>
        /// A string array containing the names of all the roles that the specified user is in for the configured applicationName.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">username</exception>
        public override string[] GetRolesForUser(string username)
        {
            Throw.IfArgumentNullOrEmpty(username, "username");
            string[] roles = null;
            if (!_UsersAndRoles.TryGetValue(username, out roles))
                Throw.ProviderException("Invalid user name");
            return roles.Distinct().ToArray();
        }

        /// <summary>
        /// Gets a list of users in the specified role for the configured applicationName.
        /// </summary>
        /// <param name="roleName">The name of the role to get the list of users for.</param>
        /// <returns>
        /// A string array containing the names of all the users who are members of the specified role for the configured applicationName.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">roleName</exception>
        public override string[] GetUsersInRole(string roleName)
        {
            Throw.IfArgumentNullOrEmpty(roleName, "roleName");
            string[] users = null;
            if (!_RolesAndusers.TryGetValue(roleName, out users))
                Throw.ProviderException("Invalid role name");
            return users.Distinct().ToArray();
        }

        /// <summary>
        /// Gets a value indicating whether the specified user is in the specified role for the configured applicationName.
        /// </summary>
        /// <param name="username">The user name to search for.</param>
        /// <param name="roleName">The role to search in.</param>
        /// <returns>
        /// true if the specified user is in the specified role for the configured applicationName; otherwise, false.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        /// username
        /// or
        /// roleName</exception>
        public override bool IsUserInRole(string username, string roleName)
        {
            ReadRoleDataStore();
            Throw.IfArgumentNullOrEmpty(username, "username");
            Throw.IfArgumentNullOrEmpty(roleName, "roleName");
            if (!_UsersAndRoles.ContainsKey(username))
                return false;
            if (!_RolesAndusers.ContainsKey(roleName))
                Throw.ProviderException("Invalid role name");
            string[] roles = this._UsersAndRoles[username];
            foreach (var role in roles)
            {
                if (string.Compare(role, roleName, true) == 0)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Removes the specified user names from the specified roles for the configured applicationName.
        /// </summary>
        /// <param name="usernames">A string array of user names to be removed from the specified roles.</param>
        /// <param name="roleNames">A string array of role names to remove the specified user names from.</param>
        /// <exception cref="T:System.NotSupportedException">
        /// One or more users do not exist
        /// or
        /// One or more roles do not exist</exception>
        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            MembershipProvider provider = Membership.Provider;
            Throw.If<string[]>(usernames, x => x.Any(s => provider.GetUser(s, false) == null), "One or more users do not exist");
            Throw.If<string[]>(roleNames, x => x.Any(s => !RoleExists(s)), "One or more roles do not exist");
            var doc = new XmlDocument();
            doc.Load(_XmlFileName);
            var roles = doc.DocumentElement;
            foreach (var role in roleNames)
            {
                var usersnode = roles.SelectSingleNode(string.Format("descendant::Role[@name='{0}']/Users", role));
                var usernodes = usersnode.SelectNodes("descendant::UserName");
                if (usernodes != null && usernodes.Count > 0)
                {
                    List<XmlNode> nodestoremove = new List<XmlNode>();
                    foreach (XmlNode usernode in usernodes)
                    {
                        if (usernames.Contains(usernode.InnerText)) nodestoremove.Add(usernode);
                    }
                    nodestoremove.ForEach(x => usersnode.RemoveChild(x));
                }
            }
            doc.Save(_XmlFileName);
            ReadRoleDataStore();
        }

        /// <summary>
        /// Gets a value indicating whether the specified role name already exists in the role data source for the configured applicationName.
        /// </summary>
        /// <param name="roleName">The name of the role to search for in the data source.</param>
        /// <returns>
        /// true if the role name already exists in the data source for the configured applicationName; otherwise, false.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">roleName</exception>
        public override bool RoleExists(string roleName)
        {
            Throw.IfArgumentNullOrEmpty(roleName, "roleName");
            return _RolesAndusers.ContainsKey(roleName);
        }
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
                name = "XmlRoleProvider";

            if (string.IsNullOrEmpty(config["description"]))
            {
                config.Remove("description");
                config.Add("description", "XML role provider");
            }
            base.Initialize(name, config);
            // Initialize _XmlFileName and make sure the path
            // is app-relative
            string path = config["xmlFileName"];

            if (String.IsNullOrEmpty(path))
                path = "~/App_Data/roles.xml";
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
                    sw.WriteLine("<Roles></Roles>");
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
            ReadRoleDataStore();
        }

        /// <summary>
        /// Builds the internal cache of roles.
        /// </summary>
        private void ReadRoleDataStore()
        {
            lock (this)
            {
                _UsersAndRoles = new Dictionary<string, string[]>();
                _RolesAndusers = new Dictionary<string, string[]>();
                var doc = new XmlDocument();
                doc.Load(_XmlFileName);
                MembershipProvider members = Membership.Provider;
                XmlNodeList nodes = doc.GetElementsByTagName("Role");

                foreach (XmlNode node in nodes)
                {
                    Throw.If<XmlNode>(node, n => n.Attributes["name"] == null ||
                        string.IsNullOrEmpty(n.Attributes["name"].Value), "Role name not found");
                    var usernodes = node.SelectNodes("descendant::Users/UserName");
                    List<string> users = new List<string>();
                    foreach (XmlNode user in usernodes)
                        users.AddIfNotNull<string>(user.InnerText);
                    _RolesAndusers.Add(node.Attributes["name"].Value, users.ToArray());
                    string role = node.Attributes["name"].Value;
                    foreach (string user in users)
                    {
                        string[] existingroles = null;
                        if (_UsersAndRoles.TryGetValue(user, out existingroles))
                        {
                            List<string> targetroles = new List<string>(existingroles);
                            targetroles.Add(role);
                            _UsersAndRoles.Remove(user);
                            _UsersAndRoles.Add(user, targetroles.ToArray());
                        }
                        else _UsersAndRoles.Add(user, new string[] { role });
                    }
                }
            }
        }
    }
}
