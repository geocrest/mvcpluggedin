
namespace Geocrest.Model.ArcGIS.WebMap
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Represents application properties associated with an ArcGIS web map.
    /// </summary>
    [DataContract(Namespace = Geocrest.XmlNamespaces.ModelVersion1)]
    public class ApplicationProperties
    {
        /// <summary>
        /// Gets or sets editing properties within the web map.
        /// </summary>
        /// <value>
        /// The editing properties.
        /// </value>
        [DataMember(Name = "editing")]
        public Editing Editing { get; set; }

        /// <summary>
        /// Gets or sets properties used for offline maps.
        /// </summary>
        /// <value>
        /// The offline properties.
        /// </value>
        [DataMember(Name = "offline")]
        public Offline Offline { get; set; }

        /// <summary>
        /// Gets or sets viewing properties of the web map. If this is null or not defined, the client should assume a logical default.
        /// </summary>
        /// <value>
        /// The viewing properties.
        /// </value>
        [DataMember(Name = "viewing")]
        public Viewing Viewing { get; set; }
    }
}
