namespace Geocrest.Model.ArcGIS.Tasks
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;
    using Newtonsoft.Json;
    using Geocrest.Model.ArcGIS.Geometry;

    /// <summary>
    /// Represents a collection of candidates from a call to an ArcGIS 
    /// <see cref="M:Geocrest.GIS.Contracts.IGeocodeServer.FindAddressCandidates(System.Collections.Generic.IDictionary{System.String,System.Object},Geocrest.Model.ArcGIS.Geometry.WKID,System.String,System.String)">Find Address Candidates</see> operation.
    /// </summary>
    [DataContract(Namespace = Geocrest.XmlNamespaces.ModelVersion1)]
    [KnownType(typeof(SpatialReference))]
    [JsonObject]
    public class AddressCandidateCollection : IEnumerable<AddressCandidate>
    {
        private AddressCandidate[] _candidates;

        /// <summary>
        /// Gets or sets the spatial reference.
        /// </summary>
        /// <value>
        /// The spatial reference.
        /// </value>
        [DataMember]
        public SpatialReference SpatialReference { get; set; }

        /// <summary>
        /// Gets or sets the candidates.
        /// </summary>
        /// <value>
        /// The candidates.
        /// </value>
        [DataMember]
        public virtual AddressCandidate[] Candidates {
            get { return this._candidates; }
            set { this._candidates = value.OrderByDescending(x => x.Score).ToArray(); }
        }

        /// <summary>
        /// Called when deserialized.
        /// </summary>
        /// <param name="context">An object of the type <see cref="T:System.Runtime.Serialization.StreamingContext">StreamingContext</see>.</param>
        [OnDeserialized]
        public void OnDeserialized(StreamingContext context)
        {
            if (this.Candidates == null) return;
            foreach (var c in this.Candidates.Where(x => x.Location != null))
            {
                c.Location.SpatialReference = this.SpatialReference;
            }
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
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<AddressCandidate> GetEnumerator()
        {
            if (this.Candidates != null)
            {
                foreach (AddressCandidate l in this.Candidates.OrderBy(x => x.Score))
                    yield return l;
            }
        }
        #endregion
    }
}
