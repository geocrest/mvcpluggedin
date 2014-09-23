namespace Geocrest.Model.ArcGIS.Geometry
{
    using System;
    using System.Runtime.Serialization;
    /// <summary>
    /// The spatial reference of the geometry object
    /// </summary>
    [DataContract(Namespace = Geocrest.XmlNamespaces.ModelVersion1)]
    public class SpatialReference
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Model.ArcGIS.Geometry.SpatialReference"/> class.
        /// </summary>
        /// <param name="wkid">A well-known spatial reference ID.</param>
        public SpatialReference(WKID wkid) { this.WKID = wkid; }
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Model.ArcGIS.Geometry.SpatialReference"/> class.
        /// </summary>
        public SpatialReference() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Model.ArcGIS.Geometry.SpatialReference" /> class.
        /// </summary>
        /// <param name="WKID">The well-known id of the spatial reference system.</param>
        /// <exception cref="T:System.ArgumentException"><paramref name="WKID"/> unable to be parsed into the 
        /// <see cref="T:Geocrest.Model.ArcGIS.Geometry.WKID"/> enumeration.</exception>
        public SpatialReference(int WKID) 
        {
            WKID parsed;
#if NET40 || SILVERLIGHT
            if (!Enum.TryParse<WKID>(WKID.ToString(), true, out parsed))
                throw new ArgumentException(string.Format("Could not parse the following well-known ID: {0}", WKID));
#else
            try
            {
                parsed = (WKID)Enum.Parse(typeof(WKID), WKID.ToString());
            }
            catch
            {
                throw new ArgumentException(string.Format("Could not parse the following well-known ID: {0}", WKID));

            }
#endif
            this.WKID = parsed;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Model.ArcGIS.Geometry.SpatialReference"/> class.
        /// </summary>
        /// <param name="WKT">The well-known text of the spatial reference system.</param>
        public SpatialReference(string WKT) { this.WKT = WKT; }
        /// <summary>
        /// Gets or sets the Well-Known ID.
        /// </summary>
        /// <value>
        /// The WKID.
        /// </value>
        [DataMember(Name = "wkid", EmitDefaultValue = false)]
        public WKID WKID { get; set; }
        /// <summary>
        /// Gets or sets the Well-Known Text.
        /// </summary>
        /// <value>
        /// The WKT.
        /// </value>
        [DataMember(Name = "wkt", EmitDefaultValue = false)]
        public string WKT { get; set; }
    }
}
