
namespace Geocrest.Model.ArcGIS.WebMap
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Represents the layer and update interval settings for location tracking in an ArcGIS web map. 
    /// </summary>
    [DataContract(Namespace = Geocrest.XmlNamespaces.ModelVersion1)]
    public class Info
    {
        /// <summary>
        /// Gets or sets the layer identifier.
        /// </summary>
        /// <value>
        /// The layer identifier.
        /// </value>
        [DataMember(Name = "layerId")]
        public string LayerId { get; set; }

        /// <summary>
        /// Gets or sets the update interval.
        /// </summary>
        /// <value>
        /// The update interval.
        /// </value>
        [DataMember(Name = "updateInterval")]
        public string UpdateInterval { get; set; }
    }
}
