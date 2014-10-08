namespace Geocrest.Model.ArcGIS.Tasks
{
    using System.Runtime.Serialization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    /// <summary>
    /// Execution type for a geoprocessing operation.
    /// </summary>
    [DataContract(Namespace = Geocrest.XmlNamespaces.ModelVersion1)]
    [JsonConverter(typeof(StringEnumConverter))]
    public enum esriExecutionType
    {
        /// <summary>
        /// Task is submitted asynchronously and the result is returned by checking the output directory with the job ID.
        /// </summary>
        [EnumMember]
        esriExecutionTypeAsynchronous,
        /// <summary>
        /// Task is submitted synchronously and the result is returned directly to the client.
        /// </summary>
        [EnumMember]
        esriExecutionTypeSynchronous
    }
}
