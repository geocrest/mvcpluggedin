namespace Geocrest.Web.Wcf
{
    using System;
    using System.ServiceModel;
    using System.ServiceModel.Description;
    using Ninject.Extensions.Wcf;

    /// <summary>
    /// Represents an IIS ServiceHost for use with Ninject. 
    /// </summary>
    /// <typeparam name="T">The type of the service</typeparam>
    public class NinjectServiceHost<T> : NinjectAbstractServiceHost<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Web.Wcf.NinjectServiceHost`1"/> class.
        /// </summary>
        /// <param name="serviceBehavior">The service behavior.</param>
        /// <param name="instance">The instance.</param>
        /// <param name="baseAddresses">The base addresses.</param>
        public NinjectServiceHost(IServiceBehavior serviceBehavior, T instance, Uri[] baseAddresses)
            : base(serviceBehavior, instance, baseAddresses) { }

        /// <summary>
        /// Invoked during the transition of a communication object into the opening state. 
        /// This is used to configure WCF bindings programmatically. 
        /// </summary>
        /// <exception cref="T:System.InvalidOperationException">Service implements multiple ServiceContract types, and no endpoints are defined in the configuration file. WebServiceHost can set up default endpoints, but only if the service implements only a single ServiceContract. Either change the service to only implement a single ServiceContract, or else define endpoints for the service explicitly in the configuration file. When more than one contract is implemented, must add base address endpoint manually</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "ServiceContract"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "WebServiceHost")]
        protected override void OnOpening()
        {
            base.OnOpening();

            foreach (Uri baseAddress in BaseAddresses)
            {
                //
                // TODO: Review rationale for logic below. Mex endpoint is hit first but not modified.
                //       This seems to work, but may not be the exact desired effect.
                //  
                bool found = false;
                foreach (ServiceEndpoint se in Description.Endpoints)
                {
                    if (se.Address.Uri == baseAddress)
                    {
                        found = true;
                        ((BasicHttpBinding)se.Binding).ReaderQuotas.MaxStringContentLength = ApiServiceRouteConfiguration.MaxStringContentLength;
                        ((BasicHttpBinding)se.Binding).MaxReceivedMessageSize = ApiServiceRouteConfiguration.MaxReceivedMessageSize;
                        ((BasicHttpBinding)se.Binding).MaxBufferSize = ApiServiceRouteConfiguration.MaxBufferSize;
                    }
                }

                if (!found)
                {
                    if (ImplementedContracts.Count > 1)
                    {
                        throw new InvalidOperationException("Service '" + Description.ServiceType.Name + "' implements multiple ServiceContract types, and no endpoints are defined in the configuration file. WebServiceHost can set up default endpoints, but only if the service implements only a single ServiceContract. Either change the service to only implement a single ServiceContract, or else define endpoints for the service explicitly in the configuration file. When more than one contract is implemented, must add base address endpoint manually");
                    }

                    var enumerator = ImplementedContracts.Values.GetEnumerator();
                    enumerator.MoveNext();
                    Type contractType = enumerator.Current.ContractType;
                    BasicHttpBinding binding = new BasicHttpBinding();

                    binding.CloseTimeout = ApiServiceRouteConfiguration.CloseTimeout;
                    binding.MaxBufferPoolSize = ApiServiceRouteConfiguration.MaxBufferPoolSize;
                    //binding.MaxBufferSize = GeodataWcfServiceConfiguration.MaxBufferSize;
                    binding.MaxReceivedMessageSize = ApiServiceRouteConfiguration.MaxReceivedMessageSize;
                    binding.MessageEncoding = ApiServiceRouteConfiguration.MessageEncoding;
                    binding.Namespace = ApiServiceRouteConfiguration.BindingNamespace;
                    binding.OpenTimeout = ApiServiceRouteConfiguration.OpenTimeout;
                    binding.ReaderQuotas.MaxStringContentLength = ApiServiceRouteConfiguration.MaxStringContentLength;
                    binding.ReceiveTimeout = ApiServiceRouteConfiguration.ReceiveTimeout;
                    binding.SendTimeout = ApiServiceRouteConfiguration.SendTimeout;
                    
                    AddServiceEndpoint(contractType, binding, ""); //baseAddress);
                }
            }

            //foreach (ServiceEndpoint se in Description.Endpoints)
            //    if (se.Behaviors.Find<WebHttpBehavior>() == null)
            //        se.Behaviors.Add(new WebHttpBehavior());

            //// disable help page.
            //ServiceDebugBehavior serviceDebugBehavior = Description.Behaviors.Find<ServiceDebugBehavior>();
            //if (serviceDebugBehavior != null)
            //{
            //    serviceDebugBehavior.HttpHelpPageEnabled = false;
            //    serviceDebugBehavior.HttpsHelpPageEnabled = false;
            //}

        }
    }
}
