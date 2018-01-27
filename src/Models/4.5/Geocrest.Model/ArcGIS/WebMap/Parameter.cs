
namespace Geocrest.Model.ArcGIS.WebMap
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Represents a parameter used as input for a definition editor.
    /// </summary>
    public class Parameter
    {
        /// <summary>
        /// Gets or sets the default value.
        /// </summary>
        /// <value>
        /// The default value.
        /// </value>
        [DataMember(Name = "defaultValue")]
        public object DefaultValue { get; set; }

        /// <summary>
        /// Gets or sets the name of the field.
        /// </summary>
        /// <value>
        /// The name of the field.
        /// </value>
        [DataMember(Name = "fieldName")]
        public string FieldName { get; set; }

        /// <summary>
        /// Gets or sets the parameter identifier.
        /// </summary>
        /// <value>
        /// The parameter identifier.
        /// </value>
        [DataMember(Name = "parameterId")]
        public int ParameterId { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        [DataMember(Name = "type")]
        public string Type { get; set; }
    }
}
