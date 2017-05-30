namespace Geocrest.Data.Sources.Gis
{
    using Geocrest.Data.Contracts.Gis;
    using Geocrest.Model;
    using Geocrest.Model.ArcGIS;
    using Geocrest.Model.ArcGIS.Geometry;
    using Geocrest.Web.Infrastructure;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Web;

    /// <summary>
    /// Represents a generic ArcGIS Server service.
    /// </summary>
    /// <seealso cref="Geocrest.Data.Contracts.Gis.IArcGISService" />
    [DataContract(Namespace = Geocrest.XmlNamespaces.InfrastructureVersion1)]
    [KnownType(typeof(FeatureServer))]
    [KnownType(typeof(MapServer))]
    [KnownType(typeof(GeocodeServer))]
    [KnownType(typeof(GeoprocessingServer))]
    [KnownType(typeof(GeometryServer))]
    [KnownType(typeof(MobileServer))]
    public abstract class ArcGISService : IArcGISService
    {
        /// <summary>
        /// Gets the parameterized URL to submit to the server.
        /// </summary>
        /// <param name="operation">The operation to perform.</param>
        /// <param name="parameters">A collection of input parameters to submit to the server.</param>
        /// <returns>
        /// Returns an instance of <see cref="T:System.String"/>.
        /// </returns>
        protected internal Uri GetUrl(string operation, IDictionary<string, object> parameters)
        {
            Throw.IfArgumentNullOrEmpty(this.Url, "url");
            string baseurl = new Uri(this.Url).GetLeftPart(UriPartial.Path);
            if (baseurl.EndsWith("/")) baseurl = baseurl.Substring(0, baseurl.Length - 1);
            baseurl = !string.IsNullOrEmpty(operation) ? (baseurl + "/" + operation).ForceJsonFormat()
                : baseurl.ForceJsonFormat();

            string query = string.Empty;
            string pair = "&{0}={1}";
            foreach (var kvp in parameters)
            {
                string value = kvp.Value != null ? kvp.Value is WKID ? ((int)kvp.Value).ToString()
                    : kvp.Value.ToString() : string.Empty;
                query += string.Format(pair, kvp.Key, Uri.EscapeUriString(value));
            }
            if (!string.IsNullOrEmpty(this.Token))
            {
                query += string.Format(pair, "token", this.Token);
            }
            return new Uri(!string.IsNullOrEmpty(this.ProxyUrl)
                ? this.ProxyUrl + "?" + baseurl + query : baseurl + query);
        }
        /// <summary>
        /// Gets the parameterized URL to submit to the server.
        /// </summary>
        /// <param name="operation">The operation to perform.</param>
        /// <param name="parameters">A collection of input parameters to submit to the server.</param>
        /// <param name="layer">The layer on which to perform the operation.</param>
        /// <returns>
        /// Returns an instance of <see cref="T:System.String" />.
        /// </returns>
        protected internal Uri GetUrl(string operation, IDictionary<string, object> parameters, LayerTableBase layer)
        {
            Throw.IfArgumentNull(layer, "layer");
            var endpoint = GetUrl(operation, parameters);
            var uri = new UriBuilder(endpoint);
            uri.Path = uri.Path.Replace("/" + operation, string.Format("/{0}/{1}", layer.ID, operation));
            return uri.Uri;
        }
        #region IArcGISService Members

        /// <summary>
        /// Determines whether the service's existing token is valid. If no token exists the method will return
        /// true to indicate that the service can be accessed as-is.
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
                try
                {
                    ArcGISService service = null;
                    var url = string.Format("{0}?token={1}", Url, this.Token);
                    if (Url.Contains(Enum.GetName(typeof(serviceType), serviceType.MapServer)))
                        service = Geocrest.Model.RestHelper.HydrateObject<MapServer>(url);
                    if (Url.Contains(Enum.GetName(typeof(serviceType), serviceType.GeocodeServer)))
                        service = Geocrest.Model.RestHelper.HydrateObject<GeocodeServer>(url);
                    if (Url.Contains(Enum.GetName(typeof(serviceType), serviceType.GPServer)))
                        service = Geocrest.Model.RestHelper.HydrateObject<GeoprocessingServer>(url);
                    if (Url.Contains(Enum.GetName(typeof(serviceType), serviceType.GeometryServer)))
                        service = Geocrest.Model.RestHelper.HydrateObject<GeometryServer>(url);
                    if (Url.Contains(Enum.GetName(typeof(serviceType), serviceType.FeatureServer)))
                        service = Geocrest.Model.RestHelper.HydrateObject<FeatureServer>(url);
                    if (Url.Contains(Enum.GetName(typeof(serviceType), serviceType.MobileServer)))
                        service = Geocrest.Model.RestHelper.HydrateObject<MobileServer>(url);
                    return service != null;
                }
                catch (HttpException ex)
                {
                    int code = ex.GetHttpCode();
                    return !(code == 498 || code == 499);
                }
            }
        }

        /// <summary>
        /// Gets or sets the current version of the ArcGIS Server instance.
        /// </summary>
        /// <value>
        /// The current version.
        /// </value>
        [DataMember]
        public double? CurrentVersion { get; set; }
        /// <summary>
        /// Gets or sets the URL through which all requests will be proxied.
        /// </summary>
        /// <value>
        /// The fully-qualified proxy URL.
        /// </value>
        public string ProxyUrl { get; set; }
        /// <summary>
        /// Gets or sets the service description.
        /// </summary>
        /// <value>
        /// The service description.
        /// </value>
        [DataMember]
        public string ServiceDescription { get; set; }

        /// <summary>
        /// Gets the REST endpoint where the service can be found.
        /// </summary>
        /// <value>
        /// The URL to the service.
        /// </value>
        [DataMember]
        public string Url { get; internal set; }

        /// <summary>
        /// Gets the name of the service.
        /// </summary>
        [DataMember]
        public string Name { get; internal set; }

        /// <summary>
        /// Gets or sets the rest helper used for hydration of objects.
        /// </summary>
        /// <value>
        /// The rest helper.
        /// </value>
        [Obsolete("The instance property of RestHelper will be removed at version 3. Instead, use the static methods on RestHelper to avoid concurrent I/O errors.", false)]
        public IRestHelper RestHelper { get; set; }

        /// <summary>
        /// Gets the type of the service.
        /// </summary>
        /// <value>
        /// The type of the service.
        /// </value>
        public serviceType ServiceType { get; internal set; }

        /// <summary>
        /// Gets or sets the token used to access secure services.
        /// </summary>
        /// <value>
        /// The token.
        /// </value>
        public string Token { get; set; }
        #endregion
    }
}
