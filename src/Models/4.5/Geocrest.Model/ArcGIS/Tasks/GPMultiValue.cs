namespace Geocrest.Model.ArcGIS.Tasks
{
    using System.Runtime.Serialization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using System.Collections.Generic;

    /// <summary>
    /// Represents a multi value geoprocessing parameter that is an array of one of the other GPParameter types.
    /// </summary>
    /// <typeparam name="T">The type of parameter contained within the array.</typeparam>
    [DataContract(Namespace = Geocrest.XmlNamespaces.ModelVersion1)]
    public class GPMultiValue<T>:GPParameter where T : GPParameter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Model.ArcGIS.Tasks.GPMultiValue`1"/> class.
        /// </summary>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="value">The array of parameters.</param>
        public GPMultiValue(string name, IEnumerable<T> value)
            : base(name)
        {
            this.Value = value;
        }

        /// <summary>
        /// Converts this instance to its equivalant JSON string representation.
        /// </summary>
        /// <returns>
        /// A JSON array containing the generic type specified in the class.
        /// </returns>
        public override string ToJson()
        {
            JsonSerializerSettings settings = new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.None
            };
            return JsonConvert.SerializeObject(this.Value, settings); 
        }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        [DataMember]
        IEnumerable<T> Value { get; set; }
    }
}
