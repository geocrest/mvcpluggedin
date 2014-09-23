
namespace Geocrest.Web.Mvc.Syndication
{
    /// <summary>
    /// Indicates that the link's context is a part of a series, and that the next in the series is the link target.
    /// </summary>
    public class NextLink : Link
    {
        /// <summary>
        /// The value of the link's rel attribute.
        /// </summary>
        public const string Relation = "next";

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Web.Mvc.Syndication.NextLink" /> class.
        /// </summary>
        /// <param name="href">The href.</param>
        /// <param name="title">The title.</param>
        public NextLink(string href, string title = null)
            : base(Relation, href, title)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Web.Mvc.Syndication.NextLink" /> class.
        /// </summary>
        public NextLink() { }
    }
}
