namespace Geocrest.Data.Sources.Gis
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using Geocrest.Data.Contracts.Gis;
    using Geocrest.Data.Sources.Gis.Constants;
    using Geocrest.Web.Infrastructure;
    using Geocrest.Model.ArcGIS.Geometry;
    using Geocrest.Model.ArcGIS.Schema;
    using Geocrest.Model.ArcGIS.Tasks;

    /// <summary>
    /// Represent an ArcGIS Server geocode service.
    /// </summary>
    [DataContract(Namespace = Geocrest.XmlNamespaces.InfrastructureVersion1)]
    public class GeocodeServer : ArcGISService, IGeocodeServer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Data.Sources.Gis.GeocodeServer"/> class.
        /// </summary>
        internal GeocodeServer() { }

        /// <summary>
        /// Gets or sets the address fields.
        /// </summary>
        /// <value>
        /// The address fields.
        /// </value>
        [DataMember]
        public Field[] AddressFields { get; set; }

        /// <summary>
        /// Gets or sets the single line address field.
        /// </summary>
        /// <value>
        /// The single line address field.
        /// </value>
        [DataMember]
        public Field SingleLineAddressField { get; set; }

        /// <summary>
        /// Gets or sets the candidate fields.
        /// </summary>
        /// <value>
        /// The candidate fields.
        /// </value>
        [DataMember]
        public Field[] CandidateFields { get; set; }

        /// <summary>
        /// Gets or sets the intersection candidate fields.
        /// </summary>
        /// <value>
        /// The intersection candidate fields.
        /// </value>
        [DataMember]
        public Field[] IntersectionCandidateFields { get; set; }

        /// <summary>
        /// Gets or sets the spatial reference.
        /// </summary>
        /// <value>
        /// The spatial reference.
        /// </value>
        [DataMember]
        public SpatialReference SpatialReference { get; set; }

        /// <summary>
        /// Gets or sets the locator properties.
        /// </summary>
        /// <value>
        /// The locator properties.
        /// </value>
        [DataMember]
        public IDictionary<string, object> LocatorProperties { get; set; }

        #region IGeocodeServer Members
        ///// <summary>
        ///// Finds address candidates matching the input address values.
        ///// </summary>
        ///// <param name="addressFields">An object of the type <see cref="IDictionary&lt;System.String,System.String&gt;">IDicationary&lt;System.String,System.String&gt;</see>.</param>
        ///// <param name="outSR">The well-known ID of the spatial reference for the returned address candidates.</param>
        ///// <param name="callback">The callback function used to retrieve the results.</param>
        ///// <param name="singleLineInput">The full address.</param>
        ///// <param name="outFields">The list of fields to be included in the returned resultset. This list is a comma-delimited list of field names.</param>
        //public void FindAddressCandidatesAsync(IDictionary<string, object> addressFields, WKID outSR, 
        //    System.Action<AddressCandidateCollection> callback, string singleLineInput = "", string outFields = "")
        //{
        //    Throw.IfArgumentNull(addressFields, "inputs");
        //    if (outSR != WKID.NotSpecified) addressFields.Add(GEOCODE.FindAddressCandidatesParam.outSR, (int)outSR);
        //    addressFields.Add(this.SingleLineAddressField.Name, singleLineInput);
        //    addressFields.Add(GEOCODE.FindAddressCandidatesParam.outFields, outFields);
        //    string endpoint = GetUrl(GEOCODE.FindAddressCandidates, addressFields);
        //    this.RestHelper.HydrateAsync<AddressCandidateCollection>(endpoint, callback);
        //}
        
        /// <summary>
        /// Finds address candidates matching the input address values.
        /// </summary>
        /// <param name="addressFields">The inputs to search for as a key/value collection of the field name and search text.</param>
        /// <param name="outSR">The well-known ID of the spatial reference for the returned address candidates.</param>
        /// <param name="singleLineInput">The full address.</param>
        /// <param name="outFields">The list of fields to be included in the returned resultset. This list is a comma-delimited list of field names.</param>
        /// <returns>
        /// Returns an instance of <see cref="T:Geocrest.Model.ArcGIS.Tasks.AddressCandidateCollection"/>.
        /// </returns>
        public AddressCandidateCollection FindAddressCandidates(IDictionary<string, object> addressFields, WKID outSR, string singleLineInput = "", string outFields = "")
        {
            if (addressFields == null)
            {
                addressFields = new Dictionary<string, object> { };
                if (string.IsNullOrEmpty(singleLineInput))
                {
                    Throw.ArgumentException("addressFields AND singleLineInput");
                }
            }
            //Throw.IfArgumentNull(addressFields, "addressFields");
            if (outSR != WKID.NotSpecified) addressFields.Add(GEOCODE.FindAddressCandidatesParam.outSR, (int)outSR);
            addressFields.Add(this.SingleLineAddressField.Name, singleLineInput);
            addressFields.Add(GEOCODE.FindAddressCandidatesParam.outFields, outFields);
            Uri endpoint = GetUrl(GEOCODE.FindAddressCandidates, addressFields);
            return Geocrest.Model.RestHelper.HydrateObject<AddressCandidateCollection>(endpoint.ToString());
        }

        ///// <summary>
        ///// Reverses geocodes the input location.
        ///// </summary>
        ///// <param name="location">The point at which to search for the closest address.</param>
        ///// <param name="distance">The distance in meters from the given location within which a matching address should be searched.</param>
        ///// <param name="outSR">The well-known ID of the spatial reference for the returned address candidate.</param>
        ///// <param name="callback">The callback function used to retrieve the results.</param>
        //public void ReverseGeocodeAsync(Geometry location, double distance, WKID outSR, Action<ReverseGeocodedAddress> callback)
        //{
        //    Throw.IfArgumentNull(location, "location");
        //    Throw.IfNullableIsNull(location.X, "X");
        //    Throw.IfNullableIsNull(location.Y, "Y");
        //    IDictionary<string, object> inputs = new Dictionary<string, object>
        //        {
        //            { "location", location.X.Value.ToString() + "," + location.Y.Value.ToString() },
        //            { "distance", distance }
        //        };
        //    if (outSR != WKID.NotSpecified) inputs.Add("outSR", (int)outSR);
        //    string endpoint = GetUrl("reverseGeocode", inputs);
        //    this.RestHelper.HydrateAsync<ReverseGeocodedAddress>(endpoint, callback);
        //}

        /// <summary>
        /// Reverses geocodes the input location.
        /// </summary>
        /// <param name="location">The point at which to search for the closest address.</param>
        /// <param name="distance">The distance in meters from the given location within which a matching address should be searched.</param>
        /// <param name="outSR">The well-known ID of the spatial reference for the returned address candidate.</param>
        public ReverseGeocodedAddress ReverseGeocode(Geometry location, double distance, WKID outSR)
        {
            Throw.IfArgumentNull(location, "location");
            Throw.IfNullableIsNull(location.X, "X");
            Throw.IfNullableIsNull(location.Y, "Y");
            IDictionary<string, object> inputs = new Dictionary<string, object>
                {
                    //{ "location", location.X.Value.ToString() + "," + location.Y.Value.ToString() },
                    { "location", location.ToString()},
                    { "distance", distance }
                };
            if (outSR != WKID.NotSpecified) inputs.Add("outSR",(int) outSR);
            Uri endpoint = GetUrl("reverseGeocode", inputs);
            return Geocrest.Model.RestHelper.HydrateObject<ReverseGeocodedAddress>(endpoint.ToString());
        }
        #endregion
    }
}
