namespace Geocrest.Model.ArcGIS.Schema
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Represents a field object inside a feature.
    /// </summary>
    [DataContract(Namespace = Geocrest.XmlNamespaces.ModelVersion1)]
    public class Field
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [DataMember(Name = "name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        [DataMember(Name = "type")]
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the alias.
        /// </summary>
        /// <value>
        /// The alias.
        /// </value>
        [DataMember(Name = "alias")]
        public string Alias { get; set; }

        /// <summary>
        /// Gets or sets the length.
        /// </summary>
        /// <value>
        /// The length.
        /// </value>
        [DataMember(Name = "length")]
        public int Length { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:Geocrest.Model.ArcGIS.Schema.Field"/> is required.
        /// </summary>
        /// <value>
        /// <b>true</b>, if required; otherwise, <b>false</b>.
        /// </value>
        [DataMember]
        public bool Required { get; set; }

        /// <summary>
        /// Gets or sets the domain for this field.
        /// </summary>
        /// <value>
        /// The domain.
        /// </value>
        [DataMember]
        public Domain Domain { get; set; }
    }  
}
