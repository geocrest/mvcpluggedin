namespace Geocrest.Model.ArcGIS.Schema
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Represents a coded value within a domain.
    /// </summary>
    [DataContract(Namespace = Geocrest.XmlNamespaces.ModelVersion1)]
    public class CodedValue
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        /// <value>
        /// The code.
        /// </value>
        [DataMember]
        public object Code { get; set; }
    }
}
