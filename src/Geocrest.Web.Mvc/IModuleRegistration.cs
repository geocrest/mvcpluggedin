namespace Geocrest.Web.Mvc
{
    using System.Web.Http;
    using System.Web.Mvc;
    using System.Web.Routing;

    /// <summary>
    /// Provides properties for defining what action should be called for an MVC area.
    /// </summary>
    public interface IModuleRegistration
    {
        #region Properties
        /// <summary>
        /// Gets the name of this area.
        /// </summary>
        string Area { get; }

        /// <summary>
        /// Gets the default action to perform.
        /// </summary>
        string DefaultAction { get; }

        /// <summary>
        /// Gets the default controller for this area.
        /// </summary>
        string DefaultController { get; }

        /// <summary>
        /// A description of the module's intended use and/or contents.
        /// </summary>
        /// <value>
        /// The summary description.
        /// </value>
        string Description { get; }

        /// <summary>
        /// Gets the label used for display purposes.
        /// </summary>
        string Label { get; }

        /// <summary>
        /// The order in which the item is displayed in the site menu.
        /// </summary>
        int ViewOrder { get; }

        /// <summary>
        /// Gets a value indicating whether to display this in the site menu.
        /// </summary>
        /// <value>
        /// <b>true</b>, if visible; otherwise, <b>false</b>.
        /// </value>
        bool Visible { get; }
        
        ///// <summary>
        ///// Gets the route prefixes that will be used in the module.
        ///// </summary>
        ///// <remarks>Used by the main application's core MVC route to exclude routes defined
        ///// in modules. This allows each module to handle their own routing without the
        ///// main application's route trying to handle it for them.</remarks>
        //string[] RoutePrefixes { get; }
        #endregion

        #region Methods
        /// <summary>
        /// Registers an area in an ASP.NET MVC application using the specified area's context information.
        /// </summary>
        /// <param name="context">Encapsulates the information that is required in order to register the area.</param>
        void RegisterArea(AreaRegistrationContext context);        

        /// <summary>
        /// Registers any HTTP routes with the application including WebApi and OData routes.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        void RegisterHttpRoutes(HttpConfiguration configuration);

        /// <summary>
        /// Registers sample requests and responses for an API.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        void RegisterSamples(HttpConfiguration configuration);

        /// <summary>
        /// Registers any SOAP service routes available with the application. This method will typically be
        /// the last of the registration methods called in order to list the service routes last in the table.
        /// This is necessary to allow correct handling of URLs for both inbound and outbound routes.
        /// </summary>
        /// <param name="routes">The routes.</param>
        void RegisterServiceRoutes(RouteCollection routes);
        #endregion
    }
}
