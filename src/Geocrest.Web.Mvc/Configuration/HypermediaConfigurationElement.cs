
namespace Geocrest.Web.Mvc.Configuration
{
    using System.Configuration;

    /// <summary>
    /// Represents a configuration element containing information about how resource types map to 
    /// Hypermedia Application Language (HAL) endpoints. 
    /// </summary>
    /// <remarks>When a HAL resource is enriched with URIs, the links are generated from the API
    /// route for that resource. This requires the name of the route, the controller, and any
    /// arguments needed. This configuration section allows the explicit mapping of POCO types 
    /// (i.e. resources) to a configured controller name.</remarks>
    public class HypermediaConfigurationElement : ConfigurationElement
    {
        #region Fields
        private const string MappingsElement = "mappings";
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the mappings to use when creating links to resources.
        /// </summary>
        [ConfigurationProperty(MappingsElement, IsDefaultCollection = true)]
        public KeyValueConfigurationCollection Mappings
        {
            get { return (KeyValueConfigurationCollection)this[MappingsElement]; }
            set { this[MappingsElement] = value; }
        }       
        #endregion        
    }
}
