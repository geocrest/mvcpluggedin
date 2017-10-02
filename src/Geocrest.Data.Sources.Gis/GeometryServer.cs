namespace Geocrest.Data.Sources.Gis
{
    using System;
    using System.Collections.Generic;
    using Geocrest.Data.Contracts.Gis;
    using Geocrest.Data.Sources.Gis.Constants;
    using Geocrest.Web.Infrastructure;
    using Geocrest.Model.ArcGIS.Geometry;

    /// <summary>
    /// Represents an ArcGIS Server geometry service.
    /// </summary>
    /// <remarks>
    /// Note that this class does not require a <see cref="T:System.Runtime.Serialization.DataContractAttribute"/> decoration 
    /// because it has no properties to serialize.
    /// </remarks>
    public class GeometryServer : ArcGISService, IGeometryServer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Data.Sources.Gis.GeometryServer"/> class.
        /// </summary>
        internal GeometryServer() { }

        #region IGeometryServer Members
        /// <summary>
        /// Projects the input geometries to a new spatial reference.
        /// </summary>
        /// <param name="geometries">The array of geometries to be projected.</param>
        /// <param name="inSR">The well-known ID of the spatial reference for the input geometries.</param>
        /// <param name="outSR">The well-known ID of the spatial reference for the returned geometries.</param>
        public GeometryCollection Project(GeometryCollection geometries, WKID inSR, WKID outSR)
        {
            Throw.IfArgumentNull(geometries, "geometries");
            Throw.IfArgumentNull(inSR, "inSR");
            Throw.IfArgumentNull(outSR, "outSR");

            IDictionary<string, object> inputs = new Dictionary<string, object>
                {
                    { "geometries", geometries },
                    { "inSR", inSR },
                    { "outSR", outSR }
                };
            Uri endpoint = GetUrl(GEOMETRY.Project, inputs);
            return Geocrest.Model.RestHelper.HydrateObject<GeometryCollection>(endpoint.ToString());
        }

        /// <summary>
        /// Projects the input geometries to a new spatial reference.
        /// </summary>
        /// <param name="geometries">The array of geometries to be projected.</param>
        /// <param name="inSR">The well-known ID of the spatial reference for the input geometries.</param>
        /// <param name="outSR">The well-known ID of the spatial reference for the returned geometries.</param>
        /// <param name="callback">The callback function used to retrieve the results.</param>
        /// <exception cref="T:System.NotImplementedException">This method is currently not implemented.</exception>
        public void ProjectAsync(GeometryCollection geometries, WKID inSR, WKID outSR, Action<GeometryCollection> callback)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
