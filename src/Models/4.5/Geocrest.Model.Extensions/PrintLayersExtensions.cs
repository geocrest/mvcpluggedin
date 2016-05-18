
namespace Geocrest.Model
{
    using System;
    using System.Drawing;
    using System.IO;
    using System.Net;
    using System.Text;
    using System.Web;
    using Geocrest.Model.ArcGIS.Tasks;

    /// <summary>
    /// Extensions methods for working with map printouts
    /// </summary>
   public static class PrintLayersExtensions
    {
        /// <summary>
        /// Gets the image.
        /// </summary>
        /// <param name="layer">The <see cref="T:Geocrest.Model.ArcGIS.Tasks.Layer" /> for
        /// which to obtain an image.</param>
        /// <param name="ctxuri">The <see cref="T:System.Uri" /> containing the scheme and authority for the request.</param>
        /// <param name="area">The print area parameter.</param>
        /// <param name="dpi">The dpi to use when generating the image.</param>
        /// <returns>
        /// Returns an <see cref="T:System.Drawing.Image" />.
        /// </returns>
        /// <exception cref="T:System.InvalidOperationException">Unable to retrieve image from the following url:  +
        ///                     url</exception>
        /// <exception cref="T:System.InvalidOperationException">If the image was unable to be retrieved.</exception>
        public static Image GetImage(this Layer layer,Uri ctxuri, PrintArea area, int dpi)
        {
            string url = "";
            try
            {
                WebClient client = new WebClient();
                url = layer.GetUrl(area, ctxuri, dpi);
                Uri uri = new Uri(url);
                byte[] data = client.DownloadData(uri);
                using (var sr = new StreamReader(new MemoryStream(data)))
                {
                    var s = sr.ReadToEnd();
                    var t = s;
                }
                return data.ToImage();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Unable to retrieve image from the following url: " +
                    url, ex);
            }
        }
        /// <summary>
        /// Returns an image from a byte array.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns>
        /// Returns an <see cref="T:System.Drawing.Image"/>
        /// </returns>
        public static Image ToImage(this byte[] data)
        {
            using (MemoryStream stream = new MemoryStream(data))
            {
                try
                {
                    return Bitmap.FromStream(stream);
                }
                catch (Exception ex)
                { return null; }
            }
        }
        /// <summary>
        /// Gets the image.
        /// </summary>
        /// <param name="layer">The <see cref="T:Geocrest.Model.ArcGIS.Tasks.Layer" /> for
        /// which to obtain an image.</param>
        /// <returns>
        /// Returns an <see cref="T:System.Drawing.Image"/>
        /// </returns>
        public static Image GetImage(this Layer layer)
        {
            byte[] data = Convert.FromBase64String(layer.ImageData);
            return data.ToImage();
        }

        /// <summary>
        /// Constructs a URL used to export a map image using the specified parameters.
        /// </summary>
        /// <param name="layer">An object of the type <see cref="T:Geocrest.Model.ArcGIS.Tasks.Layer" />.</param>
        /// <param name="area">Parameters used for generating the actual image.</param>
        /// <param name="ctxuri">A reference to the requesting url.</param>
        /// <param name="dpi">The dpi to use when generating the image.</param>
        /// <returns>
        /// Returns a URL that can be used to generate a map image.
        /// </returns>
        private static string GetUrl(this Layer layer,PrintArea area,Uri ctxuri, int dpi)
        {
            string url = layer.ServiceUrl;
            string format = layer.Format;
            string layers = layer.LayerIDs;
            bool useproxy = layer.UseProxy;            
            StringBuilder uri = new StringBuilder();
            if (useproxy)
            {
                uri.AppendFormat("{0}?", ctxuri.Scheme + "://" +
                    ctxuri.Authority + VirtualPathUtility.ToAbsolute("~/areas/mapimage/proxy.ashx"));
            }
            uri.AppendFormat("{0}/export?", url);
            uri.AppendFormat(System.Globalization.CultureInfo.InvariantCulture,
                "bbox={0},{1},{2},{3}", area.XMin, area.YMin, area.XMax, area.YMax);
            uri.AppendFormat("&size={0},{1}", area.Width, area.Height);
            uri.Append("&format=" + (!string.IsNullOrEmpty(format) ? format : "png32"));
            uri.AppendFormat("&dpi={0}", dpi);
            uri.Append("&transparent=true");
            if (!string.IsNullOrEmpty(layers))
                uri.AppendFormat("&layers=show:{0}", layers);
            if (area.SpatialReferenceID.HasValue)
                uri.AppendFormat("&imageSR={0}&bboxSR={0}", area.SpatialReferenceID);
            uri.Append("&f=image");
            return uri.ToString();
        }
    }
}
