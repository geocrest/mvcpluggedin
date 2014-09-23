namespace Geocrest.Web.Mvc.Configuration
{
    using System.Configuration;
    /// <summary>
    /// Represents a collection of <see cref="T:Geocrest.Web.Mvc.Configuration.TokenConfigurationElement" /> items.
    /// </summary>
    [ConfigurationCollection(typeof(TokenConfigurationElement), AddItemName = "token")]
    public class TokenConfigurationElementCollection :ConfigurationElementCollection
    {
        #region Methods
        /// <summary>
        /// Gets a unique key for the given element.
        /// </summary>
        /// <param name="element">The element to get a key for.</param>
        /// <returns>A unique key for the given element.</returns>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((TokenConfigurationElement)element).Value;
        }

        /// <summary>
        /// Creates a new instance of <see cref="Geocrest.Web.Mvc.Configuration.TokenConfigurationElement" /> for use with this collection.
        /// </summary>
        /// <returns>A new instance of <see cref="Geocrest.Web.Mvc.Configuration.TokenConfigurationElement" />.</returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new TokenConfigurationElement();
        }
        #endregion
    }
}
