namespace Geocrest.Model.ArcGIS
{
    using System.Net;
    using System.Runtime.Serialization;

    /// <summary>
    /// A generic error object typically returned from ESRI web services.
    /// </summary>
    [DataContract(Namespace = Geocrest.XmlNamespaces.ModelVersion1)]
    public class Error
    {
        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        /// <value>
        /// The code.
        /// </value>
        [DataMember]
        public HttpStatusCode Code { get; set; }

        /// <summary>
        /// Gets or sets the details.
        /// </summary>
        /// <value>
        /// The details.
        /// </value>
        [DataMember]
        public string[] Details { get; set; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        [DataMember]
        public string Message { get; set; }
    }

    /// <summary>
    /// A wrapper class for handling ESRI JSON errors.
    /// </summary>
    [DataContract(Namespace = Geocrest.XmlNamespaces.ModelVersion1)]
    public class ESRIException
    {
        /// <summary>
        /// Gets or sets the error returned from an ArcGIS service.
        /// </summary>
        [DataMember]
        public Error Error { get; set; }
    }
}
