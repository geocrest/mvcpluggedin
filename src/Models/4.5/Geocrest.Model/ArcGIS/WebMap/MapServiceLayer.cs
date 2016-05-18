
namespace Geocrest.Model.ArcGIS.WebMap
{
    using System.Runtime.Serialization;

    /// <summary>
    /// An operational map service layer to be displayed in the map
    /// </summary>
    [DataContract(Namespace = Geocrest.XmlNamespaces.ModelVersion1)]
    public class MapServiceLayer : OperationalLayer
    {       
        /// <summary>
        /// Gets or sets the layers to be displayed.
        /// </summary>
        /// <value>
        /// The layers.
        /// </value>
        [DataMember(Name="visibleLayers")]
        public int[] VisibleLayers { get; set; }
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