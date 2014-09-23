namespace Geocrest.Model
{
    using System.Runtime.Serialization;
    using Geocrest.Model.ArcGIS.Geometry;

    /// <summary>
    /// Represents the base class for all features in a geodatabase.
    /// </summary>
    [DataContract(Namespace = Geocrest.XmlNamespaces.ModelVersion1)]
    public class FeatureClass : ObjectClass
    {        
        /// <summary>
        /// Gets or sets the feature geometry.
        /// </summary>
        /// <value>
        /// The geometry.
        /// </value>
        /// <remarks>
        /// This property is typically interpreted from a <c>Shape</c> column, which is reserved in ESRI 
        /// geodatabases for maintenance by ArcGIS.
        /// </remarks>
        [BaseNotMapped]
        [DataMember(Order = 100)]
        public Geometry Geometry { get; set; }
    }
}
