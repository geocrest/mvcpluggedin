namespace Geocrest.Web.Mvc.Configuration
{
    using System.Configuration;

    /// <summary>
    /// Represents a single token to add to the application. Tokens are used during the 
    /// rendering of API documentation to replace values within documentation. 
    /// </summary>
    public class TokenConfigurationElement : ConfigurationElement
    {
        #region Fields
        private const string TokenAttribute = "value";
        //private const string ValueAttribute = "value";
        #endregion

        #region Methods
        /// <summary>
        /// Gets or sets the token.
        /// </summary>
        [ConfigurationProperty(TokenAttribute, IsRequired = true, IsKey = true)]
        public string Value
        {
            get { return (string)this[TokenAttribute]; }
            set { this[TokenAttribute] = value; }
        }

        ///// <summary>
        ///// Gets or sets the value of the token.
        ///// </summary>
        //[ConfigurationProperty(ValueAttribute, IsRequired = true)]
        //public string Path
        //{
        //    get { return (string)this[ValueAttribute]; }
        //    set { this[ValueAttribute] = value; }
        //}
        #endregion
    }
}
