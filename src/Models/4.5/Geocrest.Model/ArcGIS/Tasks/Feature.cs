namespace Geocrest.Model.ArcGIS.Tasks
{
    using System.Runtime.Serialization;
    using Geocrest.Model.ArcGIS.Geometry;
    /// <summary>
    /// Represents a single feature containing geometry and attributes
    /// </summary>
    [DataContract(Namespace = Geocrest.XmlNamespaces.ModelVersion1)]
    public class Feature : Record
    {
        /// <summary>
        /// Gets or sets the geometry.
        /// </summary>
        /// <value>
        /// The geometry.
        /// </value>
        [DataMember(Name = "geometry")]
        public Geometry Geometry { get; set; }       
    }
}