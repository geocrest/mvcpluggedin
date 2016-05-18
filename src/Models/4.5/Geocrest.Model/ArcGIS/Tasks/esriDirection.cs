namespace Geocrest.Model.ArcGIS.Tasks
{
    using System.Runtime.Serialization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    /// <summary>
    /// Direction of a GPParameter (i.e. input or output)
    /// </summary>
    [DataContract(Namespace = Geocrest.XmlNamespaces.ModelVersion1)]
    [JsonConverter(typeof(StringEnumConverter))]
    public enum esriDirection
    {
        /// <summary>
        /// An input parameter.
        /// </summary>
        [EnumMember]
        esriGPParameterDirectionInput,
        /// <summary>
        /// An output result parameter.
        /// </summary>
        [EnumMember]
        esriGPParameterDirectionOutput
    }
}
