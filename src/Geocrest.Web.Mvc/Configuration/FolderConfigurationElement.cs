namespace Geocrest.Web.Mvc.Configuration
{
    using System.Configuration;

    /// <summary>
    /// Represents the configuration of the location used to host modules.
    /// </summary>
    public class FolderConfigurationElement : ConfigurationElement
    {
        #region Fields
        private const string NameAttribute = "name";
        private const string PathAttribute = "path";
        #endregion

        #region Methods
        /// <summary>
        /// Gets or sets the name of the area.
        /// </summary>
        [ConfigurationProperty(NameAttribute, IsRequired = true, IsKey = true)]
        public string Name
        {
            get { return (string) this[NameAttribute]; }
            set { this[NameAttribute] = value; }
        }

        /// <summary>
        /// Gets or sets the path of the area.
        /// </summary>
        [ConfigurationProperty(PathAttribute, IsRequired = true)]
        public string Path
        {
            get { return (string)this[PathAttribute]; }
            set { this[PathAttribute] = value; }
        }
        #endregion
    }
}