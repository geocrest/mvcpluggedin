
namespace Geocrest.Model.ArcGIS.WebMap
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Represents the viewing properties of an ArcGIS web map.
    /// </summary>
    [DataContract(Namespace = Geocrest.XmlNamespaces.ModelVersion1)]
    public class Viewing
    {
        /// <summary>
        /// Gets or sets the routing.
        /// </summary>
        /// <value>
        /// The routing.
        /// </value>
        [DataMember(Name = "routing")]
        public Routing Routing { get; set; }

        /// <summary>
        /// Gets or sets the basemap gallery.
        /// </summary>
        /// <value>
        /// The basemap gallery.
        /// </value>
        [DataMember(Name = "basemapGallery")]
        public BasemapGallery BasemapGallery { get; set; }

        /// <summary>
        /// Gets or sets the measure.
        /// </summary>
        /// <value>
        /// The measure.
        /// </value>
        [DataMember(Name = "measure")]
        public Measure Measure { get; set; }

        /// <summary>
        /// Gets or sets the search.
        /// </summary>
        /// <value>
        /// The search.
        /// </value>
        [DataMember(Name = "search")]
        public Search Search { get; set; }
    }
}
