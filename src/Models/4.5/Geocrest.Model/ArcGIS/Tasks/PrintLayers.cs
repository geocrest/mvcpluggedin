namespace Geocrest.Model.ArcGIS.Tasks
{
    using System.Runtime.Serialization;

    /// <summary>
    /// This class contains all of the parameters required to generate a print image.
    /// </summary>
    [DataContract(Namespace = Geocrest.XmlNamespaces.ModelVersion1)]
    public class PrintParameters
    {
        /// <summary>
        /// Gets or sets the print area.
        /// </summary>
        /// <value>
        /// The print area.
        /// </value>
        [DataMember(Name = "PrintArea")]
        public PrintArea PrintArea { get; set; }

        /// <summary>
        /// Gets or sets the layers to print.
        /// </summary>
        /// <value>
        /// The layers.
        /// </value>
        [DataMember(Name = "Layers")]
        public Layer[] Layers { get; set; }
        /// <summary>
        /// Gets or sets the DPI.
        /// </summary>
        /// <value>
        /// The DPI.
        /// </value>
        [DataMember(Name = "DPI")]
        public int DPI { get; set; }
        /// <summary>
        /// Gets or sets the target size of the return image.
        /// </summary>
        /// <value>
        /// The desired size of the image.
        /// </value>
        [DataMember(Name = "TargetSize")]
        public int TargetSize { get; set; }
    }

    /// <summary>
    /// This class defines the parameters of the map boundary and output image.
    /// </summary>
    [DataContract(Namespace = Geocrest.XmlNamespaces.ModelVersion1)]
    public class PrintArea
    {
        /// <summary>
        /// Gets or sets the minimum X coordinate.
        /// </summary>
        /// <value>
        /// The X min.
        /// </value>
        [DataMember(Name = "XMin")]
        public double XMin { get; set; }

        /// <summary>
        /// Gets or sets the minimum Y coordinate.
        /// </summary>
        /// <value>
        /// The Y min.
        /// </value>
        [DataMember(Name = "YMin")]
        public double YMin { get; set; }

        /// <summary>
        /// Gets or sets the maximum X coordinate.
        /// </summary>
        /// <value>
        /// The X max.
        /// </value>
        [DataMember(Name = "XMax")]
        public double XMax { get; set; }

        /// <summary>
        /// Gets or sets the maximum Y coordinate.
        /// </summary>
        /// <value>
        /// The Y max.
        /// </value>
        [DataMember(Name = "YMax")]
        public double YMax { get; set; }

        /// <summary>
        /// Gets or sets the width of the map.
        /// </summary>
        /// <value>
        /// The width.
        /// </value>
        [DataMember(Name = "Width")]
        public int Width { get; set; }

        /// <summary>
        /// Gets or sets the height of the map.
        /// </summary>
        /// <value>
        /// The height.
        /// </value>
        [DataMember(Name = "Height")]
        public int Height { get; set; }

        /// <summary>
        /// Gets or sets the spatial reference ID.
        /// </summary>
        /// <value>
        /// The WKID of the spatial reference.
        /// </value>
        [DataMember(Name = "SpatialReferenceID")]
        public int? SpatialReferenceID { get; set; }
    }

    /// <summary>
    /// This class represents a map layer to use in the output image.
    /// </summary>
    [DataContract(Namespace = Geocrest.XmlNamespaces.ModelVersion1)]
    public class Layer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Model.ArcGIS.Tasks.Layer" /> class.
        /// </summary>
        public Layer()
        {
            Format = "png32";
            UseProxy = false;
        }        
        /// <summary>
        /// Gets or sets the opacity of the layer.
        /// </summary>
        /// <value>
        /// The opacity.
        /// </value>
        [DataMember(Name = "Opacity")]
        public double Opacity { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use a proxy.
        /// </summary>
        /// <value>
        /// <b>true</b>, if requests should use a proxy; otherwise, <b>false</b>.
        /// </value>
        [DataMember(Name = "UseProxy")]
        public bool UseProxy { get; set; }

        /// <summary>
        /// Gets or sets the service URL if using an ESRI map service.
        /// </summary>
        /// <value>
        /// The service URL.
        /// </value>
        [DataMember(Name = "ServiceUrl")]
        public string ServiceUrl { get; set; }

        /// <summary>
        /// Gets or sets the format. The default is png32
        /// </summary>
        /// <value>
        /// The format.
        /// </value>
        /// <remarks>
        /// Values: png | png8 | png24 | jpg | pdf | bmp | gif | svg | png32
        /// </remarks>
        [DataMember(Name = "Format")]
        public string Format { get; set; }

        /// <summary>
        /// Gets or sets the image data if using existing image data (e.g. graphicslayers).
        /// </summary>
        /// <value>
        /// The image data.
        /// </value>
        [DataMember(Name = "ImageData")]
        public string ImageData { get; set; }

        /// <summary>
        /// Gets or sets the layer ids to show in the map.
        /// </summary>
        /// <value>
        /// The array of layer ids.
        /// </value>
        [DataMember(Name = "LayerIDs")]
        public string LayerIDs { get; set; }        
    }
}
