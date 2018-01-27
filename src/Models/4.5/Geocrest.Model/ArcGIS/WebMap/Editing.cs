
namespace Geocrest.Model.ArcGIS.WebMap
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Represents editing properties within an ArcGIS web map.
    /// </summary>
    [DataContract(Namespace = Geocrest.XmlNamespaces.ModelVersion1)]
    public class Editing
    {
        /// <summary>
        /// Gets or sets the location tracking.
        /// </summary>
        /// <value>
        /// The location tracking.
        /// </value>
        [DataMember(Name = "locationTracking")]
        public LocationTracking LocationTracking { get; set; }
    }
}
