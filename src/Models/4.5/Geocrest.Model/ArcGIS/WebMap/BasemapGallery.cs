
namespace Geocrest.Model.ArcGIS.WebMap
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Provides a setting that determines whether the base map gallery is displayed in the web map.
    /// </summary>
    [DataContract(Namespace = Geocrest.XmlNamespaces.ModelVersion1)]
    public class BasemapGallery
    {
        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="BasemapGallery"/> is enabled.
        /// </summary>
        /// <value>
        ///   <c>true</c> if enabled; otherwise, <c>false</c>.
        /// </value>
        [DataMember(Name = "enabled")]
        public bool Enabled { get; set; }
    }
}
