namespace Geocrest.Web.Wcf
{
    using System.Linq;
    using System.ServiceModel;

    internal static class ServiceTypeHelper
    {
        /// <summary>
        /// Determines whether the given service is a singleton service.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <returns>
        /// <b>true</b>, if the service is a singleton; otherwise, <b>false</b>.
        /// </returns>
        public static bool IsSingletonService(object service)
        {
            var serviceBehaviorAttribute =
                service.GetType().GetCustomAttributes(typeof(ServiceBehaviorAttribute), true)
                .Cast<ServiceBehaviorAttribute>()
                .SingleOrDefault();
            return serviceBehaviorAttribute != null && serviceBehaviorAttribute.InstanceContextMode == InstanceContextMode.Single;
        }
    }
}
