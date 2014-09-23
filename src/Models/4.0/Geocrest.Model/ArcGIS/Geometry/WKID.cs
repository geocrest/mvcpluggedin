namespace Geocrest.Model.ArcGIS.Geometry
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    /// <summary>
    /// Well-known IDs of common spatial references.
    /// </summary>
    [DataContract(Namespace = Geocrest.XmlNamespaces.ModelVersion1)]
    public enum WKID
    {
        /// <summary>
        /// An invalid or unknown spatial reference
        /// </summary>
        [EnumMember]
        NotSpecified = 0,

        /// <summary>
        /// Geographic (i.e. Latitude/Longitude)
        /// </summary>
        [EnumMember(Value="4326")]
        [XmlEnum("4326")]
        Geographic = 4326,
        
        /// <summary>
        /// Virginia State Plane South
        /// </summary>
        [EnumMember(Value = "2284")]
        [XmlEnum("2284")]
        StatePlane4502 = 2284,
        
        /// <summary>
        /// Web Mercator
        /// </summary>
        [EnumMember(Value = "102100")]
        [XmlEnum("102100")]
        WebMercator = 102100
    }
}
