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
        FeatureSet Query (
            int layerId,
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
        /// <param name="layerId">The ID of the layer to query.</param>
        /// <param name="where">The where clause used to query. Any legal SQL where clause operating on the fields in the layer is allowed.</param>
        /// <param name="geometry">The geometry to apply as the spatial filter.</param>
        /// <param name="spatialRel">The spatial relationship to be applied on the input geometry while performing the query.</param>
        /// <param name="outSR">The spatial reference of the returned geometry.</param>
        /// <returns>
        /// The count of features that match the query.
        /// </returns>
        int QueryForCount (
            int layerId,
            string where,
            Geometry geometry = null,
            esriSpatialRelationship spatialRel = esriSpatialRelationship.esriSpatialRelIntersects,
            SpatialReference outSR = null
            );
        /// <summary>
        /// Executes a query on the specified layer and returns the IDs of features that match.
        /// </summary>
        /// <param name="layerId">The ID of the layer to query.</param>
        /// <param name="where">The where clause used to query. Any legal SQL where clause operating on the fields in the layer is allowed.</param>
        /// <param name="geometry">The geometry to apply as the spatial filter.</param>
        /// <param name="spatialRel">The spatial relationship to be applied on the input geometry while performing the query.</param>
        /// <param name="outSR">The spatial reference of the returned geometry.</param>
        /// <returns></returns>
        FeatureSetQuery QueryForIds(
            int layerId,
            string where,
            Geometry geometry = null,
            esriSpatialRelationship spatialRel = esriSpatialRelationship.esriSpatialRelIntersects,
            SpatialReference outSR = null
            );
        /// <summary>
        /// This operation adds, updates, and deletes features to the specified feature layer.
        /// </summary>
        /// <param name="layerId">The ID of the layer to query.</param>
        /// <param name="adds"> The array of features to be added.</param>
        /// <param name="updates">The array of features to be updated. The attributes property of 
        /// each feature should include the object ID of the feature.</param>
        /// <param name="deletes">The array of object IDs to be deleted.</param>
        /// <returns></returns>
        ApplyEditsResult ApplyEdits(
            int layerId,
            Feature[] adds = null,
            Feature[] updates = null,
            long[] deletes = null);
    }
}
