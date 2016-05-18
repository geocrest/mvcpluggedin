
namespace Geocrest.Model.ArcGIS.WebMap
{
    using System.Runtime.Serialization;
    /// <summary>
    /// Specifies settings for the output map.
    /// </summary>
    [DataContract(Namespace = Geocrest.XmlNamespaces.ModelVersion1)]
    public class ExportOptions
    {
        /// <summary>
        /// Gets or sets the dpi.
        /// </summary>
        /// <value>
        /// The dpi.
        /// </value>
        [DataMember(Name = "dpi")]
        public int DPI { get; set; }
        /// <summary>
        /// Gets or sets the size of the output.
        /// </summary>
        /// <value>
        /// The size of the output.
        /// </value>
        [DataMember(Name="outputSize")]
        public int[] OutputSize { get; set; }
    }
}