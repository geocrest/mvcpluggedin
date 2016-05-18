namespace Geocrest.Model.ArcGIS.Tasks
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    /// <summary>
    /// Represents a single record containing attributes.
    /// </summary>
    [DataContract(Namespace = Geocrest.XmlNamespaces.ModelVersion1)]
    public class Record
    {
        /// <summary>
        /// Gets or sets the attributes.
        /// </summary>
        /// <value>
        /// The attributes.
        /// </value>
        [DataMember(Name = "attributes")]
        public IDictionary<string, object> Attributes { get; set; }
    }
}
