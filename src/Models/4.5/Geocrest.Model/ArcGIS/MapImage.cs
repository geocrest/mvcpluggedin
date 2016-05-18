namespace Geocrest.Model.ArcGIS
{
    using System.Runtime.Serialization;
    using System;

    /// <summary>
    /// A map image returned from an ArcGIS Server
    /// </summary>
    [DataContract(Namespace = Geocrest.XmlNamespaces.ModelVersion1)]
    public sealed class MapImage
    {
        /// <summary>
        /// Gets or sets the URL to the map image.
        /// </summary>
        [DataMember(Name = "href")]
        public string Href { get; set; }

        /// <summary>
        /// Gets or sets the width of the image.
        /// </summary>
        [DataMember(Name = "width")]
        public int Width { get; set; }

        /// <summary>
        /// Gets or sets the height of the image.
        /// </summary>
        [DataMember(Name = "height")]
        public int Height { get; set; }

        /// <summary>
        /// Gets or sets the extent.
        /// </summary>
        [DataMember(Name = "extent")]
        public Geometry.Geometry Extent { get; set; }

        /// <summary>
        /// Gets or sets the scale of the image.
        /// </summary>
        [DataMember(Name = "scale")]
        public double Scale { get; set; }

        /// <summary>
        /// Gets or sets the error, if any.
        /// </summary>
        public Exception Error { get; set; }
    }
}
