
namespace Geocrest.Data.Sources.Gis
{
    using System;
    using System.Collections.Concurrent;
    using Geocrest.Data.Contracts.Gis;
    using Geocrest.Web.Infrastructure;

    /// <summary>
    /// Provides a thread-safe cache for retrieving ArcGIS Server catalog representations.
    /// </summary>
    public sealed class ArcGISServerCatalogCache : IArcGISServerCatalogCache
    {
        private static Lazy<ConcurrentDictionary<string, IArcGISServerCatalog>> _cache =
           new Lazy<ConcurrentDictionary<string, IArcGISServerCatalog>>(() =>
               new ConcurrentDictionary<string, IArcGISServerCatalog>());
        private IArcGISServerFactory factory;
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Data.Sources.Gis.ArcGISServerCatalogCache" /> struct.
        /// </summary>
        /// <param name="factory">The factory to use when creating the catalogs.</param>
        public ArcGISServerCatalogCache(IArcGISServerFactory factory)
        {
            Throw.IfArgumentNull(factory,"factory");
            this.factory = factory;
        }
        /// <summary>
        /// Gets the catalog representing the resource at the specified URL.
        /// </summary>
        /// <param name="url">The URL to the ArcGIS Server catalog.</param>
        /// <returns>
        /// Returns the previously created catalog from cache or the newly created catalog.
        /// </returns>
        public IArcGISServerCatalog GetCatalog(string url)
        {
            return GetCatalog(url, string.Empty);
        }
        /// <summary>
        /// Gets the catalog representing the resource at the specified URL.
        /// </summary>
        /// <param name="url">The URL to the ArcGIS Server catalog.</param>
        /// <param name="proxyUrl">The URL to use as a proxy to the catalog.</param>
        /// <returns>
        /// Returns the previously created catalog from cache or the newly created catalog.
        /// </returns>
        public IArcGISServerCatalog GetCatalog(string url, string proxyUrl)
        {
            Throw.IfArgumentNullOrEmpty(url, "url");
            Throw.IfArgumentNull(factory, "factory");
            IArcGISServerCatalog catalog;
            if (_cache.Value.TryGetValue(url.ToLower(), out catalog))
            {
                return catalog;
            }
            else
            {
                if (!string.IsNullOrEmpty(proxyUrl)) factory.ProxyUrl = proxyUrl;
                catalog = factory.CreateCatalog(url);
                Throw.If<IArcGISServerCatalog>(catalog, x => x == null, string.Format(
                    "Unable to create catalog from url '{0}'", url));
                _cache.Value.TryAdd(url.ToLower(), catalog);
                factory.ProxyUrl = null;
                return catalog;
            }
        }
    }
}
