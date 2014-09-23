
namespace Geocrest.Web.Mvc.Syndication
{
    /// <summary>
    /// An IRI that refers to the furthest following resource in a series of resources.
    /// </summary>
    public class LastLink : Link
    {
        /// <summary>
        /// The value of the link's rel attribute.
        /// </summary>
        public const string Relation = "last";

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Web.Mvc.Syndication.LastLink" /> class.
        /// </summary>
        /// <param name="href">The href.</param>
        /// <param name="title">The title.</param>
        public LastLink(string href, string title = null)
            : base(Relation, href, title)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Web.Mvc.Syndication.LastLink" /> class.
        /// </summary>
        public LastLink() { }
    }
}
