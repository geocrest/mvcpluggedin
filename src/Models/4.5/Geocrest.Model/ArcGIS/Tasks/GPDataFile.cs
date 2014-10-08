namespace Geocrest.Model.ArcGIS.Tasks
{
    using System.Runtime.Serialization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    /// <summary>
    /// Represents a data file geoprocessing parameter. The value is a JSON structure with the following fields.
    /// <list type="bullet">
    /// <item><c>url</c>: a URL to the location of the raster data file.</item>
    /// <item><c>format</c>: a string representing the format of the raster.</item>
    /// </list>
    /// </summary>
    [DataContract(Namespace = Geocrest.XmlNamespaces.ModelVersion1)]
    public class GPDataFile : GPParameter
    {
        /// <summary>
        /// Gets or sets the URL to the data file.
        /// </summary>
        /// <value>
        /// The URL.
        /// </value>
        [DataMember(Name="url")]
        public string Url { get; set; }      
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Model.ArcGIS.Tasks.GPDataFile"/> class.
        /// </summary>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="url">A URL to the data file.</param>
        public GPDataFile(string name, string url)
            : base(name)
        {
            this.Url = url;
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
                    NullValueHandling= NullValueHandling.Ignore,
                    Formatting = Formatting.None
                };
            return JsonConvert.SerializeObject(this, settings);  
        }
    }
}
