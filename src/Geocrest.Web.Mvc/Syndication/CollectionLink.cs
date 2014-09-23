
namespace Geocrest.Web.Mvc.Syndication
{
    /// <summary>
    /// The target IRI points to a resource which represents the collection resource for the context IRI.
    /// </summary>
    public class CollectionLink : Link
    {
        /// <summary>
        /// The value of the link's rel attribute.
        /// </summary>
        public const string Relation = "collection";

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Web.Mvc.Syndication.CollectionLink" /> class.
        /// </summary>
        /// <param name="href">The href.</param>
        /// <param name="title">The title.</param>
        public CollectionLink(string href, string title = null)
            : base(Relation, href, title)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Web.Mvc.Syndication.CollectionLink" /> class.
        /// </summary>
        public CollectionLink() { }
    }
}
