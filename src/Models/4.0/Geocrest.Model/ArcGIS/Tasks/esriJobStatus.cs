namespace Geocrest.Model.ArcGIS.Tasks
{
    using System.Runtime.Serialization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    /// <summary>
    /// The status of a geoprocessing job.
    /// </summary>
    [DataContract(Namespace = Geocrest.XmlNamespaces.ModelVersion1)]
    [JsonConverter(typeof(StringEnumConverter))]
    public enum esriJobStatus
    {
        /// <summary>
        /// The job is a new job.
        /// </summary>
        [EnumMember]
        esriJobNew,
        /// <summary>
        /// The job has been submitted to the server.
        /// </summary>
        [EnumMember]
        esriJobSubmitted,
        /// <summary>
        /// The job is waiting.
        /// </summary>
        [EnumMember]
        esriJobWaiting,
        /// <summary>
        /// The job is currently executing.
        /// </summary>
        [EnumMember]
        esriJobExecuting,
        /// <summary>
        /// The job has completed successfully.
        /// </summary>
        [EnumMember]
        esriJobSucceeded,
        /// <summary>
        /// The job has failed to complete.
        /// </summary>
        [EnumMember]
        esriJobFailed,
        /// <summary>
        /// The job has timed out.
        /// </summary>
        [EnumMember]
        esriJobTimedOut,
        /// <summary>
        /// The job is in the process of cancelling.
        /// </summary>
        [EnumMember]
        esriJobCancelling,
        /// <summary>
        /// The job has been cancelled.
        /// </summary>
        [EnumMember]
        esriJobCancelled,
        /// <summary>
        /// The job is in the process of being deleted.
        /// </summary>
        [EnumMember]
        esriJobDeleting,
        /// <summary>
        /// The job has been deleted.
        /// </summary>
        [EnumMember]
        esriJobDeleted
    }
}
