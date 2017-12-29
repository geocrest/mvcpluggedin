
namespace Geocrest.Model.ArcGIS.WebMap
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Provides a range domain for fields used in ArcGIS web maps.
    /// </summary>
    [DataContract(Namespace = Geocrest.XmlNamespaces.ModelVersion1)]
    public class RangeDomain : Domain
    {
        /// <summary>
        /// Gets or sets the range.
        /// </summary>
        /// <value>
        /// The range.
        /// </value>
        [DataMember(Name = "range")]
        public int[] Range { get; set; }
    }
}
