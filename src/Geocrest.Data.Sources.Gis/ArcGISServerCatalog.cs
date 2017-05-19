namespace Geocrest.Data.Sources.Gis
{
    using Geocrest.Data.Contracts.Gis;
    using Geocrest.Model.ArcGIS;
    using Model;
    using System.Runtime.Serialization;
    using System.Web;
    using Web.Infrastructure;

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

        /// <summary>
        /// Gets or sets the current token used to access this catalog.
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Gets or sets the rest helper used for hydration of objects.
        /// </summary>
        /// <value>
        /// The rest helper.
        /// </value>
        public IRestHelper RestHelper { get; set; }

        /// <summary>
        /// Determines whether the catalogs's existing token is valid. If no token exists the method will return
        /// true to indicate that the catalog can be accessed as-is.
        /// </summary>
        /// <returns>
        /// Whether the existing token is still valid or not.
        /// </returns>
        public bool IsTokenValid()
        {
            if (string.IsNullOrEmpty(this.Token))
            {
                return true;
            }
            else
            {
                Throw.If<IRestHelper>(this.RestHelper, x => x == null, "Unable to validate token because there is no rest helper defined.");
                try
                {
                    ArcGISServerCatalog catalog = this.RestHelper.Hydrate<ArcGISServerCatalog>(string.Format("{0}?token={1}", this.RootUrl, this.Token));
                    return catalog != null;                    
                }
                catch (HttpException ex)
                {
                    int code = ex.GetHttpCode();
                    return !(code == 498 || code == 499);
                }
            }
        }
        #endregion
    }
}
