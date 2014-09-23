
namespace Geocrest.Web.Mvc.Syndication
{
    /// <summary>
    /// Conveys an identifier for the link's context.
    /// </summary>
    public class SelfLink : Link
    {
        /// <summary>
        /// The value of the link's rel attribute.
        /// </summary>
        public const string Relation = "self";

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Web.Mvc.Syndication.SelfLink" /> class.
        /// </summary>
        /// <param name="href">The href.</param>
        /// <param name="title">The title.</param>
        public SelfLink(string href, string title = null) : base(Relation, href, title)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Web.Mvc.Syndication.SelfLink" /> class.
        /// </summary>
        public SelfLink() { }
    }
}
