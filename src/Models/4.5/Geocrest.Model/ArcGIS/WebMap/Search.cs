
namespace Geocrest.Model.ArcGIS.WebMap
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Represents search parameters used in the web map.
    /// </summary>
    [DataContract(Namespace = Geocrest.XmlNamespaces.ModelVersion1)]
    public class Search
    {
        /// <summary>
        /// Gets or sets a value indicating whether <see cref="Search" /> is enabled in the web map.
        /// </summary>
        /// <value>
        ///   <c>true</c> if enabled; otherwise, <c>false</c>.
        /// </value>
        [DataMember(Name = "enabled")]
        public bool Enabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not to disable the place finder.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [disable place finder]; otherwise, <c>false</c>.
        /// </value>
        [DataMember(Name = "disablePlaceFinder")]
        public bool DisablePlaceFinder { get; set; }

        /// <summary>
        /// Gets or sets the hint text.
        /// </summary>
        /// <value>
        /// The hint text.
        /// </value>
        [DataMember(Name = "hintText")]
        public string HintText { get; set; }

        /// <summary>
        /// Gets or sets the layers.
        /// </summary>
        /// <value>
        /// The layers.
        /// </value>
        [DataMember(Name = "layers")]
        public Layer[] Layers { get; set; }
    }
}
