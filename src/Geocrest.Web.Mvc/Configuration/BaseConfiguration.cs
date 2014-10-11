
namespace Geocrest.Web.Mvc.Configuration
{
    using System.Configuration;
    using System.Reflection;
    using System.Web;
    using System.Web.Configuration;
    /// <summary>
    /// Represents the main configuration section for specifying information within a configuration file.
    /// </summary>
    public class BaseConfiguration : ConfigurationSection
    {
        #region Fields
        private const string DefaultPluginSection = "mvcpluggedin"; //"gisi";
        private const string InjectionElement = "injection";
        private const string HypermediaElement = "hyperMedia";
        private const string TokensElement = "tokens";
        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the injection element.
        /// </summary>
        /// <value>
        /// The injection element.
        /// </value>
        [ConfigurationProperty(InjectionElement)]
        public InjectionConfigurationElement Injection
        {
            get { return (InjectionConfigurationElement)this[InjectionElement]; }
            set { this[InjectionElement] = value; }
        }
        /// <summary>
        /// Gets or sets the tokens collection.
        /// </summary>
        /// <value>
        /// The tokens.
        /// </value>
        [ConfigurationProperty(TokensElement, IsDefaultCollection = true)]
        public TokenConfigurationElementCollection Tokens
        {
            get { return (TokenConfigurationElementCollection)this[TokensElement]; }
            set { this[TokensElement] = value; }
        }
        /// <summary>
        /// Gets or sets the hypermedia section.
        /// </summary>
        /// <value>
        /// The hypermedia element.
        /// </value>
        [ConfigurationProperty(HypermediaElement)]
        public HypermediaConfigurationElement Hypermedia
        {
            get { return (HypermediaConfigurationElement)this[HypermediaElement]; }
            set { this[HypermediaElement] = value; }
        }
        #endregion

        #region Methods

        /// <summary>
        /// Gets a configuration section from the current application's default configuration
        /// or from the calling assembly's configuration located in the same directory.
        /// </summary>
        /// <param name="fromCallingAssembly">Determines whether the configuration instance should be
        /// retrieved from the web.config located in the calling assembly's directory or from the main 
        /// application's web.config.</param>
        /// <returns>
        /// Returns the configuration element within the specified location or null.
        /// </returns>
        public static BaseConfiguration GetInstance(bool fromCallingAssembly)
        {
            string pluginSectionName = string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings.Get("PluginSectionName")) ? DefaultPluginSection : ConfigurationManager.AppSettings.Get("PluginSectionName");
            var config = ConfigurationManager.GetSection(pluginSectionName) as BaseConfiguration;
            if (!fromCallingAssembly) return config;
            if (config != null)
            {
                var assembly = Assembly.GetCallingAssembly();
                var virtualroot = HttpRuntime.AppDomainAppVirtualPath.EndsWith("/") ? HttpRuntime.AppDomainAppVirtualPath:
                        HttpRuntime.AppDomainAppVirtualPath + "/";
                var assemblyloc = VirtualPathUtility.ToAppRelative(VirtualPathUtility.GetDirectory(assembly
                    .Location.Replace(HttpRuntime.AppDomainAppPath, virtualroot)));                
                string configpath = VirtualPathUtility.Combine(assemblyloc, "web.config");
                var moduleconfig = WebConfigurationManager.OpenWebConfiguration(configpath);                
                if (moduleconfig != null)
                {
                    return moduleconfig.GetSection(pluginSectionName) as BaseConfiguration;
                }
            }
            return config;
        }
        #endregion
    }
}
