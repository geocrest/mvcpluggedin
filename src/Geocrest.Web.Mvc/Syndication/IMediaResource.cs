namespace Geocrest.Web.Mvc.Syndication
{
    using System.Collections.Generic;

    /// <summary>
    /// An interface for media resources that can be used with AtomPub
    /// </summary>
    public interface IMediaResource
    {
        /// <summary>
        /// A permanent, universally unique identifier. Must be a valid IRI, as defined by [RFC3987].
        /// </summary>
        string Id { get; set; }
        /// <summary>
        /// Gets or sets the name of the object.
        /// </summary>
        /// <value>
        /// The name of the object.
        /// </value>
        string Title { get; set; }
        /// <summary>
        /// Gets or sets the summary.
        /// </summary>
        /// <value>
        /// The summary.
        /// </value>
        string Summary { get; set; }
        /// <summary>
        /// The type of content. Either "image/jpeg", "image/jpg", "image/png", "image/gif", "multipart/form-data" or a valid MIME media type.
        /// </summary>
        string ContentType { get; set; }
        /// <summary>
        /// Gets or sets the length of the content.
        /// </summary>
        /// <value>
        /// The length of the content.
        /// </value>
        int ContentLength { get; set; }
        /// <summary>
        /// Gets the data.
        /// </summary>
        byte[] Content { get; set; }
        /// <summary>
        /// Gets or sets location to the media.
        /// </summary>
        /// <value>
        /// The media URL.
        /// </value>
        string MediaUrl { get; set; }
        /// <summary>
        /// A collection of related resource links.
        /// </summary>
        IList<Link> Links { get; }
    }
}