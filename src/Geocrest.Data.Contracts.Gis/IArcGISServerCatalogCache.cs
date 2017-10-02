
namespace Geocrest.Data.Contracts.Gis
{

    /// <summary>
    /// Provides a single method for retrieving an ArcGIS Server catalog representation from a cache.
    /// </summary>
    public interface IArcGISServerCatalogCache
    {
        /// <summary>
        /// Gets the catalog representing the resource at the specified URL.
        /// </summary>
        /// <param name="url">The URL to the ArcGIS Server catalog.</param>
        /// <returns>
        /// Returns the previously created catalog from cache or the newly created catalog.
        /// </returns>
        IArcGISServerCatalog GetCatalog(string url);
        /// <summary>
        /// Gets the catalog representing the resource at the specified URL.
        /// </summary>
        /// <param name="url">The URL to the ArcGIS Server catalog.</param>
        /// <param name="proxyUrl">The URL to use as a proxy to the catalog.</param>
        /// <returns>
        /// Returns the previously created catalog from cache or the newly created catalog.
        /// </returns>
        IArcGISServerCatalog GetCatalog(string url, string proxyUrl);
        /// <summary>
        /// Gets the catalog representing the resource at the specified URL.
        /// </summary>
        /// <param name="url">The URL to the ArcGIS Server catalog.</param>
        /// <param name="username">The username used to access secure services.</param>
        /// <param name="password">The password used to access secure services.</param>
        /// <returns>
        /// Returns the previously created catalog from cache or the newly created catalog.
        /// </returns>
        IArcGISServerCatalog GetCatalog(string url, string username, string password);
        /// <summary>
        /// Gets the service representing the resource at the specified URL.
        /// </summary>
        /// <param name="url">The URL to the ArcGIS Server service.</param>
        /// <param name="username">The username used to access secure services.</param>
        /// <param name="password">The password used to access secure services.</param>
        /// <returns>
        /// Returns the populated service.
        /// </returns>
        IArcGISService GetService(string url, string username, string password);
        /// <summary>
        /// Gets the service representing the resource at the specified URL.
        /// </summary>
        /// <param name="url">The URL to the ArcGIS Server service.</param>
        /// <returns>
        /// Returns the populated service.
        /// </returns>
        IArcGISService GetService(string url);
    }
}
