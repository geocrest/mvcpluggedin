
namespace Geocrest.Web.Mvc.Configuration
{
    using System.Configuration;

    /// <summary>
    /// Represents a configuration element containing information about dependency injection.
    /// </summary>
    public class InjectionConfigurationElement : ConfigurationElement
    {
        #region Fields
        private const string FoldersElement = "folders";
        private const string BindingsElement = "bindings";
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the areas.
        /// </summary>
        [ConfigurationProperty(FoldersElement, IsDefaultCollection = true)]
        public FolderConfigurationElementCollection Folders
        {
            get { return (FolderConfigurationElementCollection)this[FoldersElement]; }
            set { this[FoldersElement] = value; }
        }
        /// <summary>
        /// Gets or sets the bindings to use for dependency injection.
        /// </summary>
        [ConfigurationProperty(BindingsElement)]
        public BindingConfigurationElementCollection Bindings
        {
            get { return (BindingConfigurationElementCollection)this[BindingsElement]; }
            set { this[BindingsElement] = value; }
        }
        #endregion        
    }
}
