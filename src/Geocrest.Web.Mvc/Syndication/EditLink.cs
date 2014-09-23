
namespace Geocrest.Web.Mvc.Syndication
{
    /// <summary>
    /// Refers to a resource that can be used to edit the link's context.
    /// </summary>
    public class EditLink : Link
    {
        /// <summary>
        /// The value of the link's rel attribute.
        /// </summary>
        public const string Relation = "edit";

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Web.Mvc.Syndication.EditLink" /> class.
        /// </summary>
        /// <param name="href">The href.</param>
        /// <param name="title">The title.</param>
        public EditLink(string href, string title = null)
            : base(Relation, href, title)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Web.Mvc.Syndication.EditLink" /> class.
        /// </summary>
        public EditLink(){}
    }
}
