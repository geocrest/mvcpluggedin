namespace Geocrest.Web.Mvc.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http;
    using System.Web.Http.Dispatcher;
    using Geocrest.Web.Infrastructure;
    /// <summary>
    ///   Manages a cache of <see cref="T:System.Web.Http.Controllers.IHttpController" /> types detected in the system.
    /// </summary>
    internal sealed class HttpControllerTypeCache
    {
        private readonly Lazy<Dictionary<ControllerIdentification, ILookup<string, Type>>> _cache;
        private readonly HttpConfiguration _configuration;

        /// <summary>
        /// Gets the cache of controller types.
        /// </summary>
        internal Dictionary<ControllerIdentification, ILookup<string, Type>> Cache
        {
            get { return this._cache.Value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Web.Mvc.Controllers.HttpControllerTypeCache"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <exception cref="T:System.ArgumentNullException">configuration</exception>
        public HttpControllerTypeCache(HttpConfiguration configuration)
        {
            Throw.IfArgumentNull(configuration, "configuration");
            this._configuration = configuration;
            this._cache = new Lazy<Dictionary<ControllerIdentification, ILookup<string, Type>>>(this.InitializeCache);
        }

        /// <summary>
        /// Gets all controller types within the system matching the specified controller identification.
        /// </summary>
        /// <param name="controllerName">Name of the controller.</param>
        /// <returns>
        /// Returns an <see cref="T:System.Collections.Generic.ICollection`1" /> of controller types.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        /// controllername
        /// or
        /// <see cref="P:Geocrest.Web.Mvc.Controllers.ControllerIdentification.Name"/> is null or empty
        /// </exception>
        public ICollection<Type> GetControllerTypes(ControllerIdentification controllerName)
        {
            Throw.IfArgumentNull(controllerName, "controllerName");
            Throw.IfArgumentNullOrEmpty(controllerName.Name, "controllerName");
           
            var matchingTypes = new HashSet<Type>();

            ILookup<string, Type> namespaceLookup;
            if (this._cache.Value.TryGetValue(controllerName, out namespaceLookup))
            {
                foreach (var namespaceGroup in namespaceLookup)
                {
                    matchingTypes.UnionWith(namespaceGroup);
                }
            }

            return matchingTypes;
        }

        private Dictionary<ControllerIdentification, ILookup<string, Type>> InitializeCache()
        {
            IAssembliesResolver assembliesResolver = this._configuration.Services.GetAssembliesResolver();
            IHttpControllerTypeResolver controllersResolver = this._configuration.Services.GetHttpControllerTypeResolver();

            ICollection<Type> controllerTypes = controllersResolver.GetControllerTypes(assembliesResolver);
            IEnumerable<IGrouping<ControllerIdentification, Type>> groupedByName = controllerTypes.GroupBy(
                GetControllerName, ControllerIdentification.Comparer);

            return groupedByName.ToDictionary(g => g.Key, g => g.ToLookup(t => t.Namespace ?? String.Empty, 
                StringComparer.OrdinalIgnoreCase), ControllerIdentification.Comparer);
        }

        private static ControllerIdentification GetControllerName(Type type)
        {
            string fullName = type.FullName;            
            fullName = fullName.Substring(0, fullName.Length - VersionedControllerSelector.ControllerSuffix.Length);

            // split by dot and find version
            string[] nameSplit = fullName.Split('.');

            string name = nameSplit[nameSplit.Length - 1]; // same as Type.Name
            string version = null;

            for (int i = nameSplit.Length - 2; i >= 0; i--)
            {
                string namePart = nameSplit[i];
                if (!namePart.StartsWith(VersionedControllerSelector.VersionPrefix, StringComparison.InvariantCultureIgnoreCase))
                {
                    continue;
                }

                string versionNumberStr = namePart.Substring(VersionedControllerSelector.VersionPrefix.Length);
                versionNumberStr = versionNumberStr.Replace("_",".");
                if (versionNumberStr.IsValidVersionNumber())
                {
                    version = versionNumberStr;
                    break; 
                }
            }

            return new ControllerIdentification(name, version);
        }
    }
}
