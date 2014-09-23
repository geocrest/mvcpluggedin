namespace Geocrest.Web.Mvc
{
    using System.Web.Optimization;
    /// <summary>
    /// Provides a method for registering CSS and JavaScript bundles with the application
    /// </summary>
    public interface IBundleConfiguration
    {
        /// <summary>
        /// Registers the bundles with the application's bundle collection.
        /// </summary>
        /// <param name="bundles">The global bundle collection.</param>
        void RegisterBundles(BundleCollection bundles);
    }
}
