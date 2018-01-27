
namespace Geocrest.Model.ArcGIS.WebMap
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Represents an ArcGIS web map.
    /// </summary>
    [DataContract(Namespace = Geocrest.XmlNamespaces.ModelVersion1)]
    public class WebMap
    {
        /// <summary>
        /// Gets or sets the application properties.
        /// </summary>
        /// <value>
        /// The application properties.
        /// </value>
        [DataMember(Name = "applicationProperties")]
        public ApplicationProperties ApplicationProperties { get; set; }

        /// <summary>
        /// Gets or sets the authoring application.
        /// </summary>
        /// <value>
        /// The authoring application.
        /// </value>
        [DataMember(Name = "authoringApp")]
        public string AuthoringApp { get; set; }

        /// <summary>
        /// Gets or sets the authoring application version.
        /// </summary>
        /// <value>
        /// The authoring application version.
        /// </value>
        [DataMember(Name = "authoringAppVersion")]
        public string AuthoringAppVersion { get; set; }

        /// <summary>
        /// Gets or sets the base map.
        /// </summary>
        /// <value>
        /// The base map.
        /// </value>
        [DataMember(Name = "baseMap")]
        public Basemap BaseMap { get; set; }

        /// <summary>
        /// Gets or sets the operational layers.
        /// </summary>
        /// <value>
        /// The operational layers.
        /// </value>
        [DataMember(Name = "operationalLayers")]
        public OperationalLayer[] OperationalLayers { get; set; }

        /// <summary>
        /// Gets or sets the spatial reference.
        /// </summary>
        /// <value>
        /// The spatial reference.
        /// </value>
        [DataMember(Name = "spatialReference")]
        public SpatialReference SpatialReference { get; set; }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        [DataMember(Name = "version")]
        public string Version { get; set; }
    }
}
