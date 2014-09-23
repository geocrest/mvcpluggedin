namespace Geocrest.Web.Mvc.Syndication
{
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;
    using Geocrest.Web.Infrastructure;
    /// <summary>
    /// A base class for relation links.
    /// </summary>
    [DataContract]
    public class Link
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>
        /// The id.
        /// </value>
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// The value of the link's rel attribute.
        /// </summary>
        [DataMember]
        public string Rel { get; set; }

        /// <summary>
        /// Gets the href for the link.
        /// </summary>
        [DataMember]
        public string Href { get; set; }

        /// <summary>
        /// Gets the title for the link.
        /// </summary>
        [DataMember]
        public string Title { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Web.Mvc.Syndication.Link" /> class.
        /// </summary>
        /// <param name="rel">The relation attribute for the link.</param>
        /// <param name="href">The unique IRI for the resource.</param>
        /// <param name="title">An optional title for the link.</param>
        public Link(string rel, string href, string title = null)
        {
            Throw.IfArgumentNullOrEmpty(rel, "rel");
            Throw.IfArgumentNullOrEmpty(href, "href");
            Rel = rel;
            Href = href;
            Title = title;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Web.Mvc.Syndication.Link" /> class.
        /// </summary>
        public Link() { }
        /// <summary>
        /// Returns a <see cref="T:System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// The value of the <see cref="P:Geocrest.Web.Mvc.Syndication.Link.Href"/> property.
        /// </returns>
        public override string ToString()
        {
            return Href;
        }
    }
}
