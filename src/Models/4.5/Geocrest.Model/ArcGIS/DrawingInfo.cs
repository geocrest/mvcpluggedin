namespace Geocrest.Model.ArcGIS
{
    using System.Runtime.Serialization;
    
    /// <summary>
    /// Contains rendering information about the legend.
    /// </summary>
    [DataContract(Namespace = Geocrest.XmlNamespaces.ModelVersion1)]
    public class DrawingInfo
    {
        /// <summary>Gets or sets the renderer.</summary>
        /// <value>The renderer.</value>
        [DataMember(Name = "renderer")]
        public Renderer Renderer { get; set; }

        /// <summary>
        /// Gets or sets the transparency.
        /// </summary>
        /// <value>
        /// The transparency.
        /// </value>
        [DataMember]
        public double Transparency { get; set; }
    }

    /// <summary>
    /// The specific type of renderer.
    /// </summary>
    /// <remarks>Can be SimpleRenderer, UniqueValuesRenderer, or ClassBreaksRenderer.</remarks>
    [DataContract(Namespace = Geocrest.XmlNamespaces.ModelVersion1)]
    public class Renderer
    {
        /// <summary>Gets or sets the type.</summary>
        /// <value>The type.</value>
        [DataMember(Name = "type")]
        public string Type { get; set; }

        /// <summary>Gets or sets the symbol.</summary>
        /// <value>The symbol.</value>
        [DataMember(Name = "symbol")]
        public Symbol Symbol { get; set; }

        /// <summary>Gets or sets the label.</summary>
        /// <value>The label.</value>
        [DataMember(Name = "label")]
        public string Label { get; set; }

        /// <summary>Gets or sets the description.</summary>
        /// <value>The description.</value>
        [DataMember(Name = "description")]
        public string Description { get; set; }

        /// <summary>Gets or sets the field1.</summary>
        /// <value>The field1.</value>
        [DataMember(Name = "field1")]
        public string Field1 { get; set; }

        /// <summary>Gets or sets the field2.</summary>
        /// <value>The field2.</value>
        [DataMember(Name = "field2")]
        public string Field2 { get; set; }

        /// <summary>Gets or sets the field3.</summary>
        /// <value>The field3.</value>
        [DataMember(Name = "field3")]
        public string Field3 { get; set; }

        /// <summary>Gets or sets the field delimiter.</summary>
        /// <value>The field delimiter.</value>
        [DataMember(Name = "fieldDelimiter")]
        public string FieldDelimiter { get; set; }

        /// <summary>Gets or sets the default symbol.</summary>
        /// <value>The default symbol.</value>
        [DataMember(Name = "defaultSymbol")]
        public Symbol DefaultSymbol { get; set; }

        /// <summary>Gets or sets the default label.</summary>
        /// <value>The default label.</value>
        [DataMember(Name = "defaultLabel")]
        public string DefaultLabel { get; set; }

        /// <summary>Gets or sets the unique value infos.</summary>
        /// <value>The unique value infos.</value>
        [DataMember(Name = "uniqueValueInfos")]
        public UniqueValueInfo[] UniqueValueInfos { get; set; }

        /// <summary>Gets or sets the field.</summary>
        /// <value>The field.</value>
        [DataMember(Name = "field")]
        public string Field { get; set; }

        /// <summary>Gets or sets the min value.</summary>
        /// <value>The min value.</value>
        [DataMember(Name = "minValue")]
        public double MinValue { get; set; }

        /// <summary>Gets or sets the class break infos.</summary>
        /// <value>The class break infos.</value>
        [DataMember(Name = "classBreakInfos")]
        public ClassBreakInfo[] ClassBreakInfos { get; set; }
    }

    /// <summary>
    /// Represents an individual unique value.
    /// </summary>
    public class UniqueValueInfo
    {
        /// <summary>Gets or sets the value.</summary>
        /// <value>The value.</value>
        [DataMember(Name = "value")]
        public string Value { get; set; }

        /// <summary>Gets or sets the label.</summary>
        /// <value>The label.</value>
        [DataMember(Name = "label")]
        public string Label { get; set; }

        /// <summary>Gets or sets the description.</summary>
        /// <value>The description.</value>
        [DataMember(Name = "description")]
        public string Description { get; set; }

        /// <summary>Gets or sets the symbol.</summary>
        /// <value>The symbol.</value>
        [DataMember(Name = "symbol")]
        public Symbol Symbol { get; set; }
    }

    /// <summary>
    /// Represents the class breaks for use within a class breaks renderer.
    /// </summary>
    [DataContract(Namespace = Geocrest.XmlNamespaces.ModelVersion1)]
    public class ClassBreakInfo
    {
        /// <summary>Gets or sets the class max value.</summary>
        /// <value>The class max value.</value>
        [DataMember(Name = "classMaxValue")]
        public double ClassMaxValue { get; set; }

        /// <summary>Gets or sets the label.</summary>
        /// <value>The label.</value>
        [DataMember(Name = "label")]
        public string Label { get; set; }

        /// <summary>Gets or sets the description.</summary>
        /// <value>The description.</value>
        [DataMember(Name = "description")]
        public string Description { get; set; }

        /// <summary>Gets or sets the symbol.</summary>
        /// <value>The symbol.</value>
        [DataMember(Name = "symbol")]
        public Symbol Symbol { get; set; }
    }

    /// <summary>
    /// The symbol used to display legend patches.
    /// </summary>
    /// <remarks>Can be SimpleMarkerSymbol, SimpleLineSymbol, SimpleFillSymbol, PictureMarkerSymbol, or PictureFillSymbol.</remarks>
    [DataContract(Namespace = Geocrest.XmlNamespaces.ModelVersion1)]
    public class Symbol
    {
        /// <summary>Gets or sets the type.</summary>
        /// <value>The type.</value>
        [DataMember(Name = "type")]
        public string Type { get; set; }

        /// <summary>Gets or sets the URL.</summary>
        /// <value>The URL.</value>
        [DataMember(Name = "url")]
        public string Url { get; set; }

        /// <summary>Gets or sets the image data.</summary>
        /// <value>The image data.</value>
        [DataMember(Name = "imageData")]
        public string ImageData { get; set; }

        /// <summary>Gets or sets the type of the content.</summary>
        /// <value>The type of the content.</value>
        [DataMember(Name = "contentType")]
        public string ContentType { get; set; }

        /// <summary>Gets or sets the width.</summary>
        /// <value>The width.</value>
        [DataMember(Name = "width")]
        public int Width { get; set; }

        /// <summary>Gets or sets the height.</summary>
        /// <value>The height.</value>
        [DataMember(Name = "height")]
        public int Height { get; set; }

        /// <summary>Gets or sets the color.</summary>
        /// <value>The color.</value>
        [DataMember(Name = "color")]
        public int[] Color { get; set; }

        /// <summary>Gets or sets the style.</summary>
        /// <value>The style.</value>
        [DataMember(Name = "style")]
        public string Style { get; set; }

        /// <summary>Gets or sets the size.</summary>
        /// <value>The size.</value>
        [DataMember(Name = "size")]
        public int Size { get; set; }

        /// <summary>Gets or sets the angle.</summary>
        /// <value>The angle.</value>
        [DataMember(Name = "angle")]
        public int Angle { get; set; }

        /// <summary>Gets or sets the X offset.</summary>
        /// <value>The X offset.</value>
        [DataMember(Name = "xoffset")]
        public int XOffset { get; set; }

        /// <summary>Gets or sets the Y offset.</summary>
        /// <value>The Y offset.</value>
        [DataMember(Name = "yoffset")]
        public int YOffset { get; set; }

        /// <summary>Gets or sets the outline.</summary>
        /// <value>The outline.</value>
        [DataMember(Name = "outline")]
        public SimpleLine Outline { get; set; }

        /// <summary>Gets or sets the X scale.</summary>
        /// <value>The X scale.</value>
        [DataMember(Name = "xscale")]
        public int XScale { get; set; }

        /// <summary>Gets or sets the Y scale.</summary>
        /// <value>The Y scale.</value>
        [DataMember(Name = "yscale")]
        public int YScale { get; set; }
    }

    /// <summary>
    /// Represents a simple line symbol for drawing.
    /// </summary>
    [DataContract(Namespace = Geocrest.XmlNamespaces.ModelVersion1)]
    public class SimpleLine
    {
        /// <summary>Gets or sets the type.</summary>
        /// <value>The type.</value>
        [DataMember(Name = "type")]
        public string Type { get; set; }

        /// <summary>Gets or sets the style.</summary>
        /// <value>The style.</value>
        [DataMember(Name = "style")]
        public string Style { get; set; }

        /// <summary>Gets or sets the color.</summary>
        /// <value>The color.</value>
        [DataMember(Name = "color")]
        public int[] Color { get; set; }

        /// <summary>Gets or sets the width.</summary>
        /// <value>The width.</value>
        [DataMember(Name = "width")]
        public int Width { get; set; }
    }
}
