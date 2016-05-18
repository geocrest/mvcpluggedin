namespace Geocrest.Model.ArcGIS
{
    using System.Runtime.Serialization;
    using Geocrest.Model.ArcGIS.Geometry;
    using Geocrest.Model.ArcGIS.Schema;

    /// <summary>
    /// This class represents a single layer within an ArcGIS Mobile Server.
    /// </summary>
    [DataContract(Namespace = Geocrest.XmlNamespaces.ModelVersion1)]
    public class MobileLayer :LayerTableBase
    {
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
        /// Gets or sets the display field.
        /// </summary>
        /// <value>
        /// The display field.
        /// </value>
        [DataMember]
        public string DisplayField { get; set; }
        /// <summary>
        /// Gets or sets the fields.
        /// </summary>
        /// <value>
        /// The fields.
        /// </value>
        [DataMember]
        public Field[] Fields { get; set; }
        /// <summary>
        /// Gets or sets the type of the geometry.
        /// </summary>
        /// <value>
        /// The type of the geometry.
        /// </value>
        [DataMember]
        public esriGeometryType GeometryType { get; set; }
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
        /// <summary>
        /// Gets or sets the type of data.
        /// </summary>
        /// <value>
        /// The type of data.
        /// </value>
        [DataMember]
        public string DataType { get; set; }
        /// <summary>
        /// Gets or sets the name of the feature dataset.
        /// </summary>
        /// <value>
        /// The name of the feature dataset.
        /// </value>
        [DataMember]
        public string FeatureDatasetName { get; set; }
        /// <summary>
        /// Gets or sets the name of the feature class.
        /// </summary>
        /// <value>
        /// The name of the feature class.
        /// </value>
        [DataMember]
        public string FeatureClassName { get; set; }
    }
}
