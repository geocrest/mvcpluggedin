
namespace Geocrest.Model.ArcGIS.WebMap
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Represents properties used by an offline ArcGIS web map.
    /// </summary>
    [DataContract(Namespace = Geocrest.XmlNamespaces.ModelVersion1)]
    public class Offline
    {
        /// <summary>
        /// Gets or sets the editable layers.
        /// </summary>
        /// <value>
        /// The editable layers.
        /// </value>
        [DataMember(Name = "editableLayers")]
        public EditableLayers EditableLayers { get; set; }

        /// <summary>
        /// Gets or sets the readonly layers.
        /// </summary>
        /// <value>
        /// The readonly layers.
        /// </value>
        [DataMember(Name = "readonlyLayers")]
        public ReadonlyLayers ReadonlyLayers { get; set; }
    }
}
