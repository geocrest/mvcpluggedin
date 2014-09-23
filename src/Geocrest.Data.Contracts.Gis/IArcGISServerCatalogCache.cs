
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
    }
}
