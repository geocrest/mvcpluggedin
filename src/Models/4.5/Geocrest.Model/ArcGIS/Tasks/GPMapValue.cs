namespace Geocrest.Model.ArcGIS.Tasks
{
    using System.Runtime.Serialization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using Geocrest.Model.ArcGIS;

    /// <summary>
    /// Represents a map image result from a geoprocessing task that is associated with a result map service.
    /// </summary>
    [DataContract(Namespace = Geocrest.XmlNamespaces.ModelVersion1)]
    internal sealed class GPMapValue : GPParameter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Model.ArcGIS.Tasks.GPMapValue"/> class.
        /// </summary>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="image">The map image to set as the value.</param>
        public GPMapValue(string name, MapImage image)
            : base(name)
        {
            this.mapImage = image;
        }

        /// <summary>
        /// Gets or sets the map image.
        /// </summary>
        /// <value>
        /// The map image.
        /// </value>
        [DataMember]
        public MapImage mapImage { get; set; }

        /// <summary>
        /// Converts this instance to its equivalant JSON string representation.
        /// </summary>
        public override string ToJson()
        {
            JsonSerializerSettings settings = new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.None
            };
            return JsonConvert.SerializeObject(this.mapImage, settings);  
        }
    }
}
