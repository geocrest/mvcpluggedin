//
// TODO: Review and remove if not needed. This class is replaced by ServiceHostFactory. BR 2013.04.02
//
//using System.Linq;
//using System;
//using System.ServiceModel;
//using System.ServiceModel.Description;
//using Ninject.Extensions.Wcf;

//namespace Geocrest.Web.Wcf.Factory
//{
//    /// <summary>
//    /// A service host factory that inserts a metadata endpoint to the service by overriding
//    /// the
//    /// <see cref="M:System.ServiceModel.Activation.ServiceHostFactory.CreateServiceHost(System.Type serviceType,System.Uri[] baseAddresses)"/>. This allows
//    /// the service to be used as a SOAP endpoint without using web.config.
//    /// </summary>
//    public class MexServiceHostFactory : ServiceHostFactory
//    {
//        /// <summary>
//        /// Creates a <see cref="T:System.ServiceModel.ServiceHost"/> for a
//        /// specified type of service with a specific base address.
//        /// </summary>
//        /// <param name="serviceType">Specifies the type of service to host.</param>
//        /// <param name="baseAddresses">The <see cref="T:System.Array"/> of type <see cref="T:System.Uri"/>
//        /// that contains the base addresses for the service hosted.</param>
//        /// <returns>
//        /// A <see cref="T:System.ServiceModel.ServiceHost"/> for the type of
//        /// service specified with a specific base address.
//        /// </returns>
//        protected override System.ServiceModel.ServiceHost CreateServiceHost(Type serviceType, Uri[] baseAddresses)
//        {
//            ServiceHost host = base.CreateServiceHost(serviceType, baseAddresses);
//            ServiceDebugBehavior debug = (ServiceDebugBehavior)host.Description.Behaviors.First(x =>
//                x.GetType() == typeof(ServiceDebugBehavior));
//            debug.HttpHelpPageEnabled = false;
//            debug.IncludeExceptionDetailInFaults = true;

//            // Add service metadata
//            var metadataBehavior = new ServiceMetadataBehavior();
//                metadataBehavior.HttpGetEnabled = true;
//                metadataBehavior.HttpGetUrl = new Uri("", UriKind.Relative);
//            host.Description.Behaviors.Add(metadataBehavior);

//            host.AddServiceEndpoint(ServiceMetadataBehavior.MexContractName,
//                MetadataExchangeBindings.CreateMexHttpBinding(), "mex");
//            host.AddServiceEndpoint(serviceType.GetInterfaces()[0], new BasicHttpBinding(), "");
//            return host;
//        }      
//    }
//}
