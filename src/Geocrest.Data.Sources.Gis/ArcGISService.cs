namespace Geocrest.Data.Sources.Gis
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using Geocrest.Data.Contracts.Gis;
    using Geocrest.Web.Infrastructure;
    using Geocrest.Model;
    using Geocrest.Model.ArcGIS;
    using Geocrest.Model.ArcGIS.Geometry;

    /// <summary>
    /// Represents a generic ArcGIS Server service.
    /// </summary>
    [DataContract(Namespace = Geocrest.XmlNamespaces.InfrastructureVersion1)]
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

            return new Uri(!string.IsNullOrEmpty(this.ProxyUrl)
                ? this.ProxyUrl + "?" + baseurl + query : baseurl + query);
        }

        #region IArcGISService Members
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
        public string ServiceDescription{get;set;}

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
        public IRestHelper RestHelper { get; set; }

        /// <summary>
        /// Gets the type of the service.
        /// </summary>
        /// <value>
        /// The type of the service.
        /// </value>
        public serviceType ServiceType { get; internal set; }
        #endregion
    }
}
