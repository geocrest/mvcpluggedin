
namespace Geocrest.Model.ArcGIS.WebMap
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Represents read-only layer options used by an offline ArcGIS web map.
    /// </summary>
    [DataContract(Namespace = Geocrest.XmlNamespaces.ModelVersion1)]
    public class ReadonlyLayers
    {
        /// <summary>
        /// Gets or sets a value indicating whether to download attachments.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [download attachments]; otherwise, <c>false</c>.
        /// </value>
        [DataMember(Name = "downloadAttachments")]
        public bool DownloadAttachments { get; set; }
    }
}
