namespace Geocrest.Web.Mvc.Models.Account
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Provides a representation of a user's profile that can be stored in a data source. This
    /// class is intended for use with the <see cref="T:WebMatrix.WebData.SimpleMembershipProvider"/> class.
    /// </summary>
    public class UserProfile : Resource
    {
        /// <summary>
        /// Gets or sets the user id.
        /// </summary>
        /// <value>
        /// The user id.
        /// </value>
        [Key]
        public int UserId { get; set; }
        /// <summary>
        /// Gets or sets the username for the user.
        /// </summary>
        /// <value>
        /// The username for the user.
        /// </value>
        [Display(Name="Username")]
        public string UserName { get; set; }
        /// <summary>
        /// Gets or sets the user's email.
        /// </summary>
        /// <value>
        /// The email address.
        /// </value>
        [DataType(System.ComponentModel.DataAnnotations.DataType.EmailAddress)]
        public string Email { get; set; }
        /// <summary>
        /// Gets or sets the user's  first name.
        /// </summary>
        /// <value>
        /// The user's first name.
        /// </value>
        [Display(Name="First Name")]
        public string FirstName { get; set; }
        /// <summary>
        /// Gets or sets the user's last name.
        /// </summary>
        /// <value>
        /// The user's last name.
        /// </value>
        [Display(Name="Last Name")]
        public string LastName { get; set; }
    }

}
