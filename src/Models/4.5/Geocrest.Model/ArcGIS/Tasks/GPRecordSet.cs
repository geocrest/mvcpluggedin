namespace Geocrest.Model.ArcGIS.Tasks
{
    using System.Runtime.Serialization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    /// <summary>
    /// Represents a GPRecordSet geoprocessing task input.
    /// For a large set of records, you can specify the Url property to the input records stored
    /// in a JSON structure in a file on a public server.
    /// </summary>
    [DataContract(Namespace = Geocrest.XmlNamespaces.ModelVersion1)]
    public class GPRecordSet : GPParameter
    {
        /// <summary>
        /// Gets or sets the features.
        /// </summary>
        /// <value>
        /// The feature set.
        /// </value>
        [DataMember(Name = "featureSet")]
        public FeatureSet FeatureSet { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Model.ArcGIS.Tasks.GPRecordSet"/> class.
        /// </summary>
        /// <param name="name">The name of the parameter.</param>
        public GPRecordSet(string name)
            : base(name)
        {
            this.FeatureSet = new FeatureSet();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Model.ArcGIS.Tasks.GPRecordSet"/> class.
        /// </summary>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="features">An array of features.</param>
        public GPRecordSet(string name, FeatureSet features)
            : base(name)
        {
            this.FeatureSet = features;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Model.ArcGIS.Tasks.GPRecordSet"/> class.
        /// </summary>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="url">The URL of the JSON dataset.</param>
        public GPRecordSet(string name, string url)
            : base(name)
        {
            this.Url = url;
        }

        /// <summary>
        /// Converts this instance to its equivalant JSON string representation.
        /// </summary>
        /// <returns>
        /// A JSON structure containing <c>url</c> and <c>featureSet</c> properties.
        /// </returns>
        public override string ToJson()
        {
            JsonSerializerSettings settings = new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.None
            };
            return JsonConvert.SerializeObject(this.FeatureSet, settings);
        }

        /// <summary>
        /// Gets or sets the Url to the location of the data file.
        /// </summary>
        /// <value>
        /// The URL.
        /// </value>
        [DataMember(Name = "url")]
        public string Url { get; set; }
    }
}
