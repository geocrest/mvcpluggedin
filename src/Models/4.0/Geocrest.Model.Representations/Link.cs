namespace Geocrest.Model
{
    using System;
    using System.Runtime.Serialization;
    using System.Text.RegularExpressions;
    using Newtonsoft.Json;

    /// <summary>
    /// Represents the base class for all hypermedia links. For more information on Hypertext 
    /// Application Language (HAL) see <see href="http://tools.ietf.org/id/draft-kelly-json-hal-05.html"/>
    /// </summary>
    [DataContract(Namespace = Geocrest.XmlNamespaces.ModelVersion1)]
    [KnownType(typeof(SelfLink))]
    [KnownType(typeof(EditLink))]
    [KnownType(typeof(FirstLink))]
    [KnownType(typeof(PreviousLink))]
    [KnownType(typeof(NextLink))]
    [KnownType(typeof(LastLink))]
    [KnownType(typeof(CollectionLink))]
    [KnownType(typeof(RelatedLink))]
    public class Link
    {
        /// <summary>
        /// Gets or sets the link relation.
        /// </summary>
        [DataMember(Name="rel",EmitDefaultValue=false)]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Rel { get; set; }

        /// <summary>
        /// Gets or sets the URI of this link.
        /// </summary>
        [DataMember(Name = "href", EmitDefaultValue = false)]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string HRef { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        [DataMember(Name = "title", EmitDefaultValue = false)]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:Geocrest.Model.Link"/> is templated.
        /// </summary>
        /// <value>
        /// <b>true</b>, if templated; otherwise, <b>false</b>.
        /// </value>
        [DataMember(Name = "templated", EmitDefaultValue = false)]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool Templated { get; set; }

        /// <summary>
        /// Indicates the media type expected when dereferencing the target resource.
        /// </summary>
        /// <value>
        /// The expected media type.
        /// </value>
        [DataMember(Name = "type", EmitDefaultValue = false)]
        [JsonProperty(NullValueHandling=NullValueHandling.Ignore)]
        public string Type { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Model.Link"/> class.
        /// </summary>
        /// <param name="rel">The rel.</param>
        /// <param name="href">The href.</param>
        /// <param name="title">The title.</param>
        /// <exception cref="System.ArgumentNullException">
        /// rel
        /// or
        /// href
        /// </exception>
        public Link(string rel, string href, string title = null)
        {
            if (string.IsNullOrEmpty(rel)) throw new ArgumentNullException("rel");
           // if (string.IsNullOrEmpty(href)) throw new ArgumentNullException("href");
            Rel = rel;
            HRef = href;
            Title = title;
            Templated = Regex.Match(href, @"{\w+}", RegexOptions.CultureInvariant).Success;
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return HRef;
        }
    }
}
