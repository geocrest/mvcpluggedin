namespace Geocrest.Web.Mvc.Controllers
{
    using System.Collections.Generic;
    using System.Web.Http.Dispatcher;

    /// <summary>
    /// Provides methods to select a controller based on the version number specified in the request.
    /// </summary>
    public interface IVersionedHttpControllerSelector : IHttpControllerSelector
    {
        /// <summary>
        /// Gets the API version number from the request.
        /// </summary>
        string Version { get; }
        /// <summary>
        /// Gets or sets the default version to use when selecting controllers.
        /// </summary>
        string DefaultVersion { get; set; }
        /// <summary>
        /// Gets a collection of all the versions contained within the core application 
        /// while excluding any contained within MVC areas.
        /// </summary>
        IEnumerable<string> CoreVersions { get; }
        /// <summary>
        /// Gets a dictionary of all the versions contained within the application's areas keyed by area name.
        /// Note that this does not include versions contained outside of an area. To retrieve those
        /// versions, use the <see cref="P:Geocrest.Web.Mvc.Controllers.IVersionedHttpControllerSelector.CoreVersions"/>
        /// property.
        /// </summary>
        Dictionary<string,IEnumerable<string>> AreaVersions{get;}
    }
}
