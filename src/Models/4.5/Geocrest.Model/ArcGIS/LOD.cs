namespace Geocrest.Model.ArcGIS
{
    using System.Runtime.Serialization;
    using Geocrest.Model.ArcGIS.Geometry;

    /// <summary>
    /// Represents Level-of-Detail information about specific cache levels of a cached map.
    /// </summary>
    [DataContract(Namespace = Geocrest.XmlNamespaces.ModelVersion1)]
    public class LOD
    {
        /// <summary>
        /// Gets or sets the level.
        /// </summary>
        /// <value>
        /// The level.
        /// </value>
        [DataMember]
        public int Level { get; set; }

        /// <summary>
        /// Gets or sets the resolution.
        /// </summary>
        /// <value>
        /// The resolution.
        /// </value>
        [DataMember]
        public double Resolution { get; set; }

        /// <summary>
        /// Gets or sets the scale.
        /// </summary>
        /// <value>
        /// The scale.
        /// </value>
        [DataMember]
        public double Scale { get; set; }
    }
}
