namespace Geocrest.Model.ArcGIS.Tasks
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Represents information pertaining to the execution of an asynchronous geoprocessing task on the server.
    /// </summary>
    [DataContract(Namespace = Geocrest.XmlNamespaces.ModelVersion1)]
    public class JobInfo
    {
        /// <summary>
        /// The unique job ID assigned by ArcGIS Server.
        /// </summary>
        [DataMember]
        public string JobId { get; set; }

        /// <summary>
        /// The job status.
        /// </summary>
        [DataMember]
        public esriJobStatus JobStatus { get; set; }

        /// <summary>
        /// An array of messages that include the message type and a description.
        /// </summary>
        [DataMember]
        public GPMessage[] Messages { get; set; }
    }
}
