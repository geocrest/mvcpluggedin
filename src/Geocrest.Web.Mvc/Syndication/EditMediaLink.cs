
namespace Geocrest.Web.Mvc.Syndication
{
    /// <summary>
    /// Refers to a resource that can be used to edit media associated with the link's context.
    /// </summary>
    public class EditMediaLink : Link
    {
        /// <summary>
        /// The value of the link's rel attribute.
        /// </summary>
        public const string Relation = "edit-media";

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Web.Mvc.Syndication.EditMediaLink" /> class.
        /// </summary>
        /// <param name="href">The href.</param>
        /// <param name="title">The title.</param>
        public EditMediaLink(string href, string title = null)
            : base(Relation, href, title)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Web.Mvc.Syndication.EditMediaLink" /> class.
        /// </summary>
        public EditMediaLink() { }
    }
}
