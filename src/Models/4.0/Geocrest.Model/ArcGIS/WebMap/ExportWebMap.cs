
namespace Geocrest.Model.ArcGIS.WebMap
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Representation of a web map to be printed.
    /// </summary>
    [DataContract(Namespace = Geocrest.XmlNamespaces.ModelVersion1)]
    public class ExportWebMap
    {
        /// <summary>
        /// Gets or sets the map options.
        /// </summary>
        /// <value>
        /// The map options.
        /// </value>
        [DataMember(Name = "mapOptions")]
        public MapOptions MapOptions { get; set; }
        /// <summary>
        /// Gets or sets the operational layers.
        /// </summary>
        /// <value>
        /// The operational layers.
        /// </value>
        [DataMember(Name="operationalLayers")]
        public OperationalLayer[] OperationalLayers { get; set; }
        /// <summary>
        /// Gets or sets the base map.
        /// </summary>
        /// <value>
        /// The base map.
        /// </value>
        [DataMember(Name="baseMap")]
        public Basemap BaseMap { get; set; }
        /// <summary>
        /// Gets or sets the export options.
        /// </summary>
        /// <value>
        /// The export options.
        /// </value>
        [DataMember(Name="exportOptions")]
        public ExportOptions ExportOptions { get; set; }
    }
}