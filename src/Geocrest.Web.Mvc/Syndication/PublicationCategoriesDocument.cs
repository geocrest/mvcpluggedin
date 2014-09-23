
namespace Geocrest.Web.Mvc.Syndication
{
    using System.Collections.Generic;
    using Geocrest.Web.Infrastructure;

    /// <summary>
    /// Provides a service document for returning AtomPub categories.
    /// </summary>
    public class PublicationCategoriesDocument : IPublicationCategoriesDocument
    {
        /// <summary>
        /// An IRI categorization scheme for all the categories contained within this document.
        /// </summary>
        public string Scheme { get; set; }
        /// <summary>
        /// The categories within this document.
        /// </summary>
        public IEnumerable<IPublicationCategory> Categories { get; set; }
        /// <summary>
        /// Indicates whether the list of categories is a fixed or an open set.
        /// </summary>
        public bool IsFixed { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Web.Mvc.Syndication.PublicationCategoriesDocument" /> class.
        /// </summary>
        /// <param name="scheme">The scheme.</param>
        /// <param name="categories">The categories.</param>
        /// <param name="isFixed">if set to <c>true</c> is fixed.</param>
        public PublicationCategoriesDocument(string scheme, IEnumerable<IPublicationCategory> categories, bool isFixed = false)
        {
            Throw.IfArgumentNullOrEmpty(scheme, "scheme");
            Throw.IfArgumentNull(categories, "categories");
           
            Scheme = scheme;
            Categories = categories;
            IsFixed = isFixed;
        }
    }
}
