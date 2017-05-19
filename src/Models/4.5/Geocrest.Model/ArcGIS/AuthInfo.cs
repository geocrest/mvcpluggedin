using System.Runtime.Serialization;

namespace Geocrest.Model.ArcGIS
{
    /// <summary>
    /// Represent authorization information about an ArcGIS Server.
    /// </summary>
    [DataContract(Namespace = Geocrest.XmlNamespaces.ModelVersion1)]
    public class AuthInfo
    {

        /// <summary>
        /// Gets or sets the token services URL.
        /// </summary>
        /// <value>
        /// The token services URL.
        /// </value>
        [DataMember]
        public string TokenServicesUrl { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this instance is token based security.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is token based security; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IsTokenBasedSecurity { get; set; }
    }
}
