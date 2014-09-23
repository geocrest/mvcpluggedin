namespace Geocrest.Model.ArcGIS.Tasks
{
    using System.Runtime.Serialization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    /// <summary>
    /// Represents a linear unit geoprocessing parameter.
    /// </summary>
    [DataContract(Namespace = Geocrest.XmlNamespaces.ModelVersion1)]
    public class GPLinearUnit : GPParameter
    {       
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Model.ArcGIS.Tasks.GPLinearUnit"/> class.
        /// </summary>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="units">The ESRI units of measure.</param>
        /// <param name="distance">The linear distance.</param>
        public GPLinearUnit(string name, esriUnits units, double distance):base(name)
        {
            this.Units = units;
            this.Distance = distance;
        }
        /// <summary>
        /// Gets or sets the distance.
        /// </summary>
        /// <value>
        /// The distance.
        /// </value>
        [DataMember(Name="distance")]
        public double Distance { get; set; }
        /// <summary>
        /// Gets or sets the units.
        /// </summary>
        /// <value>
        /// The units.
        /// </value>
        [DataMember(Name="units")]
        public esriUnits Units { get; set; }

        /// <summary>
        /// Converts this instance to its equivalant JSON string representation.
        /// </summary>
        /// <returns>
        /// A JSON structure containing <c>units</c> and <c>distance</c> properties.
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
    }
}
