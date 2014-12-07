
namespace Geocrest.Web.Mvc
{
    #region Usings
    using Elmah;
    using Geocrest.Data.Contracts;
    using Geocrest.Web.Infrastructure;
    using Geocrest.Web.Infrastructure.DependencyResolution;
    using Geocrest.Web.Mvc.Configuration;
    using Geocrest.Web.Mvc.Controllers;
    using Geocrest.Web.Mvc.DependencyResolution;
    using Geocrest.Web.Mvc.Documentation;
    using Geocrest.Web.Mvc.Formatting;
    using Geocrest.Web.Mvc.Models;
    using Newtonsoft.Json;
    using Ninject;
    using Ninject.Extensions.Conventions;
    using Ninject.Modules;
    using Ninject.Web.Common;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data.Entity;
    using System.IO;
    using System.Linq;
    using System.Net.Http.Formatting;
    using System.Reflection;
    using System.Web;
    using System.Web.Configuration;
    using System.Web.Http;
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;
    using System.Xml.Serialization;
    using WebMatrix.WebData;
    #endregion

    /// <summary>
    /// This class provides core functionality by introducing dependency injection into the application
    /// and registering the application's routes, either by registering the routes explicitly or calling one
    /// of the overridable methods. 
    /// </summary>
    /// <remarks>
    /// Use of this class will provide the application with most of the common functionality such as
    /// registering the default filters and routes while allowing any class that inherits this class
    /// to focus on application-specific requirements.
    /// </remarks>
    public class BaseApplication : NinjectHttpApplication
    {
        #region Fields
        private static IKernel _kernel;
        private static List<IModuleRegistration> _allareas;
        private static List<Assembly> _areaassemblies;
        private static string adminRole = "Administrators";
        private static SimpleMembershipInitializer<DbContext> _initializer;
        private static object _initializerLock = new object();
        private static bool _isInitialized;
        private static bool? simpleMembership;
        private static Dictionary<string, string> validAPIResponseFormats = new Dictionary<string, string>
        { 
            {"xml","application/xml"},
            {"json", "application/json"},
            {"halxml", "application/xml"},
            {"haljson", "application/json"}
        };
        internal static List<string> _areafolders;
        #endregion

        #region Static
        #region Properties
        /// <summary>
        /// Gets or sets the application title.
        /// </summary>
        /// <value>
        /// The application title.
        /// </value>
        public static string ApplicationTitle { get; set; }
        /// <summary>
        /// Gets or sets the application description.
        /// </summary>
        /// <value>
        /// The application description.
        /// </value>
        public static string ApplicationDescription { get; set; }
        /// <summary>
        /// Gets the collection of response formats, keyed by query string argument value, that are valid as query string arguments.
        /// </summary>
        /// <value>
        /// The query string response formats.
        /// </value>
        public static IDictionary<string, string> QueryStringResponseFormats { get { return validAPIResponseFormats; } }
        /// <summary>
        /// Gets the name of the query string parameter to use when requesting a specific content type. 
        /// This value is <c>f</c>.
        /// </summary>
        /// <value>
        /// The format parameter name.
        /// </value>
        public static string FormatParameter { get { return "f"; } }
        /// <summary>
        /// Gets the role used to identify administrative users for API access.
        /// </summary>
        /// <value>
        /// The admin role.
        /// </value>
        public static string AdminRole { get { return adminRole; } }

        /// <summary>
        /// Provides the environments that should be considered debugging environments.
        /// Requires a "DebugVersions" AppSetting in the web.config
        /// </summary>
        public static Environment[] DebugVersions
        {
            get
            {
                var debug = ConfigurationManager.AppSettings["DebugVersions"];
                if (debug == null)
                    Throw.SettingsPropertyNotFound("Must have a 'DebugVersions' app setting in web.config");
                return Array.ConvertAll<string, Environment>(debug.ToLower().Split(','),
                    x => (Environment)Enum.Parse(typeof(Environment), x, true));
            }
        }

        /// <summary>
        /// Gets the current environment where the application is being deployed.
        /// Requires a "Environment" AppSetting in the web.config as one of the following: Development, Staging, or Production
        /// </summary>
        public static Environment CurrentEnvironment
        {
            get
            {
                var env = ConfigurationManager.AppSettings["Environment"];
                if (env == null)
                    Throw.SettingsPropertyNotFound("Must have an 'Environment' app setting in web.config");
                return (Environment)Enum.Parse(typeof(Environment), env.ToLower(), true);
            }
        }

