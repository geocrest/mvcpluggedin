namespace Geocrest.Data.Sources.Gis
{
    using System.Runtime.Serialization;
    using Geocrest.Data.Contracts.Gis;
    using Geocrest.Model.ArcGIS;
    using Geocrest.Model.ArcGIS.Geometry;
    /// <summary>
    /// Represents an ArcGIS Server mobile service.
    /// </summary>
    [DataContract(Namespace = Geocrest.XmlNamespaces.InfrastructureVersion1)]
    public class MobileServer : ArcGISService, IMobileServer
    {
        #region IMobileServer Members
        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        [DataMember]
        public string Description { get; set; }
        /// <summary>
        /// Gets or sets the name of the map.
        /// </summary>
        /// <value>
        /// The name of the map.
        /// </value>
        [DataMember]
        public string MapName { get; set; }
        /// <summary>
        /// Gets or sets the spatial reference.
        /// </summary>
        /// <value>
        /// The spatial reference.
        /// </value>
        [DataMember]
        public SpatialReference SpatialReference { get; set; }
        /// <summary>
        /// Gets or sets the units.
        /// </summary>
        /// <value>
        /// The units.
        /// </value>
        [DataMember]
        public string Units { get; set; }
        /// <summary>
        /// Gets or sets the full extent.
        /// </summary>
        /// <value>
        /// The full extent.
        /// </value>
        [DataMember]
        public Geometry FullExtent { get; set; }
        /// <summary>
        /// Gets or sets the initial extent.
        /// </summary>
        /// <value>
        /// The initial extent.
        /// </value>
        [DataMember]
        public Geometry InitialExtent { get; set; }
        /// <summary>
        /// Gets or sets the layer infos.
        /// </summary>
        /// <value>
        /// The layer infos.
        /// </value>
        [DataMember]
        public LayerTableBase[] Layers { get; set; }
        #endregion       
    }
}
