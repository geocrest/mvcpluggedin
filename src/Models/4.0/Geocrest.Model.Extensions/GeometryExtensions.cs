
namespace Geocrest.Model
{
    using System;
    using Geocrest.Model.ArcGIS.Geometry;
    /// <summary>
    /// Extension methods used with <see cref="T:Geocrest.Model.ArcGIS.Geometry.Geometry"/> objects.
    /// </summary>
    public static class GeometryExtensions
    {
        /// <summary>
        /// Constructs an envelope around an input point.
        /// </summary>
        /// <param name="point">A <see cref="T:Geocrest.Model.ArcGIS.Geometry.Geometry"/> point.</param>
        /// <param name="height">The desired height of the envelope.</param>
        /// <param name="width">The desired width of the envelope.</param>
        /// <returns>
        /// Returns an envelope as a <see cref="T:Geocrest.Model.ArcGIS.Geometry.Geometry"/> object.
        /// </returns>
        /// <exception cref="T:System.NotSupportedException">If the input is not a valid point.</exception>
        public static Geocrest.Model.ArcGIS.Geometry.Geometry ConstructEnvelope(this
            Geocrest.Model.ArcGIS.Geometry.Geometry point, double height, double width)
        {
            if (point.GeometryType == esriGeometryType.esriGeometryPoint && point.X.HasValue &&
                point.Y.HasValue)
            {
                Geocrest.Model.ArcGIS.Geometry.Geometry returnEnv = 
                    new Geocrest.Model.ArcGIS.Geometry.Geometry
                {
                    XMax = point.X.Value + (width / 2),
                    XMin = point.X.Value - (width / 2),
                    YMax = point.Y.Value + (height / 2),
                    YMin = point.Y.Value - (height / 2)
                };
                return returnEnv;
            }
            else throw new NotSupportedException("Only esriGeometryPoint is supported.");
        }
        /// <summary>
        /// Expands the specified envelope by the specified zoom factor. If the zoom factor is
        /// greater than 1, the envelope will increase in size; if the zoom factor is less than 1,
        /// the envelope will decrease in size. A zoom factor of 1 will return the same envelope.
        /// </summary>
        /// <param name="envelope">The <see cref="T:Geocrest.Model.ArcGIS.Geometry.Geometry"/> 
        /// envelope to expand.</param>
        /// <param name="zoom">The desired expansion level: if greater than 1, the envelope will
        /// zoom out; if less than 1, it will zoom in.</param>
        /// <returns>The expanded/contracted envelope.</returns>
        /// <exception cref="T:System.NotSupportedException">If the input geometry is not an envelope.</exception>
        public static Geocrest.Model.ArcGIS.Geometry.Geometry Expand(this 
            Geocrest.Model.ArcGIS.Geometry.Geometry envelope, double zoom)
        {
            //if not an envelope throw an error
            if (!(envelope.YMin.HasValue || envelope.XMin.HasValue || envelope.YMax.HasValue
                || envelope.XMax.HasValue))
            {
                throw new NotSupportedException("Geometry type must be an envelope.");
            }
            double width, height, xfactor, yfactor, addwidth, addheight;
            width = envelope.XMax.Value - envelope.XMin.Value;
            height = envelope.YMax.Value - envelope.YMin.Value;
            xfactor = width * zoom;
            yfactor = height * zoom;
            addwidth = xfactor - width;
            addheight = yfactor - height;
            return new Geocrest.Model.ArcGIS.Geometry.Geometry(envelope.XMin.Value - (addwidth / 2),
                envelope.YMin.Value - (addheight / 2), envelope.XMax.Value + (addwidth / 2),
                envelope.YMax.Value + (addheight / 2), envelope.SpatialReference);
        }
        /// <summary>
        /// Gets the extent of the geometry.
        /// </summary>
        /// <param name="geometry">A <see cref="T:Geocrest.Model.ArcGIS.Geometry.Geometry"/>
        /// containing a point, polyline, or a polygon.</param>
        /// <returns>
        /// Returns the extent of the <see cref="T:Geocrest.Model.ArcGIS.Geometry.Geometry"/>.
        /// </returns>
        /// <exception cref="T:System.NullReferenceException">If the output extent doesn't have valid values.</exception>
        /// <exception cref="T:System.NotSupportedException"><paramref name="geometry"/> is not a 
        /// point, polyline, polygon, or envelope.</exception>
        /// <remarks>Note that if the input is a point, the output will be an envelope constructed
        /// aroung the point width a width and height of 100 map units.</remarks>
        public static Geocrest.Model.ArcGIS.Geometry.Geometry GetExtent(this 
            Geocrest.Model.ArcGIS.Geometry.Geometry geometry)
        {
            double? xmin = null, ymin = null, xmax = null, ymax = null;
            if (geometry.Rings != null || geometry.Paths != null)
            {
                double[][][] array = geometry.Rings != null ? geometry.Rings : geometry.Paths;
                if (array != null)
                {
                    foreach (double[][] ring in array)
                    {
                        foreach (double[] path in ring)
                        {
                            if (!xmin.HasValue || path[0] < xmin) xmin = path[0];
                            if (!xmax.HasValue || path[0] > xmax) xmax = path[0];
                            if (!ymin.HasValue || path[1] < ymin) ymin = path[1];
                            if (!ymax.HasValue || path[1] > ymax) ymax = path[1];
                        }
                    }
                }
                if (!(xmin.HasValue || ymin.HasValue || xmax.HasValue || ymax.HasValue))
                    throw new NullReferenceException();
                return new Geocrest.Model.ArcGIS.Geometry.Geometry
                    (xmin.Value, ymin.Value, xmax.Value, ymax.Value, geometry.SpatialReference);
            }
            else if (geometry.GeometryType == esriGeometryType.esriGeometryPoint)
            {
                return geometry.ConstructEnvelope(100, 100);
            }
            else if (geometry.HasProperty("xmin") && geometry.HasProperty("ymin") && geometry.HasProperty("xmax")
                && geometry.HasProperty("ymax"))
            {
                return geometry;
            }
            else
                throw new NotSupportedException("Invalid geometry type");
        }
    }
}
