namespace Geocrest.Model.ArcGIS.Tasks
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using Newtonsoft.Json;
    using Geocrest.Model.ArcGIS.Geometry;

    /// <summary>
    /// A single identify result.
    /// </summary>
    [DataContract(Namespace = Geocrest.XmlNamespaces.ModelVersion1)]
    public class IdentifyResult
    {       
        /// <summary>
        /// Gets or sets the field to use for the display name.
        /// </summary>
        /// <value>
        /// The field to use for the display name.
        /// </value>
        [DataMember]
        public string DisplayFieldName { get; set; }

        /// <summary>
        /// Gets or sets the layer ID.
        /// </summary>
        /// <value>
        /// The layer ID.
        /// </value>
        [DataMember]
        public int LayerID { get; set; }

        /// <summary>
        /// Gets or sets the name of the layer.
        /// </summary>
        /// <value>
        /// The name of the layer.
        /// </value>
        [DataMember]
        public string LayerName { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        [DataMember]
        public string Value { get; set; }

        /// <summary>
        /// Gets or sets the attributes.
        /// </summary>
        /// <value>
        /// The attributes.
        /// </value>
        [DataMember]
        public IDictionary<string, object> Attributes { get; set; }

        /// <summary>
        /// Gets or sets the type of the geometry.
        /// </summary>
        /// <value>
        /// The type of the geometry.
        /// </value>
        [DataMember(EmitDefaultValue=false)]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string GeometryType { get; set; }

        /// <summary>
        /// Gets or sets the geometry.
        /// </summary>
        /// <value>
        /// The geometry.
        /// </value>
        [DataMember(EmitDefaultValue = false)]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Geometry Geometry { get; set; }
    }
}
