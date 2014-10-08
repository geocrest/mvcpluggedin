namespace Geocrest.Model.ArcGIS.Tasks
{
    using System.Runtime.Serialization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    /// <summary>
    /// Geoprocessing parameter requirement types
    /// </summary>
    [DataContract(Namespace = Geocrest.XmlNamespaces.ModelVersion1)]
    [JsonConverter(typeof(StringEnumConverter))]
    public enum esriParameterType
    {
        /// <summary>
        /// The parameter is required.
        /// </summary>
        [EnumMember]
        esriGPParameterTypeRequired,
        /// <summary>
        /// The parameter is optional.
        /// </summary>
        [EnumMember]
        esriGPParameterTypeOptional,
        /// <summary>
        /// The parameter is a derived output from the task.
        /// </summary>
        [EnumMember]
        esriGPParameterTypeDerived
    }
}
