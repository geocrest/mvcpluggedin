namespace Geocrest.Web.Mvc.Documentation
{
    using System.Collections.Generic;
    using System.Web.Http.Description;

    /// <summary>
    /// Provides a collection of <see cref="T:System.Web.Http.Description.ApiDescription"/> instances
    /// for a given area and API version.
    /// </summary>
    public interface IVersionedApiExplorer : IApiExplorer
    {
        /// <summary>
        /// Gets or sets the area within which to explore. If this property is not set, the entire space should be searched.
        /// </summary>     
        string Area { get; set; }        
        /// <summary>
        /// Gets the version for which to return <see cref="T:System.Web.Http.Description.ApiDescription"/> instances.
        /// </summary>
        string Version { get; }
        ///// <summary>
        ///// Gets a collection of all the versions contained within the area specified by the 
        ///// <see cref="P:Geocrest.Web.Mvc.Documentation.IVersionedApiExplorer.Area"/> property.
        ///// </summary>
        //IEnumerable<string> AllVersions { get; }
        /// <summary>
        /// Sets the version to an explicitly defined version.
        /// </summary>
        /// <param name="version">The version.</param>
        void SetVersion(string version);
    }
}
