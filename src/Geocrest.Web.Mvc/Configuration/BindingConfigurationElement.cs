namespace Geocrest.Web.Mvc.Configuration
{
    using System.Configuration;

    /// <summary>
    /// Represents a binding to use when resolving dependency injections.
    /// </summary>    
    public sealed class BindingConfigurationElement: ConfigurationElement
    {
        #region Fields        
        private const string NameAttribute = "name";
        private const string ServiceTypeAttribute = "serviceType";
        private const string ImplementationTypeAttribute = "implementationType";
        private const string GenericServiceTypeAttribute = "genericServiceTypeParameter";
        private const string GenericImplementationTypeAttribute = "genericImplementationTypeParameter";
        private const string IsNamedAttribute = "isNamed";
        private const string ToSelfAttribute = "toSelf";
        private const string ConstructorArgumentsAttribute = "constructorArguments";
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the name of the binding.
        /// </summary>
        [ConfigurationProperty(NameAttribute,IsKey=true,IsRequired=true)]
        public string Name
        {
            get { return (string)this[NameAttribute]; }
            set { this[NameAttribute] = value; }
        }
        /// <summary>
        /// Indicates whether this binding is a named binding.
        /// </summary>
        [ConfigurationProperty(IsNamedAttribute, DefaultValue=false)]
        public bool IsNamed
        {
            get { return (bool)this[IsNamedAttribute]; }
            set { this[IsNamedAttribute] = value; }
        }
        /// <summary>
        /// Indicates whether this binding should bind to itself.
        /// </summary>
        [ConfigurationProperty(ToSelfAttribute, DefaultValue = false)]
        public bool ToSelf
        {
            get { return (bool)this[ToSelfAttribute]; }
            set { this[ToSelfAttribute] = value; }
        }
        /// <summary>
        /// Gets or sets the service contract type to bind as a fully-qualified name.
        /// </summary>
        [ConfigurationProperty(ServiceTypeAttribute, IsRequired = true)]
        public string ServiceType
        {
            get { return (string)this[ServiceTypeAttribute]; }
            set { this[ServiceTypeAttribute] = value; }
        }
        /// <summary>
        /// Gets or sets the concrete class type as a fully qualified name that implements the service contract.
        /// </summary>
        [ConfigurationProperty(ImplementationTypeAttribute)]
        public string ImplementationType
        {
            get { return (string)this[ImplementationTypeAttribute]; }
            set { this[ImplementationTypeAttribute] = value; }
        }
        /// <summary>
        /// Gets or sets the generic type parameter for the service.
        /// </summary>
        /// <value>
        /// The generic service type parameter.
        /// </value>
        [ConfigurationProperty(GenericServiceTypeAttribute)]
        public string GenericServiceTypeParameter
        {
            get { return (string)this[GenericServiceTypeAttribute]; }
            set { this[GenericServiceTypeAttribute] = value; }
        }
        /// <summary>
        /// Gets or sets the generic type parameter for the implementation.
        /// </summary>
        /// <value>
        /// The generic implementation type parameter.
        /// </value>
        [ConfigurationProperty(GenericImplementationTypeAttribute)]
        public string GenericImplementationTypeParameter
        {
            get { return (string)this[GenericImplementationTypeAttribute]; }
            set { this[GenericImplementationTypeAttribute] = value; }
        }
        /// <summary>
        /// Gets or sets the constructor arguments, if any, to use for this binding.
        /// </summary>
        [ConfigurationProperty(ConstructorArgumentsAttribute)]
        public NameValueConfigurationCollection ConstructorArguments
        {
            get { return (NameValueConfigurationCollection)this[ConstructorArgumentsAttribute]; }
            set { this[ConstructorArgumentsAttribute] = value; }
        }
        #endregion      
    }
}
