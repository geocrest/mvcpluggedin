
namespace Geocrest.Model.ArcGIS.Tasks
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Represents a feature set result returned from a map service query.
    /// </summary>
    [DataContract(Namespace = Geocrest.XmlNamespaces.ModelVersion1)]
    public class FeatureSetQuery : FeatureSet
    {
        /// <summary>
        /// Gets or sets the count.
        /// </summary>
        /// <value>
        /// The count.
        /// </value>
        [DataMember(Name="count")]
        public int Count { get; set; }
        /// <summary>
        /// Gets or sets the name of the object id field.
        /// </summary>
        /// <value>
        /// The name of the object id field.
        /// </value>
        [DataMember(Name = "objectIdFieldName")]
        public string ObjectIdFieldName { get; set; }
        /// <summary>
        /// Gets or sets the object ids.
        /// </summary>
        /// <value>
        /// The object ids.
        /// </value>
        [DataMember(Name = "objectIds")]
        public int[] ObjectIds { get; set; }
    }
}
