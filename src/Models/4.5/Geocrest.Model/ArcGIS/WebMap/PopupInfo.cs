
namespace Geocrest.Model.ArcGIS.WebMap
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Provides settings to define the look and feel of popup windows when a user clicks or queries a feature.
    /// </summary>
    [DataContract(Namespace = Geocrest.XmlNamespaces.ModelVersion1)]
    public class PopupInfo
    {
        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        [DataMember(Name = "description")]
        public object Description { get; set; }

        /// <summary>
        /// Gets or sets the field infos.
        /// </summary>
        /// <value>
        /// The field infos.
        /// </value>
        [DataMember(Name = "fieldInfos")]
        public FieldInfo[] FieldInfos { get; set; }

        /// <summary>
        /// Gets or sets the media infos.
        /// </summary>
        /// <value>
        /// The media infos.
        /// </value>
        [DataMember(Name = "mediaInfos")]
        public object[] MediaInfos { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [show attachments].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [show attachments]; otherwise, <c>false</c>.
        /// </value>
        [DataMember(Name = "showAttachments")]
        public bool ShowAttachments { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        [DataMember(Name = "title")]
        public string Title { get; set; }
    }
}
