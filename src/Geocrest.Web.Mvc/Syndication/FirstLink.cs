
namespace Geocrest.Web.Mvc.Syndication
{
    /// <summary>
    /// An IRI that refers to the furthest preceding resource in a series of resources.
    /// </summary>
    public class FirstLink : Link
    {
        /// <summary>
        /// The value of the link's rel attribute.
        /// </summary>
        public const string Relation = "first";

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Web.Mvc.Syndication.FirstLink" /> class.
        /// </summary>
        /// <param name="href">The href.</param>
        /// <param name="title">The title.</param>
        public FirstLink(string href, string title = null)
            : base(Relation, href, title)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Web.Mvc.Syndication.FirstLink" /> class.
        /// </summary>
        public FirstLink() { }
    }
}
