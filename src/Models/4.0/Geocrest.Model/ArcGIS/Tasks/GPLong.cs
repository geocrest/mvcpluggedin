namespace Geocrest.Model.ArcGIS.Tasks
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Represents a long integer geoprocessing parameter.
    /// </summary>
    [DataContract(Namespace = Geocrest.XmlNamespaces.ModelVersion1)]
    public class GPLong : GPParameter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Model.ArcGIS.Tasks.GPLong"/> class.
        /// </summary>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="value">An integer value.</param>
        public GPLong(string name, int value) : base(name)
        {
            this.Value = value;
        }

        /// <summary>
        /// Converts this instance to its equivalant JSON string representation.
        /// </summary>
        /// <returns>
        /// Returns a number as a string.
        /// </returns>
        public override string ToJson()
        {
            return this.Value.ToString();
        }
    
        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public int Value { get; set; }
    }
}
