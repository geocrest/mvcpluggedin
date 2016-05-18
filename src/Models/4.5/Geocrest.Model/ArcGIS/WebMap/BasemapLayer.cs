
namespace Geocrest.Model.ArcGIS.WebMap
{
    using System.ComponentModel;
    using System.Runtime.Serialization;
    /// <summary>
    /// An individual base map layer within the web map's basemap.
    /// </summary>
    [DataContract(Namespace = Geocrest.XmlNamespaces.ModelVersion1)]
    public class BasemapLayer
    {
        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        /// <value>
        /// The URL.
        /// </value>
        [DataMember(Name = "url")]
        public string Url { get; set; }
        /// <summary>
        /// Gets or sets the opacity.
        /// </summary>
        /// <value>
        /// The opacity.
        /// </value>
        [DataMember(Name = "opacity")]
        [DefaultValue(1)]
        public double Opacity { get; set; }
    }
}