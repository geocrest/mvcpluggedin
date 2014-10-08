namespace Geocrest.Model
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using Newtonsoft.Json;

    /// <summary>
    /// Represents a resource that complies with the Hypertext Application Language specification
    /// described here: <see href="http://stateless.co/hal_specification.html"/>
    /// </summary>
    public interface IHalResource
    {
        /// <summary>
        /// Adds a link to the entity.
        /// </summary>
        /// <param name="link">The link.</param>
        /// <exception cref="T:System.ArgumentNullException">link</exception>
        void AddLink(Link link);
       
        /// <summary>
        /// Gets or sets the links associated with this entity.
        /// </summary>
        [DataMember(Name = "_links", Order = 0, EmitDefaultValue = false)]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        IList<Link> Links { get; set; }

        /// <summary>
        /// For identifying how the target URI relates to the resource. The value
        /// of this will either be 'self' or null. 
        /// </summary>
        /// <value>
        /// The relation of the target URI.
        /// </value>
        string Rel { get; set; }

        /// <summary>
        /// Gets or sets the URI for this resource.
        /// </summary>
        /// <value>
        /// The URI of this unique resource.
        /// </value>
        string HRef { get; set; }
    }
}
