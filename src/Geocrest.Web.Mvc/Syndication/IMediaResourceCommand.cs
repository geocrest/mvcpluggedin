namespace Geocrest.Web.Mvc.Syndication
{
    /// <summary>
    /// An interface for commands that can create or update media resources
    /// </summary>
    public interface IMediaResourceCommand
    {
        /// <summary>
        /// The title of the resource.
        /// </summary>
        string Title { get; set; }

        /// <summary>
        /// A short description of the content.
        /// </summary>
        string Summary { get; set; }
        /// <summary>
        /// The type of content. Either "image/jpeg", "image/jpg", "image/png", "image/gif", "multipart/form-data" or a valid MIME media type.
        /// </summary>
        string ContentType { get; set; }
        /// <summary>
        /// Gets the data.
        /// </summary>
        byte[] Content { get; set; }
    }
}