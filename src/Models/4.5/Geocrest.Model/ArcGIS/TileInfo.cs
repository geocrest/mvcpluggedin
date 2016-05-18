namespace Geocrest.Model.ArcGIS
{
    using System.Runtime.Serialization;
    using Geocrest.Model.ArcGIS.Geometry;

    /// <summary>
    /// Represents information about the tiling scheme of a cached map.
    /// </summary>
    [DataContract(Namespace = Geocrest.XmlNamespaces.ModelVersion1)]
    public class TileInfo
    {
        /// <summary>
        /// Gets or sets the columns.
        /// </summary>
        /// <value>
        /// The columns.
        /// </value>
        [DataMember(Name = "cols")]
        public int Columns { get; set; }

        /// <summary>
        /// Gets or sets the compression quality.
        /// </summary>
        /// <value>
        /// The compression quality.
        /// </value>
        [DataMember]
        public int CompressionQuality { get; set; }

        /// <summary>
        /// Gets or sets the DPI.
        /// </summary>
        /// <value>
        /// The DPI.
        /// </value>
        [DataMember]
        public int DPI { get; set; }

        /// <summary>
        /// Gets or sets the format.
        /// </summary>
        /// <value>
        /// The format.
        /// </value>
        [DataMember]
        public string Format { get; set; }

        /// <summary>
        /// Gets or sets the cache levels.
        /// </summary>
        /// <value>
        /// The LO ds.
        /// </value>
        [DataMember]
        public LOD[] LODs { get; set; }

        /// <summary>
        /// Gets or sets the origin.
        /// </summary>
        /// <value>
        /// The origin.
        /// </value>
        [DataMember]
        public Geocrest.Model.ArcGIS.Geometry.Geometry Origin { get; set; }

        /// <summary>
        /// Gets or sets the rows.
        /// </summary>
        /// <value>
        /// The rows.
        /// </value>
        [DataMember]
        public int Rows { get; set; }

        /// <summary>
        /// Gets or sets the spatial reference.
        /// </summary>
        /// <value>
        /// The spatial reference.
        /// </value>
        [DataMember]
        public SpatialReference SpatialReference { get; set; }
    }
}
