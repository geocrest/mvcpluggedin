namespace Geocrest.Web.Mvc.Controllers
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Web.Http;
    using Geocrest.Web.Infrastructure;
    using System.Linq;
    /// <summary>
    /// Represents an <see cref="T:Geocrest.Web.Mvc.Controllers.IVersionedHttpControllerSelector" /> 
    /// implementation that supports versioning and selects an controller based on versioning by convention 
    /// (namespace.VMajor_Minor_Build_Revision.xController). 
    /// The controller to invoke is determined by the "version" key in the "Accept" HTTP header.
    /// </summary>
    /// <remarks>
    /// Derived classes must implement 
    /// <see cref="M:Geocrest.Web.Mvc.Controllers.AcceptHeaderVersionedControllerSelectorBase.GetVersion(System.Net.Http.Headers.MediaTypeWithQualityHeaderValue)"/>
    /// </remarks>
    public abstract class AcceptHeaderVersionedControllerSelectorBase : VersionedControllerSelector
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Web.Mvc.Controllers.AcceptHeaderVersionedControllerSelectorBase"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        protected AcceptHeaderVersionedControllerSelectorBase(HttpConfiguration configuration)
            : base(configuration)
        {
        }

        /// <summary>
        /// Gets the controller identification from the incoming request.
        /// </summary>
        /// <param name="request">The request containing information about the desired version.</param>
        /// <returns>
        /// Returns the information about the current request.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">request</exception>
        protected override ControllerIdentification GetControllerIdentificationFromRequest(HttpRequestMessage request)
        {
            Throw.IfArgumentNull(request, "request");

            string controllerName = this.GetControllerNameFromRequest(request);

            // get "accept" HTTP header value
            var acceptHeader = request.Headers.Accept;
            string apiVersion = this.GetVersionFromHeader(acceptHeader);
            if (!string.IsNullOrEmpty(apiVersion))
                this.Version = apiVersion;
            else if (!string.IsNullOrEmpty(this.DefaultVersion))
            {
                // if version not specified in request but a default version is set,
                // check to see if a controller exists for the default version
                string area = string.Empty;
                var routeData = request.GetRouteData();
                if (routeData != null && routeData.Values.ContainsKey("area") && routeData.Values["area"] != null)
                    area = routeData.Values["area"].ToString().ToLower();
                if (!string.IsNullOrEmpty(area))
                {
                    // if the request is coming from an area check for a matching version within the area's version
                    if (this.AreaVersions[area].Contains(this.DefaultVersion))
                        this.Version = apiVersion = this.DefaultVersion;
                }
                else // check for a matching version within the core versions (i.e. outside of an area)
                {
                    if (this.CoreVersions.Contains(this.DefaultVersion))
                        this.Version = apiVersion = this.DefaultVersion;
                }
            }
            return new ControllerIdentification(controllerName, this.Version);
        }

        /// <summary>
        /// Returns the API version from the collection with accept header values. If more than one media type is 
        /// requested then the first one with a version number will be returned.
        /// </summary>
        /// <param name="acceptHeader">A collection of accept headers.</param>
        private string GetVersionFromHeader(IEnumerable<MediaTypeWithQualityHeaderValue> acceptHeader)
        {
            foreach (MediaTypeWithQualityHeaderValue headerValue in acceptHeader)
            {
                string version = this.GetVersion(headerValue);

                if (!string.IsNullOrEmpty(version))
                    return version;
            }
            return null;
        }

        /// <summary>
        /// Derived classes implement this to return an API version from the specified media type string.
        /// </summary>
        /// <param name="mediaType">The media type containing the version number.</param>
        /// <returns>
        /// Returns the version number specified in the accept header.
        /// </returns>
        protected abstract string GetVersion(MediaTypeWithQualityHeaderValue mediaType);
    }
}
