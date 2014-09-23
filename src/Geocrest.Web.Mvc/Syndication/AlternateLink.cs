
namespace Geocrest.Web.Mvc.Syndication
{
    /// <summary>
    /// Refers to a substitute for this link.
    /// </summary>
    public class AlternateLink : Link
    {
        /// <summary>
        /// The value of the link's rel attribute.
        /// </summary>
        public const string Relation = "alternate";

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Web.Mvc.Syndication.AlternateLink" /> class.
        /// </summary>
        /// <param name="href">The href.</param>
        /// <param name="title">The title.</param>
        public AlternateLink(string href, string title = null)
            : base(Relation, href, title)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Web.Mvc.Syndication.AlternateLink" /> class.
        /// </summary>
        public AlternateLink() { }
    }
}
