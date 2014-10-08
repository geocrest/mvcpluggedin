
namespace Geocrest.Model.ArcGIS.WebMap
{
    using System.Runtime.Serialization;
    /// <summary>
    /// A collection of layers for displaying client-side graphics
    /// </summary>
    [DataContract(Namespace = Geocrest.XmlNamespaces.ModelVersion1)]
    public class FeatureCollection
    {
        /// <summary>
        /// Gets or sets the layers.
        /// </summary>
        /// <value>
        /// The layers.
        /// </value>
        [DataMember(Name="layers")]
        public FeatureLayer[] Layers { get; set; }
    }
}