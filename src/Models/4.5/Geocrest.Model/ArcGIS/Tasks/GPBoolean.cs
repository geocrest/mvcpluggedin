namespace Geocrest.Model.ArcGIS.Tasks
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Represents a boolean geoprocessing parameter.
    /// </summary>
    [DataContract(Namespace = Geocrest.XmlNamespaces.ModelVersion1)]
    public class GPBoolean : GPParameter
    {
        /// <summary>
        /// Gets or sets the value of this parameter.
        /// </summary>
        public bool Value { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Model.ArcGIS.Tasks.GPBoolean"/> class.
        /// </summary>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="value">A true or false literal value.</param>
        public GPBoolean(string name, bool value) :base(name)
        {          
            this.Value = value;
        }

        /// <summary>
        /// Converts this instance to its equivalant JSON string representation.
        /// </summary>
        /// <returns>
        /// Returns "true" or "false"
        /// </returns>
        public override string ToJson()
        {
            return !this.Value ? "false" : "true";
        }
    }
}
