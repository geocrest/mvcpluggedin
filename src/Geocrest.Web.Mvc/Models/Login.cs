
namespace Geocrest.Web.Mvc.Models
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// View model for a user's login information.
    /// </summary>
    public class Login
    {
        /// <summary>
        /// Gets or sets the username used to login.
        /// </summary>
        /// <value>
        /// The username.
        /// </value>
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the password used to login.
        /// </summary>
        /// <value>
        /// The password.
        /// </value>
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to remember the current user.
        /// </summary>
        /// <value>
        ///   <c>true</c> if a cookie should be set; otherwise, <c>false</c>.
        /// </value>
        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}
