namespace Geocrest.Model.ArcGIS.Schema
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Represents a relationship between two tables in a map service.
    /// </summary>
    [DataContract(Namespace = Geocrest.XmlNamespaces.ModelVersion1)]
    public class Relationship
    {
        /// <summary>
        /// Gets or sets the ID.
        /// </summary>
        /// <value>
        /// The ID.
        /// </value>
        [DataMember]
        public int ID { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the ID of the related table.
        /// </summary>
        /// <value>
        /// The related table ID.
        /// </value>
        [DataMember]
        public int RelatedTableID { get; set; }
    }
}
