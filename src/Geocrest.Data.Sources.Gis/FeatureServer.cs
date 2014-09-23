namespace Geocrest.Data.Sources.Gis
{
    using System.Runtime.Serialization;
    using Geocrest.Data.Contracts.Gis;
    using Geocrest.Model.ArcGIS;

    /// <summary>
    /// Represents an ArcGIS Server feature service for editing features.
    /// </summary>
    [DataContract(Namespace = Geocrest.XmlNamespaces.InfrastructureVersion1)]
    public class FeatureServer : ArcGISService, IFeatureServer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Data.Sources.Gis.FeatureServer"/> class.
        /// </summary>
        internal FeatureServer() { }

        #region IFeatureServer Members
        /// <summary>
        /// Gets or sets the layers.
        /// </summary>
        /// <value>
        /// The layers.
        /// </value>
        [DataMember(Name="layers")]
        public LayerTableBase[] LayerInfos { get; set; }

        /// <summary>
        /// Gets or sets the table infos.
        /// </summary>
        /// <value>
        /// The table infos.
        /// </value>
        [DataMember(Name = "tables")]
        public LayerTableBase[] TableInfos { get; set; }
        #endregion
    }
}
