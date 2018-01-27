
namespace Geocrest.Model.ArcGIS.WebMap
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Represents settings that define how a field in the dataset participates (or does not participate) in a popup window.
    /// </summary>
    [DataContract(Namespace = Geocrest.XmlNamespaces.ModelVersion1)]
    public class FieldInfo
    {
        /// <summary>
        /// Gets or sets the name of the field.
        /// </summary>
        /// <value>
        /// The name of the field.
        /// </value>
        [DataMember(Name = "fieldName")]
        public string FieldName { get; set; }

        /// <summary>
        /// Gets or sets the format.
        /// </summary>
        /// <value>
        /// The format.
        /// </value>
        [DataMember(Name = "format")]
        public Format Format { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is editable.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is editable; otherwise, <c>false</c>.
        /// </value>
        [DataMember(Name = "isEditable")]
        public bool IsEditable { get; set; }

        /// <summary>
        /// Gets or sets the label.
        /// </summary>
        /// <value>
        /// The label.
        /// </value>
        [DataMember(Name = "label")]
        public string Label { get; set; }

        /// <summary>
        /// Gets or sets the string field option.
        /// </summary>
        /// <value>
        /// The string field option.
        /// </value>
        [DataMember(Name = "stringFieldOption")]
        public string StringFieldOption { get; set; }

        /// <summary>
        /// Gets or sets the tooltip.
        /// </summary>
        /// <value>
        /// The tooltip.
        /// </value>
        [DataMember(Name = "tooltip")]
        public string Tooltip { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="FieldInfo"/> is visible.
        /// </summary>
        /// <value>
        ///   <c>true</c> if visible; otherwise, <c>false</c>.
        /// </value>
        [DataMember(Name = "visible")]
        public bool Visible { get; set; }
    }
}
