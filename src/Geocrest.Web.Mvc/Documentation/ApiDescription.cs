namespace Geocrest.Web.Mvc.Documentation
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Represents a serializable form of the 
    /// <see cref="T:System.Web.Http.Description.ApiDescription">ApiDescription</see> class.
    /// </summary>
    [DataContract(Namespace = Geocrest.XmlNamespaces.InfrastructureVersion1)]
    public class ApiDescription
    {
        /// <summary>
        /// Gets or sets the relative path.
        /// </summary>
        /// <value>
        /// The relative path.
        /// </value>
        [DataMember(Name="relativePath")]
        public string RelativePath { get; set; }

        /// <summary>
        /// Gets or sets the HTTP method.
        /// </summary>
        /// <value>
        /// The HTTP method.
        /// </value>
        [DataMember(Name="httpMethod")]
        public string HttpMethod { get; set; }

        /// <summary>
        /// Gets or sets the documentation.
        /// </summary>
        /// <value>
        /// The documentation.
        /// </value>
        [DataMember(Name="documentation")]
        public string Documentation { get; set; }
        
        /// <summary>
        /// Gets or sets the return type.
        /// </summary>
        /// <value>
        /// The type of object returned.
        /// </value>
        [DataMember(Name="returnType")]
        public string ReturnType { get; set; }

        /// <summary>
        /// Gets or sets the parameter descriptions.
        /// </summary>
        /// <value>
        /// The parameter descriptions.
        /// </value>
        [DataMember(Name="parameterDescriptions")]
        public ApiParameterDescription[] ParameterDescriptions { get; set; }
    }    
}
