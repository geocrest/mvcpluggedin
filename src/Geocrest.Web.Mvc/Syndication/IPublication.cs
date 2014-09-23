
namespace Geocrest.Web.Mvc.Syndication
{
    using System;
    using System.Collections.Generic;
    /// <summary>
    /// An interface for publications that can be returned as Atom entries.
    /// </summary>
    public interface IPublication
    {
        /// <summary>
        /// A permanent, universally unique identifier. Must be a valid IRI, as defined by [RFC3987].
        /// </summary>
        string Id { get; }

        /// <summary>
        /// A human-readable title for the publication.
        /// </summary>
        string Title { get; set; }

        /// <summary>
        /// A short summary, abstract, or excerpt of a publication.
        /// </summary>
        string Summary { get; set; }

        /// <summary>
        /// The content of the publication.
        /// </summary>
        string Content { get; set; }

        /// <summary>
        /// The type of content. Either "text", "html", "xhtml" or a valid MIME media type.
        /// </summary>
        string ContentType { get; set; }
        /// <summary>
        /// The most recent instant in time when the publication was modified.
        /// </summary>
        DateTime? LastUpdated { get; set; }

        /// <summary>
        /// The initial creation or first availability of the publication.
        /// </summary>
        DateTime? PublishDate { get; set; }

        /// <summary>
        /// A collection of categories associated with the publication.
        /// </summary>
        IEnumerable<IPublicationCategory> Categories { get; set; }

        /// <summary>
        /// An IRI categorization scheme for all the categories associated with the publication.
        /// </summary>
        string CategoriesScheme { get; set; }

        /// <summary>
        /// A collection of related resource links.
        /// </summary>
        ICollection<Link> Links { get; }
        /// <summary>
        /// Gets or sets a value indicating whether this publication is a draft.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this publication is a draft; otherwise, <c>false</c>.
        /// </value>
        bool IsDraft { get; set; }
    }
}
