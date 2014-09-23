namespace Geocrest.Model.ArcGIS
{
    using System.Runtime.Serialization;
    using Geocrest.Model.ArcGIS.Geometry;
    using Geocrest.Model.ArcGIS.Schema;

    /// <summary>
    /// Represents a single layer or table in a map document.
    /// </summary>
    [DataContract(Namespace = Geocrest.XmlNamespaces.ModelVersion1)]
    public class LayerTable : LayerTableBase
    {
        /// <summary>
        /// Gets or sets the current version.
        /// </summary>
        /// <value>
        /// The current version.
        /// </value>
        [DataMember]
        public double CurrentVersion { get; set; }        
        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        [DataMember]
        public string Type { get; set; }
        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        [DataMember]
        public string Description { get; set; }
        /// <summary>
        /// Gets or sets the definition expression.
        /// </summary>
        /// <value>
        /// The definition expression.
        /// </value>
        [DataMember]
        public string DefinitionExpression { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this instance has attachments.
        /// </summary>
        /// <value>
        /// <b>true</b>, if this instance has attachments; otherwise, <b>false</b>.
        /// </value>
        [DataMember]
        public bool HasAttachments { get; set; }
        /// <summary>
        /// Gets or sets the display field.
        /// </summary>
        /// <value>
        /// The display field.
        /// </value>
        [DataMember]
        public string DisplayField { get; set; }
        /// <summary>
        /// Gets or sets the type id field.
        /// </summary>
        /// <value>
        /// The type id field.
        /// </value>
        [DataMember]
        public string TypeIdField { get; set; }
        /// <summary>
        /// Gets or sets the fields.
        /// </summary>
        /// <value>
        /// The fields.
        /// </value>
        [DataMember]
        public Field[] Fields { get; set; }
        /// <summary>
        /// Gets or sets the types.
        /// </summary>
        /// <value>
        /// The types.
        /// </value>
        [DataMember]
        public Subtype[] Types { get; set; }
        /// <summary>
        /// Gets or sets the capabilities.
        /// </summary>
        /// <value>
        /// The capabilities.
        /// </value>
        [DataMember]
        public string Capabilities { get; set; }
        /// <summary>
        /// Gets or sets the relationships.
        /// </summary>
        /// <value>
        /// The relationships.
        /// </value>
        [DataMember]
        public Relationship[] Relationships { get; set; }

        #region Layers Only
        /// <summary>
        /// Gets or sets the type of the geometry.
        /// </summary>
        /// <value>
        /// The type of the geometry.
        /// </value>
        [DataMember]
        public esriGeometryType GeometryType { get; set; }
        /// <summary>
        /// Gets or sets the copyright text.
        /// </summary>
        /// <value>
        /// The copyright text.
        /// </value>
        [DataMember]
        public string CopyrightText { get; set; }
        /// <summary>
        /// Gets or sets the parent layer.
        /// </summary>
        /// <value>
        /// The parent layer.
        /// </value>
        [DataMember]
        public LayerTable ParentLayer { get; set; }
        /// <summary>
        /// Gets or sets the sub layers.
        /// </summary>
        /// <value>
        /// The sub layers.
        /// </value>
        [DataMember]
        public LayerTable[] SubLayers { get; set; }
        /// <summary>
        /// Gets or sets the min scale.
        /// </summary>
        /// <value>
        /// The min scale.
        /// </value>
        [DataMember]
        public double MinScale { get; set; }
        /// <summary>
        /// Gets or sets the max scale.
        /// </summary>
        /// <value>
        /// The max scale.
        /// </value>
        [DataMember]
        public double MaxScale { get; set; }
        /// <summary>
        /// Gets or sets the extent.
        /// </summary>
        /// <value>
        /// The extent.
        /// </value>
        [DataMember]
        public Geocrest.Model.ArcGIS.Geometry.Geometry Extent { get; set; }

        #endregion

        #region Feature Layers Only
        /// <summary>
        /// Gets or sets the drawing info.
        /// </summary>
        /// <value>
        /// The drawing info.
        /// </value>
        [DataMember]
        public DrawingInfo DrawingInfo { get; set; }
        #endregion

       
    }
}
