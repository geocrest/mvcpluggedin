namespace Geocrest.Model
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;
    using Geocrest.Model.ArcGIS.Geometry;
    /// <summary>
    /// Represents the base class for all point features in a geodatabase.
    /// </summary>
    [DataContract(Namespace = Geocrest.XmlNamespaces.ModelVersion1)]
    public class PointFeatureClass : FeatureClass, IPointFeatureClass
    {
        /// <summary>
        /// Gets or sets the X-axis member of a coordinate pair defining the location in the 
        /// Virginia State Plane 4502 coordinate system. 
        /// </summary>
        /// <value>
        /// A decimal of units in feet, like <i>11785900.36468340</i>.
        /// </value>
        /// <remarks>
        /// This property is typically mapped to a <c>StatePlaneX</c> column.
        /// </remarks>
        [DataMember(Order = 42)]
        [Display(Name = "State Plane X")]
        [BaseMapped]
        public decimal? StatePlaneX { get; set; }

        /// <summary>
        /// Gets or sets the Y-axis member of a coordinate pair defining the location in the 
        /// Virginia State Plane 4502 coordinate system. 
        /// </summary>
        /// <value>
        /// A decimal of units in feet, like <i>3726308.38044003</i>.
        /// </value>
        /// <remarks>
        /// This property is typically mapped to a <c>StatePlaneY</c> column.
        /// </remarks>
        [DataMember(Order = 43)]
        [Display(Name = "State Plane Y")]
        [BaseMapped]
        public decimal? StatePlaneY { get; set; }

        /// <summary>
        /// Gets or sets the latitude of a geographic coordinate defining the location.
        /// </summary>
        /// <value>
        /// A decimal of geographic decimal degrees, like <i>37.55224820</i>.
        /// </value>
        /// <remarks>
        /// This property is typically mapped to a <c>Latitude</c> column.
        /// </remarks>
        [DataMember(Order = 44)]
        [BaseMapped]
        public decimal? Latitude { get; set; }

        /// <summary>
        /// Gets or sets the longitude of a geographic coordinate defining the location.
        /// </summary>
        /// <value>
        /// A decimal of geographic decimal degrees, like <i>-77.45482005</i>.
        /// </value>
        /// <remarks>
        /// This property is typically mapped to a <c>Longitude</c> column.
        /// </remarks>
        [DataMember(Order = 45)]
        [BaseMapped]
        public decimal? Longitude { get; set; }

        /// <summary>
        /// Called when deserialized.
        /// </summary>
        /// <param name="context">An object of the type <see cref="T:System.Runtime.Serialization.StreamingContext"/>.</param>
        [OnDeserialized]
        public void OnDeserialized(StreamingContext context)
        {
            if (Longitude.HasValue && Latitude.HasValue && Geometry == null)
                this.Geometry = new Geometry(Convert.ToDouble(Longitude.Value),
                    Convert.ToDouble(Latitude.Value), WKID.Geographic);
        }
    }
}
