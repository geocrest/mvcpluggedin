namespace Geocrest.Model.ArcGIS
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Provides the basic ID/Name combination for layer or table objects
    /// </summary>
    [DataContract(Namespace = Geocrest.XmlNamespaces.ModelVersion1)]
    public class LayerTableBase
    {
        /// <summary>
        /// Gets or sets the ID of the layer or table in the map.
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
    }
}
