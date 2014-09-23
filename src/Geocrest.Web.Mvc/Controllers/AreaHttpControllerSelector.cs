namespace Geocrest.Web.Mvc.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Web.Http;
    using System.Web.Http.Controllers;
    using System.Web.Http.Dispatcher;
    using System.Web.Http.Routing;
    using Geocrest.Web.Infrastructure;
    
    /// <summary>
    /// This class enables retrieval of an HttpControllerDescriptor for types located within MVC areas.
    /// </summary>
    public class AreaHttpControllerSelector  : DefaultHttpControllerSelector
    {
        private Dictionary<string, Type> apiControllerTypes;
        private const string AreaRouteVariableName = "area";
        private HttpConfiguration config;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Web.Mvc.Controllers.AreaHttpControllerSelector"/> class.
        /// </summary>
        /// <param name="configuration">The configuration object, typically from <see cref="P:System.Web.Http.GlobalConfiguration.Configuration"/>.</param>
        public AreaHttpControllerSelector(HttpConfiguration configuration)
            : base(configuration)
        {
            Throw.IfArgumentNull(configuration,"configuration");
            this.config = configuration;
        }


        /// <summary>
        /// Selects a <see cref="T:System.Web.Http.Controllers.HttpControllerDescriptor" /> for the given <see cref="T:System.Net.Http.HttpRequestMessage" />.
        /// </summary>
        /// <param name="request">The HTTP request message.</param>
        /// <returns>
        /// The <see cref="T:System.Web.Http.Controllers.HttpControllerDescriptor" /> instance for the given <see cref="T:System.Net.Http.HttpRequestMessage" />.
        /// </returns>
        /// <remarks>This override is used to control the selection of descriptors within areas.</remarks>
        public override HttpControllerDescriptor SelectController(HttpRequestMessage request)
        {
            var controllerName = base.GetControllerName(request);
            var controllerType = GetControllerType(request, controllerName);
            if (controllerType == null)
            {
                return null;
            }

            return new HttpControllerDescriptor(this.config, controllerName, controllerType);           
        }

        /// <summary>
        /// Gets the API controller types.
        /// </summary>
        private Dictionary<string, Type> ApiControllerTypes
        {
            get
            {
                if (this.apiControllerTypes != null)
                {
                    return this.apiControllerTypes;
                }

                var assemblies = AppDomain.CurrentDomain.GetAssemblies();

                this.apiControllerTypes = assemblies.SelectMany(a => a.GetTypes().Where(t =>
                    !t.IsAbstract && t.Name.EndsWith(ControllerSuffix) &&
                    typeof(IHttpController).IsAssignableFrom(t))).ToDictionary(t => t.FullName, t => t);

                return this.apiControllerTypes;
            }
        }

        /// <summary>
        /// Gets the type of the controller.
        /// </summary>
        /// <param name="request">The request message.</param>
        /// <param name="controllerName">Name of the controller.</param>
        private Type GetControllerType(HttpRequestMessage request, string controllerName)
        {
            IHttpRouteData routeData = request.GetRouteData();
            if (!routeData.Values.ContainsKey(AreaRouteVariableName))
            {
                return null;
            }

            var areaName = routeData.Values[AreaRouteVariableName].ToString().ToLower();
            if (string.IsNullOrEmpty(areaName))
            {
                return null;
            }

            return ApiControllerTypes.Where(t => t.Key.ToLower().Contains(string.Format(".{0}.", areaName))
                && t.Key.EndsWith(string.Format(".{0}{1}", controllerName, ControllerSuffix),
                StringComparison.OrdinalIgnoreCase)).Select(t => t.Value).FirstOrDefault();
        }
    }
}
