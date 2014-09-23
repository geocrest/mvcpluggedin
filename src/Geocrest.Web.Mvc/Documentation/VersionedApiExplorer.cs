namespace Geocrest.Web.Mvc.Documentation
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Globalization;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Formatting;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Web.Http;
    using System.Web.Http.Controllers;
    using System.Web.Http.Description;
    using System.Web.Http.Dispatcher;
    using System.Web.Http.Routing;
    using Geocrest.Web.Infrastructure;
    using Geocrest.Web.Mvc.Resources;

    /// <summary>
    /// Provides a collection of <see cref="T:System.Web.Http.Description.ApiDescription"/> instances for a
    /// given area and version number.
    /// </summary>
    public class VersionedApiExplorer : IVersionedApiExplorer
    {
        #region Fields
        private readonly HttpConfiguration configuration;
        private Lazy<Collection<System.Web.Http.Description.ApiDescription>> apiDescription;
        private const string ActionVariableName = "action";
        private const string ControllerVariableName = "controller";
        private static readonly Regex _actionVariableRegex = new Regex(String.Format(CultureInfo.CurrentCulture, "{{{0}}}", ActionVariableName), RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
        private static readonly Regex _controllerVariableRegex = new Regex(String.Format(CultureInfo.CurrentCulture, "{{{0}}}", ControllerVariableName), RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
        private string _area;
        private string _version;
        #endregion

        #region Ctor
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Web.Mvc.Documentation.VersionedApiExplorer"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <exception cref="T:System.ArgumentNullException">configuration</exception>
        public VersionedApiExplorer(HttpConfiguration configuration)
        {
            Throw.IfArgumentNull(configuration, "configuration");
            this.configuration = configuration;
            this.apiDescription = new Lazy<Collection<System.Web.Http.Description.ApiDescription>>(InitializeApiDescriptions);
        }
        #endregion

        #region IApiExplorer Members
        /// <summary>
        /// Gets the API descriptions.
        /// </summary>
        public Collection<System.Web.Http.Description.ApiDescription> ApiDescriptions
        {
            get { return this.apiDescription.Value; }
        }
        #endregion

        #region IVersionedApiExplorer Members
        /// <summary>
        /// Gets or sets the area within which to explore. If this property is not set, the entire space will be searched.
        /// </summary>
        public string Area 
        {
            get { return this._area; }
            set 
            {
                this._area = value;
                this.apiDescription = new Lazy<Collection<System.Web.Http.Description.ApiDescription>>(this.InitializeApiDescriptions);
            }
        }
        /// <summary>
        /// Gets the version for which to return <see cref="T:System.Web.Http.Description.ApiDescription" /> instances.
        /// </summary>
        public string Version 
        {
            get 
            {
                if (!this.apiDescription.IsValueCreated)
                    SetVersion();
                return this._version; 
            }
        }
        /// <summary>
        /// Sets the version to an explicitly defined version.
        /// </summary>
        /// <param name="version">The version.</param>
        /// <exception cref="T:System.ArgumentNullException">version</exception>
        /// <exception cref="T:System.ArgumentException">Invalid value for version number</exception>
        public void SetVersion(string version)
        {
            Throw.IfArgumentNullOrEmpty(version, "version");
            if (version.IsValidVersionNumber())
            {
                this._version = version;
                this.apiDescription = new Lazy<Collection<System.Web.Http.Description.ApiDescription>>(InitializeApiDescriptions);
            }
            else
                Throw.ArgumentException("Invalid value for version number");
        }
        #endregion

        #region Public Class Members
        /// <summary>
        /// Gets or sets the documentation provider. The provider will be responsible for documenting the API.
        /// </summary>
        /// <value>
        /// The documentation provider.
        /// </value>
        public IDocumentationProvider DocumentationProvider { get; set; }
        
        /// <summary>
        /// Determines whether the action should be considered for 
        /// <see cref="P:System.Web.Http.Description.IApiExplorer.ApiDescriptions"/> generation. 
        /// Called when initializing the <see cref="P:System.Web.Http.Description.IApiExplorer.ApiDescriptions"/>.
        /// </summary>
        /// <param name="actionVariableValue">The action variable value from the route.</param>
        /// <param name="actionDescriptor">The action descriptor.</param>
        /// <param name="route">The route.</param>
        /// <returns><b>true</b> if the action should be considered for 
        /// <see cref="P:System.Web.Http.Description.IApiExplorer.ApiDescriptions"/> generation, <c>false</c> otherwise.</returns>
        /// <exception cref="T:System.ArgumentNullException">actionDescriptor</exception>
        /// <exception cref="T:System.ArgumentNullException">route</exception>
        public virtual bool ShouldExploreAction(string actionVariableValue, HttpActionDescriptor actionDescriptor, IHttpRoute route)
        {
            Throw.IfArgumentNull(actionDescriptor, "actionDescriptor");
            Throw.IfArgumentNull(route, "route");
            ApiExplorerSettingsAttribute setting = actionDescriptor.GetCustomAttributes<ApiExplorerSettingsAttribute>().FirstOrDefault();
            NonActionAttribute nonAction = actionDescriptor.GetCustomAttributes<NonActionAttribute>().FirstOrDefault();
            return (setting == null || !setting.IgnoreApi) &&
                (nonAction == null) &&
                MatchRegexConstraint(route, ActionVariableName, actionVariableValue);
        }

        /// <summary>
        /// Determines whether the controller should be considered for <see cref="P:System.Web.Http.Description.IApiExplorer.ApiDescriptions" /> generation. Called when initializing the <see cref="P:System.Web.Http.Description.ApiExplorer.ApiDescriptions" />.
        /// </summary>
        /// <param name="controllerVariableValue">The controller variable value from the route.</param>
        /// <param name="controllerDescriptor">The controller descriptor.</param>
        /// <param name="route">The route.</param>
        /// <returns>
        /// true if the controller should be considered for <see cref="P:System.Web.Http.Description.IApiExplorer.ApiDescriptions" /> generation, false otherwise.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">controllerDescriptor</exception>
        /// <exception cref="T:System.ArgumentNullException">route</exception>
        public virtual bool ShouldExploreController(string controllerVariableValue,
            HttpControllerDescriptor controllerDescriptor, IHttpRoute route)
        {
            //if (string.IsNullOrEmpty(this.Area))
            //    return false;
            Throw.IfArgumentNull(controllerDescriptor, "controllerDescriptor");
            Throw.IfArgumentNull(route, "route");

            ApiExplorerSettingsAttribute setting = controllerDescriptor.GetCustomAttributes<ApiExplorerSettingsAttribute>().FirstOrDefault();
            var controllerType = controllerDescriptor.ControllerType.FullName;
            string area = route.Defaults.ContainsKey("area") ? route.Defaults["area"].ToString().ToLower() : string.Empty;
            bool shouldExplore = (setting == null || !setting.IgnoreApi) &&
                (string.IsNullOrEmpty(area) || string.IsNullOrEmpty(this.Area) ||
                area == this.Area.ToLower()) &&
                (controllerType.ToLower().Contains(string.Format(".{0}.", area)) &&
                controllerType.ToLower().EndsWith(string.Format(".{0}{1}", controllerDescriptor.ControllerName.ToLower(),
                DefaultHttpControllerSelector.ControllerSuffix.ToLower()))) &&
                MatchRegexConstraint(route, ControllerVariableName, controllerVariableValue) &&
                this._version == controllerDescriptor.Version();
            return shouldExplore;
        }

        /// <summary>
        /// Gets a collection of HttpMethods supported by the action. Called when initializing the 
        /// <see cref="P:System.Web.Http.Description.IApiExplorer.ApiDescriptions"/>.
        /// </summary>
        /// <param name="route">The route.</param>
        /// <param name="actionDescriptor">The action descriptor.</param>
        /// <returns>A collection of HttpMethods supported by the action.</returns>
        /// <exception cref="T:System.ArgumentNullException">route</exception>
        /// <exception cref="T:System.ArgumentNullException">actionDescriptor</exception>
        public virtual Collection<HttpMethod> GetHttpMethodsSupportedByAction(IHttpRoute route, HttpActionDescriptor actionDescriptor)
        {
            Throw.IfArgumentNull(route, "route");
            Throw.IfArgumentNull(actionDescriptor, "actionDescriptor");
           
            IList<HttpMethod> supportedMethods = new List<HttpMethod>();
            IList<HttpMethod> actionHttpMethods = actionDescriptor.SupportedHttpMethods;
            HttpMethodConstraint httpMethodConstraint = route.Constraints.Values.FirstOrDefault(c => typeof(HttpMethodConstraint).IsAssignableFrom(c.GetType())) as HttpMethodConstraint;

            if (httpMethodConstraint == null)            
                supportedMethods = actionHttpMethods;            
            else            
                supportedMethods = httpMethodConstraint.AllowedMethods.Intersect(actionHttpMethods).ToList();
            
            return new Collection<HttpMethod>(supportedMethods);
        }
        #endregion

        #region Private Methods
        private Collection<System.Web.Http.Description.ApiDescription> InitializeApiDescriptions()
        {
            Collection<System.Web.Http.Description.ApiDescription> apiDescriptions = new Collection<System.Web.Http.Description.ApiDescription>();
            var controllerSelector = configuration.Services.GetVersionedHttpControllerSelector();
            if (controllerSelector == null) 
                Throw.InvalidOperation(ExceptionStrings.RequiresIVersionedHttpControllerSelector);
            
            // set default version
            SetVersion();

            IDictionary<string, HttpControllerDescriptor> controllerMappings = controllerSelector.GetControllerMapping();
            if (controllerMappings != null)
            {
                foreach (var route in configuration.Routes)
                    ExploreRouteControllers(controllerMappings, route, apiDescriptions);
            }
            return apiDescriptions;
        }
        
        private void ExploreRouteControllers(IDictionary<string, HttpControllerDescriptor> controllerMappings, IHttpRoute route, Collection<System.Web.Http.Description.ApiDescription> apiDescriptions)
        {
            string routeTemplate = route.RouteTemplate;
            object controllerVariableValue;
            if (_controllerVariableRegex.IsMatch(routeTemplate))
            {
                // unbound controller variable, {controller}
                foreach (KeyValuePair<string, HttpControllerDescriptor> controllerMapping in controllerMappings)
                {
                    controllerVariableValue = controllerMapping.Key;
                    HttpControllerDescriptor controllerDescriptor = controllerMapping.Value;

                    if (this.ShouldExploreController(controllerVariableValue.ToString(), controllerDescriptor, route))
                    {
                        // expand {controller} variable
                        string expandedRouteTemplate = _controllerVariableRegex.Replace(routeTemplate, controllerVariableValue.ToString());
                        ExploreRouteActions(route, expandedRouteTemplate, controllerDescriptor, apiDescriptions);
                    }
                }
            }
            else
            {
                // bound controller variable, {controller = "controllerName"}
                if (route.Defaults.TryGetValue(ControllerVariableName, out controllerVariableValue))
                {
                    HttpControllerDescriptor controllerDescriptor;
                    if (controllerMappings.TryGetValue(controllerVariableValue.ToString(), out controllerDescriptor) && this.ShouldExploreController(controllerVariableValue.ToString(), controllerDescriptor, route))
                    {
                        ExploreRouteActions(route, routeTemplate, controllerDescriptor, apiDescriptions);
                    }
                }
            }
        }
        
        private void ExploreRouteActions(IHttpRoute route, string localPath, HttpControllerDescriptor controllerDescriptor, Collection<System.Web.Http.Description.ApiDescription> apiDescriptions)
        {
            ServicesContainer controllerServices = controllerDescriptor.Configuration.Services;
            ILookup<string, HttpActionDescriptor> actionMappings = controllerServices.GetActionSelector().GetActionMapping(controllerDescriptor);
            object actionVariableValue;
            if (actionMappings != null)
            {
                if (_actionVariableRegex.IsMatch(localPath))
                {
                    // unbound action variable, {action}
                    foreach (IGrouping<string, HttpActionDescriptor> actionMapping in actionMappings)
                    {
                        // expand {action} variable
                        actionVariableValue = actionMapping.Key;
                        string expandedLocalPath = _actionVariableRegex.Replace(localPath, actionVariableValue.ToString());
                        PopulateActionDescriptions(actionMapping, actionVariableValue.ToString(), route, expandedLocalPath, apiDescriptions);
                    }
                }
                else if (route.Defaults.TryGetValue(ActionVariableName, out actionVariableValue))
                {
                    // bound action variable, { action = "actionName" }
                    PopulateActionDescriptions(actionMappings[actionVariableValue.ToString()], actionVariableValue.ToString(), route, localPath, apiDescriptions);
                }
                else
                {
                    // no {action} specified, e.g. {controller}/{id}
                    foreach (IGrouping<string, HttpActionDescriptor> actionMapping in actionMappings)                    
                        PopulateActionDescriptions(actionMapping, null, route, localPath, apiDescriptions);                    
                }
            }
        }

        private void PopulateActionDescriptions(IEnumerable<HttpActionDescriptor> actionDescriptors, string actionVariableValue, IHttpRoute route, string localPath, Collection<System.Web.Http.Description.ApiDescription> apiDescriptions)
        {
            foreach (HttpActionDescriptor actionDescriptor in actionDescriptors)
            {
                if (this.ShouldExploreAction(actionVariableValue, actionDescriptor, route))                
                    PopulateActionDescriptions(actionDescriptor, route, localPath, apiDescriptions);                
            }
        }

        private void PopulateActionDescriptions(HttpActionDescriptor actionDescriptor, IHttpRoute route, string localPath, Collection<System.Web.Http.Description.ApiDescription> apiDescriptions)
        {
            string apiDocumentation = GetApiDocumentation(actionDescriptor);

            // parameters
            IList<System.Web.Http.Description.ApiParameterDescription> parameterDescriptions = CreateParameterDescriptions(actionDescriptor);

            // expand all parameter variables
            string finalPath;

            if (!TryExpandUriParameters(route, actionDescriptor, parameterDescriptions, out finalPath))            
                // the action cannot be reached due to parameter mismatch, e.g. routeTemplate = "/users/{name}" and GetUsers(id)
                return;
            
            // request formatters
            System.Web.Http.Description.ApiParameterDescription bodyParameter = parameterDescriptions.FirstOrDefault(description => description.Source == ApiParameterSource.FromBody);
            IEnumerable<MediaTypeFormatter> supportedRequestBodyFormatters = bodyParameter != null ?
                actionDescriptor.Configuration.Formatters.Where(f => f.CanReadType(bodyParameter.ParameterDescriptor.ParameterType)) :
                Enumerable.Empty<MediaTypeFormatter>();

            // response formatters
            Type returnType = actionDescriptor.ReturnType;
            IEnumerable<MediaTypeFormatter> supportedResponseFormatters = returnType != null ?
                actionDescriptor.Configuration.Formatters.Where(f => f.CanWriteType(returnType)) :
                Enumerable.Empty<MediaTypeFormatter>();

            // get HttpMethods supported by an action. Usually there is one HttpMethod per action but we allow multiple of them per action as well.
            IList<HttpMethod> supportedMethods = this.GetHttpMethodsSupportedByAction(route, actionDescriptor);

            foreach (HttpMethod method in supportedMethods)
            {
                var description = new System.Web.Http.Description.ApiDescription()
                {
                    Documentation = apiDocumentation,
                    HttpMethod = method,
                    RelativePath = finalPath,
                    ActionDescriptor = actionDescriptor,
                    Route = route
                };
                foreach (var mtf in supportedRequestBodyFormatters)
                    description.SupportedRequestBodyFormatters.Add(mtf);
                foreach (var mtf in supportedResponseFormatters)
                    description.SupportedResponseFormatters.Add(mtf);
                foreach (var par in parameterDescriptions)
                    description.ParameterDescriptions.Add(par);

                apiDescriptions.Add(description);
            }
        }
        private IList<System.Web.Http.Description.ApiParameterDescription> CreateParameterDescriptions(HttpActionDescriptor actionDescriptor)
        {
            IList<System.Web.Http.Description.ApiParameterDescription> parameterDescriptions = new List<System.Web.Http.Description.ApiParameterDescription>();
            HttpActionBinding actionBinding = GetActionBinding(actionDescriptor);

            // try get parameter binding information if available
            if (actionBinding != null)
            {
                HttpParameterBinding[] parameterBindings = actionBinding.ParameterBindings;
                if (parameterBindings != null)
                {
                    foreach (HttpParameterBinding parameter in parameterBindings)                    
                        parameterDescriptions.Add(CreateParameterDescriptionFromBinding(parameter));                    
                }
            }
            else
            {
                Collection<HttpParameterDescriptor> parameters = actionDescriptor.GetParameters();
                if (parameters != null)
                {
                    foreach (HttpParameterDescriptor parameter in parameters)                    
                        parameterDescriptions.Add(CreateParameterDescriptionFromDescriptor(parameter));                    
                }
            }

            return parameterDescriptions;
        }

        private System.Web.Http.Description.ApiParameterDescription CreateParameterDescriptionFromDescriptor(HttpParameterDescriptor parameter)
        {
            System.Web.Http.Description.ApiParameterDescription parameterDescription = new System.Web.Http.Description.ApiParameterDescription();
            parameterDescription.ParameterDescriptor = parameter;
            parameterDescription.Name = parameter.Prefix ?? parameter.ParameterName;
            parameterDescription.Documentation = GetApiParameterDocumentation(parameter);
            parameterDescription.Source = ApiParameterSource.Unknown;
            return parameterDescription;
        }

        private System.Web.Http.Description.ApiParameterDescription CreateParameterDescriptionFromBinding(HttpParameterBinding parameterBinding)
        {
            System.Web.Http.Description.ApiParameterDescription parameterDescription = CreateParameterDescriptionFromDescriptor(parameterBinding.Descriptor);
            if (parameterBinding.WillReadBody)
            {
                parameterDescription.Source = ApiParameterSource.FromBody;
            }
            else if (parameterBinding.WillReadUri())
            {
                parameterDescription.Source = ApiParameterSource.FromUri;
            }

            return parameterDescription;
        }

        private static bool TryExpandUriParameters(IHttpRoute route, HttpActionDescriptor actionDescriptor,
            ICollection<System.Web.Http.Description.ApiParameterDescription> parameterDescriptions, out string expandedRouteTemplate)
        {
            string template = route.RouteTemplate.Replace("*","");
            Dictionary<string, string> optParameterValuesForRoute = new Dictionary<string, string>();
            Dictionary<string, string> reqParameterValuesForRoute = new Dictionary<string, string>();
            var segments = template.Split(new[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
            var routeParams = segments.Where(x => x.StartsWith("{") && x.EndsWith("}"));
            StringBuilder paramString = new StringBuilder();
            foreach (var paramDescriptor in parameterDescriptions)
            {
                if (paramDescriptor.Source == ApiParameterSource.FromUri && paramDescriptor.ParameterDescriptor.IsOptional)                
                    optParameterValuesForRoute.Add(paramDescriptor.Name, "{" + paramDescriptor.Name + "}");                
                else if (paramDescriptor.Source == ApiParameterSource.FromUri && !paramDescriptor.ParameterDescriptor.IsOptional)
                    reqParameterValuesForRoute.Add(paramDescriptor.Name, "{" + paramDescriptor.Name + "}");
            }
            if (parameterDescriptions.Any(x => x.ParameterDescriptor.IsOptional))
            {
                //if (!actionDescriptor.SupportedHttpMethods.Contains(HttpMethod.Get))
                //    paramString.Append("/");
                //else
                    paramString.Append("?");

                foreach (var param in optParameterValuesForRoute)
                    paramString.AppendFormat("{2}{0}={1}", param.Key, param.Value, paramString.ToString().Length > 1 ? "&" : string.Empty);
            }

            expandedRouteTemplate = template.Replace("{version}", actionDescriptor.ControllerDescriptor.Version())
                .Replace("{controller}", actionDescriptor.ControllerDescriptor.ControllerName);
            if (expandedRouteTemplate.EndsWith("/")) expandedRouteTemplate = 
                expandedRouteTemplate.Substring(0, expandedRouteTemplate.Length -1);
            expandedRouteTemplate += paramString.ToString();

            foreach (var segment in routeParams)
            {
                if (!reqParameterValuesForRoute.ContainsValue(segment))
                    expandedRouteTemplate = expandedRouteTemplate.Replace(segment, "");
            }
            expandedRouteTemplate = expandedRouteTemplate.Replace("//", "/");
            return true;
        }

        private string GetApiDocumentation(HttpActionDescriptor actionDescriptor)
        {
            IDocumentationProvider documentationProvider = this.DocumentationProvider ?? actionDescriptor.Configuration.Services.GetDocumentationProvider();
            if (documentationProvider == null)            
                return "No documentation available.";            

            return documentationProvider.GetDocumentation(actionDescriptor);
        }

        private string GetApiParameterDocumentation(HttpParameterDescriptor parameterDescriptor)
        {
            IDocumentationProvider documentationProvider = this.DocumentationProvider ?? parameterDescriptor.Configuration.Services.GetDocumentationProvider();
            if (documentationProvider == null)            
                return "No documentation available.";            

            return documentationProvider.GetDocumentation(parameterDescriptor);
        }

        private static HttpActionBinding GetActionBinding(HttpActionDescriptor actionDescriptor)
        {
            HttpControllerDescriptor controllerDescriptor = actionDescriptor.ControllerDescriptor;
            if (controllerDescriptor == null)            
                return null;            

            ServicesContainer controllerServices = controllerDescriptor.Configuration.Services;
            IActionValueBinder actionValueBinder = controllerServices.GetActionValueBinder();
            HttpActionBinding actionBinding = actionValueBinder != null ? actionValueBinder.GetBinding(actionDescriptor) : null;
            return actionBinding;
        }

        private static bool MatchRegexConstraint(IHttpRoute route, string parameterName, string parameterValue)
        {
            IDictionary<string, object> constraints = route.Constraints;
            if (constraints != null)
            {
                object constraint;
                if (constraints.TryGetValue(parameterName, out constraint))
                {
                    // treat the constraint as a string which represents a Regex.
                    // note that we don't support custom constraint (IHttpRouteConstraint) because it might rely on the request and some runtime states
                    string constraintsRule = constraint as string;
                    if (constraintsRule != null)
                    {
                        string constraintsRegEx = "^(" + constraintsRule + ")$";
                        return parameterValue != null && Regex.IsMatch(parameterValue, constraintsRegEx, RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
                    }
                }
            }

            return true;
        }
        /// <summary>
        /// Sets the version to a default version but only if the current version is not set.
        /// </summary>
        private void SetVersion()
        {
            if (!string.IsNullOrEmpty(this._version)) return;
            var controllerSelector = configuration.Services.GetVersionedHttpControllerSelector();
            if (controllerSelector == null)
                Throw.InvalidOperation(ExceptionStrings.RequiresIVersionedHttpControllerSelector);

            if (!string.IsNullOrEmpty(this.Area))
            {
                // if a default version is specified and it exists within the area's versions then use it;
                // otherwise, if the area contains at least one version use the first one (the highest version)
                if (!string.IsNullOrEmpty(controllerSelector.DefaultVersion) &&
                    controllerSelector.AreaVersions[this.Area.ToLower()].Contains(controllerSelector.DefaultVersion))
                    this._version = controllerSelector.DefaultVersion;
                else if (controllerSelector.AreaVersions[this.Area.ToLower()].Count() > 0)
                    this._version = controllerSelector.AreaVersions[this.Area.ToLower()].First();
            }
            else // check for a matching version within the core versions (i.e. outside of an area)
            {
                // if a default version is specified and it exists within the core versions then use it;
                // otherwise, if the core contains at least one version use the first one (the highest version)
                if (!string.IsNullOrEmpty(controllerSelector.DefaultVersion) &&
                    controllerSelector.CoreVersions.Contains(controllerSelector.DefaultVersion))
                    this._version = controllerSelector.DefaultVersion;
                else if (controllerSelector.CoreVersions.Count() > 0)
                    this._version = controllerSelector.CoreVersions.First();
            }         
        }
        #endregion
    }   
}
