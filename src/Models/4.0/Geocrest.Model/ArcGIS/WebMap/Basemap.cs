
namespace Geocrest.Model.ArcGIS.WebMap
{
    using System.Runtime.Serialization;
    /// <summary>
    /// Represents the base map within a web map
    /// </summary>
    [DataContract(Namespace = Geocrest.XmlNamespaces.ModelVersion1)]
    public class Basemap
    {
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        [DataMember(Name="title")]
        public string Title { get; set; }
        /// <summary>
        /// Gets or sets the basemap layers.
        /// </summary>
        /// <value>
        /// The basemap layers.
        /// </value>
        [DataMember(Name="baseMapLayers")]
        public BasemapLayer[] BasemapLayers { get; set; }
    }
}