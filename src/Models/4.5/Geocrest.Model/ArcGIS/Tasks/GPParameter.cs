namespace Geocrest.Model.ArcGIS.Tasks
{
    using System.Runtime.Serialization;
    
    /// <summary>
    /// Represents a base geoprocessing input parameter type.
    /// </summary>  
    [DataContract(Namespace = Geocrest.XmlNamespaces.ModelVersion1)]
    public abstract class GPParameter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Model.ArcGIS.Tasks.GPParameter"/> class.
        /// </summary>
        /// <param name="name">The name of the parameter.</param>
        public GPParameter(string name)
        {
            this.Name = name;
        }

        /// <summary>
        /// Gets or sets the name of the parameter.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Converts this instance to its equivalant JSON string representation.
        /// </summary>
        /// <returns>
        /// Return value will depend on the derived type.
        /// </returns>
        public abstract string ToJson();
    }
}
