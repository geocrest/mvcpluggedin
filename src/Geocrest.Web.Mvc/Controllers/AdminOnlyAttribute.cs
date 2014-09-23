namespace Geocrest.Web.Mvc.Controllers
{

    /// <summary>
    /// Provides a shortcut attribute to specify the administrative role as read from the web.config.
    /// </summary>
    /// <remarks>To use this attribute, specify an AppSetting in the web.config with a key of 'AdminRole'</remarks>
    public class AdminOnlyAttribute : AuthorizeAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Web.Mvc.Controllers.AdminOnlyAttribute"/> class.
        /// </summary>
        public AdminOnlyAttribute()
        {
            Roles = BaseApplication.AdminRole;
        }
    }
}
