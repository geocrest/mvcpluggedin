
namespace Geocrest.Web.Mvc.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Web.Http;
    using Geocrest.Web.Infrastructure;
    /// <summary>
    /// Represents an <see cref="T:Geocrest.Web.Mvc.Controllers.IVersionedHttpControllerSelector" /> 
    /// implementation that supports versioning and selects a controller based on the media type within the Accept header.
    /// The version should be specified with the requested media type like 'application/json; version=1' or in
    /// the query string with an argument like 'v=1'.
    /// </summary>
    public sealed class AcceptHeaderVersionedControllerSelector : AcceptHeaderVersionedControllerSelectorBase
    {
        internal const string QueryStringArgument = "v";
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Web.Mvc.Controllers.AcceptHeaderVersionedControllerSelector"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public AcceptHeaderVersionedControllerSelector(HttpConfiguration configuration) : base(configuration) { }
        /// <summary>
        /// Gets the controller identification from the incoming request.
        /// </summary>
        /// <param name="request">The request containing information about the desired version.</param>
        /// <returns>
        /// Returns an instance of <see cref="T:Geocrest.Web.Mvc.Controllers.ControllerIdentification">ControllerIdentification</see>.
        /// </returns>
        /// <remarks>This override will check the query string for a version argument named 'v' and, if
        /// one exists and is a valid version, return that version instead of the one in the accept header.</remarks>
        /// <exception cref="T:System.ArgumentNullException">request</exception>
        protected override ControllerIdentification GetControllerIdentificationFromRequest(HttpRequestMessage request)
        {
            Throw.IfArgumentNull(request, "request");
            var query = request.GetQueryNameValuePairs().Where(x => x.Key == QueryStringArgument);
            if (query.Any() && query.First().Value != null && query.First().Value.IsValidVersionNumber())
            {
                this.Version = query.First().Value;
                string controllerName = this.GetControllerNameFromRequest(request);
                return new ControllerIdentification(controllerName, this.Version);
            }
            else
                return base.GetControllerIdentificationFromRequest(request);
        }
        /// <summary>
        /// Derived classes implement this to return an API version from the specified media type string.
        /// </summary>
        /// <param name="mediaType">The media type containing the version number.</param>
        /// <returns>The requested version.</returns>
        protected override string GetVersion(MediaTypeWithQualityHeaderValue mediaType)
        {
            // get version
            NameValueHeaderValue versionParameter = mediaType.Parameters.FirstOrDefault(parameter =>
                parameter.Name == "version");

            if (versionParameter == null) return null;

            // parse version
            return versionParameter.Value.IsValidVersionNumber() ? versionParameter.Value : null;
        }
    }
}
