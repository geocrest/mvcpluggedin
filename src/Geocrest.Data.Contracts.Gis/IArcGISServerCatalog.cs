
namespace Geocrest.Data.Contracts.Gis
{
    using Geocrest.Model.ArcGIS;
    /// <summary>
    /// Provides access to an ArcGIS Server's services.
    /// </summary>
    public interface IArcGISServerCatalog
    {
        /// <summary>
        /// Gets or sets the current version of the ArcGIS Server instance.
        /// </summary>
        /// <value>
        /// The current version.
        /// </value>
        double CurrentVersion { get; set; }
        /// <summary>
        /// Gets or sets the folders.
        /// </summary>
        /// <value>
        /// The folders.
        /// </value>
        string[] Folders { get; set; }
        /// <summary>
        /// Gets or sets the basic service information of each service contained within this catalog.
        /// </summary>
        /// <value>
        /// An array of name/type pairs.
        /// </value>
        ArcGISServiceInfo[] ServiceInfos { get; set; }
        /// <summary>
        /// Gets or sets the actual services contained within this catalog.
        /// </summary>
        /// <value>
        /// The ArcGIS services.
        /// </value>
        IArcGISService[] Services { get; set; }
        /// <summary>
        /// Gets the root URL to the ArcGIS Service instance.
        /// </summary>
        string RootUrl { get; set; }
    }    
}
