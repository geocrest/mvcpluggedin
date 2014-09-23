
namespace Geocrest.Web.Mvc.Controllers
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Web.Http;
    using Geocrest.Web.Infrastructure;

    /// <summary>
    ///   Represents an <see cref="T:Geocrest.Web.Mvc.Controllers.IVersionedHttpControllerSelector" /> 
    ///   implementation that supports versioning and selects an controller based on versioning by convention 
    ///   (namespace.VMajor_Minor_Build_Revision.xController).
    ///   The controller to invoke is determined by the number in the "X-Api-Version" HTTP header.
    /// </summary>
    public sealed class VersionHeaderVersionedControllerSelector : VersionedControllerSelector
    {
        /// <summary>
        ///   Defines the name of the HTTP header that selects the API version
        /// </summary>
        public const string ApiVersionHeaderName = "X-Api-Version";

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Web.Mvc.Controllers.VersionHeaderVersionedControllerSelector"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public VersionHeaderVersionedControllerSelector(HttpConfiguration configuration)
            : base(configuration)
        {
        }

        /// <summary>
        /// Gets the controller identification from the incoming request.
        /// </summary>
        /// <param name="request">The request containing information about the desired version.</param>
        /// <returns>
        /// Returns an instance of <see cref="T:Geocrest.Web.Mvc.Controllers.ControllerIdentification">ControllerIdentification</see>.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">request</exception>
        protected override ControllerIdentification GetControllerIdentificationFromRequest(HttpRequestMessage request)
        {
            Throw.IfArgumentNull(request, "request");

            // get the version number from the HTTP header
            IEnumerable<string> values;
            string apiVersion = null;
            if (request.Headers.TryGetValues(ApiVersionHeaderName, out values))
            {
                foreach (string value in values)
                {
                    if (value.IsValidVersionNumber())
                    {
                        apiVersion = value;
                        break;
                    }                   
                }
            }
            if (string.IsNullOrEmpty(apiVersion))
                apiVersion = this.DefaultVersion;
            string controllerName = this.GetControllerNameFromRequest(request);

            return new ControllerIdentification(controllerName, apiVersion);
        }
    }
}
