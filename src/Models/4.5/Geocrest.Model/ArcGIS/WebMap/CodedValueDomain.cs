
namespace Geocrest.Model.ArcGIS.WebMap
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Provides a code value domain for fields used in ArcGIS web maps.
    /// </summary>
    [DataContract(Namespace = Geocrest.XmlNamespaces.ModelVersion1)]
    public class CodedValueDomain : Domain
    {
        /// <summary>
        /// Gets or sets the coded values.
        /// </summary>
        /// <value>
        /// The coded values.
        /// </value>
        [DataMember(Name = "codedValues")]
        public CodedValue[] CodedValues { get; set; }
    }    
}
