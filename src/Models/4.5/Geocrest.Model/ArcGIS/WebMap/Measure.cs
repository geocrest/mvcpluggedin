
namespace Geocrest.Model.ArcGIS.WebMap
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Provides a setting that determines whether the measure tool is enabled in the web map.
    /// </summary>
    [DataContract(Namespace = Geocrest.XmlNamespaces.ModelVersion1)]
    public class Measure
    {
        /// <summary>
        /// Gets or sets a value indicating whether <see cref="Measure"/> is enabled.
        /// </summary>
        /// <value>
        ///   <c>true</c> if enabled; otherwise, <c>false</c>.
        /// </value>
        [DataMember(Name = "enabled")]
        public bool Enabled { get; set; }
    }
}
