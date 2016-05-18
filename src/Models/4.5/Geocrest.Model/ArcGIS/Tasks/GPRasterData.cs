namespace Geocrest.Model.ArcGIS.Tasks
{
    using System.Runtime.Serialization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    /// <summary>
    /// Represents raster data geoprocessing parameter.
    /// </summary>
    [DataContract(Namespace = Geocrest.XmlNamespaces.ModelVersion1)]
    public class GPRasterData:GPParameter
    {
        /// <summary>
        /// Gets or sets the format.
        /// </summary>
        /// <value>
        /// The format.
        /// </value>
        [DataMember(Name="format")]
        public string Format { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Model.ArcGIS.Tasks.GPRasterData" /> class.
        /// </summary>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="url">The URL to the raster image.</param>
        /// <param name="format">The format of the raster image.</param>
        public GPRasterData(string name, string url, string format)
            : base(name)
        {
            this.Url = url;
            this.Format = format;
        }

        /// <summary>
        /// Converts this instance to its equivalant JSON string representation.
        /// </summary>
        /// <returns>
        /// A JSON structure containing <c>url</c> and <c>format</c> properties.
        /// </returns>
        public override string ToJson()
        {
            JsonSerializerSettings settings = new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.None
            }; 
            return JsonConvert.SerializeObject(this, settings); 
        }

        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        /// <value>
        /// The URL.
        /// </value>
        [DataMember(Name="url")]
        public string Url { get; set; }
    }
}
