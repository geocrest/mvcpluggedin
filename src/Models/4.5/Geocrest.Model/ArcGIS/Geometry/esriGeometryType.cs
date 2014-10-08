namespace Geocrest.Model.ArcGIS.Geometry
{
    using System.Runtime.Serialization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    /// <summary>
    /// Available geometry types.
    /// </summary>
    [DataContract(Namespace = Geocrest.XmlNamespaces.ModelVersion1)]
    [JsonConverter(typeof(StringEnumConverter))]
    public enum esriGeometryType
    {
        /// <summary>
        /// A zero-dimensional point
        /// </summary>
         [EnumMember]
         esriGeometryPoint,
         /// <summary>
         /// A set of points
         /// </summary>
         [EnumMember]
         esriGeometryMultipoint,
         /// <summary>
         /// A one-dimensional line
         /// </summary>
         [EnumMember]
         esriGeometryPolyline,
         /// <summary>
         /// A two-dimensional polygon
         /// </summary>
         [EnumMember]
         esriGeometryPolygon,
         /// <summary>
         /// A two-dimensional rectangle
         /// </summary>
         [EnumMember]
         esriGeometryEnvelope
    }
}
