
namespace Geocrest.Web.Mvc.Syndication
{
    using Geocrest.Web.Infrastructure;

    /// <summary>
    /// A single AtomPub publication category
    /// </summary>
    public class PublicationCategory : IPublicationCategory
    {
        /// <summary>
        /// Required. The name (term) of the category.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Optional.
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Web.Mvc.Syndication.PublicationCategory" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="label">The label.</param>
        public PublicationCategory(string name, string label = null)
        {
            Throw.IfArgumentNullOrEmpty(name, "name");
            Name = name;
            Label = label;
        }
    }
}
