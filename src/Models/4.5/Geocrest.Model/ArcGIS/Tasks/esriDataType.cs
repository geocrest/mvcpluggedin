namespace Geocrest.Model.ArcGIS.Tasks
{
    using System.Runtime.Serialization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    /// <summary>
    /// The various ESRI data types for GPParameters. Note that literal data types use the literal value for the 
    /// <see cref="P:Geocrest.Model.ArcGIS.Tasks.GPParameterInfo.DefaultValue"/> property.
    /// Non-literals use a JSON representation. Literal types are GPBoolean | GPDouble | GPLong |
    /// GPString.
    /// </summary>
    [DataContract(Namespace = Geocrest.XmlNamespaces.ModelVersion1)]
    [JsonConverter(typeof(StringEnumConverter))]
    public enum esriDataType
    {
        /// <summary>
        /// A literal Boolean value.
        /// </summary>
        [EnumMember]
        GPBoolean,
        /// <summary>
        /// A literal Double value.
        /// </summary>
        [EnumMember]
        GPDouble,
        /// <summary>
        /// A literal Long integer value.
        /// </summary>
        [EnumMember]
        GPLong,
        /// <summary>
        /// A literal String value.
        /// </summary>
        [EnumMember]
        GPString,
        /// <summary>
        /// A number that represents the number of milliseconds since epoch (January 1, 1970) in UTC.
        /// </summary>
        [EnumMember]
        GPDate,
        /// <summary>
        /// A string whose values can be ESRI linear unit types (e.g. <c>esriMeters</c>, <c>esriMiles</c>, etc).
        /// </summary>
        [EnumMember]
        GPLinearUnit,
        /// <summary>
        /// A JSON structure with a <c>url</c> field. The value of the url field is a URL to the location of the data file.
        /// </summary>
        [EnumMember]
        GPDataFile,
        /// <summary>
        /// A JSON structure with the following fields.
        /// <list type="bullet">
        /// <item><c>url</c>: a URL to the location of the raster data file.</item>
        /// <item><c>format</c>: a string representing the format of the raster.</item>
        /// </list>
        /// </summary>
        [EnumMember]
        GPRasterData,
        /// <summary>
        /// A JSON structure with the field <c>features</c>.
        /// </summary>
        [EnumMember]
        GPRecordSet,
        /// <summary>
        /// If the GP Service is associated with a result map service, 
        /// the result is a JSON structure with a <c>mapImage</c> field. The mapImage value 
        /// has <c>href</c>, <c>width</c>, <c>height</c>, <c>extent</c>, and <c>scale</c> properties.
        /// If the GP service is <i>not</i> associated with a result map service, 
        /// the result is a JSON structure with the following fields.
        /// <list type="bullet">
        /// <item><c>url</c>: a URL to the location of the raw raster data.</item>
        /// <item><c>format</c>: a string representing the format of the raster.</item>
        /// </list>
        /// </summary>
        [EnumMember]
        GPRasterDataLayer,
        /// <summary>
        /// If the GP Service is associated with a result map service,
        /// the result is a JSON structure with a <c>mapImage</c> field. The mapImage value 
        /// has <c>href</c>, <c>width</c>, <c>height</c>, <c>extent</c>, and <c>scale</c> properties.
        /// If the GP service is <i>not</i> associated with a result map service, 
        /// the result is a JSON structure with the following fields.
        /// <list type="bullet">
        /// <item><c>features</c>: an array of features.</item>
        /// <item><c>spatialReference</c>: the well known ID of a spatial reference.</item>
        /// <item><c>geometryType</c>: the geometry type of the layer.</item>
        /// </list>
        /// </summary>
        [EnumMember]
        GPFeatureRecordSetLayer,
        /// <summary>
        /// The fully-qualified data type for a GPMultiValue parameter is GPMultiValue:&lt;memberDataTyp>, 
        /// where memberDataType is one of the data types defined above. For ex. GPMultiValue:GPString, 
        /// GPMultiValue:GPLong, etc.
        /// The parameter value for GPMultiValue data types is a JSON array. Each element in this array 
        /// is of the data type as defined by the memberDataType suffix of the fully-qualified GPMultiValue 
        /// data type name.
        /// </summary>
        [EnumMember]
        GPMultiValue
    }
}
