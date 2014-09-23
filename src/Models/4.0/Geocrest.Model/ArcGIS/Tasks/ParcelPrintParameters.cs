namespace Geocrest.Model.ArcGIS.Tasks
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Provides a wrapped object for POSTing print parameters that contain a PIN and a zoom level.
    /// </summary>
    [DataContract]
    public class ParcelPrintParameters : PrintParameters
    {
        /// <summary>
        /// Gets or sets the parcel identification number
        /// </summary>
        /// <value>
        /// The PIN.
        /// </value>
        [DataMember]
        public string PIN { get; set; }
        /// <summary>
        /// Gets or sets the zoom level.
        /// </summary>
        /// <value>
        /// The zoom level value.
        /// </value>
        [DataMember]
        public string Zoom { get; set; }
    }
}