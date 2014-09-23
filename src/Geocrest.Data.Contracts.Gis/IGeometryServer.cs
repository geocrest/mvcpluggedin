namespace Geocrest.Data.Contracts.Gis
{
    using System;
    using Geocrest.Model.ArcGIS.Geometry;
    /// <summary>
    /// Provides access to geometry server operations.
    /// </summary>
    public interface IGeometryServer:IArcGISService
    {
        /// <summary>
        /// Projects the input geometries to a new spatial reference.
        /// </summary>
        /// <param name="geometries">The array of geometries to be projected.</param>
        /// <param name="inSR">The well-known ID of the spatial reference for the input geometries.</param>
        /// <param name="outSR">The well-known ID of the spatial reference for the returned geometries.</param>
        /// <returns></returns>
        GeometryCollection Project(GeometryCollection geometries, WKID inSR, WKID outSR);

        /// <summary>
        /// Projects the input geometries to a new spatial reference.
        /// </summary>
        /// <param name="geometries">The array of geometries to be projected.</param>
        /// <param name="inSR">The well-known ID of the spatial reference for the input geometries.</param>
        /// <param name="outSR">The well-known ID of the spatial reference for the returned geometries.</param>
        /// <param name="callback">The callback function used to retrieve the results.</param>
        void ProjectAsync(GeometryCollection geometries, WKID inSR, WKID outSR, Action<GeometryCollection> callback);
    }
}
