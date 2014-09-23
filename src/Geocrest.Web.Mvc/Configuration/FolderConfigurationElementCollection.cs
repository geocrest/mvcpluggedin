namespace Geocrest.Web.Mvc.Configuration
{
    using System.Configuration;

    /// <summary>
    /// Represents a collection of <see cref="T:Geocrest.Web.Mvc.Configuration.FolderConfigurationElement" /> items.
    /// </summary>
    [ConfigurationCollection(typeof(FolderConfigurationElement), AddItemName = "folder")]
    public class FolderConfigurationElementCollection : ConfigurationElementCollection
    {
        #region Methods
        /// <summary>
        /// Gets a unique key for the given element.
        /// </summary>
        /// <param name="element">The element to get a key for.</param>
        /// <returns>A unique key for the given element.</returns>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((FolderConfigurationElement)element).Name;
        }

        /// <summary>
        /// Creates a new instance of <see cref="T:Geocrest.Web.Mvc.Configuration.FolderConfigurationElement" /> for use with this collection.
        /// </summary>
        /// <returns>A new instance of <see cref="T:Geocrest.Web.Mvc.Configuration.FolderConfigurationElement" />.</returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new FolderConfigurationElement();
        }
        #endregion
    }
}