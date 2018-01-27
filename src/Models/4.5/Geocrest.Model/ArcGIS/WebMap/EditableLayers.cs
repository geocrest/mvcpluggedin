
namespace Geocrest.Model.ArcGIS.WebMap
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Represents offline editing options within an ArcGIS web map.
    /// </summary>
    [DataContract(Namespace = Geocrest.XmlNamespaces.ModelVersion1)]
    public class EditableLayers
    {
        /// <summary>
        /// Gets or sets a string value indicating which data is retrieved while edits are sent to the server.
        /// </summary>
        /// <value>
        /// Must be one of the following values: none, featuresAndAttachments, features.
        /// </value>
        [DataMember(Name = "download")]
        public string Download { get; set; }

        /// <summary>
        /// Gets or sets a string value indicating how data is synched.
        /// </summary>
        /// <value>
        /// Must be one of the following values: uploadFeaturesAndAttachments, syncFeaturesAndAttachments, syncFeaturesUploadAttachments.
        /// </value>
        [DataMember(Name = "sync")]
        public string Sync { get; set; }
    }
}
