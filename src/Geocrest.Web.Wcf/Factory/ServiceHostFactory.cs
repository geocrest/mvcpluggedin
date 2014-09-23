namespace Geocrest.Web.Wcf.Factory
{
    using System;
    using System.Linq;
    using System.ServiceModel;
    using System.ServiceModel.Description;
    using Ninject.Extensions.Wcf;

    /// <summary>
    /// Represents a WCF Service Host Factory for use with Ninject and MVC service routes.
    /// </summary>
    /// <remarks>
    /// This class builds the SOAP service and metadata endpoints, allowing it to be hosted without a web.config.
    /// </remarks>
    public class ServiceHostFactory : NinjectServiceHostFactory
    {
        /// <summary>
        /// Creates a <see cref="T:System.ServiceModel.ServiceHost"/> for a
        /// specified type of service with a specific base address.
        /// </summary>
        /// <param name="serviceType">Specifies the type of service to host.</param>
        /// <param name="baseAddresses">The <see cref="T:System.Array"/> of type <see cref="T:System.Uri"/>
        /// that contains the base addresses for the service hosted.</param>
        /// <returns>
        /// A <see cref="T:System.ServiceModel.ServiceHost"/> for the type of
        /// service specified with a specific base address.
        /// </returns>
        protected override System.ServiceModel.ServiceHost CreateServiceHost(Type serviceType, Uri[] baseAddresses)
        {
            //WcfServiceHost host = new WcfServiceHost(serviceType, baseAddresses);
            //return host;

            // Create a NinjectIISHostingServiceHost. 
            ServiceHost host = base.CreateServiceHost(serviceType, baseAddresses);

            ServiceDebugBehavior debug = (ServiceDebugBehavior)host.Description.Behaviors.First(x => x.GetType() == typeof(ServiceDebugBehavior));
            debug.HttpHelpPageEnabled = false;
            debug.IncludeExceptionDetailInFaults = true;

            // Add metadata behavior.
            var metadataBehavior = new ServiceMetadataBehavior();
            metadataBehavior.HttpGetEnabled = true;
            metadataBehavior.HttpGetUrl = new Uri("", UriKind.Relative);
            host.Description.Behaviors.Add(metadataBehavior);

            // Create custom binding.
            //  BasicHttpBinding binding = new BasicHttpBinding();

            //Binding binding = CreateBasicHttpBinding();
            //EndpointAddress address = CreateEndPoint();
            //var serviceClient = new MyServiceClient(httpBinding, address);

            ////CustomBinding binding = (CustomBinding)PDAServiceContractClient.CreateDefaultBinding();
            //HttpTransportBindingElement httpBindingElement = new HttpTransportBindingElement();
            //httpBindingElement.MaxBufferSize = Int32.MaxValue;
            //httpBindingElement.MaxReceivedMessageSize = Int32.MaxValue;
            //binding.Elements.Add(httpBindingElement);

            // Add service endpoint
            host.AddServiceEndpoint(ServiceMetadataBehavior.MexContractName, MetadataExchangeBindings.CreateMexHttpBinding(), "mex");
            //      host.AddServiceEndpoint(serviceType.GetInterfaces()[0], binding, "");

            return host;
        }

        /// <summary>
        /// Gets the type of service host to be created by this factory.
        /// </summary>
        /// <value>The type created by the factory class: 
        /// <see cref="T:Geocrest.Web.Wcf.NinjectServiceHost`1"/>.</value>
        protected override Type ServiceHostType
        {
            get
            {
                return typeof(NinjectServiceHost<>);
            }
        }
    }
}
