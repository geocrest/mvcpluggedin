namespace Geocrest.Web.Mvc.Controllers
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Web.Http;
    using System.Web.Http.Controllers;
    using System.Web.Http.Dispatcher;
    using System.Web.Http.Routing;
    using System.Web.Mvc;
    using Geocrest.Web.Infrastructure;
    using Geocrest.Web.Mvc.Resources;

    /// <summary>
    ///   Represents an <see cref="T:Geocrest.Web.Mvc.Controllers.IVersionedHttpControllerSelector" /> 
    ///   implementation that supports versioning and selects a controller based on versioning by convention 
    ///   (namespace.VMajor_Minor_Build_Revision.xController).
    ///   How the actual controller to be invoked is determined, is up to the derived class to implement.
    /// </summary>
    public abstract class VersionedControllerSelector : IVersionedHttpControllerSelector
    {
        #region Fields
        private static LockValue<string> _VersionPrefix = "V";
        private static LockValue<string> _ControllerSuffix = DefaultHttpControllerSelector.ControllerSuffix;
        private readonly HttpConfiguration _configuration;
        private readonly Lazy<ConcurrentDictionary<ControllerIdentification, HttpControllerDescriptor>> _controllerInfoCache;
        private readonly HttpControllerTypeCache _controllerTypeCache;
        private Lazy<IEnumerable<string>> _coreVersions;
        private Lazy<Dictionary<string, IEnumerable<string>>> _areaVersions;
        /// <summary>
        /// The convention-based key when referencing controllers
        /// </summary>
        protected const string ControllerKey = "controller";
        #endregion

        #region Static Methods
        /// <summary>
        /// Gets or sets the suffix in the Controller <see cref="T:System.Type" />'s name.
        /// </summary>
        /// <value>
        /// The controller suffix.
        /// </value>
        /// <exception cref="T:System.ArgumentNullException">value</exception>
        /// <exception cref="T:System.ArgumentException">value is empty</exception>
        /// <exception cref="T:System.InvalidOperationException">If the process has already run</exception>
        public static string ControllerSuffix
        {
            get { return _ControllerSuffix; }
            set
            {
                Throw.IfArgumentNull(value,"value");
                Throw.IfArgumentNullOrEmpty(value, "value", x =>
                    {
                        return new ArgumentException(String.Format(ExceptionStrings.CannotSetEmptyValue, "ControllerSuffix"), "value");
                    });
                Throw.If<bool>(_ControllerSuffix.IsLocked, x => x == true,
                    String.Format(ExceptionStrings.ControllerDiscoveryProcessAlreadyRun, "ControllerSuffix"),
                    x =>
                    {
                        return new InvalidOperationException(x.Message);
                    });
               
                _ControllerSuffix = value;
            }
        }

        /// <summary>
        /// Gets or sets the prefix used for identifying a controller version in the <see cref="P:System.Type.FullName"/>. 
        /// Examples and usage in remarks.
        /// </summary>
        /// <remarks>
        ///  <para>
        ///     Make sure to set this property in the Application_Start method.
        /// </para>
        /// 
        ///  <para>
        ///     For example, when this is set to "V", a controller in the namespace of Company.V1.ProductController will identify the ProductController as being version 1, but will not identify 
        ///     Company.Version1.ProductController as being a version 1 controller.
        /// </para>
        /// </remarks>
        /// <exception cref="T:System.ArgumentNullException">value</exception>
        /// <exception cref="T:System.ArgumentException">value is empty</exception>
        /// <exception cref="T:System.InvalidOperationException">If the process has already run</exception>
        public static string VersionPrefix
        {
            get { return _VersionPrefix; }
            set
            {
                Throw.IfArgumentNull(value, "value");
                Throw.IfArgumentNullOrEmpty(value, "value", x =>
                {
                    return new ArgumentException(String.Format(ExceptionStrings.CannotSetEmptyValue, "VersionPrefix"), "value");
                });
                Throw.If<bool>(_ControllerSuffix.IsLocked, x => x == true,
                    String.Format(ExceptionStrings.ControllerDiscoveryProcessAlreadyRun, "VersionPrefix"),
                    x =>
                    {
                        return new InvalidOperationException(x.Message);
                    });
                
                _VersionPrefix = value;
            }
        }
        #endregion

        #region Ctor
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Web.Mvc.Controllers.VersionedControllerSelector"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <exception cref="T:System.ArgumentNullException">configuration</exception>
        protected VersionedControllerSelector(HttpConfiguration configuration)
        {
            Throw.IfArgumentNull(configuration, "configuration");            

            this._controllerInfoCache =
                    new Lazy<ConcurrentDictionary<ControllerIdentification, HttpControllerDescriptor>>(this.InitializeControllerInfoCache);
            this._configuration = configuration;
            this._controllerTypeCache = new HttpControllerTypeCache(this._configuration);
            this._areaVersions = new Lazy<Dictionary<string, IEnumerable<string>>>(this.InitializeAreaVersions);
            this._coreVersions = new Lazy<IEnumerable<string>>(this.InitializeCoreVersions);
        }
        #endregion

        #region IVersionedHttpControllerSelector Members

        /// <summary>
        /// Gets the API version number from the request.
        /// </summary>
        public string Version { get; protected set; }
        /// <summary>
        /// Gets or sets the default version to use when selecting controllers.
        /// </summary>
        public string DefaultVersion { get; set; }

        /// <summary>
        /// Gets a collection of all the versions contained within the core application
        /// while excluding any contained within MVC areas.
        /// </summary>
        public IEnumerable<string> CoreVersions
        {
            get
            {
                return this._coreVersions.Value;
            }
        }

        /// <summary>
        /// Gets a dictionary of all the versions contained within the application's areas keyed by area name.
        /// Note that this does not include versions contained outside of an area. To retrieve those
        /// versions, use the <see cref="P:Geocrest.Web.Mvc.Controllers.IVersionedHttpControllerSelector.CoreVersions" />
        /// property.
        /// </summary>
        public Dictionary<string,IEnumerable<string>> AreaVersions
        {
            get
            {
                return this._areaVersions.Value;
            }
        }       
        #endregion

        #region IHttpControllerSelector Members

        /// <summary>
        /// Selects a <see cref="T:System.Web.Http.Controllers.HttpControllerDescriptor" /> for the given <see cref="T:System.Net.Http.HttpRequestMessage" />.
        /// </summary>
        /// <param name="request">The request message.</param>
        /// <returns>
        /// An <see cref="T:System.Web.Http.Controllers.HttpControllerDescriptor" /> instance.
        /// </returns>
        /// <exception cref="T:System.Web.Http.HttpResponseException">The response status and reason should contain
        /// error information.
        /// </exception>
        /// <exception cref="T:System.ArgumentNullException">request</exception>
        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope",
                Justification = "Caller is responsible for disposing of response instance.")]
        public HttpControllerDescriptor SelectController(HttpRequestMessage request)
        {
            Throw.IfArgumentNull(request, "request");

            // reset version for each request
            this.Version = null;

            // set a default version if not already set
            //if (string.IsNullOrEmpty(this.DefaultVersion)) 
            //{
            //    // extract 'area' route value if requested
            //    string area = string.Empty;
            //    var routeData = request.GetRouteData();
            //    if (routeData != null && routeData.Values.ContainsKey("area") && routeData.Values["area"] != null)
            //        area = routeData.Values["area"].ToString(); 
            //    this.DefaultVersion = this.GetVersions(area).Count() > 0 ? this.GetVersions(area).First() : null;
            //}

            ControllerIdentification controllerName = this.GetControllerIdentificationFromRequest(request);
            if (String.IsNullOrEmpty(controllerName.Name))
                Throw.HttpResponse(request.CreateErrorResponse(HttpStatusCode.NotFound,
                    string.Format(ExceptionStrings.ResourceNotFound,request.RequestUri),
                    string.Format(ExceptionStrings.ControllerNameNotFound, request.RequestUri)));            

            HttpControllerDescriptor controllerDescriptor;
            if (this._controllerInfoCache.Value.TryGetValue(controllerName, out controllerDescriptor))            
                return controllerDescriptor;
            
            ICollection<Type> matchingTypes = this._controllerTypeCache.GetControllerTypes(controllerName);

            // ControllerInfoCache is already initialized.
            Contract.Assert(matchingTypes.Count != 1);

            if (matchingTypes.Count == 0)
            {
                // no matching types                
                Throw.HttpResponse(request.CreateErrorResponse(HttpStatusCode.NotFound,
                    string.Format(ExceptionStrings.ResourceNotFound, request.RequestUri),
                    string.Format(ExceptionStrings.DefaultControllerFactory_ControllerNameNotFound, 
                    controllerName.Name)));
            }
            else
            {
                // multiple matching types
                Throw.Generic(CreateAmbiguousControllerException(request.GetRouteData().Route, controllerName.Name, matchingTypes));
            }

            // Throw.HttpResponse does not return a value so we have to return null here
            return null;
        }

        /// <summary>
        /// Returns a map, keyed by controller string, of all <see cref="T:System.Web.Http.Controllers.HttpControllerDescriptor" /> that the selector can select.  This is primarily called by <see cref="T:System.Web.Http.Description.IApiExplorer" /> to discover all the possible controllers in the system.
        /// </summary>
        /// <returns>
        /// A map of all <see cref="T:System.Web.Http.Controllers.HttpControllerDescriptor" /> that the selector can select, or null if the selector does not have a well-defined mapping of <see cref="T:System.Web.Http.Controllers.HttpControllerDescriptor" />.
        /// </returns>
        public IDictionary<string, HttpControllerDescriptor> GetControllerMapping()
        {
            return this._controllerInfoCache.Value.ToDictionary(c => VersionPrefix + c.Key.Version + "." + c.Key.Name, c => c.Value, StringComparer.OrdinalIgnoreCase);
        }

        #endregion        
        
        #region Protected Methods
        /// <summary>
        /// Gets the controller identification from the incoming request.
        /// </summary>
        /// <param name="request">The request containing information about the desired version.</param>
        /// <returns>
        /// The identification information about the controller from the request.
        /// </returns>
        protected abstract ControllerIdentification GetControllerIdentificationFromRequest(HttpRequestMessage request);
        /// <summary> 
        /// Gets the name of the controller from the request route data
        /// </summary>
        /// <param name="request">The incoming request message.</param>
        /// <returns>The name of the controller matching the request.</returns>
        /// <exception cref="T:System.ArgumentNullException">request</exception>
        protected string GetControllerNameFromRequest(HttpRequestMessage request)
        {
            Throw.IfArgumentNull(request, "request");
            IHttpRouteData routeData = request.GetRouteData();
            if (routeData == null)            
                return default(String);            

            // Look up controller in route data
            object controllerName;
            routeData.Values.TryGetValue(ControllerKey, out controllerName);

            return controllerName.ToString() ?? string.Empty;
        }

        /// <summary>
        /// Retrieves a list of all versions by examining each entry in this instance's controller mappings.
        /// </summary>
        /// <param name="controllerMappings">The controller mappings.</param>
        /// <param name="area">The optional area for which to get versions.</param>
        /// <returns>
        /// Returns the versions in the system.
        /// </returns>
        protected IEnumerable<string> GetVersions(IDictionary<string, HttpControllerDescriptor> controllerMappings,
            string area = "")
        {
            List<string> versions = new List<string>();
            //List<string> areaversions = new List<string>();
            foreach (var controller in controllerMappings)
            {
                string fullName = controller.Key;
                Type controllerType = controller.Value.ControllerType;
                string controllerName = fullName.Substring(fullName.LastIndexOf(".") + 1);
                string version = fullName.Replace("." + controllerName, "")
                    .Substring(VersionedControllerSelector.VersionPrefix.Length)
                    .Replace("_", ".");

                if (string.IsNullOrEmpty(version)) continue;
                                
                if (!string.IsNullOrEmpty(area) && 
                    version.IsValidVersionNumber() && 
                    !versions.Contains(version) &&
                    controllerType.FullName.ToLower().Contains(string.Format(".{0}.", area.ToLower())))
                {                    
                    versions.Add(version);
                }
                else if (string.IsNullOrEmpty(area) &&
                    version.IsValidVersionNumber() &&
                    !versions.Contains(version) &&
                    controllerType.FullName.ToLower().Contains(string.Format(VersionedControllerSelector.VersionPrefix + "{0}.{1}{2}",
                    version,controllerName,DefaultHttpControllerSelector.ControllerSuffix)))
                {
                    versions.Add(version);
                }               
            }           
            return versions.OrderByDescending(x => x).AsEnumerable();
        }

        #endregion

        #region Private Methods
        private Dictionary<string, IEnumerable<string>> InitializeAreaVersions()
        {
            IAssembliesResolver assembliesResolver = this._configuration.Services.GetAssembliesResolver();
            var areas = new List<AreaRegistration>();
            var mappings = this.GetControllerMapping();
            foreach(var a in assembliesResolver.GetAssemblies())
            {
                areas.AddRange(a.GetInstances<AreaRegistration>());
                areas.AddRange(a.GetInstances<ModuleRegistration>());
            }
            return areas.ToDictionary(a => a.AreaName.ToLower(), a => GetVersions(mappings,a.AreaName.ToLower()));
        }
        private IEnumerable<string> InitializeCoreVersions()
        {
            return GetVersions(this.GetControllerMapping(), string.Empty);
        }
        private static Exception CreateAmbiguousControllerException(IHttpRoute route, string controllerName,
                                                                         ICollection<Type> matchingTypes)
        {
            Contract.Assert(route != null);
            Contract.Assert(controllerName != null);
            Contract.Assert(matchingTypes != null);

            // Generate an exception containing all the controller types
            StringBuilder typeList = new StringBuilder();
            foreach (Type matchedType in matchingTypes)
            {
                typeList.AppendLine();
                typeList.Append(matchedType.FullName);
            }
            string errorMessage = String.Format(ExceptionStrings.DefaultControllerFactory_ControllerNameAmbiguous_WithRouteTemplate,
                controllerName, route.RouteTemplate, typeList, Environment.NewLine);
            return new InvalidOperationException(errorMessage);
        }

        private ConcurrentDictionary<ControllerIdentification, HttpControllerDescriptor> InitializeControllerInfoCache()
        {
            // lock dependend properties
            _VersionPrefix.Lock();

            // let's find and cache the found controllers
            var result = new ConcurrentDictionary<ControllerIdentification, HttpControllerDescriptor>(ControllerIdentification.Comparer);
            var duplicateControllers = new HashSet<ControllerIdentification>();
            Dictionary<ControllerIdentification, ILookup<string, Type>> controllerTypeGroups = this._controllerTypeCache.Cache;

            foreach (KeyValuePair<ControllerIdentification, ILookup<string, Type>> controllerTypeGroup in controllerTypeGroups)
            {
                ControllerIdentification controllerName = controllerTypeGroup.Key;
                foreach (IGrouping<string, Type> controllerTypesGroupedByNs in controllerTypeGroup.Value)
                {
                    foreach (Type controllerType in controllerTypesGroupedByNs)
                    {
                        if (result.Keys.Contains(controllerName))
                        {
                            duplicateControllers.Add(controllerName);
                            break;
                        }
                        else
                            result.TryAdd(controllerName, new HttpControllerDescriptor(this._configuration,
                                controllerName.Name, controllerType));
                    }
                }
            }
            foreach (ControllerIdentification duplicateController in duplicateControllers)
            {
                HttpControllerDescriptor descriptor;
                result.TryRemove(duplicateController, out descriptor);
            }
            return result;
        }
        #endregion
    }
}
