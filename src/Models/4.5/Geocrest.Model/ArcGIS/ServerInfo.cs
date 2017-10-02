using System.Runtime.Serialization;

namespace Geocrest.Model.ArcGIS
{
    /// <summary>
    /// Represents root information about an ArcGIS Server.
    /// </summary>
    [DataContract(Namespace = Geocrest.XmlNamespaces.ModelVersion1)]
    public class ServerInfo
    {
        /// <summary>
        /// Gets or sets the full version.
        /// </summary>
        /// <value>
        /// The full version.
        /// </value>
        [DataMember]
        public string FullVersion { get; set; }
        /// <summary>
        /// Gets or sets the authentication information.
        /// </summary>
        /// <value>
        /// The authentication information.
        /// </value>
        [DataMember]
        public AuthInfo AuthInfo { get; set; }

    }
}
