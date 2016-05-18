namespace Geocrest.Model.ArcGIS
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Provides information about the map document used to configure a map server.
    /// </summary>
    [DataContract(Namespace = Geocrest.XmlNamespaces.ModelVersion1)]
    public class DocumentInfo
    {
        /// <summary>
        /// Gets or sets the antialiasing mode.
        /// </summary>
        /// <value>
        /// The antialiasing mode.
        /// </value>
        [DataMember]
        public string AntialiasingMode { get; set; }

        /// <summary>
        /// Gets or sets the author of the map.
        /// </summary>
        /// <value>
        /// The author.
        /// </value>
        [DataMember]
        public string Author { get; set; }

        /// <summary>
        /// Gets or sets the category.
        /// </summary>
        /// <value>
        /// The category.
        /// </value>
        [DataMember]
        public string Category { get; set; }
        
        /// <summary>
        /// Gets or sets the comments about the map.
        /// </summary>
        /// <value>
        /// The comments.
        /// </value>
        [DataMember]
        public string Comments { get; set; }

        /// <summary>
        /// Gets or sets the keywords.
        /// </summary>
        /// <value>
        /// The keywords.
        /// </value>
        [DataMember]
        public string Keywords { get; set; }
       
        /// <summary>
        /// Gets or sets the subject.
        /// </summary>
        /// <value>
        /// The subject.
        /// </value>
        [DataMember]
        public string Subject { get; set; }

        /// <summary>
        /// Gets or sets the text antialiasing mode.
        /// </summary>
        /// <value>
        /// The text antialiasing mode.
        /// </value>
        [DataMember]
        public string TextAntialiasingMode { get; set; }

        /// <summary>
        /// Gets or sets the title of the map.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        [DataMember]
        public string Title { get; set; }
    }
}
