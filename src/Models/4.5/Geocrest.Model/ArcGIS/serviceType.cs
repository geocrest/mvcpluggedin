namespace Geocrest.Model.ArcGIS
{
    using System.Runtime.Serialization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    /// <summary>
    /// Possible types of ArcGIS services.
    /// </summary>
    [DataContract(Namespace = Geocrest.XmlNamespaces.ModelVersion1)]
    [JsonConverter(typeof(StringEnumConverter))]
    public enum serviceType
    {
        /// <summary>
        /// Represents a service used for feature edits.
        /// </summary>
        [EnumMember]
        FeatureServer,
        /// <summary>
        /// Represents a service used to locate address candidates. 
        /// </summary>
        [EnumMember]
        GeocodeServer,
        /// <summary>
        /// Represents a service used to provide access to an underlying geodatabase source.
        /// </summary>
        [EnumMember]
        GeodataServer,
        /// <summary>
        /// Represents a service used to perform geometric operations.
        /// </summary>
        [EnumMember]
        GeometryServer,
        /// <summary>
        /// Represents a service used to perform a geoprocessing model.
        /// </summary>
        [EnumMember]
        GPServer,
        /// <summary>
        /// Represents a core mapping service used to display geographic information.
        /// </summary>
        [EnumMember]
        MapServer,
        /// <summary>
        /// Represents a mobile service used to access feature class data.
        /// </summary>
        [EnumMember]
        MobileServer,
        /// <summary>
        /// Represents an image server for viewing and processing imagery and raster data.
        /// </summary>
        [EnumMember]
        ImageServer
    }
}
