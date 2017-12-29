
namespace Geocrest.Model.ArcGIS.WebMap
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Represents the spatial reference of an ArcGIS web map.
    /// </summary>
    [DataContract(Namespace = Geocrest.XmlNamespaces.ModelVersion1)]
    public class SpatialReference
    {
        /// <summary>
        /// Gets or sets the latest wkid.
        /// </summary>
        /// <value>
        /// The latest wkid.
        /// </value>
        [DataMember(Name = "latestWkid")]
        public int LatestWkid { get; set; }

        /// <summary>
        /// Gets or sets the wkid.
        /// </summary>
        /// <value>
        /// The wkid.
        /// </value>
        [DataMember(Name = "wkid")]
        public int Wkid { get; set; }
    }
}
