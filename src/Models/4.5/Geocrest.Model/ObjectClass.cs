namespace Geocrest.Model
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;
    using Newtonsoft.Json;

    /// <summary>
    /// Represents the base class for all objects and features in a geodatabase.
    /// </summary>
    [DataContract(Namespace = Geocrest.XmlNamespaces.ModelVersion1)]
    public abstract class ObjectClass : Resource
    {
        /// <summary>
        /// Gets or sets the unique Object ID maintained by ArcGIS.
        /// </summary>
        /// <value>
        /// A long integer.
        /// </value>
        /// <remarks>
        /// This property is typically mapped to an <c>ObjectID</c> column, which is reserved in ESRI 
        /// geodatabases for maintenance by ArcGIS.
        /// </remarks>
        [JsonIgnore]
        [IgnoreDataMember]
        public int ObjectID { get; set; }

        /// <summary>
        /// Gets or sets the user name of the last person to modify the object or feature in the geodatabase.
        /// </summary>
        /// <value>
        /// A string containing an Active Directory login.
        /// </value>
        /// <remarks>
        /// This property is typically mapped to an <c>EditBy</c> column.
        /// </remarks>
        [DataMember(Order = 60)]
        [StringLength(15)]
        [Display(Name = "Last Edited By")]
        public string EditBy { get; set; }

        /// <summary>
        /// Gets or sets the date-time of the last modification to the object or feature in the geodatabase.
        /// </summary>
        /// <value>
        /// A <see cref="T:System.DateTime"/> value.
        /// </value>
        /// <remarks>
        /// This property is typically mapped to an <c>EditDate</c> column.
        /// </remarks>
        [DataMember(Order = 61)]
        [Display(Name = "Last Edit Date")]
        [DataType(DataType.DateTime)]
        public DateTime? EditDate { get; set; }
    }
}
