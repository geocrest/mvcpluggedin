namespace Geocrest.Model.ArcGIS.Geometry
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using Geocrest.Model.ArcGIS.Geometry;

    /// <summary>
    /// Represents a collection of geometries.
    /// </summary>
    [DataContract(Namespace = Geocrest.XmlNamespaces.ModelVersion1)]
    [JsonObject]
    public class GeometryCollection : IEnumerable<Geometry>
    {
        /// <summary>
        /// Gets or sets the url for a large set of geometries.
        /// </summary>
        /// <value>
        /// The url
        /// </value>
        [DataMember]
        public string Url { get; set; }

        private Geometry[] _geometries;
        private esriGeometryType _geometryType = esriGeometryType.esriGeometryPoint;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Model.ArcGIS.Geometry.GeometryCollection"/> class.
        /// </summary>
        public GeometryCollection() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Model.ArcGIS.Geometry.GeometryCollection"/> class.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        public GeometryCollection(Geometry geometry)
        {
            List<Geometry> geometries = new List<Geometry>();
            if (geometry != null)
            {
                geometries.Add(geometry);
                this.GeometryType = geometry.GeometryType;
            }
            this.Geometries = geometries.ToArray();
        }

        /// <summary>
        /// Gets or sets the geometries.
        /// </summary>
        /// <value>
        /// The geometries.
        /// </value>
        [DataMember(Name = "geometries", Order=1)]
        public Geometry[] Geometries
        {
            get { return this._geometries; }
            set { this._geometries = value; }
        }

        /// <summary>
        /// Gets or sets the type of the geometry.
        /// </summary>
        /// <value>
        /// The type of the geometry.
        /// </value>
        [DataMember(Name = "geometryType", Order=0)]
        public esriGeometryType GeometryType
        {
            get { return this._geometryType; }
            set { this._geometryType = value; }
        }

        /// <summary>
        /// Called when deserialized.
        /// </summary>
        /// <param name="context">An object of the type <see cref="T:System.Runtime.Serialization.StreamingContext">StreamingContext</see>.</param>
        [OnDeserialized]
        public void OnDeserialized(StreamingContext context)
        {
            if (this.Geometries == null) return;
            //foreach (var c in this.Geometries.Where(x => x.Location != null))
            //{
            //    c.Location.SpatialReference = this.SpatialReference;
            //}
        }

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
                ContractResolver = new OptionalPropertiesContractResolver("wkid")
            };
            return JsonConvert.SerializeObject(this, settings);
        }
        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();           
        }

        #endregion

        #region IEnumerable<AddressCandidate> Members

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<Geometry> GetEnumerator()
        {
            if (this.Geometries != null)
            {
                foreach (Geometry l in this.Geometries)
                    yield return l;
            }
        }

        #endregion       
    }
}
