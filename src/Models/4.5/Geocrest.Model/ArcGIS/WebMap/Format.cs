
namespace Geocrest.Model.ArcGIS.WebMap
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Represents settings that format numerical or date fields to provide more detail about how the value should be displayed in a web map popup window.
    /// </summary>
    [DataContract(Namespace = Geocrest.XmlNamespaces.ModelVersion1)]
    public class Format
    {
        /// <summary>
        /// Gets or sets the date format.
        /// </summary>
        /// <value>
        /// The date format.
        /// </value>
        [DataMember(Name = "dateFormat")]
        public string DateFormat { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [digit separator].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [digit separator]; otherwise, <c>false</c>.
        /// </value>
        [DataMember(Name = "digitSeparator")]
        public bool DigitSeparator { get; set; }

        /// <summary>
        /// Gets or sets the places.
        /// </summary>
        /// <value>
        /// The places.
        /// </value>
        [DataMember(Name = "places")]
        public int Places { get; set; }
    }
}
