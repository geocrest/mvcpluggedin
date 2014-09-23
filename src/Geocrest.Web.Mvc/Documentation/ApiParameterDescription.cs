namespace Geocrest.Web.Mvc.Documentation
{
    using System.Runtime.Serialization;

    /// <summary>
    /// This class represents a serializable form of the 
    /// <see cref="T:System.Web.Http.Description.ApiParameterDescription">ApiParameterDescription</see> class.
    /// </summary>
    [DataContract(Namespace = Geocrest.XmlNamespaces.InfrastructureVersion1)]
    public class ApiParameterDescription
    {
        /// <summary>
        /// Gets or sets the source.
        /// </summary>
        /// <value>
        /// The source.
        /// </value>
        [DataMember(Name = "source")]
        public string Source { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [DataMember(Name = "name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the documentation.
        /// </summary>
        /// <value>
        /// The documentation.
        /// </value>
        [DataMember(Name = "documentation")]
        public string Documentation { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        [DataMember(Name = "type")]
        public string Type { get; set; }
    }  
}
