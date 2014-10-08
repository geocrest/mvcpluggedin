namespace Geocrest.Model.ArcGIS.Tasks
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;
    using Newtonsoft.Json;
    using Geocrest.Model.ArcGIS.Geometry;

    /// <summary>
    /// Represents a collection of address matches from a call to an ArcGIS 
    /// <see cref="M:Geocrest.GIS.Contracts.IGeocodeServer2.GeocodeAddresses(System.Collections.Generic.IDictionary{System.String,System.Object},Geocrest.Model.ArcGIS.Geometry.WKID,System.String,System.String)">Geocode Addresses</see> operation.
    /// </summary>
    /// <remarks>This collection provides matched candidates from a geocoding operation. Introduced at ArcGIS 10.1.</remarks>
    [DataContract(Namespace = Geocrest.XmlNamespaces.ModelVersion1)]
    [KnownType(typeof(SpatialReference))]
    [JsonObject]
    public class AddressMatchCollection : AddressCandidateCollection
    {
        /// <summary>
        /// Gets or sets the candidates that are considered geocoding matches.
        /// </summary>
        /// <value>
        /// The matched candidates.
        /// </value>
        [DataMember(Name="locations")]
        public override AddressCandidate[] Candidates
        {
            get
            {
                return base.Candidates;
            }
            set
            {
                base.Candidates = value;
            }
        }
    }
}
