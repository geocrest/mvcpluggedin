using System.Runtime.Serialization;

namespace Geocrest.Model.ArcGIS.Tasks
{
    /// <summary>
    /// Represents a result from a feature service <c>ApplyEdits</c> operation.
    /// </summary>
    [DataContract(Namespace = Geocrest.XmlNamespaces.ModelVersion1)]
    public class EditResult
    {
        /// <summary>
        /// Gets or sets the Object ID of the feature being edited.
        /// </summary>
        [DataMember(Name = "objectId")]
        public long ObjectId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the operation was successful.
        /// </summary>
        /// <value>
        ///   <c>true</c> if success; otherwise, <c>false</c>.
        /// </value>
        [DataMember(Name = "success")]
        public bool Success { get; set; }
    }
}
