
namespace Geocrest.Model.ArcGIS.WebMap
{
    using System.ComponentModel;
    using System.Runtime.Serialization;

    /// <summary>
    /// An operational layer displaying client-side graphics to be displayed in the map
    /// </summary>
    [DataContract(Namespace = Geocrest.XmlNamespaces.ModelVersion1)]
    public class GraphicsLayer : OperationalLayer
    {
        /// <summary>
        /// Gets or sets the feature collection.
        /// </summary>
        /// <value>
        /// The feature collection.
        /// </value>
        [DataMember(Name="featureCollection")]
        public FeatureCollection FeatureCollection { get; set; }    
    }
}