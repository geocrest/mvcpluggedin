namespace Geocrest.Model.ArcGIS.Tasks
{
    using System.Runtime.Serialization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    /// <summary>
    /// Represents a message returns from a geoprocessing task.
    /// </summary>
    [DataContract(Namespace = Geocrest.XmlNamespaces.ModelVersion1)]
    public class GPMessage
    {
        /// <summary>
        /// Gets or sets the type of the message.
        /// </summary>
        /// <value>
        /// The type of the message.
        /// </value>
        [DataMember]
        public GPMessageType MessageType { get; set; }
        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        [DataMember]
        public string Description { get; set; }
    }

    /// <summary>
    /// An enumeration containing the types of messages returned from a geoprocessing task.
    /// </summary>
    [DataContract(Namespace = Geocrest.XmlNamespaces.ModelVersion1)]
    [JsonConverter(typeof(StringEnumConverter))]
    public enum GPMessageType
    {
        /// <summary>
        /// Informational message.
        /// </summary>
        [EnumMember]
        Informative,
        /// <summary>
        /// Message indicates the task has warning but was not in error.
        /// </summary>
        [EnumMember]
        Warning,
        /// <summary>
        /// An error has occurred and the task has been aborted.
        /// </summary>
        [EnumMember]
        Error,
        /// <summary>
        /// An empty message.
        /// </summary>
        [EnumMember]
        Empty,
        /// <summary>
        /// The task has been aborted.
        /// </summary>
        [EnumMember]
        Abort
    }
}
