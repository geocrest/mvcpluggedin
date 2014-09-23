namespace Geocrest.Data.Contracts.Gis
{
    using System;

    /// <summary>
    /// Provides methods for instantiating new instances of ArcGIS Services.
    /// </summary>
    public interface IArcGISServerFactory
    {
        ///// <summary>
        ///// Creates a representation of the catalog object located at the input service endpoint.
        ///// </summary>
        ///// <param name="url">The url to the REST endpoint.</param>
        ///// <param name="callback">A callback action used to retrieve the result of the operation.</param>
        //void CreateCatalogAsync(string url, Action<IArcGISServerCatalog> callback);
        /// <summary>
        /// Creates a representation of the catalog object located at the input service endpoint.
        /// </summary>
        /// <param name="url">The url to the REST endpoint.</param>
        /// <returns>
        /// Returns an instance of <see cref="T:Geocrest.Data.Contracts.Gis.IArcGISServerCatalog">IArcGISServerCatalog</see>.
        /// </returns>
        IArcGISServerCatalog CreateCatalog(string url);
        /// <summary>
        /// Creates a representation of the service object located at the input service endpoint.
        /// </summary>
        /// <param name="url">The url to the REST endpoint.</param>
        /// <returns>
        /// Returns an instance of <see cref="IArcGISService"/>.
        /// </returns>
        IArcGISService CreateService(string url);
        /// <summary>
        /// Creates a representation of the service object located at the input service endpoint.
        /// </summary>
        /// <param name="url">The url to the REST endpoint.</param>
        /// <param name="currentVersion">The current version of ArcGIS Server.</param>
        /// <returns>
        /// Returns an instance of <see cref="IArcGISService"/>.
        /// </returns>
        IArcGISService CreateService(string url, double? currentVersion);
        ///// <summary>
        ///// Creates a representation of the service object located at the input service endpoint.
        ///// </summary>
        ///// <param name="url">The url to the REST endpoint.</param>
        ///// <param name="callback">A callback action used to retrieve the result of the operation.</param>
        //void CreateServiceAsync(string url, Action<IArcGISService> callback);
        /// <summary>
        /// Gets or sets the URL through which all requests will be proxied.
        /// </summary>
        /// <value>
        /// The fully-qualified proxy URL.
        /// </value>
        string ProxyUrl { get; set; }
    }
}
