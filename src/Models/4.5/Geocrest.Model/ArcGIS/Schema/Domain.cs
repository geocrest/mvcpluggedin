namespace Geocrest.Model.ArcGIS.Schema
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Represents a domain applied to fields in a geodatabase table.
    /// </summary>
    [DataContract(Namespace = Geocrest.XmlNamespaces.ModelVersion1)]
    public class Domain
    {
        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        [DataMember]
        public string Type { get; set; }
        
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [DataMember]
        public string Name { get; set; }
        
        /// <summary>
        /// Gets or sets the range.
        /// </summary>
        /// <value>
        /// The range.
        /// </value>
        [DataMember]
        public int[] Range { get; set; }

        /// <summary>
        /// Gets or sets the coded values.
        /// </summary>
        /// <value>
        /// The coded values.
        /// </value>
        [DataMember]
        public CodedValue[] CodedValues { get; set; }
    }
}
