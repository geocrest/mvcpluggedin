namespace Geocrest.Model.ArcGIS.Tasks
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Represents an ESRI GPFeatureRecordSetLayer used for geoprocessing service parameters.
    /// </summary>
    [DataContract(Namespace = Geocrest.XmlNamespaces.ModelVersion1)]
    public class GPFeatureRecordSetLayer : GPRecordSet
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Model.ArcGIS.Tasks.GPFeatureRecordSetLayer"/> class.
        /// </summary>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="geometry">A single geometry to add to the parameter.</param>
        public GPFeatureRecordSetLayer(string name, Geometry.Geometry geometry)
            : base(name, new FeatureSet(geometry))
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Model.ArcGIS.Tasks.GPFeatureRecordSetLayer"/> class.
        /// </summary>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="features">A collection of features to add to the parameter.</param>
        public GPFeatureRecordSetLayer(string name, FeatureSet features)
            : base(name, features)
        {
        }
    }
}
