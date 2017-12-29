
namespace Geocrest.Model.ArcGIS.WebMap
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Represents a layer defining styling, geometry, and attribute information for features used in an ArcGIS web map.
    /// </summary>
    [DataContract(Namespace = Geocrest.XmlNamespaces.ModelVersion1)]
    public class Layer
    {
        /// <summary>
        /// Gets or sets the definition editor.
        /// </summary>
        /// <value>
        /// The definition editor.
        /// </value>
        [DataMember(Name = "definitionEditor")]
        public DefinitionEditor DefinitionEditor { get; set; }

        /// <summary>
        /// Gets or sets the field.
        /// </summary>
        /// <value>
        /// The field.
        /// </value>
        [DataMember(Name = "field")]
        public Field Field { get; set; }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        [DataMember(Name = "id")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets a popupInfo object defining the popup window content for the layer.
        /// </summary>
        /// <value>
        /// The popup information.
        /// </value>
        [DataMember(Name = "popupInfo")]
        public PopupInfo PopupInfo { get; set; }

        /// <summary>
        /// Gets or sets the sub layer.
        /// </summary>
        /// <value>
        /// The sub layer.
        /// </value>
        [DataMember(Name = "subLayer")]
        public int SubLayer { get; set; }
    }
}
