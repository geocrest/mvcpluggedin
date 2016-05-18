
namespace Geocrest.Model.ArcGIS.Tasks
{
    using System.Runtime.Serialization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    /// <summary>
    /// Represents linear units for use with ArcGIS.
    /// </summary>
    [DataContract(Namespace = Geocrest.XmlNamespaces.ModelVersion1)]
    [JsonConverter(typeof(StringEnumConverter))]
    public enum esriUnits
    {
        /// <summary>
        /// Unknown units
        /// </summary>
        [EnumMember]
        esriUnknownUnits,
        /// <summary>
        /// Inches
        /// </summary>
        [EnumMember]
        esriInches,
        /// <summary>
        /// Screen Points
        /// </summary>
        [EnumMember]
        esriPoints,
        /// <summary>
        /// Feet
        /// </summary>
        [EnumMember]
        esriFeet,
        /// <summary>
        /// Yards
        /// </summary>
        [EnumMember]
        esriYards,
        /// <summary>
        /// Miles
        /// </summary>
        [EnumMember]
        esriMiles,
        /// <summary>
        /// Nautical Miles
        /// </summary>
        [EnumMember]
        esriNauticalMiles,
        /// <summary>
        /// Millimeters
        /// </summary>
        [EnumMember]
        esriMillimeters,
        /// <summary>
        /// Centimeters
        /// </summary>
        [EnumMember]
        esriCentimeters,
        /// <summary>
        /// Meters
        /// </summary>
        [EnumMember]
        esriMeters,
        /// <summary>
        /// Kilometers
        /// </summary>
        [EnumMember]
        esriKilometers,
        /// <summary>
        /// Latitude/Longitude expressed as decimal degrees.
        /// </summary>
        [EnumMember]
        esriDecimalDegrees,
        /// <summary>
        /// Latitude/Longitude expressed as decimeters
        /// </summary>
        [EnumMember]
        esriDecimeters,
    }
}
