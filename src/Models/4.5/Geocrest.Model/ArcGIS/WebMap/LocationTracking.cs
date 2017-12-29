
namespace Geocrest.Model.ArcGIS.WebMap
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Represents location tracking properties within an ArcGIS web map. 
    /// If locationTracking is set and enabled, the collector app will update the feature service at the 
    /// defined interval with the current location of the user logged into the collector application.
    /// </summary>
    [DataContract(Namespace = Geocrest.XmlNamespaces.ModelVersion1)]
    public class LocationTracking
    {
        /// <summary>
        /// Gets or sets a value indicating whether <see cref="LocationTracking"/> is enabled.
        /// </summary>
        /// <value>
        ///   <c>true</c> if enabled; otherwise, <c>false</c>.
        /// </value>
        [DataMember(Name = "enabled")]
        public bool Enabled { get; set; }

        /// <summary>
        /// Gets or sets the information.
        /// </summary>
        /// <value>
        /// The information.
        /// </value>
        [DataMember(Name = "info")]
        public Info Info { get; set; }
    }
}
