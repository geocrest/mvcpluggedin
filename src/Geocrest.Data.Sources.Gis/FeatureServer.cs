namespace Geocrest.Data.Sources.Gis
{
    using Geocrest.Data.Contracts.Gis;
    using Geocrest.Model.ArcGIS;
    using Model.ArcGIS.Geometry;
    using Model.ArcGIS.Tasks;
    using System;
    using System.Collections.Generic;
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
        /// Executes a query on the specified layer and returns a featureset result.
        /// </summary>
        /// <param name="layer">The layer on which to perform the query.</param>
        /// <param name="where">The where clause used to query.</param>
        /// <param name="outFields">The fields to include in the results.</param>
        /// <param name="geometry">The geometry to apply as the spatial filter.</param>
        /// <param name="spatialRel">The spatial relationship to be applied on the input geometry while performing the query.</param>
        /// <param name="outSR">The spatial reference of the returned geometry.</param>
        /// <param name="objectIds">The specific object IDs to return.</param>
        /// <param name="orderByFields">The fields by which to order the results.</param>
        /// <param name="returnGeometry">If set to <c>true</c> the results will contain geometry.</param>
        /// <param name="returnDistinctValues">If set to <c>true</c> the results will contain distinct values.</param>
        /// <returns></returns>
        public FeatureSet Query(LayerTableBase layer,
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
            Throw.IfArgumentNull(layer, "layer");
            IDictionary<string, object> inputs = new Dictionary<string, object>()
            {
                { "geometryType", geometry != null ? geometry.GeometryType.ToString() : "" },
                { "geometry", geometry},
                { "inSR", geometry != null ? geometry.SpatialReference.ToString() : ""},
                { "outSR", outSR },
                { "where", where},
                { "objectIds", objectIds != null ? string.Join(",", objectIds) : null},
                { "returnGeometry", returnGeometry },
                { "outFields", outFields },
                { "orderByFields", orderByFields },
                { "returnDistinctValues", returnDistinctValues }
            };
            Uri endpoint = GetUrl("query", inputs);
            Uri baseUri = new Uri(this.Url);
            return RestHelper.Hydrate<FeatureSet>(endpoint.ToString().Replace(endpoint.AbsolutePath, baseUri.AbsolutePath + "/" + layer.ID + "/query"));
        }

        /// <summary>
        /// Executes a query on the specified layer and returns the count of features that match.
        /// </summary>
        /// <param name="layer">The layer on which to perform the query.</param>
        /// <param name="where">The where clause used to query.</param>
        /// <param name="geometry">The geometry to apply as the spatial filter.</param>
        /// <param name="spatialRel">The spatial relationship to be applied on the input geometry while performing the query.</param>
        /// <param name="outSR">The spatial reference of the returned geometry.</param>
        /// <returns>
        /// The count of features that match the query.
        /// </returns>
        public int QueryForCount(
            LayerTableBase layer, 
            string where,
            Geometry geometry = null,
            esriSpatialRelationship spatialRel = esriSpatialRelationship.esriSpatialRelIntersects,
            SpatialReference outSR = null)
        {
            Throw.IfArgumentNull(layer, "layer");
            IDictionary<string, object> inputs = new Dictionary<string, object>()
            {
                { "geometryType", geometry != null ? geometry.GeometryType.ToString() : "" },
                { "geometry", geometry},
                { "inSR", geometry != null ? geometry.SpatialReference.ToString() : ""},
                { "outSR", outSR },
                { "where", where},                
                { "returnGeometry", false },
                { "returnCountOnly", true }
            };
            Uri endpoint = GetUrl("query", inputs);
            Uri baseUri = new Uri(this.Url);
            var response = RestHelper.Hydrate<FeatureSetQuery>(endpoint.ToString().Replace(endpoint.AbsolutePath, baseUri.AbsolutePath + "/" + layer.ID + "/query"));
            return response.Count;
        }

        /// <summary>
        /// Executes a query on the specified layer and returns the IDs of features that match.
        /// </summary>
        /// <param name="layer">The layer on which to perform the query.</param>
        /// <param name="where">The where clause used to query.</param>
        /// <param name="geometry">The geometry to apply as the spatial filter.</param>
        /// <param name="spatialRel">The spatial relationship to be applied on the input geometry while performing the query.</param>
        /// <param name="outSR">The spatial reference of the returned geometry.</param>
        /// <returns></returns>
        public FeatureSetQuery QueryForIds(
            LayerTableBase layer, 
            string where,
            Geometry geometry = null,
            esriSpatialRelationship spatialRel = esriSpatialRelationship.esriSpatialRelIntersects,
            SpatialReference outSR = null
            )
        {
            Throw.IfArgumentNull(layer, "layer");
            IDictionary<string, object> inputs = new Dictionary<string, object>()
            {
                { "geometryType", geometry != null ? geometry.GeometryType.ToString() : "" },
                { "geometry", geometry},
                { "inSR", geometry != null ? geometry.SpatialReference.ToString() : ""},
                { "outSR", outSR },
                { "where", where},
                { "returnGeometry", false },
                { "returnCountOnly", false },
                { "returnIdsOnly", true }
            };
            Uri endpoint = GetUrl("query", inputs);
            Uri baseUri = new Uri(this.Url);
            return RestHelper.Hydrate<FeatureSetQuery>(endpoint.ToString().Replace(endpoint.AbsolutePath, baseUri.AbsolutePath + "/" + layer.ID + "/query"));            
        }



        #endregion
    }
}
