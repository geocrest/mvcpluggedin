namespace Geocrest.Web.Mvc.Models
{
    using Geocrest.Data.Contracts;
    using Geocrest.Web.Infrastructure;
    using System.Linq;
    using System.Web.Profile;
    using System.Web.Security;

    /// <summary>
    /// Represents a user's profile in the application.
    /// </summary>
    public class BaseProfile : DefaultProfile
    {
        private IRepository repository;
     
        /// <summary>
        /// Creates a profile for the specified user name when the application is configured to use the SimpleMembershipProvider.
        /// </summary>
        /// <param name="repository">The repository to use when retrieving items from the database.</param>
        /// <param name="username">The username.</param>
        /// <returns></returns>
        public static BaseProfile CreateProfile(IRepository repository, string username)
        {
            Throw.IfArgumentNull(repository, "repository");
            var p = ProfileBase.Create(username) as BaseProfile;
            var up = repository.FindBy<UserProfile>(x => x.UserName == username).SingleOrDefault();
            p.repository = repository;
            if (up != null)
            {
                p.FirstName = up.FirstName;
                p.LastName = up.LastName;
                p.Email = up.Email;
            }
            return p;
        }
        /// <summary>
        /// Creates a profile for the specified user name when the application is configured to use something other than SimpleMembershipProvider.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns></returns>
        public static BaseProfile CreateProfile(string username)
        {
            var user = Membership.GetUser(username,false);
            var p = ProfileBase.Create(username) as BaseProfile;
            if (user != null && p != null)
            {
                p.FirstName = p["FirstName"] as string;
                p.LastName = p["LastName"] as string;
                p.Email = user.Email;
            }

            return p;
        }
        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        /// <value>
        /// The first name.
        /// </value>
        public string FirstName 
        {
            get { return this["FirstName"] as string; }
            set { this["FirstName"] = value; }
        }
        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        /// <value>
        /// The last name.
        /// </value>
        public string LastName
        {
            get { return this["LastName"] as string; }
            set { this["LastName"]= value; }
        }
        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>
        /// The email.
        /// </value>
        public string Email 
        {
            get { return this["Email"] as string; }
            set { this["Email"] = value; }
        }
        /// <summary>
        /// Gets or sets the application.
        /// </summary>
        /// <value>
        /// The application.
        /// </value>
        public string Application
        {
            get { return this["Application"] as string; }
            set { this["Application"] = value; }
        }
        /// <summary>
        /// Updates the profile data source with changed profile property values.
        /// </summary>
        /// <remarks>If the SimpleMembershipProvider is configured for use, then the
        /// profile properties will be saved to the associated UserProfile table.</remarks>
        public override void Save()
        {            
            if (BaseApplication.IsSimpleMembershipProviderConfigured() && this.repository != null)
            {
                var up = this.repository.FindBy<UserProfile>(x => x.UserName == this.UserName).SingleOrDefault();
                if (up != null)
                {
                    up.FirstName = this.FirstName;
                    up.LastName = this.LastName;
                    up.Email = this.Email;
                    this.repository.Update<UserProfile>(up);
                    this.repository.Save();
                }
            }
            else
            {
                base.Save();
            }
        }
    }
}
