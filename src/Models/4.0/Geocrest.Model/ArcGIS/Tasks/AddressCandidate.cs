namespace Geocrest.Model.ArcGIS.Tasks
{
    using Geocrest.Model.ArcGIS.Geometry;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;

    /// <summary>
    /// Represents a candidate from a call to an ArcGIS 
    /// <see cref="M:Geocrest.GIS.Contracts.IGeocodeServer.FindAddressCandidates(System.Collections.Generic.IDictionary{System.String,System.Object},Geocrest.Model.ArcGIS.Geometry.WKID,System.String,System.String)">Find Address Candidates</see> operation.
    /// </summary>
    [DataContract(Namespace = Geocrest.XmlNamespaces.ModelVersion1)]
    public class AddressCandidate : IComparable
    {
        /// <summary>
        /// Stores the internal reference exposed as the Score property.
        /// </summary>
        private double _score;

        /// <summary>
        /// Gets or sets the Reference ID of a feature (Ref_ID) returned from non-intersection 
        /// geocode candidates. This is defined in the locator as either the <c>ObjectID</c> or some other 
        /// unique key on the reference features. 
        /// </summary>
        /// <value>
        /// The Ref_ID value.
        /// </value>
        [DataMember]
#if NET40 || SILVERLIGHT
        [Display(Name = "Reference ID")]
#endif
        public string ReferenceID { get; set; }

        /// <summary>
        /// Gets or sets the User Field of a feature (User_fld) returned from non-intersection 
        /// geocode candidates. This is defined in the locator as a field on the reference features. 
        /// </summary>
        /// <value>
        /// The User_fld value.
        /// </value>
        [DataMember]
#if NET40 || SILVERLIGHT
        [Display(Name = "User Field")]
#endif
        public string UserField { get; set; }

        /// <summary>
        /// Gets the label.
        /// </summary>
        [DataMember]
        public string label
        {
            get { return Address; }
            set { }
        }

        /// <summary>
        /// Gets or sets the address.
        /// </summary>
        /// <value>
        /// The address.
        /// </value>
        [DataMember]
        public string Address { get; set; }

        /// <summary>
        /// Gets or sets the attributes.
        /// </summary>
        /// <value>
        /// The attributes.
        /// </value>
        [DataMember]
        public System.Collections.Generic.Dictionary<string, object> Attributes { get; set; }

        /// <summary>
        /// Gets or sets the location.
        /// </summary>
        /// <value>
        /// The location.
        /// </value>
        [DataMember]
        public Geometry Location { get; set; }

        /// <summary>
        /// Gets or sets the score.
        /// </summary>
        /// <value>
        /// The score.
        /// </value>
        [DataMember]
        public double Score
        {
            get { return this._score; }
            set { this._score = value; }
        }

        /// <summary>
        /// Called when deserialized.
        /// </summary>
        /// <param name="context">An object of the type <see cref="T:System.Runtime.Serialization.StreamingContext">StreamingContext</see>.</param>
        [OnDeserialized]
        public void OnDeserialized(StreamingContext context)
        {           
            this.ReferenceID = this.Attributes.Count > 0 &&
                this.Attributes.ContainsKey("Ref_ID") &&
                this.Attributes["Ref_ID"] != null ?
                this.Attributes["Ref_ID"].ToString() : string.Empty;

            this.UserField = this.Attributes.Count > 0 &&
                this.Attributes.ContainsKey("User_fld") &&
                this.Attributes["User_fld"] != null ?
                this.Attributes["User_fld"].ToString() : string.Empty;
        }

        #region IComparable Members
        /// <summary>
        /// Returns the result of comparing the current instance Score value with the input object Score value.
        /// </summary>
        /// <param name="obj">An object to compare this instance with.</param>
        /// <returns>
        /// Returns an integer used by IComparable to sort this object within a collection.
        /// </returns>
        /// <exception cref="System.Exception">Object is not a GeocodeCandidate.</exception>
        /// <remarks>
        /// Sort order here is descending. Use first list item to find the best matching candidate after sorting.
        /// </remarks>
        public int CompareTo(object obj)
        {
            if (obj is AddressCandidate)
            {
                AddressCandidate geocodeCandidate = (AddressCandidate)obj;
                return -this._score.CompareTo(geocodeCandidate.Score);
            }

            throw new Exception("Object is not a GeocodeCandidate.");
        }
        #endregion
    }
}
