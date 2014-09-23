
namespace Geocrest.Web.Infrastructure
{
    using System;

    /// <summary>
    /// An exception that is raised when a dynamic module fails to load properly.
    /// </summary>
    [Serializable]
    public class ModuleLoadException : ApplicationException
    {
        /// <summary>
        /// When overridden in a derived class, sets the <see cref="T:System.Runtime.Serialization.SerializationInfo" /> with information about the exception.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination.</param>
        /// <PermissionSet>
        ///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Read="*AllFiles*" PathDiscovery="*AllFiles*" />
        ///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="SerializationFormatter" />
        ///   </PermissionSet>
        public override void GetObjectData(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Web.Infrastructure.ModuleLoadException" /> class.
        /// </summary>
        /// <param name="moduleName">Name of the module that caused the exception.</param>
        public ModuleLoadException(string moduleName) 
            : base(Resources.Exceptions.ModuleLoad)
        {
            Throw.IfArgumentNullOrEmpty(moduleName, "moduleName");
            this.ModuleName = moduleName;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Web.Infrastructure.ModuleLoadException" /> class.
        /// </summary>
        /// <param name="innerException">The inner exception.</param>
        public ModuleLoadException(Exception innerException)
            : base(Resources.Exceptions.ModuleLoad, innerException)
        {            
            this.ModuleName = string.Empty;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Web.Infrastructure.ModuleLoadException" /> class.
        /// </summary>
        /// <param name="moduleName">Name of the module that caused the exception.</param>
        /// <param name="innerException">An inner exception.</param>
        public ModuleLoadException(string moduleName, Exception innerException)
            : base(Resources.Exceptions.ModuleLoad, innerException)
        {
            Throw.IfArgumentNullOrEmpty(moduleName, "moduleName");
            this.ModuleName = moduleName;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Web.Infrastructure.ModuleLoadException" /> class.
        /// </summary>
        /// <param name="moduleName">Name of the module that caused the exception.</param>
        /// <param name="message">The message to display.</param>
        public ModuleLoadException(string moduleName, string message)
            : base(message)
        {
            Throw.IfArgumentNullOrEmpty(moduleName, "moduleName");
            this.ModuleName = moduleName;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Web.Infrastructure.ModuleLoadException" /> class.
        /// </summary>
        /// <param name="moduleName">Name of the module that caused the exception.</param>
        /// <param name="message">The message to display.</param>
        /// <param name="innerException">An inner exception.</param>
        public ModuleLoadException(string moduleName, string message, Exception innerException)
            : base(message, innerException)
        {
            Throw.IfArgumentNullOrEmpty(moduleName, "moduleName");
            this.ModuleName = moduleName;
        }
        /// <summary>
        /// Gets the name of the module.
        /// </summary>
        /// <value>
        /// The name of the module.
        /// </value>
        public string ModuleName { get; private set; }
    }
}
