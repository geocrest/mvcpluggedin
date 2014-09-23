namespace Geocrest.Data.Contracts.Gis
{
    using Geocrest.Model.ArcGIS;
    using Geocrest.Model.ArcGIS.Geometry;
    /// <summary>
    /// Provides access to MobileServer operations.
    /// </summary>
    public interface IMobileServer : IArcGISService
    {
        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>       
        string Description { get; set; }
        /// <summary>
        /// Gets or sets the name of the map.
        /// </summary>
        /// <value>
        /// The name of the map.
        /// </value>
        string MapName { get; set; }
        /// <summary>
        /// Gets or sets the spatial reference.
        /// </summary>
        /// <value>
        /// The spatial reference.
        /// </value>
        SpatialReference SpatialReference { get; set; }
        /// <summary>
        /// Gets or sets the units.
        /// </summary>
        /// <value>
        /// The units.
        /// </value>
        string Units { get; set; }
        /// <summary>
        /// Gets or sets the full extent.
        /// </summary>
        /// <value>
        /// The full extent.
        /// </value>
        Geometry FullExtent { get; set; }
        /// <summary>
        /// Gets or sets the initial extent.
        /// </summary>
        /// <value>
        /// The initial extent.
        /// </value>
        Geometry InitialExtent { get; set; }
        /// <summary>
        /// Gets or sets the layer infos.
        /// </summary>
        /// <value>
        /// The layer infos.
        /// </value>
        LayerTableBase[] Layers { get; set; }
    }
}
