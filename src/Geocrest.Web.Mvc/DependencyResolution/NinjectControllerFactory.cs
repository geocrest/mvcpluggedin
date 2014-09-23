namespace Geocrest.Web.Mvc.DependencyResolution
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using System.Web.Mvc;
    using System.Web.Routing;
    using System.Web.SessionState;
    /// <summary>
    /// Provides controller creation for MVC Areas that are injected through the dynamic plug-in model.
    /// </summary>
    public class NinjectControllerFactory : DefaultControllerFactory
    {
        #region Fields
        private TextInfo ti = new CultureInfo("en-US", false).TextInfo;
        private const string NAMESPACES = "Namespaces";
        private const string CONTROLLER = "Controller";
        private const string ASSEMBLY = "assembly";
        #endregion

        #region Overrides
        /// <summary>
        /// Creates the specified controller by using the specified request context.
        /// </summary>
        /// <param name="requestContext">The context of the HTTP request, which includes the HTTP context and route data.</param>
        /// <param name="controllerName">The name of the controller.</param>
        /// <returns>
        /// The controller.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="requestContext"/> parameter is null.</exception>
        ///   
        /// <exception cref="T:System.ArgumentException">The <paramref name="controllerName"/> parameter is null or empty.</exception>
        public override IController CreateController(RequestContext requestContext, string controllerName)
        {
            return base.CreateController(requestContext, controllerName);
        }
        /// <summary>
        /// Retrieves the controller instance for the specified request context and controller type.
        /// </summary>
        /// <param name="requestContext">The context of the HTTP request, which includes the HTTP context and route data.</param>
        /// <param name="controllerType">The type of the controller.</param>
        /// <returns>
        /// The controller instance.
        /// </returns>
        /// <exception cref="T:System.Web.HttpException">
        ///   <paramref name="controllerType"/> is null.</exception>
        ///   
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="controllerType"/> cannot be assigned.</exception>
        ///   
        /// <exception cref="T:System.InvalidOperationException">An instance of <paramref name="controllerType"/> cannot be created.</exception>
        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            if (controllerType == null) return null;
            return base.GetControllerInstance(requestContext, controllerType);
        }
        /// <summary>
        /// Returns the controller's session behavior.
        /// </summary>
        /// <param name="requestContext">The request context.</param>
        /// <param name="controllerType">The type of the controller.</param>
        /// <returns>
        /// The controller's session behavior.
        /// </returns>
        protected override SessionStateBehavior GetControllerSessionBehavior(
            RequestContext requestContext, Type controllerType)
        {
            return base.GetControllerSessionBehavior(requestContext, controllerType);
        }
        /// <summary>
        /// Retrieves the controller type for the specified name and request context.
        /// </summary>
        /// <param name="requestContext">The context of the HTTP request, which includes the HTTP context and route data.</param>
        /// <param name="controllerName">The name of the controller.</param>
        /// <returns>
        /// The controller type.
        /// </returns>
        protected override Type GetControllerType(RequestContext requestContext, string controllerName)
        {
            if (requestContext.RouteData.DataTokens != null && requestContext.RouteData.DataTokens.Count > 0)
            {
                string[] namespaces = (string[])requestContext.RouteData.DataTokens[NAMESPACES];                
                string assembly = (requestContext.RouteData.Values[ASSEMBLY] != null) ?
                    requestContext.RouteData.Values[ASSEMBLY].ToString() : string.Empty;
                if (namespaces != null && namespaces.Length > 0)
                {
                    foreach (var ns in namespaces.Where(x => !string.IsNullOrEmpty(x)))
                    {
                        string controllertype = ns + "." + ti.ToTitleCase(controllerName) + CONTROLLER;
                        Assembly loaded = null;
                        Type t = null;
                        // look for specific assembly first
                        if (!string.IsNullOrEmpty(assembly))
                        {
                            loaded = AppDomain.CurrentDomain.GetAssemblies().SingleOrDefault(x =>
                                x.ManifestModule.Name.Contains(assembly));
                            if (loaded != null)
                            {
                                t = loaded.GetType(controllertype, false, true);
                                if (t != null) return t;
                                else return base.GetControllerType(requestContext, controllerName);
                            }
                            else
                            {
                                t = GetControllerTypeFromAllAssemblies(requestContext, controllertype);
                                if (t != null) return t;
                            }
                        }
                        else
                        {
                            t = GetControllerTypeFromAllAssemblies(requestContext, controllertype);
                            if (t != null) return t;
                        }
                    }
                }
            }
            return base.GetControllerType(requestContext, controllerName);
        }
        #endregion

        #region Methods

        #region Private
        /// <summary>
        /// Gets the controller type from the first loaded assembly that contains the specified type.
        /// </summary>
        /// <param name="ctx">An object of the type <see cref="T:System.Web.Routing.RequestContext"/>.</param>
        /// <param name="controllerType">Type of the controller.</param>
        /// <returns>
        /// Returns an instance of <see cref="T:System.Type"/>
        /// </returns>
        /// <remarks>
        /// Only use this method as a last resort to find the correct type since it looks in all the loaded
        /// assemblies.
        /// </remarks>
        private Type GetControllerTypeFromAllAssemblies(RequestContext ctx, string controllerType)
        {
            // try the executing assembly first
            Type t = Assembly.GetExecutingAssembly().GetType(controllerType, false, true);
            return (t != null) ? t : AppDomain.CurrentDomain.GetAssemblies().Select(a => 
                    a.GetType(controllerType,false, true)).FirstOrDefault(type => type != null);
        }
        #endregion

        #endregion
    }
}
