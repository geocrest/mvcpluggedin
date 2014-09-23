
namespace Geocrest.Web.Infrastructure
{
    using System;
    using System.ServiceModel;

    /// <summary>
    /// Occurs when a web service endpoint is unavailable or not found
    /// </summary>
    [Serializable]
    public sealed class DataSourceUnavailableException : EndpointNotFoundException
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
        /// Initializes a new instance of the <see cref="T:Geocrest.Web.Infrastructure.DataSourceUnavailableException"/> class.
        /// </summary>
        /// <param name="url">An object of the type <see cref="T:System.String">String</see>.</param>
        public DataSourceUnavailableException(string url) :
            base(string.Format("The resource located at '{0}' is unavailable.", url))
        {
            this.Url = url;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Web.Infrastructure.DataSourceUnavailableException"/> class.
        /// </summary>
        public DataSourceUnavailableException() :
            base("The underlying data source is unavailable to complete the request.") { }
        /// <summary>
        /// Gets the Url that was requested.
        /// </summary>
        public string Url { get; private set; }
    }
}
