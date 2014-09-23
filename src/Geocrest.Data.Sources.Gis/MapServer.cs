namespace Geocrest.Data.Sources.Gis
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;
    using Geocrest.Data.Contracts.Gis;
    using Geocrest.Web.Infrastructure;
    using Geocrest.Model.ArcGIS;
    using Geocrest.Model.ArcGIS.Geometry;
    using Geocrest.Model.ArcGIS.Tasks;

    /// <summary>
    /// Represents an ArcGIS Server map service.
    /// </summary>
    [DataContract(Namespace = Geocrest.XmlNamespaces.InfrastructureVersion1)]
    public class MapServer : ArcGISService, IMapServer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Data.Sources.Gis.MapServer"/> class.
        /// </summary>
        internal MapServer() { }

        #region Properties
        /// <summary>
        /// Gets or sets the capabilities.
        /// </summary>
        /// <value>
        /// The capabilities.
        /// </value>
        [DataMember]
        public string Capabilities { get; set; }

        /// <summary>
        /// Gets or sets the copyright text.
        /// </summary>
        /// <value>
        /// The copyright text.
        /// </value>
        [DataMember]
        public string CopyrightText { get; set; }
             
        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the document info.
        /// </summary>
        /// <value>
        /// The document info.
        /// </value>
        [DataMember]
        public DocumentInfo DocumentInfo { get; set; }

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
        [DataMember(Name = "layers")]
        public LayerTableInfo[] LayerInfos { get; set; }

        /// <summary>
        /// Gets the actual layers in the map.
        /// </summary>
        /// <value>
        /// The layers.
        /// </value>
        [DataMember]
        public LayerTable[] Layers { get; internal set; }

        /// <summary>
        /// Gets or sets the name of the map.
        /// </summary>
        /// <value>
        /// The name of the map.
        /// </value>
        [DataMember]
        public string MapName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether single fused map cache.
        /// </summary>
        /// <value>
        /// <b>true</b>, if single fused map cache; otherwise, <b>false</b>.
        /// </value>
        [DataMember]
        public bool SingleFusedMapCache { get; set; }

        /// <summary>
        /// Gets or sets the spatial reference.
        /// </summary>
        /// <value>
        /// The spatial reference.
        /// </value>
        [DataMember]
        public SpatialReference SpatialReference { get; set; }

        /// <summary>
        /// Gets or sets the supported image format types.
        /// </summary>
        /// <value>
        /// The supported image format types.
        /// </value>
        [DataMember]
        public string SupportedImageFormatTypes { get; set; }

        /// <summary>
        /// Gets or sets the table infos.
        /// </summary>
        /// <value>
        /// The table infos.
        /// </value>
        [DataMember(Name = "tables")]
        public LayerTableInfo[] TableInfos { get; set; }

        /// <summary>
        /// Gets the actual tables in the map.
        /// </summary>
        /// <value>
        /// The tables.
        /// </value>
        [DataMember]
        public LayerTable[] Tables { get; internal set; }

        /// <summary>
        /// Gets or sets the information about the map's cache (if available).
        /// </summary>
        /// <value>
        /// The tile info.
        /// </value>
        [DataMember]
        public TileInfo TileInfo { get; set; }

        /// <summary>
        /// Gets or sets the units.
        /// </summary>
        /// <value>
        /// The units.
        /// </value>
        [DataMember]
        public string Units { get; set; }
        #endregion
        
        #region IMapServer Members
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
        public IdentifyResultCollection Identify(
                                                 Geometry geometry, 
                                                 int tolerance, 
                                                 Geometry mapExtent, 
                                                 ImageDisplayParameters imageDisplay,
                                                 IdentifyLayersOption identifyoption = IdentifyLayersOption.All, 
                                                 int[] layers = null, 
                                                 bool returnGeometry = true,
                                                 System.Collections.Generic.IDictionary<LayerTable, string> layerDefs = null)
        {
            try
            {
                ValidateGeometry(geometry);
                ValidateGeometry(mapExtent);
            }
            catch (ArgumentNullException ex)
            {
                Throw.InvalidOperation("",
                    x => new InvalidOperationException("Geometry object is invalid.", ex));
            }
            IDictionary<string, object> inputs = new Dictionary<string, object>()
            {
                { "geometryType", geometry.GeometryType },
                { "geometry", geometry},
                { "sr", geometry.SpatialReference.WKID},
                { "layers", GetLayers(identifyoption,layers)},
                { "imageDisplay", string.Format("{0},{1},{2}",imageDisplay.Width.ToString(),
                    imageDisplay.Height.ToString(),imageDisplay.DPI.ToString())},
                { "mapExtent",string.Format("{0},{1},{2},{3}",mapExtent.XMin.Value.ToString(),
                    mapExtent.YMin.Value.ToString(),mapExtent.XMax.Value.ToString(),mapExtent.YMax.Value.ToString())},
                { "returnGeometry",returnGeometry.ToString()},
                { "layerDefs",GetLayerDefs(layerDefs)},
                { "tolerance", tolerance}
            };
            Uri endpoint = GetUrl("identify", inputs);
            return RestHelper.Hydrate<IdentifyResultCollection>(endpoint.ToString());
        }

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
        public void IdentifyAsync(
                                  Geometry geometry,
                                  int tolerance,
                                  Geometry mapExtent,
                                  ImageDisplayParameters imageDisplay,
                                  System.Action<IdentifyResultCollection> callback,
                                  IdentifyLayersOption identifyoption = IdentifyLayersOption.All,
                                  int[] layers = null,
                                  bool returnGeometry = true,
                                  System.Collections.Generic.IDictionary<LayerTable, string> layerDefs = null)
        {
            throw new System.NotImplementedException();
        }

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
        public FeatureSetQuery Query(
                                     int layerId,
                                     string query, 
                                     WKID outSR, 
                                     esriSpatialRelationship relationship,
                                     Geometry geometry = null, 
                                     bool returnGeometry = true,
                                     int[] objectIds = null, 
                                     string outFields = "", 
                                     bool returnIdsOnly = false,
                                     bool returnCountOnly = false)
        {
            LayerTableInfo layer = this.LayerInfos.Single(x => x.ID == layerId);
            if (layer == null) Throw.ArgumentOutOfRange("layerId");
            IDictionary<string, object> inputs = new Dictionary<string, object>()
            {
                { "returnGeometry",returnGeometry.ToString()},
                { "returnIdsOnly", returnIdsOnly },
                { "returnCountOnly", returnCountOnly },
                { "outFields", outFields },
                { "where", query },
                { "objectIds", objectIds },
            };
            if (outSR != WKID.NotSpecified) inputs.Add("outSR", outSR);
            if (geometry != null)
            {
                try
                {
                    ValidateGeometry(geometry);
                }
                catch (ArgumentNullException ex)
                {
                    Throw.InvalidOperation("",
                        x => new InvalidOperationException("Geometry object is invalid.", ex));
                }
                inputs.Add("geometry",geometry);
                inputs.Add("geometryType", geometry.GeometryType);
                inputs.Add("spatialRel", relationship);
                inputs.Add("inSR", geometry.SpatialReference.WKID);
            }
                       
            Uri endpoint = GetUrl(string.Format("{0}/query",layer.ID ), inputs);
            return RestHelper.Hydrate<FeatureSetQuery>(endpoint.ToString());
        }

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
        /// <exception cref="System.NotImplementedException"></exception>
        public FeatureSetQuery Query(
                                     int layerId, 
                                     string query, 
                                     WKID outSR,
                                     bool returnGeometry = true, 
                                     int[] objectIds = null, 
                                     string outFields = "", 
                                     bool returnIdsOnly = false, 
                                     bool returnCountOnly = false)
        {
            return Query(layerId, query, outSR, esriSpatialRelationship.esriSpatialRelContains,
                null, returnGeometry, objectIds, outFields, returnIdsOnly, returnCountOnly);
        }

        /// <summary>
        /// Validates the geometry.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="geometry"/> is null
        /// or
        /// <see cref="P:Geocrest.Model.ArcGIS.Geometry.Geometry.SpatialReference"/> property on <paramref name="geometry"/> is null
        /// or
        /// <see cref="P:Geocrest.Model.ArcGIS.Geometry.SpatialReference.WKID"/> property on the <paramref name="geometry"/>
        /// <see cref="P:Geocrest.Model.ArcGIS.Geometry.Geometry.SpatialReference"/> property is null.</exception>
        public virtual void ValidateGeometry(Geometry geometry)
        {
            Throw.IfArgumentNull(geometry, "geometry");
            Throw.IfArgumentNull(geometry.SpatialReference, "SpatialReference");
            Throw.If<WKID>(geometry.SpatialReference.WKID,x => x == WKID.NotSpecified, "WKID");
            // test for empty geometry
            switch (geometry.GeometryType)
            {
                case esriGeometryType.esriGeometryPoint:
                    Throw.IfNullableIsNull<double>(geometry.X, "X");
                    Throw.IfNullableIsNull<double>(geometry.Y, "Y");
                    break;
                case esriGeometryType.esriGeometryEnvelope:
                    Throw.IfNullableIsNull<double>(geometry.XMin, "XMin");
                    Throw.IfNullableIsNull<double>(geometry.XMax, "XMax");
                    Throw.IfNullableIsNull<double>(geometry.YMin, "YMin");
                    Throw.IfNullableIsNull<double>(geometry.YMax, "YMax");
                    break;
                case esriGeometryType.esriGeometryPolyline:
                    Throw.IfArgumentNull(geometry.Paths, "Paths");
                    break;
                case esriGeometryType.esriGeometryPolygon:
                    Throw.IfArgumentNull(geometry.Rings, "Rings");
                    break;
            }
        }
        #endregion

        /// <summary>
        /// Gets the layer defs.
        /// </summary>
        /// <param name="layerDefs">The layer defs.</param>
        /// <returns></returns>
        private string GetLayerDefs(IDictionary<LayerTable, string> layerDefs = null)
        {
            if (layerDefs == null) return string.Empty;
            string value = string.Empty;
            foreach (var kvp in layerDefs)
                value += string.IsNullOrEmpty(value) ? ";" + kvp.Key.ID + ":" + kvp.Value : kvp.Key.ID + ":" + kvp.Value;
            return value;
        }

        /// <summary>
        /// Gets the layers.
        /// </summary>
        /// <param name="option">The option.</param>
        /// <param name="layers">The layers.</param>
        /// <returns></returns>
        private string GetLayers(IdentifyLayersOption option, int[] layers = null)
        {
            if (option == IdentifyLayersOption.Visible && layers == null) return IdentifyLayersOption.All.ToString();
            else if (option == IdentifyLayersOption.Visible && layers != null)
                return String.Format("visible:{0}", string.Join(",", layers));
            return option.ToString();
        }
    }
}
