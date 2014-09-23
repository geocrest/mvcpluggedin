
namespace Geocrest.Model.ArcGIS.WebMap
{
    using Geocrest.Model.ArcGIS.Geometry;
    using Geocrest.Model.ArcGIS.Schema;
    using System.Runtime.Serialization;
    /// <summary>
    /// Defines how the feature layer is rendered
    /// </summary>
    [DataContract(Namespace = Geocrest.XmlNamespaces.ModelVersion1)]
    public class LayerDefinition
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [DataMember(Name="name")]
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the object identifier field.
        /// </summary>
        /// <value>
        /// The object identifier field.
        /// </value>
        [DataMember(Name="objectIdField")]
        public string ObjectIdField { get; set; }
        /// <summary>
        /// Gets or sets the fields.
        /// </summary>
        /// <value>
        /// The fields.
        /// </value>
        [DataMember(Name="fields")]
        public Field[] Fields { get; set; }
        /// <summary>
        /// Gets or sets the drawing information.
        /// </summary>
        /// <value>
        /// The drawing information.
        /// </value>
        [DataMember(Name="drawingInfo")]
        public DrawingInfo DrawingInfo { get; set; }
        /// <summary>
        /// Gets or sets the definition expression.
        /// </summary>
        /// <value>
        /// The definition expression.
        /// </value>
        [DataMember(Name="definitionExpression")]
        public string DefinitionExpression { get; set; }
        /// <summary>
        /// Gets or sets the object ids.
        /// </summary>
        /// <value>
        /// The object ids.
        /// </value>
        [DataMember(Name="objectIds")]
        public int[] ObjectIds { get; set; }
        /// <summary>
        /// Gets or sets the geometry.
        /// </summary>
        /// <value>
        /// The geometry.
        /// </value>
        [DataMember(Name="geometry")]
        public Geometry Geometry { get; set; }
        /// <summary>
        /// Gets or sets the type of the geometry.
        /// </summary>
        /// <value>
        /// The type of the geometry.
        /// </value>
        [DataMember(Name = "geometryType")]
        public esriGeometryType GeometryType { get; set; }
        /// <summary>
        /// Gets or sets the spatial relative.
        /// </summary>
        /// <value>
        /// The spatial relative.
        /// </value>
        [DataMember(Name="spatialRel")]
        public string SpatialRel { get; set; }
    }
}