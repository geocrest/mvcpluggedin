namespace Geocrest.Data.Contracts.Gis
{
    using System;
    using System.Collections.Generic;
    using Geocrest.Model.ArcGIS;
    using Geocrest.Model.ArcGIS.Geometry;
    using Geocrest.Model.ArcGIS.Tasks;

    /// <summary>
    /// Provides access to map server operations
    /// </summary>
    public interface IMapServer:IArcGISService
    {
        #region Properties
        /// <summary>
        /// Gets or sets the name of the map.
        /// </summary>
        /// <value>
        /// The name of the map.
        /// </value>
        string MapName { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        string Description { get; set; }

        /// <summary>
        /// Gets or sets the copyright text.
        /// </summary>
        /// <value>
        /// The copyright text.
        /// </value>
        string CopyrightText { get; set; }

        /// <summary>
        /// Gets or sets the layer infos.
        /// </summary>
        /// <value>
        /// The layer infos.
        /// </value>
        LayerTableInfo[] LayerInfos { get; set; }

        /// <summary>
        /// Gets or sets the table infos.
        /// </summary>
        /// <value>
        /// The table infos.
        /// </value>
        LayerTableInfo[] TableInfos { get; set; }

        /// <summary>
        /// Gets or sets the spatial reference.
        /// </summary>
        /// <value>
        /// The spatial reference.
        /// </value>
        SpatialReference SpatialReference { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether single fused map cache.
        /// </summary>
        /// <value>
        /// <b>true</b>, if single fused map cache; otherwise, <b>false</b>.
        /// </value>
        bool SingleFusedMapCache { get; set; }

        /// <summary>
        /// Gets or sets the initial extent.
        /// </summary>
        /// <value>
        /// The initial extent.
        /// </value>
        Geometry InitialExtent { get; set; }

        /// <summary>
        /// Gets or sets the full extent.
        /// </summary>
        /// <value>
        /// The full extent.
        /// </value>
        Geometry FullExtent { get; set; }

        /// <summary>
        /// Gets or sets the units.
        /// </summary>
        /// <value>
        /// The units.
        /// </value>
        string Units { get; set; }

        /// <summary>
        /// Gets or sets the supported image format types.
        /// </summary>
        /// <value>
        /// The supported image format types.
        /// </value>
        string SupportedImageFormatTypes { get; set; }

        /// <summary>
        /// Gets or sets the document info.
        /// </summary>
        /// <value>
        /// The document info.
        /// </value>
        DocumentInfo DocumentInfo { get; set; }

        /// <summary>
        /// Gets or sets the capabilities.
        /// </summary>
        /// <value>
        /// The capabilities.
        /// </value>
        string Capabilities { get; set; }

        /// <summary>
        /// Gets or sets the information about the map's cache (if available).
        /// </summary>
        /// <value>
        /// The tile info.
        /// </value>
        TileInfo TileInfo { get; set; }

        /// <summary>
        /// Gets the actual layers in the map.
        /// </summary>
        /// <value>
        /// The layers.
        /// </value>
        LayerTable[] Layers { get; }

        /// <summary>
        /// Gets the actual tables in the map.
        /// </summary>
        /// <value>
        /// The tables.
        /// </value>
        LayerTable[] Tables { get; }
        #endregion

        /// <summary>
        /// Performs an identify operation on a map service.
        /// </summary>
        /// <param name="geometry">The geometry to identify on.</param>
        /// <param name="tolerance">The distance in screen pixels from the specified geometry within which the identify should be performed.</param>
        /// <param name="mapExtent">The extent or bounding box of the map currently being viewed.</param>
        /// <param name="imageDisplay">The screen image display parameters (width, height and DPI) of the map being currently viewed.</param>
        /// <param name="callback">The callback function used to retrieve the results.</param>
        /// <param name="identifyoption">How to perform the identify.</param>
        /// <param name="layers">The layers to perform the identify operation on.</param>
        /// <param name="returnGeometry">If true, the resultset will include the geometries associated with each result.</param>
        /// <param name="layerDefs">Allows you to filter the features of individual layers in the exported map by specifying definition expressions for those layers.</param>
        void IdentifyAsync(
            Geometry geometry, 
            int tolerance,
            Geometry mapExtent,
            ImageDisplayParameters imageDisplay,
            Action<IdentifyResultCollection> callback,
            IdentifyLayersOption identifyoption = IdentifyLayersOption.All,
            int[] layers =null, 
            bool returnGeometry = true,
            IDictionary<LayerTable,string> layerDefs = null);

        /// <summary>
        /// Performs an identify operation on a map service.
        /// </summary>
        /// <param name="geometry">The geometry to identify on.</param>
        /// <param name="tolerance">The distance in screen pixels from the specified geometry within which the identify should be performed.</param>
        /// <param name="mapExtent">The extent or bounding box of the map currently being viewed.</param>
        /// <param name="imageDisplay">The screen image display parameters (width, height and DPI) of the map being currently viewed.</param>
        /// <param name="identifyoption">How to perform the identify.</param>
        /// <param name="layers">The layers to perform the identify operation on.</param>
        /// <param name="returnGeometry">If true, the resultset will include the geometries associated with each result.</param>
        /// <param name="layerDefs">Allows you to filter the features of individual layers in the exported map by specifying definition expressions for those layers.</param>
        /// <returns>
        /// A collection of <see cref="T:Geocrest.Model.ArcGIS.Tasks.IdentifyResult">IdentifyResult</see>s.
        /// </returns>
        IdentifyResultCollection Identify(
            Geometry geometry,
            int tolerance,
            Geometry mapExtent,
            ImageDisplayParameters imageDisplay,
            IdentifyLayersOption identifyoption = IdentifyLayersOption.All,
            int[] layers = null,
            bool returnGeometry = true,
            IDictionary<LayerTable, string> layerDefs = null);

        /// <summary>
        /// Performs a query on the specified layer.
        /// </summary>
        /// <param name="layerId">The id of the layer to query.</param>
        /// <param name="query">A where clause for the query filter. Any legal SQL where clause operating on the fields in the layer is allowed.</param>
        /// <param name="outSR">The spatial reference of the returned geometry.</param>
        /// <param name="relationship">The spatial relationship to be applied on the input geometry while performing the query.</param>
        /// <param name="geometry">The geometry to apply as the spatial filter.</param>
        /// <param name="returnGeometry">If true, the resultset includes the geometry associated with each result.</param>
        /// <param name="objectIds">The object IDs of this layer / table to be queried.</param>
        /// <param name="outFields">The list of fields to be included in the returned resultset.</param>
        /// <param name="returnIdsOnly">If true, the response only includes an array of object IDs. Otherwise the response is a feature set.</param>
        /// <param name="returnCountOnly">If true, the response only includes the count (number of features / records) that would be returned by a query. Otherwise the response is a feature set.</param>
        /// <returns></returns>
        FeatureSetQuery Query(
            int layerId,
            string query,
            WKID outSR,
            esriSpatialRelationship relationship,
            Geometry geometry = null,
            bool returnGeometry = true,
            int[] objectIds = null,
            string outFields = "",
            bool returnIdsOnly = false,
            bool returnCountOnly = false);

        /// <summary>
        /// Performs a query on the specified layer.
        /// </summary>
        /// <param name="layerId">The id of the layer to query.</param>
        /// <param name="query">A where clause for the query filter. Any legal SQL where clause operating on the fields in the layer is allowed.</param>
        /// <param name="outSR">The spatial reference of the returned geometry.</param>
        /// <param name="returnGeometry">If true, the resultset includes the geometry associated with each result.</param>
        /// <param name="objectIds">The object IDs of this layer / table to be queried.</param>
        /// <param name="outFields">The list of fields to be included in the returned resultset.</param>
        /// <param name="returnIdsOnly">If true, the response only includes an array of object IDs. Otherwise the response is a feature set.</param>
        /// <param name="returnCountOnly">If true, the response only includes the count (number of features / records) that would be returned by a query. Otherwise the response is a feature set.</param>
        /// <returns></returns>
        FeatureSetQuery Query(
            int layerId,
            string query,
            WKID outSR,            
            bool returnGeometry = true,
            int[] objectIds = null,
            string outFields = "",
            bool returnIdsOnly = false,
            bool returnCountOnly = false);
    }
}
