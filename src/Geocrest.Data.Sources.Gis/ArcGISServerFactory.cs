namespace Geocrest.Data.Sources.Gis
{
    using System;
    using System.Collections.Generic;
    using Geocrest.Data.Contracts.Gis;
    using Geocrest.Web.Infrastructure;
    using Geocrest.Model;
    using Geocrest.Model.ArcGIS;

    /// <summary>
    /// Provides a method for instantiating a new instance of an 
    /// <see cref="T:Geocrest.Data.Sources.Gis.ArcGISServerCatalog"/>
    /// or <see cref="T:Geocrest.Data.Sources.Gis.ArcGISService"/>.
    /// </summary>
    public class ArcGISServerFactory : IArcGISServerFactory
    {
        private string JSON = "f=json";        

        //private int svcloaded = 0, totalcount = 0;
        private IRestHelper rest;
        private serviceType[] supportedtypes = new serviceType[] 
        { 
            serviceType.GeocodeServer,serviceType.GeometryServer,
            serviceType.GPServer, serviceType.MapServer
        };        
        //private List<ArcGISService> services;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Data.Sources.Gis.ArcGISServerFactory" /> class.
        /// </summary>
        /// <param name="resthelper">The rest helper used for hydration of objects.</param>
        public ArcGISServerFactory(IRestHelper resthelper)
        {
            Throw.IfArgumentNull(resthelper, "resthelper");
            this.rest = resthelper;
        }

        #region Properties
        /// <summary>
        /// Gets or sets the URL through which all requests will be proxied.
        /// </summary>
        /// <value>
        /// The fully-qualified proxy URL.
        /// </value>
        public string ProxyUrl { get; set; }        
        #endregion
        ///// <summary>
        ///// Creates a representation of the catalog object located at the input service endpoint.
        ///// </summary>
        ///// <param name="url">The url to the REST endpoint.</param>
        ///// <param name="callback">A callback action used to retrieve the result of the operation.</param>
        //public void CreateCatalogAsync(string url, Action<IArcGISServerCatalog> callback)
        //{
        //    Throw.IfArgumentNullOrEmpty(url, "url");
        //    Throw.IfArgumentNull(callback, "callback");
        //    Uri uri = new Uri(url);
        //    string rooturl = url = uri.GetLeftPart(UriPartial.Path);
        //    if (rooturl.EndsWith("/")) rooturl = rooturl.Substring(0, rooturl.Length - 1); 

        //    if (!url.EndsWith("/")) url += "/";
        //    if (!url.Contains("?")) url += "?";
        //    int len = url.IndexOf("?");
        //    url = url.Substring(0, len >0? len : url.Length);
        //    services = new List<ArcGISService>();
        //    svcloaded = totalcount = 0;
        //    // get root catalog
        //    this.rest.HydrateAsync<ArcGISServerCatalog>(url +"?"+ JSON, cb =>
        //        {
        //            if (cb != null)
        //            {
        //                cb.RootUrl = url;
        //                List<ArcGISServerCatalog> folders = new List<ArcGISServerCatalog>();
        //                int foldercnt = 0;
        //                // get a count each folder's services and add to the total count of services 
        //                // also, add the hydrated folders to a list for future enumeration
        //                foreach (var f in cb.Folders)
        //                {
        //                    this.rest.HydrateAsync<ArcGISServerCatalog>(url + f + "?"+ JSON, folder =>
        //                    {
        //                        if (folder != null)
        //                        {
        //                            foldercnt += 1;
        //                            totalcount += folder.ServiceInfos.Count(x => supportedtypes.Contains(x.Type));
        //                            folders.Add(folder);
        //                            folder.RootUrl = rooturl;

        //                            //if all folders have been retrieved get services within each folder
        //                            if (foldercnt == cb.Folders.Length)
        //                            {
        //                                foreach (var subcatalog in folders)
        //                                {
        //                                    GetServices(subcatalog, folderservices =>
        //                                    {
        //                                        // finally get top level services
        //                                        totalcount += cb.ServiceInfos.Count(x => supportedtypes.Contains(x.Type));
        //                                        GetServices(cb, toplevel =>
        //                                        {
        //                                            cb.Services = services.ToArray();
        //                                            callback(cb);
        //                                        });
        //                                    });
        //                                }
        //                            }
        //                        }
        //                    });
        //                }
        //            }
        //        });
        //}

        /// <summary>
        /// Creates a representation of the catalog object located at the input service endpoint.
        /// </summary>
        /// <param name="url">The url to the REST endpoint.</param>
        /// <returns>
        /// Returns an instance of <see cref="T:Geocrest.Data.Contracts.Gis.IArcGISServerCatalog">IArcGISServerCatalog</see>.
        /// </returns>
        public IArcGISServerCatalog CreateCatalog(string url)
        {
            Throw.IfArgumentNullOrEmpty(url, "url");
            Uri uri = new Uri(url);
            string rooturl = uri.GetLeftPart(UriPartial.Path);
            if (rooturl.EndsWith("/")) rooturl = rooturl.Substring(0, rooturl.Length - 1);                

            // get root catalog
            List<IArcGISService> services = new List<IArcGISService>();
            ArcGISServerCatalog catalog = this.rest.Hydrate<ArcGISServerCatalog>(
                !string.IsNullOrEmpty(this.ProxyUrl) ? this.ProxyUrl + "?" + url + "?" : url);            
            if (catalog != null)
            {
                (catalog as ArcGISServerCatalog).RootUrl = rooturl;
                string template = catalog.RootUrl + "/{0}/{1}";
                List<IArcGISServerCatalog> folders = new List<IArcGISServerCatalog>();
                foreach (var f in catalog.Folders)
                {
                    // need to catch any exceptions if a service folder is down 
                    // but allow the rest of the catalog to populate
                    IArcGISServerCatalog folder = null;
                    try
                    {
                        folder = CreateCatalog(rooturl + "/" + f);
                        //folder = this.rest.Hydrate<ArcGISServerCatalog>(url + f + "?" + JSON);
                    }
                    catch { }
                    if (folder != null)
                    {
                        folder.RootUrl = rooturl;
                        folders.Add(folder);
                        foreach (var service in folder.ServiceInfos)
                        {
                            services.AddIfNotNull<IArcGISService>(this.CreateService(string.Format(template,
                                service.Name, service.Type.ToString()) + "?" + JSON, folder.CurrentVersion));
                        }
                    }
                }               
                foreach(var service in catalog.ServiceInfos)
                {
                    services.Add(this.CreateService(string.Format(template,
                        service.Name.Contains("/") ? service.Name.Substring(service.Name.IndexOf("/") + 1) 
                        : service.Name,service.Type.ToString()) + "?" + JSON, catalog.CurrentVersion));
                }
                catalog.Services = services.ToArray();     
            }
            return catalog;
        }

        ///// <summary>
        ///// Creates a representation of the service object located at the input service endpoint.
        ///// </summary>
        ///// <param name="url">The url to the REST endpoint.</param>
        ///// <param name="callback">A callback action used to retrieve the result of the operation.</param>
        //public void CreateServiceAsync(string url, Action<IArcGISService> callback)
        //{
        //    Throw.IfArgumentNullOrEmpty(url, "url");
        //    Throw.IfArgumentNull(callback, "callback");
        //    if (!url.Contains("?")) url += "?" + JSON;
        //    else if (!url.Contains(JSON)) url += JSON;
        //    if (url.Contains(Enum.GetName(typeof(serviceType), serviceType.MapServer)))
        //        this.rest.HydrateAsync<MapServer>(url, cb => { callback((IArcGISService)ReturnService(cb, url, serviceType.MapServer)); });
        //    else if (url.Contains(Enum.GetName(typeof(serviceType), serviceType.GeocodeServer)))
        //        this.rest.HydrateAsync<GeocodeServer>(url, cb => { callback((IArcGISService)ReturnService(cb, url, serviceType.GeocodeServer)); });
        //    else if (url.Contains(Enum.GetName(typeof(serviceType), serviceType.GeometryServer)))
        //        this.rest.HydrateAsync<GeometryServer>(url, cb => { callback((IArcGISService)ReturnService(cb, url, serviceType.GeometryServer)); });
        //    else if (url.Contains(Enum.GetName(typeof(serviceType), serviceType.GPServer)))
        //        this.rest.HydrateAsync<GeoprocessingServer>(url, cb => { callback((IArcGISService)ReturnService(cb, url, serviceType.GPServer)); });
        //    else if (url.Contains(Enum.GetName(typeof(serviceType), serviceType.FeatureServer)))
        //        this.rest.HydrateAsync<FeatureServer>(url, cb => { callback((IArcGISService)ReturnService(cb, url, serviceType.FeatureServer)); });
        //}

        /// <summary>
        /// Creates a representation of the service object located at the input service endpoint.
        /// </summary>
        /// <param name="url">The url to the REST endpoint.</param>
        /// <param name="currentVersion">The current version of ArcGIS Server.</param>
        /// <returns>
        /// Returns an instance of <see cref="IArcGISService"/>.
        /// </returns>
        public IArcGISService CreateService(string url, double? currentVersion)
        {
            Throw.IfArgumentNullOrEmpty(url, "url");
            ReturnServiceToCaller returntosender = new ReturnServiceToCaller(ReturnService);
            if (!url.Contains("?")) url += "?" + JSON;
            else if (!url.Contains(JSON)) url += "&" + JSON;
            // need to catch any exceptions if a service is down 
            // but allow the rest of the catalog to populate
            string hydrateurl = !string.IsNullOrEmpty(this.ProxyUrl)
                ? this.ProxyUrl + "?" + url : url;
            try
            {
                if (url.Contains(Enum.GetName(typeof(serviceType), serviceType.MapServer)))
                    return returntosender((ArcGISService)this.rest.Hydrate<MapServer>(hydrateurl), url, serviceType.MapServer, currentVersion);
                if (url.Contains(Enum.GetName(typeof(serviceType), serviceType.GeocodeServer)))
                    return returntosender((ArcGISService)this.rest.Hydrate<GeocodeServer>(hydrateurl), url, serviceType.GeocodeServer, currentVersion);
                if (url.Contains(Enum.GetName(typeof(serviceType), serviceType.GPServer)))
                    return returntosender((ArcGISService)this.rest.Hydrate<GeoprocessingServer>(hydrateurl), url, serviceType.GPServer, currentVersion);
                if (url.Contains(Enum.GetName(typeof(serviceType), serviceType.GeometryServer)))
                    return returntosender((ArcGISService)this.rest.Hydrate<GeometryServer>(hydrateurl), url, serviceType.GeometryServer, currentVersion);
                if (url.Contains(Enum.GetName(typeof(serviceType), serviceType.FeatureServer)))
                    return returntosender((ArcGISService)this.rest.Hydrate<FeatureServer>(hydrateurl), url, serviceType.FeatureServer, currentVersion);
                if (url.Contains(Enum.GetName(typeof(serviceType), serviceType.MobileServer)))
                    return returntosender((ArcGISService)this.rest.Hydrate<MobileServer>(hydrateurl), url, serviceType.MobileServer, currentVersion);
            }
            catch { }
            return null;
        }
        /// <summary>
        /// Creates a representation of the service object located at the input service endpoint.
        /// </summary>
        /// <param name="url">The url to the REST endpoint.</param>
        /// <returns>
        /// Returns an instance of <see cref="IArcGISService" />.
        /// </returns>
        public IArcGISService CreateService(string url)
        {
            return this.CreateService(url, null);
        }
        //private void GetServices(IArcGISServerCatalog catalog, Action<IArcGISService[]> callback)
        //{
        //    Throw.IfArgumentNull(catalog, "catalog");
        //    Throw.IfArgumentNull(callback, "callback");
        //    string template = catalog.RootUrl + "/{0}/{1}";
        //    ReturnServicesToAction returnServices = new ReturnServicesToAction(ReturnServices);
        //    foreach (ArcGISServiceInfo info in catalog.ServiceInfos)
        //    {
        //        switch (info.Type)
        //        {
        //            case serviceType.FeatureServer:
        //            case serviceType.GeodataServer:
        //            case serviceType.GeometryServer:
        //                this.rest.HydrateAsync<GeometryServer>(
        //                    string.Format(template, info.Name, info.Type.ToString())+ "?" + JSON, cb =>
        //                    {
        //                        returnServices(cb, template, info, callback);
        //                    });
        //                break;
        //            case serviceType.GPServer:
        //                this.rest.HydrateAsync<GeoprocessingServer>(
        //                    string.Format(template, info.Name, info.Type.ToString()) + "?" + JSON, cb =>
        //                    {
        //                        returnServices(cb, template, info, callback);
        //                    });
        //                break;
        //            case serviceType.GeocodeServer:
        //                this.rest.HydrateAsync<GeocodeServer>(
        //                    string.Format(template, info.Name, info.Type.ToString()) + "?" + JSON, cb =>
        //                    {
        //                        returnServices(cb, template, info, callback);
        //                    });
        //                break;
        //            case serviceType.MapServer:
        //                this.rest.HydrateAsync<MapServer>(
        //                    string.Format(template, info.Name, info.Type.ToString()) + "?" + JSON, cb =>
        //                    {
        //                        returnServices(cb, template, info, callback);
        //                    });
        //                break;
        //        }

        //    }
        //}

        /// <summary>
        /// Returns the service.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="url">The URL.</param>
        /// <param name="type">The type.</param>
        /// <param name="version">The ArcGIS Server version.</param>
        /// <returns>
        /// Returns an instance of <see cref="IArcGISService"/>.
        /// </returns>
        private IArcGISService ReturnService(ArcGISService service, string url, serviceType type, double? version)
        {
            Throw.IfArgumentNull(service, "service");            
            url = url.Contains("?") ? url.Substring(0, url.IndexOf("?")) : url;
            if (url.EndsWith("/")) url = url.Substring(0, url.LastIndexOf("/"));
            string name = url.Substring(0, url.LastIndexOf("/"));
            name = name.Substring(name.LastIndexOf("/")+1);
            service.Name = name;            
            service.RestHelper = rest;
            service.Url = url;
            service.CurrentVersion = version;
            service.ProxyUrl = this.ProxyUrl;
            service.ServiceType = type;
            if (type == serviceType.GPServer) PopuplateGPTasks(service);
            return (IArcGISService)service;
        }

        /// <summary>
        /// Popuplates the GP tasks.
        /// </summary>
        /// <param name="service">The service.</param>
        private void PopuplateGPTasks(ArcGISService service)
        {
            if (!(service is GeoprocessingServer)) return;
            GeoprocessingServer gpserver = (GeoprocessingServer)service;
            gpserver.Tasks = new List<IGeoprocessingTask>();
            string url = service.Url.EndsWith("/") ? service.Url + "{0}" : service.Url + "/{0}";           
            foreach (string task in gpserver.TaskNames)
            {
                string taskurl = string.Format(url, Uri.EscapeDataString(task));
                string proxyurl = !string.IsNullOrEmpty(this.ProxyUrl)
                    ? this.ProxyUrl + "?" + taskurl + "?" + JSON : taskurl + "?" + JSON;
                GeoprocessingTask gptask = this.rest.Hydrate<GeoprocessingTask>(proxyurl);
                if (gptask != null)
                {
                    gptask.Url = taskurl;
                    gptask.RestHelper = rest;
                }
                gpserver.Tasks.Add(gptask);
            }
        }

        ///// <summary>
        ///// Returns the services to the input action only when all of the services in the catalog have been populated.
        ///// </summary>
        ///// <param name="service">The instantiated service object.</param>
        ///// <param name="unformattedurl">The unformatted url of the service that will be formatted using the input information.</param>
        ///// <param name="info">The service configuration info object used to format the url.</param>
        ///// <param name="callback">The action to call that returns all of the services.</param>
        //private void ReturnServices(ArcGISService service,
        //    string unformattedurl, ArcGISServiceInfo info, Action<IArcGISService[]> callback)
        //{
        //    svcloaded += 1;
        //    if (service != null)
        //    {
        //        service.RestHelper = rest;
        //        service.Name = info.Name.Substring(info.Name.IndexOf("/") + 1);
        //        service.Url = string.Format(unformattedurl, info.Name, info.Type.ToString());
        //        service.ServiceType = info.Type;
        //        if (info.Type == serviceType.GPServer) PopuplateGPTasks(service);
        //        services.Add(service);
        //    }
        //    if (svcloaded == totalcount) callback(services.ToArray());
        //}
        //delegate void ReturnServicesToAction(ArcGISService service, string unformattedurl, ArcGISServiceInfo info, Action<IArcGISService[]> callback);
   
        delegate IArcGISService ReturnServiceToCaller(ArcGISService service, string url, serviceType type, double? version);
    }
}
