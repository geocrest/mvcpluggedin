namespace Geocrest.Model.ArcGIS.Tasks
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Represents a string geoprocessing parameter.
    /// </summary>
    [DataContract(Namespace = Geocrest.XmlNamespaces.ModelVersion1)]
    public class GPString : GPParameter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Model.ArcGIS.Tasks.GPString"/> class.
        /// </summary>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="value">A literal string value.</param>
        public GPString(string name, string value) : base(name)
        {
            this.Value = value;
        }

        /// <summary>
        /// Converts this instance to its equivalant JSON string representation.
        /// </summary>
        /// <returns>
        /// Returns the literal string value.
        /// </returns>
        public override string ToJson()
        {
            return this.Value;
        }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public string Value { get; set; }
    }
}
