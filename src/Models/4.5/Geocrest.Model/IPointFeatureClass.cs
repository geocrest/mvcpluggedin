namespace Geocrest.Model
{
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;

    /// <summary>
    /// Provides access to the properties of a point feature class.
    /// </summary>
    public interface IPointFeatureClass
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
#if NET45 || SILVERLIGHT
        [Display(Name = "State Plane X")]
#endif
        decimal? StatePlaneX { get; set; }

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
#if NET45 || SILVERLIGHT
        [Display(Name = "State Plane Y")]
#endif
        decimal? StatePlaneY { get; set; }

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
        decimal? Latitude { get; set; }

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
        decimal? Longitude { get; set; }
    }
}
