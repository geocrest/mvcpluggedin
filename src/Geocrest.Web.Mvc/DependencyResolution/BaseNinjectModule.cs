namespace Geocrest.Web.Mvc.DependencyResolution
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Web.Http;
    using System.Web.Http.Dispatcher;
    using Ninject.Modules;
    using Ninject.Syntax;
    using Ninject.Web.Common;
    using Geocrest.Web.Infrastructure;
    using Geocrest.Web.Mvc.Configuration;
    using Geocrest.Web.Mvc.Controllers;
    /// <summary>
    /// Sets dependency injection bindings required by a module host when the Ninject kernel loads.
    /// </summary>
    /// <remarks>
    /// This class declares Ninject bindings that depend on the locations of data and services. These are 
    /// stored in the configuration file of each module host.
    /// </remarks>
    public abstract class BaseNinjectModule : NinjectModule
    {
        /// <summary>
        /// Loads the module into the kernel.
        /// </summary>
        public override void Load()
        {
            // Get configuration specific to each module host
            BaseConfiguration config = GetConfigurationForModule();
            if (config == null)
                Throw.ConfigurationErrors("Could not load configuration data for the module.");           

            // Get bindings
            foreach (BindingConfigurationElement binding in config.Injection.Bindings)
            {
                var b = BindType(binding);
                
                if (binding.IsNamed) b = (IBindingNamedWithOrOnSyntax<object>)b.Named(binding.Name);
                foreach (NameValueConfigurationElement arg in binding.ConstructorArguments)
                {
                    b = (IBindingNamedWithOrOnSyntax<object>)b.WithConstructorArgument(arg.Name, arg.Value);
                }
            }

            // Get hypermedia mappings from configuration
            var mappings = new Dictionary<Type, string>();
            foreach (KeyValueConfigurationElement mapping in config.Hypermedia.Mappings)
            {
                var type = Type.GetType(mapping.Key);
                if (type != null)
                    mappings.Add(type, mapping.Value);
            }

            this.LoadMappings(mappings);           
        }

        /// <summary>
        /// Loads the hypermedia mappings that are specified in each module host's web configuration file. 
        /// The default implementation of this method does nothing. It is overridden here so that the 
        /// module can set mappings for a specific class that provides hypermedia mappings.
        /// </summary>
        protected virtual void LoadMappings(Dictionary<Type, string> mappings){}

        /// <summary>
        /// Binds a service type to a concrete implementation type.
        /// </summary>
        /// <param name="binding">The binding containing the service and implementation types.</param>
        /// <returns>
        /// Returns the binding for the contract.
        /// </returns>
        protected abstract IBindingNamedWithOrOnSyntax<object> BindType(BindingConfigurationElement binding);

        /// <summary>
        /// Gets the web configuration for the specific module instance.
        /// </summary>
        /// <returns>
        /// Returns the configuration information contained within the running module host assembly.
        /// </returns>
        protected abstract BaseConfiguration GetConfigurationForModule();
    }
}
