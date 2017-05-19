
namespace Geocrest.Data.Contracts.Gis
{
    using Geocrest.Model;
    using Geocrest.Model.ArcGIS;
    /// <summary>
    /// Provides access to common ArcGIS service properties
    /// </summary>    
    public interface IArcGISService
    {
        /// <summary>
        /// Gets or sets the service description.
        /// </summary>
        /// <value>
        /// The service description.
        /// </value>
        string ServiceDescription { get; set; }
        /// <summary>
        /// Gets the REST endpoint where the service can be found.
        /// </summary>
        /// <value>
        /// The URL to the service.
        /// </value>
        string Url { get; }
        /// <summary>
        /// Gets the name of the service.
        /// </summary>
        string Name { get; }
        /// <summary>
        /// Gets or sets the rest helper used for hydration of objects.
        /// </summary>
        /// <value>
        /// The rest helper.
        /// </value>
        IRestHelper RestHelper { get; set; }
        /// <summary>
        /// Gets the type of the service.
        /// </summary>
        /// <value>
        /// The type of the service.
        /// </value>
        serviceType ServiceType { get; }
        /// <summary>
        /// Gets or sets the URL through which all requests will be proxied.
        /// </summary>
        /// <value>
        /// The fully-qualified proxy URL.
        /// </value>
        string ProxyUrl { get; set; }
        /// <summary>
        /// Gets or sets the token used to access secure services.
        /// </summary>
        /// <value>
        /// The token.
        /// </value>
        string Token { get; set; }
        /// <summary>
        /// Gets or sets the current version of the ArcGIS Server instance.
        /// </summary>
        /// <value>
        /// The current version.
        /// </value>
        double? CurrentVersion { get; set; }      
        /// <summary>
        /// Determines whether the service's existing token is valid. If no token exists the method will return 
        /// true to indicate that the service can be accessed as-is.
        /// </summary>
        /// <returns>Whether the existing token is still valid or not.</returns>
        bool IsTokenValid();
    }
}
