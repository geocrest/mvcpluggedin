namespace Geocrest.Model.ArcGIS.Tasks
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Represents a date geoprocessing parameter. The value is a number that represents 
    /// the number of milliseconds since epoch (January 1, 1970) in UTC.
    /// </summary>
    [DataContract(Namespace = Geocrest.XmlNamespaces.ModelVersion1)]
    public class GPDate : GPParameter
    {
        /// <summary>
        /// Gets or sets the format.
        /// </summary>
        /// <value>
        /// The format.
        /// </value>
        public string Format { get; set; }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Model.ArcGIS.Tasks.GPDate"/> class.
        /// </summary>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="value">A <see cref="T:System.DateTime"/> based on January 1, 1970 in UTC.</param>
        public GPDate(string name, DateTime value)
            : base(name)
        {
            this.Value = value;
            this.Format = "M/d/yyyy h:mm:ss tt";
        }

        /// <summary>
        /// Converts this instance to its equivalant JSON string representation.
        /// </summary>
        /// <returns>
        /// A number that represents the number of milliseconds since epoch (January 1, 1970) in UTC.
        /// </returns>
        public override string ToJson()
        {
            return string.Format("{0}", Math.Round((this.Value.ToUniversalTime() -
                new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds));
        }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public DateTime Value { get; set; }
    }
}
