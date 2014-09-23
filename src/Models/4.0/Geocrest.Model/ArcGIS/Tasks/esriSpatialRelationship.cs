namespace Geocrest.Model.ArcGIS
{
    using System.Runtime.Serialization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    /// <summary>
    /// Spatial relationships used when performing spatial queries.
    /// </summary>
    [DataContract(Namespace = Geocrest.XmlNamespaces.ModelVersion1)]
    [JsonConverter(typeof(StringEnumConverter))]
    public enum esriSpatialRelationship
    {
        /// <summary>
        /// Geometries intersect each other.
        /// </summary>
        [EnumMember]
        esriSpatialRelIntersects,
        /// <summary>
        /// One geometry contains another.
        /// </summary>
        [EnumMember]
        esriSpatialRelContains,
        /// <summary>
        /// Geometries cross boundaries.
        /// </summary>
        [EnumMember]
        esriSpatialRelCrosses,
        /// <summary>
        /// Geometry envelopes intersect.
        /// </summary>
        [EnumMember]
        esriSpatialRelEnvelopeIntersects,
        /// <summary>
        /// Geometry indexes intersect.
        /// </summary>
        [EnumMember]
        esriSpatialRelIndexIntersects,
        /// <summary>
        /// 
        /// </summary>
        [EnumMember]
        esriSpatialRelOverlaps,
        /// <summary>
        /// 
        /// </summary>
        [EnumMember]
        esriSpatialRelTouches,
        /// <summary>
        /// 
        /// </summary>
        [EnumMember]
        esriSpatialRelWithin,
        /// <summary>
        /// 
        /// </summary>
        [EnumMember]
        esriSpatialRelRelation
    }
}
