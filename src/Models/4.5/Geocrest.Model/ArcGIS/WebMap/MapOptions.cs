
namespace Geocrest.Model.ArcGIS.WebMap
{
    using Geocrest.Model.ArcGIS.Geometry;
    using System.Runtime.Serialization;

    /// <summary>
    /// Defines map display properties
    /// </summary>
    [DataContract(Namespace = Geocrest.XmlNamespaces.ModelVersion1)]
    public class MapOptions
    {
        /// <summary>
        /// Gets or sets the spatial reference of the map.
        /// </summary>
        /// <value>
        /// The spatial reference.
        /// </value>
        [DataMember(Name = "spatialReference")]
        public SpatialReference SpatialReference { get; set; }
        /// <summary>
        /// Gets or sets the extent of the map.
        /// </summary>
        /// <value>
        /// The extent.
        /// </value>
        [DataMember(Name = "extent")]
        public Geometry Extent { get; set; }
        /// <summary>
        /// Gets or sets the scale at which the map will be exported.
        /// </summary>
        /// <value>
        /// The scale.
        /// </value>
        [DataMember(Name = "scale")]
        public double Scale { get; set; }
        /// <summary>
        /// Gets or sets the rotation of the map.
        /// </summary>
        /// <value>
        /// The rotation.
        /// </value>
        [DataMember(Name = "rotation")]
        public int Rotation { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether to show attribution.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the map image should show attribution; otherwise, <c>false</c>.
        /// </value>
        [DataMember(Name = "showAttribution")]
        public bool ShowAttribution { get; set; }
    }
}