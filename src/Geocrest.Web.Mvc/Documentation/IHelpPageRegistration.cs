namespace Geocrest.Web.Mvc.Documentation
{
    using System.Web.Http;

    /// <summary>
    /// Provides a single method for registering help page samples
    /// </summary>
    public interface IHelpPageRegistration
    {
        /// <summary>
        /// Registers sample requests and responses for an API.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        void RegisterSamples(HttpConfiguration configuration);
    }
}
