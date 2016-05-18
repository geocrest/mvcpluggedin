namespace Geocrest.Model.ArcGIS.Tasks
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;
    using Newtonsoft.Json;
    using Geocrest.Model.ArcGIS.Geometry;
    using Geocrest.Model.ArcGIS.Schema;

    /// <summary>
    /// Represents a collection of records.
    /// </summary>
    [DataContract(Namespace = Geocrest.XmlNamespaces.ModelVersion1)]
    [JsonObject]
    public class RecordSet : IEnumerable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Model.ArcGIS.Tasks.RecordSet" /> class.
        /// </summary>
        public RecordSet() 
        {
            this.Records = new Record[] { };
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Model.ArcGIS.Tasks.RecordSet" /> class.
        /// </summary>
        /// <param name="records">The initial set of records.</param>
        /// <exception cref="T:System.ArgumentNullException">'records' parameter must not be null.</exception>
        public RecordSet(IEnumerable<Record> records)
        {
            if (records == null)
                throw new ArgumentNullException("records", "'records' parameter must not be null");
            this.Records = new List<Record>(records).ToArray();
        }
        /// <summary>
        /// Gets or sets the records.
        /// </summary>
        /// <value>
        /// The records.
        /// </value>
        [DataMember(Name = "records")]
        public Record[] Records { get; set; }
        /// <summary>
        /// Returns a string that represents this instance as a JSON object.
        /// </summary>
        /// <returns>
        /// A JSON string representing this instance.
        /// </returns>
        public override string ToString()
        {
            JsonSerializerSettings settings = new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.None,
                DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Ignore
            };
            return JsonConvert.SerializeObject(this, settings);
        }
        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        public virtual IEnumerator GetEnumerator()
        {
            if (Records != null)
            {
                foreach (Record r in Records)
                    yield return r;
            }
        }
    }
}
