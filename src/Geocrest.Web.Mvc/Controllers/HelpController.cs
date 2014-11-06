namespace Geocrest.Web.Mvc.Controllers
{
    using System;
    using System.Web.Http;
    using System.Web.Http.Description;
    using System.Web.Mvc;
    using Geocrest.Web.Infrastructure;
    using Geocrest.Web.Mvc.Documentation;

    /// <summary>
    /// The controller that will handle requests for the help page.
    /// </summary>
    public class HelpController : BaseController
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Web.Mvc.Controllers.HelpController" /> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public HelpController(HttpConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        /// <value>
        /// The configuration.
        /// </value>
        public HttpConfiguration Configuration { get; private set; }

        /// <summary>
        /// Returns the main help page
        /// </summary>
        /// <param name="apiId">The API friendly name as specified from 
        /// <see cref="M:Geocrest.Web.Mvc.Documentation.ApiDescriptionExtensions.GetFriendlyId(System.Web.Http.Description.ApiDescription,System.String)"/>.</param>
        /// <param name="homepage">An optional homepage URL to specify for return link.</param>
        /// <param name="version">An optional version number.</param>
        /// <returns>
        /// The help page for the specified API controller or all controllers if none specified.
        /// </returns>
        public virtual ActionResult Index(string apiId, string homepage = "", string version = "")
        {
            ViewBag.Area = string.Empty;
            ViewBag.Version = !string.IsNullOrEmpty(version) ? new Version(version).ToString() : "";
            if (!string.IsNullOrEmpty(apiId))
                return GetApi(apiId, Configuration.Services.GetDefaultApiExplorer(), homepage, version);
            else
            {
                var explorer = Configuration.Services.GetDefaultApiExplorer();
                return View("~/Views/Help/Index.cshtml",explorer.ApiDescriptions);
            }
        }
        /// <summary>
        /// Returns API documentation for the specified area and controller API.
        /// </summary>
        /// <param name="id">The area name.</param>
        /// <param name="apiId">The API friendly name as specified from
        /// <see cref="M:Geocrest.Web.Mvc.Documentation.ApiDescriptionExtensions.GetFriendlyId(System.Web.Http.Description.ApiDescription,System.String)" />.</param>
        /// <param name="partial">if set to <b>true</b> return a partial view.</param>
        /// <param name="homepage">An optional homepage URL to specify for return link.</param>
        /// <param name="version">The optional version.</param>
        /// <returns>
        /// The help page for the specified area's API controller.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">id</exception>
        public virtual ActionResult Api(string id, string apiId, bool partial = false, string homepage = "", string version = "")
        {
            Throw.IfArgumentNullOrEmpty(id, "id");
            ViewBag.Area = id;
            ViewBag.Version = !string.IsNullOrEmpty(version) ? version.GetVersion().ToString() : "";
            if (Session["HelpHomePage"] == null || !string.IsNullOrEmpty(homepage))
                Session["HelpHomePage"] = homepage;
            var explorer = (IApiExplorer)Configuration.Services.GetVersionedApiExplorer();
            if (explorer != null)
            {
                ((IVersionedApiExplorer)explorer).Area = id;
                if (!string.IsNullOrEmpty(version))
                    ((IVersionedApiExplorer)explorer).SetVersion(version);
            }
            else
                explorer = Configuration.Services.GetDefaultApiExplorer();
            if (!String.IsNullOrEmpty(apiId))
            {
                return GetApi(apiId, explorer, Session["HelpHomePage"].ToString(), version);
            }
            else
            {
                var descripts = explorer.ApiDescriptions;                
                return partial ? (ActionResult)PartialView("~/Views/Help/Index.cshtml", descripts) :
                    View("~/Views/Help/Index.cshtml", descripts);
            }
        }
        private ActionResult GetApi(string apiId, IApiExplorer explorer, string homepage, string version)
        {
            HelpPageApiModel apiModel = Configuration.GetHelpPageApiModel(explorer,apiId, homepage, version);
            Throw.NotFoundIfNull(apiModel);            
            return View("~/Views/Help/Api.cshtml", apiModel);            
        }
    }
}