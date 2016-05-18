namespace Geocrest.Model.ArcGIS.Tasks
{
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;
    using Geocrest.Model.ArcGIS.Geometry;
    
    /// <summary>
    /// Represents a result from an ArcGIS <see cref="M:Geocrest.GIS.Contracts.IGeocodeServer.ReverseGeocode(Geocrest.Model.ArcGIS.Geometry.Geometry,System.Double,Geocrest.Model.ArcGIS.Geometry.WKID)"/> operation.
    /// </summary>
    [DataContract(Namespace = Geocrest.XmlNamespaces.ModelVersion1)]
    public class ReverseGeocodedAddress 
    {
        /// <summary>
        /// Gets or sets the address ID.
        /// </summary>
        [DataMember]
        public string value { get; set; }

        /// <summary>
        /// Gets or sets the address elements. The <c>Street</c> sub-element contains the address value.
        /// </summary>
        [DataMember]
        public System.Collections.Generic.Dictionary<string, object> Address { get; set; }              

        ///// <summary>
        ///// Gets or sets the attributes.
        ///// </summary>
        ///// <value>
        ///// The attributes.
        ///// </value>
        //[DataMember]
        //public System.Collections.Generic.Dictionary<string, object> Attributes { get; set; }

        /// <summary>
        /// Gets or sets the location.
        /// </summary>
        [DataMember]
        public Geometry Location { get; set; }

        ///// <summary>
        ///// Called when deserialized.
        ///// </summary>
        ///// <param name="context">An object of the type <see cref="System.Runtime.Serialization.StreamingContext">StreamingContext</see>.</param>
        //[OnDeserialized]
        //public void OnDeserialized(StreamingContext context)
        //{
        //    this.value = this.Attributes.Count > 0 && this.Attributes["User_fld"] != null ?
        //        this.Attributes["User_fld"].ToString() : string.Empty;
        //}
    }
}
