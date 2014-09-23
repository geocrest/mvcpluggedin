namespace Geocrest.Web.Wcf
{
    using System;
    using System.ServiceModel;

    /// <summary>
    /// Represents constants to be used with WCF service configuration for module endpoints exposed 
    /// through the Web API. This approach is less than ideal. These values should ultimately be stored in
    /// config of the WebHost for the module. However, the correct approach presents special 
    /// challenges for both dynamic service routes and Ninject.
    /// </summary>
    public static class ApiServiceRouteConfiguration
    {
        // Initialize the binding namespace with the solution-level constant.
        // It is unlikely that this needs to be modified, but this may provide the option to do so.
        private static string bindingNamespace = Geocrest.XmlNamespaces.ApiVersion1;
        
        // Initialize 10 minute durations for common WCF time-outs.
        private static TimeSpan closeTimeout = new TimeSpan(0, 10, 0);
        private static TimeSpan openTimeout = new TimeSpan(0, 10, 0);
        private static TimeSpan receiveTimeout = new TimeSpan(0, 10, 0);
        private static TimeSpan sendTimeout = new TimeSpan(0, 10, 0);

        // Initialize the max integer value for common WCF content sizes.
        private static Int32 maxBufferSize = Int32.MaxValue;
        private static Int32 maxReceivedMessageSize = Int32.MaxValue;
        private static Int32 maxStringContentLength = Int32.MaxValue;

        // Initialize without a buffer pool.
        private static Int32 maxBufferPoolSize = 0; 

        // Initialize for MTOM encoding.
        private static WSMessageEncoding messageEncoding = WSMessageEncoding.Mtom;
        
        /// <summary>
        /// Gets or sets the binding namespace, which is initialized from the solution-level linked class named <b>XmlNamespaceInfo.cs</b>.
        /// </summary>
        /// <value>
        /// The binding namespace.
        /// </value>
        public static string BindingNamespace
        {
            get { return ApiServiceRouteConfiguration.bindingNamespace; }
            set { ApiServiceRouteConfiguration.bindingNamespace = value; }
        }

        /// <summary>
        /// Gets or sets the close timeout. This is used while closing channels when no explicit timeout value is specified.
        /// </summary>
        /// <value>
        /// The close timeout. 
        /// </value>
        /// <remarks>
        /// It appears that timeouts may not always be honored from VS or IISExpress hosts.
        /// </remarks>
        public static TimeSpan CloseTimeout
        {
            get { return closeTimeout; }
            set { closeTimeout = value; }
        }

        /// <summary>
        /// Gets or sets the maximum size in bytes of any buffer pools used by the transport.
        /// MaxBufferPoolSize can be set to 0 to disallow buffering for large messages.
        /// </summary>
        /// <value>
        /// The size of the max buffer pool.
        /// </value>
        public static Int32 MaxBufferPoolSize
        {
            get { return maxBufferPoolSize; }
            set { maxBufferPoolSize = value; }
        }

        /// <summary>
        /// Gets or sets the maximum size of the buffer to use.
        /// This must match MaxReceivedMessageSize for buffered transfers.
        /// </summary>
        /// <value>
        /// The size of the max buffer. The WCF default is 65536.
        /// </value>
        public static Int32 MaxBufferSize
        {
            get { return maxBufferSize; }
            set { maxBufferSize = value; }
        }

        /// <summary>
        /// Gets or sets the maximum allowable message size in bytes.
        /// This must match MaxBufferSize for buffered transfers.
        /// </summary>
        /// <value>
        /// The size of the max received message. The WCF default is 65536.
        /// </value>
        public static Int32 MaxReceivedMessageSize
        {
            get { return maxReceivedMessageSize; }
            set { maxReceivedMessageSize = value; }
        }

        /// <summary>
        /// Gets or sets the maximum string length returned by the reader.
        /// </summary>
        /// <value>
        /// The length of the max string content. The WCF default is 8192.
        /// </value>
        public static Int32 MaxStringContentLength
        {
            get { return maxStringContentLength; }
            set { maxStringContentLength = value; }
        }

        /// <summary>
        /// The messaging encoding.
        /// </summary>
        /// <value>
        /// The WS message encoding. The WCF default it Text, but Mtom is typically used.
        /// </value>
        public static WSMessageEncoding MessageEncoding
        {
            get { return messageEncoding; }
            set { messageEncoding = value; }
        }

        /// <summary>
        /// Gets or sets the open timeout. This is used when opening channels when no explicit timeout value is specified.
        /// </summary>
        /// <value>
        /// The open timeout.
        /// </value>
        /// <remarks>
        /// It appears that timeouts may not always be honored from VS or IISExpress hosts.
        /// </remarks>
        public static TimeSpan OpenTimeout
        {
            get { return ApiServiceRouteConfiguration.openTimeout; }
            set { ApiServiceRouteConfiguration.openTimeout = value; }
        }

        /// <summary>
        /// Gets or sets the receive timeout. This is used by the Service Framework Layer to initialize 
        /// the session-idle timeout which controls how long a session can be idle before timing out. 
        /// Note that this property applies only to the server and is ignored when set in client configuration.
        /// </summary>
        /// <value>
        /// The receive timeout.
        /// </value>
        /// <remarks>
        /// It appears that timeouts may not always be honored from VS or IISExpress hosts.
        /// </remarks>
        public static TimeSpan ReceiveTimeout
        {
            get { return ApiServiceRouteConfiguration.receiveTimeout; }
            set { ApiServiceRouteConfiguration.receiveTimeout = value; }
        }

        /// <summary>
        /// Gets or sets the send timeout. This is used to initialize the OperationTimeout, which
        /// governs the whole process of sending a message, including receiving a reply message for a 
        /// request-reply service operation. This timeout also applies when sending reply messages 
        /// from a callback contract method. 
        /// </summary>
        /// <value>
        /// The send timeout.
        /// </value>
        /// <remarks>
        /// It appears that timeouts may not always be honored from VS or IISExpress hosts.
        /// </remarks>
        public static TimeSpan SendTimeout
        {
            get { return ApiServiceRouteConfiguration.sendTimeout; }
            set { ApiServiceRouteConfiguration.sendTimeout = value; }
        }
    }
}
