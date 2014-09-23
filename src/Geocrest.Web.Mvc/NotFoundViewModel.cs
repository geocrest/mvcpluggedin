
namespace Geocrest.Web.Mvc
{
    /// <summary>
    /// View model containing 404 error information.
    /// </summary>
    public class NotFoundViewModel
    {
        /// <summary>
        /// Gets or sets the requested URL.
        /// </summary>
        /// <value>
        /// The requested URL.
        /// </value>
        public string RequestedUrl { get; set; }
        /// <summary>
        /// Gets or sets the referrer URL.
        /// </summary>
        /// <value>
        /// The referrer URL.
        /// </value>
        public string ReferrerUrl { get; set; }
    }
}
