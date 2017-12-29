
namespace Geocrest.Model.ArcGIS.Tasks
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Provides the results from a geoprocessing service execute task operation.
    /// </summary>
    [DataContract(Namespace = Geocrest.XmlNamespaces.ModelVersion1)]
    public class GPResultSet
    {
        /// <summary>
        /// Gets or sets the results.
        /// </summary>
        /// <value>
        /// The results.
        /// </value>
        [DataMember]
        public GPResult[] Results { get; set; }

        /// <summary>
        /// An array of messages that include the message type and a description.
        /// </summary>
        /// <value>
        /// The messages.
        /// </value>
        [DataMember]
        public GPMessage[] Messages { get; set; }
    }
}
