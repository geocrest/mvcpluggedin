
namespace Geocrest.Model.ArcGIS.WebMap
{
    using System.ComponentModel;
    using System.Runtime.Serialization;

    /// <summary>
    /// An operational layer to be displayed in the map
    /// </summary>
    [DataContract(Namespace = Geocrest.XmlNamespaces.ModelVersion1)]
    public abstract class OperationalLayer
    {       
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        [DataMember(Name="id")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the item identifier.
        /// </summary>
        /// <value>
        /// The item identifier.
        /// </value>
        [DataMember(Name = "itemId")]
        public string ItemId { get; set; }

        /// <summary>
        /// Gets or sets the type of the layer.
        /// </summary>
        /// <value>
        /// The type of the layer.
        /// </value>
        [DataMember(Name = "layerType")]
        public string LayerType { get; set; }

        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        /// <value>
        /// The URL.
        /// </value>
        [DataMember(Name = "url")]
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the token.
        /// </summary>
        /// <value>
        /// The token.
        /// </value>
        [DataMember(Name="token")]
        public string Token { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        [DataMember(Name="title")]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the opacity.
        /// </summary>
        /// <value>
        /// The opacity.
        /// </value>
        [DataMember(Name="opacity")]
        [DefaultValue(1)]
        public double Opacity { get; set; }

        /// <summary>
        /// Gets or sets the minimum scale.
        /// </summary>
        /// <value>
        /// The minimum scale.
        /// </value>
        [DataMember(Name = "minScale")]
        public int MinScale { get; set; }

        /// <summary>
        /// Gets or sets the maximum scale.
        /// </summary>
        /// <value>
        /// The maximum scale.
        /// </value>
        [DataMember(Name = "maxScale")]
        public int MaxScale { get; set; }   
    }
}
