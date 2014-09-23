namespace Geocrest.Data.Sources.Gis
{
    using Ninject.Modules;
    using Geocrest.Data.Contracts.Gis;
    using Geocrest.Model;
    using Geocrest.Model.ArcGIS.Tasks;

    /// <summary>
    /// Provides Ninject bindings for the various data sources.
    /// </summary>
    public class NinjectionModule:NinjectModule
    {
        /// <summary>
        /// Loads the module into the kernel.
        /// </summary>
        public override void Load()
        {
            Bind<IRestHelper>().To<RestHelper>();
            Bind<IWebHelper>().To<WrappedWebClient>();
            Bind<IArcGISServerFactory>().To<ArcGISServerFactory>();
            Bind<IArcGISServerCatalogCache>().To<ArcGISServerCatalogCache>();
        }
    }
}
