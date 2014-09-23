namespace Geocrest.Data.Sources.Gis
{
    using System.Runtime.Serialization;
    using Geocrest.Data.Contracts.Gis;
    using Geocrest.Model.ArcGIS;

    /// <summary>
    /// Represents a container for ArcGIS Server services.
    /// </summary>
    [DataContract(Namespace = Geocrest.XmlNamespaces.InfrastructureVersion1)]
    public class ArcGISServerCatalog : IArcGISServerCatalog
    {
        #region IArcGISServerCatalog Members
        /// <summary>
        /// Gets or sets the current version of the ArcGIS Server instance.
        /// </summary>
        /// <value>
        /// The current version.
        /// </value>
        [DataMember]
        public double CurrentVersion { get; set; }

        /// <summary>
        /// Gets or sets the folders.
        /// </summary>
        /// <value>
        /// The folders.
        /// </value>
        [DataMember]
        public string[] Folders { get; set; }

        /// <summary>
        /// Gets or sets the basic service information of each service contained within this catalog.
        /// </summary>
        /// <value>
        /// An array of name/type pairs.
        /// </value>
        [DataMember(Name = "services")]
        public ArcGISServiceInfo[] ServiceInfos { get; set; }
        
        /// <summary>
        /// Gets or sets the actual services contained within this catalog.
        /// </summary>
        /// <value>
        /// The ArcGIS services.
        /// </value>
        [DataMember]
        public IArcGISService[] Services { get; set; }

        /// <summary>
        /// Gets or sets the root URL to the ArcGIS Service instance.
        /// </summary>
        public string RootUrl { get; set; }
        #endregion        
    }    
}
