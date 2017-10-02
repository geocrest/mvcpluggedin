namespace Geocrest.Data.Sources.Gis
{
    using Geocrest.Data.Contracts.Gis;
    using Geocrest.Model.ArcGIS;
    using Model.ArcGIS.Geometry;
    using Model.ArcGIS.Tasks;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Runtime.Serialization;
    using Web.Infrastructure;

    /// <summary>
    /// Represents an ArcGIS Server feature service for editing features.
    /// </summary>
    [DataContract(Namespace = Geocrest.XmlNamespaces.InfrastructureVersion1)]
    public class FeatureServer : ArcGISService, IFeatureServer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Data.Sources.Gis.FeatureServer"/> class.
        /// </summary>
        internal FeatureServer() { }

        #region IFeatureServer Members
        /// <summary>
        /// Gets or sets the layers.
        /// </summary>
        /// <value>
        /// The layers.
        /// </value>
        [DataMember(Name="layers")]
        public LayerTableBase[] LayerInfos { get; set; }

        /// <summary>
        /// Gets or sets the table infos.
        /// </summary>
        /// <value>
        /// The table infos.
        /// </value>
        [DataMember(Name = "tables")]
        public LayerTableBase[] TableInfos { get; set; }

        /// <summary>
        /// This operation adds, updates, and deletes features to the specified feature layer.
        /// </summary>
        /// <param name="layerId">The ID of the layer to query.</param>
        /// <param name="adds">The array of features to be added.</param>
        /// <param name="updates">The array of features to be updated. The attributes property of
        /// each feature should include the object ID of the feature.</param>
        /// <param name="deletes">The array of object IDs to be deleted.</param>
        /// <returns></returns>
        public ApplyEditsResult ApplyEdits(int layerId, Feature[] adds = null, Feature[] updates = null, long[] deletes = null)
        {
            LayerTableBase layer = this.LayerInfos.Single(x => x.ID == layerId);
            if (layer == null) Throw.ArgumentOutOfRange("layerId");
            IDictionary<string, object> inputs = new Dictionary<string, object>()
            {                
            };
            Uri endpoint = GetUrl("applyEdits", inputs, layer);
            var nvc = new NameValueCollection();
            nvc.Add("adds", JsonConvert.SerializeObject(adds) ?? "[]");
            nvc.Add("updates", JsonConvert.SerializeObject(updates) ?? "[]");
            nvc.Add("deletes", JsonConvert.SerializeObject(deletes) ?? "[]");
            return Geocrest.Model.RestHelper.HydrateObject<ApplyEditsResult>(endpoint.ToString(), nvc);
        }

        /// <summary>
        /// Executes a query on the specified layer and returns a featureset result.
        /// </summary>
        /// <param name="layerId">The ID of the layer to query.</param>
        /// <param name="where">The where clause used to query. Any legal SQL where clause operating on the fields in the layer is allowed.</param>
        /// <param name="geometry">The geometry to apply as the spatial filter.</param>
        /// <param name="spatialRel">The spatial relationship to be applied on the input geometry while performing the query.</param>
        /// <param name="outSR">The spatial reference of the returned geometry.</param>
        /// <param name="outFields">The list of fields to be included in the returned result set.</param>
        /// <param name="objectIds">The object IDs of this layer/table to be queried.</param>
        /// <param name="orderByFields">The fields by which to order the results.</param>
        /// <param name="returnGeometry">If set to <c>true</c> the result includes the geometry associated with each feature returned.</param>
        /// <param name="returnDistinctValues">If set to <c>true</c> it returns distinct values based on the fields specified in outFields.</param>
        /// <returns></returns>
        public FeatureSet Query(int layerId,
            string where,
            Geometry geometry = null,
            esriSpatialRelationship spatialRel = esriSpatialRelationship.esriSpatialRelIntersects,
            SpatialReference outSR = null,
            string outFields = "*",
            int[] objectIds = null,
            string orderByFields = "",
            bool returnGeometry = true,
            bool returnDistinctValues = false)
        {
            LayerTableBase layer = this.LayerInfos.Single(x => x.ID == layerId);
            if (layer == null) Throw.ArgumentOutOfRange("layerId");
            IDictionary<string, object> inputs = new Dictionary<string, object>()
            {                           
                { "where", where },
                { "objectIds", objectIds != null ? string.Join(",", objectIds) : null},
                { "returnGeometry", returnGeometry },
                { "outFields", outFields },
                { "orderByFields", orderByFields },
                { "returnDistinctValues", returnDistinctValues }
            };

            if (outSR != null && outSR.WKID != WKID.NotSpecified) inputs.Add("outSR", outSR);
            if (geometry != null)
            {
                try
                {
                    ValidateGeometry(geometry);
                }
                catch (ArgumentNullException ex)
                {
                    Throw.InvalidOperation("", x => new InvalidOperationException("Geometry object is invalid.", ex));
                }
                inputs.Add("geometry", geometry);
                inputs.Add("geometryType", geometry.GeometryType);
                inputs.Add("spatialRel", spatialRel);
                inputs.Add("inSR", geometry.SpatialReference);
            }

            Uri endpoint = GetUrl("query", inputs, layer);
            return Geocrest.Model.RestHelper.HydrateObject<FeatureSet>(endpoint.ToString());
        }

        /// <summary>
        /// Executes a query on the specified layer and returns the count of features that match.
        /// </summary>
        /// <param name="layerId">The ID of the layer to query.</param>
        /// <param name="where">The where clause used to query. Any legal SQL where clause operating on the fields in the layer is allowed.</param>
        /// <param name="geometry">The geometry to apply as the spatial filter.</param>
        /// <param name="spatialRel">The spatial relationship to be applied on the input geometry while performing the query.</param>
        /// <param name="outSR">The spatial reference of the returned geometry.</param>
        /// <returns>
        /// The count of features that match the query.
        /// </returns>
        public int QueryForCount(
            int layerId, 
            string where,
            Geometry geometry = null,
            esriSpatialRelationship spatialRel = esriSpatialRelationship.esriSpatialRelIntersects,
            SpatialReference outSR = null)
        {
            LayerTableBase layer = this.LayerInfos.Single(x => x.ID == layerId);
            if (layer == null) Throw.ArgumentOutOfRange("layerId");
            IDictionary<string, object> inputs = new Dictionary<string, object>()
            {              
                { "where", where},                
                { "returnGeometry", false },
                { "returnCountOnly", true }
            };

            if (outSR != null && outSR.WKID != WKID.NotSpecified) inputs.Add("outSR", outSR);
            if (geometry != null)
            {
                try
                {
                    ValidateGeometry(geometry);
                }
                catch (ArgumentNullException ex)
                {
                    Throw.InvalidOperation("", x => new InvalidOperationException("Geometry object is invalid.", ex));
                }
                inputs.Add("geometry", geometry);
                inputs.Add("geometryType", geometry.GeometryType);
                inputs.Add("spatialRel", spatialRel);
                inputs.Add("inSR", geometry.SpatialReference);
            }
            Uri endpoint = GetUrl("query", inputs, layer);
            var response = Geocrest.Model.RestHelper.HydrateObject<FeatureSetQuery>(endpoint.ToString());
            return response.Count;
        }

        /// <summary>
        /// Executes a query on the specified layer and returns the IDs of features that match.
        /// </summary>
        /// <param name="layerId">The ID of the layer to query.</param>
        /// <param name="where">The where clause used to query. Any legal SQL where clause operating on the fields in the layer is allowed.</param>
        /// <param name="geometry">The geometry to apply as the spatial filter.</param>
        /// <param name="spatialRel">The spatial relationship to be applied on the input geometry while performing the query.</param>
        /// <param name="outSR">The spatial reference of the returned geometry.</param>
        /// <returns></returns>
        public FeatureSetQuery QueryForIds(
            int layerId, 
            string where,
            Geometry geometry = null,
            esriSpatialRelationship spatialRel = esriSpatialRelationship.esriSpatialRelIntersects,
            SpatialReference outSR = null
            )
        {
            LayerTableBase layer = this.LayerInfos.Single(x => x.ID == layerId);
            if (layer == null) Throw.ArgumentOutOfRange("layerId");
            IDictionary<string, object> inputs = new Dictionary<string, object>()
            {             
                { "where", where},
                { "returnGeometry", false },
                { "returnCountOnly", false },
                { "returnIdsOnly", true }
            };

            if (outSR != null && outSR.WKID != WKID.NotSpecified) inputs.Add("outSR", outSR);
            if (geometry != null)
            {
                try
                {
                    ValidateGeometry(geometry);
                }
                catch (ArgumentNullException ex)
                {
                    Throw.InvalidOperation("", x => new InvalidOperationException("Geometry object is invalid.", ex));
                }
                inputs.Add("geometry", geometry);
                inputs.Add("geometryType", geometry.GeometryType);
                inputs.Add("spatialRel", spatialRel);
                inputs.Add("inSR", geometry.SpatialReference);
            }
            Uri endpoint = GetUrl("query", inputs, layer);
            return Geocrest.Model.RestHelper.HydrateObject<FeatureSetQuery>(endpoint.ToString());            
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
            Throw.If<WKID>(geometry.SpatialReference.WKID, x => x == WKID.NotSpecified, "WKID");
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
    }
}