        /// <summary>
        /// Gets the kernel instance containing the application's bindings.
        /// </summary>
        /// <value>
        /// The kernel associated with the Ninject application.
        /// </value>
        /// <remarks>This static property hides the inherited 
        /// <see cref="P:Ninject.Web.Common.NinjectHttpApplication.Kernel"/> property.</remarks>
        public static new IKernel Kernel
        {
            get { return BaseApplication._kernel; }
        }

        /// <summary>
        /// Gets the currently logged-in user profile.
        /// </summary>
        public static BaseProfile Profile
        {
            get
            {
                if (IsSimpleMembershipProviderConfigured())
                {
                    IRepository repo = BaseApplication.Kernel.Get<IRepository>();
                    return BaseProfile.CreateProfile(repo, HttpContext.Current.User.Identity.Name);
                }
                else
                {
                    return BaseProfile.CreateProfile(HttpContext.Current.User.Identity.Name);
                }
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Determines whether the simple membership provider is configured for the current application.
        /// </summary>
        /// <param name="configurationfile">The full path to the configuration file.</param>
        /// <returns>
        ///   <c>true</c> if the application is using simple membership; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsSimpleMembershipProviderConfigured(string configurationfile)
        {
            if (BaseApplication.simpleMembership.HasValue) return BaseApplication.simpleMembership.Value;
            bool enabled;
            // check the appsettings value first
            Boolean.TryParse(!string.IsNullOrEmpty(ConfigurationManager.AppSettings[WebSecurity.EnableSimpleMembershipKey]) ?
                ConfigurationManager.AppSettings[WebSecurity.EnableSimpleMembershipKey] : "false", out enabled);
            if (!enabled)
            {
                BaseApplication.simpleMembership = false;
                return BaseApplication.simpleMembership.Value;
            }

            // next get the configuration and check for the provider     
            System.Configuration.Configuration configuration = null;
            if (configurationfile.ToLower().EndsWith("web.config"))
                configuration = WebConfigurationManager.OpenWebConfiguration(configurationfile);
            else
                configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            if (configuration != null)
            {
                MembershipSection membershipSection = configuration.GetSection("system.web/membership") as MembershipSection;
                if (membershipSection != null)
                {
                    string defaultprovider = membershipSection.DefaultProvider;
                    foreach (ProviderSettings provider in membershipSection.Providers)
                    {
                        if (provider.Name == defaultprovider && provider.Type.Contains("WebMatrix.WebData.SimpleMembershipProvider"))
                        {
                            BaseApplication.simpleMembership = true;
                            return BaseApplication.simpleMembership.Value;
                        }
                    }
                }
            }
            BaseApplication.simpleMembership = false;
            return BaseApplication.simpleMembership.Value;
        }
        /// <summary>
        /// Determines whether the simple membership provider is configured for the current application.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if the application is using simple membership; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsSimpleMembershipProviderConfigured()
        {
            return IsSimpleMembershipProviderConfigured("/web.config");
        }
        /// <summary>
        /// Gets all areas found within the configured injection folder.
        /// </summary>
        /// <returns>
        /// Returns an array of <see cref="T:Geocrest.Web.Mvc.IModuleRegistration"/> instances.
        /// </returns>
        public static IModuleRegistration[] GetModules()
        {
            if (BaseApplication._allareas != null) return BaseApplication._allareas.ToArray();
            // create new list and populate with configured areas
            BaseApplication._allareas = new List<IModuleRegistration>();
            foreach (Assembly a in BaseApplication.GetAssemblies())
            {
                BaseApplication._allareas.AddRange(a.GetInstances<ModuleRegistration>());
            }
            return BaseApplication._allareas.ToArray();
        }
        /// <summary>
        /// Logs the specified exception using ELMAH.
        /// </summary>
        /// <param name="exception">The exception to log.</param>
        /// <remarks>Use this method in a web host in the case where ELMAH is not available.</remarks>
        public static void LogException(Exception exception)
        {
            try
            {
                if (exception != null)
                {
                    ErrorSignal.FromCurrentContext().Raise(exception);
                }
                else
                {
                    ErrorSignal.FromCurrentContext().Raise(new System.ApplicationException(@"A request to 
log an error has been made but no error was given."));
                }
            }
            catch { }
        }
        /// <summary>
        /// Populates a private class variable with all of the assemblies located within the 
        /// configured injection folder. This allows for a single retrieval of those files 
        /// so that other methods do not need to load the assemblies multiple times.
        /// </summary>
        /// <returns>An array of <see cref="T:System.Reflection.Assembly"/> instances for the current application,
        /// including any modules currently loaded.</returns>
        protected static Assembly[] GetAssemblies()
        {
            if (BaseApplication._areaassemblies != null) return BaseApplication._areaassemblies.ToArray();
            BaseApplication._areaassemblies = new List<Assembly>();
            BaseApplication._areafolders = new List<string>();
            var config = BaseConfiguration.GetInstance(false);
            if (config != null && config.Injection.Folders != null)
            {
                config.Injection.Folders
                    .Cast<FolderConfigurationElement>()
                    .ForEach(f =>
                    {
                        if (!string.IsNullOrEmpty(f.Path))
                        {
                            string path = f.Path;
                            if (path.StartsWith("~"))
                                path = HttpContext.Current.Server.MapPath(path);
                            BaseApplication._areafolders.AddIfNotNull<string>(path);
                            DirectoryInfo dinfo = new DirectoryInfo(path);
                            if (dinfo.Exists)
                            {
                                foreach (FileInfo fi in dinfo.EnumerateFiles("*.dll", SearchOption.AllDirectories))
                                {
                                    //****************************************************************************
                                    // this should be the only place where the additional assemblies are loaded
                                    //****************************************************************************
                                    if (AppDomain.CurrentDomain.GetAssemblies().Count(x =>
                                        x.ManifestModule.FullyQualifiedName.Contains(fi.Name)) == 0 &&
                                        fi.Name != "Geocrest.Data.Sources.Gis.dll" &&
                                        fi.Name != "Geocrest.Data.Sources.dll" &&
                                        fi.Name != "Geocrest.Model.Representations.dll")
                                    {
                                        BaseApplication._areaassemblies.Add(Assembly.LoadFrom(fi.FullName));
                                    }
                                }
                            }
                        }
                    });
            }
            return BaseApplication._areaassemblies.ToArray();
        }
        #endregion
        #endregion

        #region Instance

        #region Public
        /// <summary>
        /// Handles errors during assembly resolution
        /// </summary>
        /// <param name="sender">An object of the type <see cref="T:System.Object">Object</see>.</param>
        /// <param name="args">The <see cref="T:System.ResolveEventArgs"/> instance containing the event data.</param>
        /// <returns>
        /// Returns an instance of <see cref="T:System.Reflection.Assembly"/>
        /// </returns>
        public Assembly AssemblyResolve(object sender, ResolveEventArgs args)
        {
            return BaseApplication.GetAssemblies().SingleOrDefault(a => a.FullName == args.Name);
        }
        #endregion

        #region Protected
        /// <summary>
        /// Registers any global filters. The <see cref="T:System.Web.Mvc.HandleErrorAttribute"/> is already 
        /// added within this method.
        /// </summary>
        /// <param name="filters">The global filter collection.</param>
        protected virtual void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new CompressAttribute());
        }

        /// <summary>
        /// Registers core routes such as ignore routes and help page routes as well as any 
        /// additional routes required by the application. 
        /// <para>The following routes have been ignored within this method:
        /// <list type="bullet">
        /// <item>content/*</item>
        /// <item>scripts/*</item>
        /// <item>views/*</item>
        /// <item>{resource}.axd/{*pathInfo}</item>
        /// <item>{*favicon}</item>
        /// <item>{*allsvc}</item>
        /// </list></para>
        /// </summary>
        /// <param name="routes">The global route collection.</param>
        protected virtual void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.css/{*pathInfo}");
            routes.IgnoreRoute("{resource}.js/{*pathInfo}");
            routes.IgnoreRoute("{resource}.jpg/{*pathInfo}");
            routes.IgnoreRoute("content/*");
            routes.IgnoreRoute("scripts/*");
            routes.IgnoreRoute("views/*");
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("{*favicon}", new { favicon = @"(.*/)?favicon.ico(/.*)?" });
            routes.IgnoreRoute("{*allsvc}", new { allsvc = @".*\.svc(/.*)?" });
            HelpPageConfig.RegisterRoutes(routes);
            RegisterAreas();
        }

        /// <summary>
        /// Raises the <see cref="E:Geocrest.Web.Mvc.BaseApplication.KernelCreated"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected virtual void OnKernelCreated(KernelCreatedEventArgs e)
        {
            if (KernelCreated != null)
                KernelCreated(this, e);
        }

        /// <summary>
        /// Registers css or javascript bundles for the application.
        /// </summary>
        /// <param name="bundles">The global bundle collection.</param>
        protected virtual void RegisterBundles(BundleCollection bundles)
        {
            BaseApplication.GetAssemblies()
                .ForEach(a =>
                    a.GetInterfaceInstances<IBundleConfiguration>()
                        .ForEach(b =>
                            b.RegisterBundles(bundles)));
            AppDomain.CurrentDomain.GetAssemblies()
                .ForEach(a =>
                    a.GetInterfaceInstances<IBundleConfiguration>()
                        .ForEach(b =>
                            b.RegisterBundles(bundles)));
        }


        /// <summary>
        /// Handles the BeginRequest event of the Application control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        protected virtual void Application_BeginRequest(object sender, EventArgs e)
        {
            HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            HttpContext.Current.Response.Cache.SetNoStore();
            EnableCrossDomainAjaxCall();
        }
        /// <summary>
        /// Handles the Error event of the Application control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        protected void Application_Error(object sender, EventArgs e)
        {
            Exception exception = Server.GetLastError();
            try
            {
                // Avoid IIS getting in the middle
                Response.TrySkipIisCustomErrors = true;
                Response.Clear();
                Server.ClearError();

                // Check for dangerous request and set content type because this type of
                // error occurs before content negotiation; therefore, the response's
                // content type is still set to the request's accept header
                if (exception.Message.Contains(Resources.ExceptionStrings.PotentialDanger))
                {
                    var accepts = string.Join(",", Request.AcceptTypes);
                    var qs = Request.QueryString.GetValues(BaseApplication.FormatParameter);
                    // check query string override first
                    if (Request.QueryString.AllKeys.Contains(BaseApplication.FormatParameter) &&
                        BaseApplication.QueryStringResponseFormats.Keys.Any(f => { return qs.Contains(f); }))
                    {
                        Response.ContentType = BaseApplication.QueryStringResponseFormats[qs.First()];
                    }
                    else
                    {
                        Response.ContentType = accepts.Contains("text/xml") ||
                            accepts.Contains("application/xml") || accepts.Length == 0 ?
                            "application/xml" : "application/json";
                    }
                }

                // *** If the request came from a WebApi request send back response as xml or json and exit this method ***                
                if (Response.ContentType.Contains("application/xml") ||
                    Response.ContentType.Contains("text/xml"))
                {
                    // Log error except when the request contains /_vti_bin/ListData.svc
                    if (!Request.Url.AbsolutePath.Contains("/_vti_bin/ListData.svc"))
                        ErrorSignal.FromCurrentContext().Raise(exception);

                    bool includeDetail = GlobalConfiguration.Configuration.ShouldIncludeErrorDetail(Request);
                    Response.StatusCode = (exception is HttpException) ? (exception as HttpException).GetHttpCode() : 500;
                    XmlSerializer xml = new XmlSerializer(typeof(HttpError));
                    xml.Serialize(Response.Output, includeDetail ||
                        User.IsInRole(BaseApplication.AdminRole) ? new HttpError(exception, includeDetail) :
                        new HttpError(exception.Message));
                    return;
                }
                else if (Response.ContentType.Contains("application/json") ||
                    Response.ContentType.Contains("text/json"))
                {
                    // Log error
                    ErrorSignal.FromCurrentContext().Raise(exception);

                    bool includeDetail = GlobalConfiguration.Configuration.ShouldIncludeErrorDetail(Request);
                    Response.StatusCode = (exception is HttpException) ? (exception as HttpException).GetHttpCode() : 500;
                    Response.Write(JsonConvert.SerializeObject(includeDetail ||
                        User.IsInRole(BaseApplication.AdminRole) ? new HttpError(exception, includeDetail) :
                        new HttpError(exception.Message)));
                    return;
                }

                // *** If the request came from an html page send back response as html *** 
                ThrowMVCError(exception);
            }
            catch { }
        }

        #endregion

        #region Private
        private void ThrowMVCError(Exception exception)
        {
            RouteData routeData = new RouteData();
            routeData.Values.Add("controller", "Error");
            routeData.Values.Add("exception", exception);
            routeData.Values.Add("action", "Error");
            if (exception.Message.Contains(Resources.ExceptionStrings.ControllerNotFoundCheck))
            {
                // not an HttpException but Ninject could not find the controller so it's the same as a 404
                routeData.Values["action"] = "Http404";
                routeData.Values.Add("url", HttpContext.Current.Request.Url.OriginalString);
            }
            else if (exception is HttpException || exception is HttpResponseException)
            {
                int code = exception is HttpException ? ((HttpException)exception).GetHttpCode() :
                    (int)((HttpResponseException)exception).Response.StatusCode;
                switch (code)
                {
                    case 404:
                        // Page not found.
                        routeData.Values["action"] = "Http404";
                        routeData.Values.Add("url", HttpContext.Current.Request.Url.OriginalString);
                        break;
                }
            }

            // Call target Controller and pass the routeData.
            IController errorController = BaseApplication.Kernel.Get<ErrorController>();
            errorController.Execute(new RequestContext(
                 new HttpContextWrapper(Context), routeData));
        }

        /// <summary>
        /// Enables cross domain ajax call.
        /// </summary>
        private void EnableCrossDomainAjaxCall()
        {
            HttpContext.Current.Response.AddHeader("Access-Control-Allow-Origin", "*");
            HttpContext.Current.Response.AddHeader("XDomainRequestAllowed", "1");
            if (HttpContext.Current.Request.HttpMethod == "OPTIONS")
            {
                HttpContext.Current.Response.AddHeader("Access-Control-Allow-Methods", "GET, POST, PUT");
                HttpContext.Current.Response.AddHeader("Access-Control-Allow-Headers", "Content-Type, Accept");
                HttpContext.Current.Response.AddHeader("Access-Control-Max-Age", "1728000");
                HttpContext.Current.Response.End();
            }
        }

        /// <summary>
        /// Registers the areas that are found in the configured injection folder.
        /// </summary>
        private void RegisterAreas()
        {
            // Registers areas within the implementing application
            try
            {
                AreaRegistration.RegisterAllAreas();
            }
            catch { }

            // Register areas found within the configured injection folder
            foreach (IModuleRegistration module in BaseApplication.GetModules())
            {
                var context = new AreaRegistrationContext(module.Area, RouteTable.Routes, null);
                string ns = module.GetType().Namespace;

                if (ns != null) context.Namespaces.Add(ns + ".Controllers");

                try
                {
                    module.RegisterArea(context);
                    module.RegisterHttpRoutes(GlobalConfiguration.Configuration);
                    module.RegisterSamples(GlobalConfiguration.Configuration);
                }
                catch (Exception ex)
                {
                    ErrorLog.GetDefault(null).Log(new Elmah.Error(new ModuleLoadException(module.Area, ex)));
                }
            }
        }
        /// <summary>
        /// Registers the WCF service routes within the implementing application's modules.
        /// </summary>
        private void RegisterServiceRoutes()
        {
            // Register service routes within each module
            foreach (IModuleRegistration module in BaseApplication.GetModules())
            {
                module.RegisterServiceRoutes(RouteTable.Routes);
            }
        }
        #endregion

        #region Overrides
        /// <summary>
        /// Called when the application is started. Calls all of the registration methods required by
        /// the application and sets global configuration properties.
        /// </summary>
        protected override void OnApplicationStarted()
        {
            // Set admin role from web.config
            adminRole = !string.IsNullOrEmpty(ConfigurationManager.AppSettings["AdminRole"]) ?
                ConfigurationManager.AppSettings["AdminRole"] : "";

            // Only use the razor view engine
            System.Web.Mvc.ViewEngines.Engines.Clear();
            System.Web.Mvc.ViewEngines.Engines.Add(new ThemableRazorViewEngine());

            // Resolve assemblies that fail to load
            AppDomain.CurrentDomain.AssemblyResolve += AssemblyResolve;

            // Register default stuff as well as any additional filters/routes required by the implementing application
            RegisterGlobalFilters(GlobalFilters.Filters);

            // Register all non-WCF routes
            RegisterRoutes(RouteTable.Routes);

            // Register WCF routes
            RegisterServiceRoutes();

            //// Help page registration 
            HelpPageConfig.Register(GlobalConfiguration.Configuration);

            // Register a final global 404 not found route
            RouteTable.Routes.MapRoute("NotFound", "{*url}",
                new { controller = "Error", action = "Http404" },
                new[] { "Geocrest.Web.Mvc.Controllers" });

            // Bundles for javascript/css minification
            RegisterBundles(BundleTable.Bundles);

            // Add enrichers for HAL
            GlobalConfiguration.Configuration.MessageHandlers.Add(new EnrichingHandler());

            // Add formatters
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings = new JsonSerializerSettings
            {
                Formatting = Newtonsoft.Json.Formatting.Indented,
                //NullValueHandling = NullValueHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Include,

            };
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.AddQueryStringMapping(BaseApplication.FormatParameter, "json", "application/json");
            GlobalConfiguration.Configuration.Formatters.XmlFormatter.AddQueryStringMapping(BaseApplication.FormatParameter, "xml", "application/xml");
            GlobalConfiguration.Configuration.Formatters.Add(new PlainTextFormatter());
            GlobalConfiguration.Configuration.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Default;

            // Ensure ASP.NET Simple Membership is initialized only once per app start
            if (IsSimpleMembershipProviderConfigured())
                System.Threading.LazyInitializer.EnsureInitialized(ref _initializer, ref _isInitialized, ref _initializerLock);
        }

        /// <summary>
        /// Creates the kernel that will manage the application. When complete, will fire the 
        /// <see cref="E:Geocrest.Web.Mvc.BaseApplication.KernelCreated"/> event.
        /// </summary>
        /// <returns>
        /// The created kernel.
        /// </returns>
        protected override Ninject.IKernel CreateKernel()
        {
            if (BaseApplication.Kernel != null) return BaseApplication.Kernel;

            // Specify the factory that will handle MVC controller creation within areas.
            //ControllerBuilder.Current.SetControllerFactory(new NinjectControllerFactory());

            // Initialize the assemblies
            GetAssemblies();

            // Create the kernel and set resolvers for MVC and WebAPI
            BaseApplication._kernel = new StandardKernel();
            NinjectDependencyResolver res = new NinjectDependencyResolver(BaseApplication.Kernel);
            GlobalConfiguration.Configuration.DependencyResolver = res;

            // Register interfaces bound to classes
            BaseApplication.Kernel.Bind(scanner =>
                scanner.From(BaseApplication.GetAssemblies())
                .SelectAllClasses()
                .WithAttribute<NinjectionAttribute>()
                .BindAllInterfaces());

            // Register any modules that inherit from BaseNinjectModule or NinjectModule
            var loaded = BaseApplication._kernel.GetModules();
            var loadedTypes = AppDomain.CurrentDomain.GetAssemblies().Concat(BaseApplication.GetAssemblies())
                .SelectMany(x => x.GetTypes()
                .Where(t =>
                    (t.IsSubclassOf(typeof(NinjectModule)) ||
                    t.IsSubclassOf(typeof(BaseNinjectModule))) &&
                    !t.IsAbstract && t.GetConstructor(Type.EmptyTypes) != null &&
                    !BaseApplication._kernel.HasModule(t.FullName)));

            List<INinjectModule> modules = new List<INinjectModule>();
            loadedTypes.Distinct().ForEach(x  => modules.AddIfNotNull((INinjectModule)Activator.CreateInstance(x)));
            BaseApplication.Kernel.Load(modules);

            // raise the kernel creation event
            this.OnKernelCreated(new KernelCreatedEventArgs(BaseApplication.Kernel));
            return BaseApplication.Kernel;
        }
        #endregion

        #region Events
        /// <summary>
        /// Occurs when the Ninject kernel has been created.
        /// </summary>
        public event EventHandler<KernelCreatedEventArgs> KernelCreated;
        #endregion
        #endregion


        private class SimpleMembershipInitializer<T> where T : DbContext
        {
            public SimpleMembershipInitializer()
            {
                System.Data.Entity.Database.SetInitializer<T>(null);

                try
                {
                    using (var ctx = BaseApplication.Kernel.Get<T>())
                    {
                        ctx.Database.CreateIfNotExists();
                        WebSecurity.InitializeDatabaseConnection(ctx.Database.Connection.ConnectionString,
                            "System.Data.SqlClient", "UserProfile", "UserId", "UserName", autoCreateTables: true);
                    }

                }
                catch (Exception ex)
                {
                    Throw.InvalidOperation("The ASP.NET Simple Membership database could not be initialized. " + ex.Message);
                }
            }
        }
    }
}
