namespace Geocrest.Web.Mvc.DependencyResolution
{
    using System.Collections.Generic;
    using System.Net.Http.Formatting;
    using System.Web.Hosting;
    using System.Web.Http;
    using System.Web.Http.Controllers;
    using System.Web.Http.Description;
    using System.Web.Http.Dispatcher;
    using System.Web.Http.Metadata;
    using System.Web.Http.Metadata.Providers;
    using System.Web.Http.ModelBinding;
    using System.Web.Http.Validation;
    using Ninject;
    using Ninject.Modules;
    using Geocrest.Web.Mvc.Controllers;
    using Geocrest.Web.Mvc.Documentation;

    /// <summary>
    /// Registers the default bindings for the core framework application. Since the release
    /// of MVC 4 RTW, the default services are not being respected by our 
    /// <see cref="T:Geocrest.Web.Mvc.DependencyResolution.NinjectDependencyResolver"/>. To help with
    /// resolve the issue, this class explicitly binds the default interfaces with the appropriate
    /// implementations.
    /// </summary>
    internal sealed class WebApiNinjectionModule : NinjectModule
    {
        /// <summary>
        /// Loads the module into the kernel.
        /// </summary>
        public override void Load()
        {
            Bind<IHttpControllerSelector>().To<AcceptHeaderVersionedControllerSelector>()
                .WithConstructorArgument("configuration", GlobalConfiguration.Configuration);
            Bind<IDocumentationProvider>().To<XmlCommentDocumentationProvider>()
                .WithConstructorArgument("searchSubDirectories", true)
                .WithConstructorArgument("folders", new List<string>(BaseApplication._areafolders.ToArray()) 
                {
                    HostingEnvironment.MapPath("~/bin")
                }.ToArray());
            GlobalConfiguration.Configuration.SetDocumentationProvider(
                this.Kernel.TryGet<IDocumentationProvider>());
            Bind<IApiExplorer>().To<VersionedApiExplorer>()
                .WithConstructorArgument("configuration", GlobalConfiguration.Configuration);
            //Bind<AreaApiExplorer>().ToSelf()
            //    .WithConstructorArgument("configuration", GlobalConfiguration.Configuration);
            Bind<HelpController>().ToSelf()
                .WithConstructorArgument("configuration", GlobalConfiguration.Configuration);
            Bind<IAssembliesResolver>().To<DefaultAssembliesResolver>();
            Bind<IHttpControllerTypeResolver>().To<DefaultHttpControllerTypeResolver>();
            Bind<IHttpActionSelector>().To<ApiControllerActionSelector>();
            Bind<IActionValueBinder>().To<DefaultActionValueBinder>();
            Bind<IBodyModelValidator>().To<DefaultBodyModelValidator>();
            Bind<IContentNegotiator>().To<DefaultContentNegotiator>();
            Bind<IHttpControllerActivator>().To<DefaultHttpControllerActivator>();
            Bind<IHttpActionInvoker>().To<ApiControllerActionInvoker>();
            Bind<ModelMetadataProvider>().To<DataAnnotationsModelMetadataProvider>();
        }
    }
}
