
namespace Geocrest.Model.ArcGIS.WebMap
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Represents a field used in the layer definition of an ArcGIS web map.
    /// </summary>
    [DataContract(Namespace = Geocrest.XmlNamespaces.ModelVersion1)]
    public class Field
    {
        /// <summary>
        /// Gets or sets the alias.
        /// </summary>
        /// <value>
        /// The alias.
        /// </value>
        [DataMember(Name = "alias")]
        public string Alias { get; set; }

        /// <summary>
        /// Gets or sets the domain for this field.
        /// </summary>
        /// <value>
        /// The domain.
        /// </value>
        [DataMember(Name = "domain")]
        public Domain Domain { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Field"/> is editable.
        /// </summary>
        /// <value>
        ///   <c>true</c> if editable; otherwise, <c>false</c>.
        /// </value>
        [DataMember(Name = "editable")]
        public bool Editable { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not the field is an exact match.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [exact match]; otherwise, <c>false</c>.
        /// </value>
        [DataMember(Name = "exactMatch")]
        public bool ExactMatch { get; set; }

        /// <summary>
        /// Gets or sets the length.
        /// </summary>
        /// <value>
        /// The length.
        /// </value>
        [DataMember(Name = "length")]
        public int Length { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [DataMember(Name = "name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Field"/> is nullable.
        /// </summary>
        /// <value>
        ///   <c>true</c> if nullable; otherwise, <c>false</c>.
        /// </value>
        [DataMember(Name = "nullable")]
        public bool Nullable { get; set; }

        /// <summary>
        /// Gets or sets a string defining the field type.
        /// </summary>
        /// <value>
        /// Must be one of the following values: esriFieldTypeBlob, esriFieldTypeDate, esriFieldTypeDouble, esriFieldTypeGeometry, esriFieldTypeGlobalID, esriFieldTypeGUID, esriFieldTypeInteger, esriFieldTypeOID, esriFieldTypeRaster, esriFieldTypeSingle, esriFieldTypeSmallInteger, esriFieldTypeString, esriFieldTypeXML.
        /// </value>
        [DataMember(Name = "type")]
        public string Type { get; set; }
    }
}
