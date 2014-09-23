
namespace Geocrest.Model.ArcGIS.WebMap
{
    using Geocrest.Model.ArcGIS.Tasks;
    using System.Runtime.Serialization;

    /// <summary>
    /// An operational feature layer to be displayed in the map
    /// </summary>
    [DataContract(Namespace = Geocrest.XmlNamespaces.ModelVersion1)]
    public class FeatureLayer : OperationalLayer
    {
        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        /// <value>
        /// The URL.
        /// </value>
        [DataMember(Name = "url")]
        public string Url { get; set; }
        /// <summary>
        /// Gets or sets the layer definition.
        /// </summary>
        /// <value>
        /// The layer definition.
        /// </value>
        [DataMember(Name="layerDefinition")]
        public LayerDefinition LayerDefinition { get; set; }
        /// <summary>
        /// Gets or sets the feature set.
        /// </summary>
        /// <value>
        /// The feature set.
        /// </value>
        [DataMember(Name="featureSet")]
        public FeatureSet FeatureSet { get; set; }
        /// <summary>
        /// Gets or sets the selection object ids.
        /// </summary>
        /// <value>
        /// The selection object ids.
        /// </value>
        [DataMember(Name="selectionObjectIds")]
        public int[] SelectionObjectIds { get; set; }
        /// <summary>
        /// Gets or sets the selection symbol.
        /// </summary>
        /// <value>
        /// The selection symbol.
        /// </value>
        [DataMember(Name="selectionSymbol")]
        public Symbol SelectionSymbol { get; set; }

    }
}