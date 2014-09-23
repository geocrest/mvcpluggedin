
namespace Geocrest.Web.Mvc.Syndication
{
    /// <summary>
    /// Identifies a related resource.
    /// </summary>
    public class RelatedLink : Link
    {
        /// <summary>
        /// The value of the link's rel attribute.
        /// </summary>
        public const string Relation = "related";

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Web.Mvc.Syndication.RelatedLink" /> class.
        /// </summary>
        /// <param name="href">The href.</param>
        /// <param name="title">The title.</param>
        public RelatedLink(string href, string title = null)
            : base(Relation, href, title)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Web.Mvc.Syndication.RelatedLink" /> class.
        /// </summary>
        public RelatedLink() { }
    }
}
