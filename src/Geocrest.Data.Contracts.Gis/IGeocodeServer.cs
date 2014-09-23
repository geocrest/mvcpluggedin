namespace Geocrest.Data.Contracts.Gis
{
    using System;
    using System.Collections.Generic;
    using Geocrest.Model.ArcGIS.Geometry;
    using Geocrest.Model.ArcGIS.Schema;
    using Geocrest.Model.ArcGIS.Tasks;
    /// <summary>
    /// Provides access to geocode server operations.
    /// </summary>
    public interface IGeocodeServer:IArcGISService
    {       
        /// <summary>
        /// Gets or sets the address fields.
        /// </summary>
        /// <value>
        /// The address fields.
        /// </value>
        Field[] AddressFields { get; set; }
        /// <summary>
        /// Gets or sets the single line address field.
        /// </summary>
        /// <value>
        /// The single line address field.
        /// </value>
        Field SingleLineAddressField { get; set; }
        /// <summary>
        /// Gets or sets the candidate fields.
        /// </summary>
        /// <value>
        /// The candidate fields.
        /// </value>
        Field[] CandidateFields { get; set; }
        /// <summary>
        /// Gets or sets the intersection candidate fields.
        /// </summary>
        /// <value>
        /// The intersection candidate fields.
        /// </value>
        Field[] IntersectionCandidateFields { get; set; }
        /// <summary>
        /// Gets or sets the spatial reference.
        /// </summary>
        /// <value>
        /// The spatial reference.
        /// </value>
        SpatialReference SpatialReference { get; set; }
        /// <summary>
        /// Gets or sets the locator properties.
        /// </summary>
        /// <value>
        /// The locator properties.
        /// </value>
        IDictionary<string, object> LocatorProperties { get; set; }
        /// <summary>
        /// Finds address candidates matching the input address values.
        /// </summary>
        /// <param name="addressFields">The inputs to search for as a key/value collection of the field name and search text.</param>
        /// <param name="outSR">The well-known ID of the spatial reference for the returned address candidates.</param>
        /// <param name="singleLineInput">The full address.</param>
        /// <param name="outFields">The list of fields to be included in the returned resultset. This list is a comma-delimited list of field names.</param>
        /// <returns>
        /// Returns an instance of <see cref="T:Geocrest.Model.ArcGIS.Tasks.AddressCandidateCollection"/>
        /// </returns>
        AddressCandidateCollection FindAddressCandidates(IDictionary<string, object> addressFields, WKID outSR,string singleLineInput="", string outFields = "");

        ///// <summary>
        ///// Finds address candidates matching the input address values.
        ///// </summary>
        ///// <param name="addressFields">The inputs to search for as a key/value collection of the field name and search text.</param>
        ///// <param name="outSR">The well-known ID of the spatial reference for the returned address candidates.</param>
        ///// <param name="callback">The callback function used to retrieve the results.</param>
        ///// <param name="singleLineInput">The full address.</param>
        ///// <param name="outFields">The list of fields to be included in the returned resultset. This list is a comma-delimited list of field names.</param>
        //void FindAddressCandidatesAsync(IDictionary<string, object> addressFields, WKID outSR, Action<AddressCandidateCollection> callback, string singleLineInput = "", string outFields = "");
        /// <summary>
        /// Reverses geocodes the input location.
        /// </summary>
        /// <param name="location">The point at which to search for the closest address.</param>
        /// <param name="distance">The distance in meters from the given location within which a matching address should be searched.</param>
        /// <param name="outSR">The well-known ID of the spatial reference for the returned address candidate.</param>
        ReverseGeocodedAddress ReverseGeocode(Geometry location, double distance, WKID outSR);
        ///// <summary>
        ///// Reverses geocodes the input location.
        ///// </summary>
        ///// <param name="location">The point at which to search for the closest address.</param>
        ///// <param name="distance">The distance in meters from the given location within which a matching address should be searched.</param>
        ///// <param name="outSR">The well-known ID of the spatial reference for the returned address candidate.</param>
        ///// <param name="callback">The callback function used to retrieve the results.</param>
        //void ReverseGeocodeAsync(Geometry location, double distance, WKID outSR, Action<ReverseGeocodedAddress> callback);
    }
}
