
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
        private static Lazy<ConcurrentDictionary<string, IArcGISService>> _serviceCache =
           new Lazy<ConcurrentDictionary<string, IArcGISService>>(() =>
               new ConcurrentDictionary<string, IArcGISService>());
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
                this.CacheServices(catalog);
                return catalog;
            }
        }

        /// <summary>
        /// Gets the catalog representing the resource at the specified URL.
        /// </summary>
        /// <param name="url">The URL to the ArcGIS Server catalog.</param>
        /// <param name="username">The username used to access secure services.</param>
        /// <param name="password">The password used to access secure services.</param>
        /// <returns>
        /// Returns the previously created catalog from cache or the newly created catalog.
        /// </returns>
        public IArcGISServerCatalog GetCatalog(string url, string username, string password)
        {
            Throw.IfArgumentNullOrEmpty(url, "url");
            Throw.IfArgumentNullOrEmpty(username, "username");
            Throw.IfArgumentNullOrEmpty(password, "password");
            IArcGISServerCatalog catalog;
            if (_cache.Value.TryGetValue(url.ToLower(), out catalog))
            {
                if (!catalog.IsTokenValid())
                {
                    catalog.Token = ArcGISServerFactory.CreateToken(url, username, password);
                    foreach(var service in catalog.Services)
                    {
                        service.Token = catalog.Token;
                    }
                    this.CacheServices(catalog);
                    _cache.Value.TryUpdate(url.ToLower(), catalog, catalog);
                }
                return catalog;
            }
            else
            {
                catalog = factory.CreateCatalog(url, username, password);
                Throw.If<IArcGISServerCatalog>(catalog, x => x == null, string.Format(
                    "Unable to create catalog from url '{0}'", url));
                _cache.Value.TryAdd(url.ToLower(), catalog);
                this.CacheServices(catalog);
                return catalog;
            }
        }

        /// <summary>
        /// Gets the service representing the resource at the specified URL.
        /// </summary>
        /// <param name="url">The URL to the ArcGIS Server service.</param>
        /// <returns>
        /// Returns the populated service.
        /// </returns>
        public IArcGISService GetService(string url)
        {
            Throw.IfArgumentNullOrEmpty(url, "url");
            IArcGISService service;
            if (_serviceCache.Value.TryGetValue(url.ToLower(), out service))
            {                
                return service;
            }
            else
            {
                service = factory.CreateService(url);
                Throw.If<IArcGISService>(service, x => x == null, string.Format(
                    "Unable to create service from url '{0}'", url));
                _serviceCache.Value.TryAdd(url.ToLower(), service);
                return service;
            }
        }

        /// <summary>
        /// Gets the service representing the resource at the specified URL.
        /// </summary>
        /// <param name="url">The URL to the ArcGIS Server service.</param>
        /// <param name="username">The username used to access secure services.</param>
        /// <param name="password">The password used to access secure services.</param>
        /// <returns>
        /// Returns the populated service.
        /// </returns>
        public IArcGISService GetService(string url, string username, string password)
        {
            Throw.IfArgumentNullOrEmpty(url, "url");
            Throw.IfArgumentNullOrEmpty(username, "username");
            Throw.IfArgumentNullOrEmpty(password, "password");
            IArcGISService service;
            if (_serviceCache.Value.TryGetValue(url.ToLower(), out service))
            {
                if (!service.IsTokenValid())
                {
                    service.Token = ArcGISServerFactory.CreateToken(url, username, password);
                }
                return service;
            }
            else
            {
                service = factory.CreateService(url, username, password);
                Throw.If<IArcGISService>(service, x => x == null, string.Format(
                    "Unable to create service from url '{0}'", url));
                _serviceCache.Value.TryAdd(url.ToLower(), service);
                return service;
            }
        }
        private void CacheServices(IArcGISServerCatalog catalog)
        {
            foreach(var service in catalog.Services)
            {
                _serviceCache.Value.AddOrUpdate(service.Url.ToLower(), service, (key, value) =>
                {
                    return service;
                });
            }
        }        
    }
}
