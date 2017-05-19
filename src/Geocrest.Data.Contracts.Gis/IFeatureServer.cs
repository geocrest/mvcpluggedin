namespace Geocrest.Data.Contracts.Gis
{
    using Geocrest.Model.ArcGIS;
    using Model.ArcGIS.Geometry;
    using Model.ArcGIS.Tasks;

    /// <summary>
    /// Provides access to feature service operations
    /// </summary>
    public interface IFeatureServer : IArcGISService
    {
        /// <summary>
        /// Gets or sets the layers.
        /// </summary>
        /// <value>
        /// The layers.
        /// </value>
        LayerTableBase[] LayerInfos { get; set; }
        /// <summary>
        /// Gets or sets the table infos.
        /// </summary>
        /// <value>
        /// The table infos.
        /// </value>
        LayerTableBase[] TableInfos { get; set; }
        /// <summary>
        /// Executes a query on the specified layer and returns a featureset result.
        /// </summary>
        /// <param name="layer">The layer on which to perform the query.</param>
        /// <param name="where">The where clause used to query.</param>
        /// <param name="geometry">The geometry to apply as the spatial filter.</param>
        /// <param name="spatialRel">The spatial relationship to be applied on the input geometry while performing the query.</param>
        /// <param name="outSR">The spatial reference of the returned geometry.</param>
        /// <param name="outFields">The fields to include in the results.</param>
        /// <param name="objectIds">The specific object IDs to return.</param>
        /// <param name="orderByFields">The fields by which to order the results.</param>
        /// <param name="returnGeometry">If set to <c>true</c> the results will contain geometry.</param>
        /// <param name="returnDistinctValues">If set to <c>true</c> the results will contain distinct values.</param>
        /// <returns></returns>
        FeatureSet Query (
            LayerTableBase layer,
            string where,
            Geometry geometry = null,
            esriSpatialRelationship spatialRel = esriSpatialRelationship.esriSpatialRelIntersects,
            SpatialReference outSR = null,
            string outFields = "*",
            int[] objectIds = null,
            string orderByFields = "",
            bool returnGeometry = true,
            bool returnDistinctValues = false
            );
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
        int QueryForCount (
            LayerTableBase layer,
            string where,
            Geometry geometry = null,
            esriSpatialRelationship spatialRel = esriSpatialRelationship.esriSpatialRelIntersects,
            SpatialReference outSR = null
            );
        /// <summary>
        /// Executes a query on the specified layer and returns the IDs of features that match.
        /// </summary>
        /// <param name="layer">The layer on which to perform the query.</param>
        /// <param name="where">The where clause used to query.</param>
        /// <param name="geometry">The geometry to apply as the spatial filter.</param>
        /// <param name="spatialRel">The spatial relationship to be applied on the input geometry while performing the query.</param>
        /// <param name="outSR">The spatial reference of the returned geometry.</param>
        /// <returns></returns>
        FeatureSetQuery QueryForIds(
            LayerTableBase layer,
            string where,
            Geometry geometry = null,
            esriSpatialRelationship spatialRel = esriSpatialRelationship.esriSpatialRelIntersects,
            SpatialReference outSR = null
            );
    }
}
