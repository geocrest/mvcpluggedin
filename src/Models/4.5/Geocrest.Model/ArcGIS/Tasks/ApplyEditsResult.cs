using System.Runtime.Serialization;

namespace Geocrest.Model.ArcGIS.Tasks
{
    /// <summary>
    /// Provides the results of add, update, and delete operations.
    /// </summary>
    [DataContract(Namespace = Geocrest.XmlNamespaces.ModelVersion1)]
    public class ApplyEditsResult
    {
        /// <summary>
        /// Gets or sets the results of an <c>add</c> operation.
        /// </summary>
        [DataMember(Name = "addResults")]
        public EditResult[] AddResults { get; set; }
        /// <summary>
        /// Gets or sets the results of an <c>update</c> operation.
        /// </summary>
        [DataMember(Name = "updateResults")]
        public EditResult[] UpdateResults { get; set; }
        /// <summary>
        /// Gets or sets the results of an <c>delete</c> operation.
        /// </summary>
        [DataMember(Name = "deleteResults")]
        public EditResult[] DeleteResults { get; set; }
    }
}
