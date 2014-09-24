namespace Geocrest.Web.Mvc
{
    using Geocrest.Web.Infrastructure;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.IO;
    using System.Linq;
    using System.Web;
    using System.Web.Hosting;
    using System.Web.Mvc;

    /// <summary>
    /// Provides a razor view engine for rendering different views for different themes.
    /// To use, add a <c>Theme</c> value to your AppSettings section in web.config.
    /// </summary>
    public class ThemableRazorViewEngine : RazorViewEngine
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Web.Mvc.ThemableRazorViewEngine" /> class.
        /// </summary>
        /// <param name="theme">The theme.</param>
        public ThemableRazorViewEngine(string theme)
        {
            Throw.IfArgumentNullOrEmpty(theme, "theme");
            this.Theme = theme;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Web.Mvc.ThemableRazorViewEngine" />.
        /// </summary>
        public ThemableRazorViewEngine()
        {
            this.Theme = ConfigurationManager.AppSettings["Theme"] ?? string.Empty;
        }
        /// <summary>
        /// Gets or sets the theme to use when rendering views.
        /// </summary>
        /// <value>
        /// The name of the theme.
        /// </value>
        public string Theme { get; set; }
        /// <summary>
        /// Finds the specified partial view by using the specified controller context.
        /// </summary>
        /// <param name="controllerContext">The controller context.</param>
        /// <param name="partialViewName">The name of the partial view.</param>
        /// <param name="useCache">true to use the cached partial view.</param>
        /// <returns>
        /// The partial view.
        /// </returns>
        public override ViewEngineResult FindPartialView(ControllerContext controllerContext, string partialViewName, bool useCache)
        {
            // Get the name of the controller from the path
            string area, controller;
            var keyPath = GetKeyPath(controllerContext, partialViewName, out area, out controller);

            if (fileExists(controllerContext, partialViewName))
            {
                return CacheLocation(controllerContext, keyPath, partialViewName, string.Empty, true);
            }
            string overrideViewName = !string.IsNullOrEmpty(this.Theme)
                                          ? partialViewName + "." + this.Theme.ToLower()
                                          : partialViewName;
            ViewEngineResult result = NewFindPartialView(controllerContext, overrideViewName, useCache,
                keyPath, area, controller);

            // If we're looking for a themed view and couldn't find it try again without modifying the viewname
            if (overrideViewName.Contains("." + this.Theme.ToLower()) && (result == null || result.View == null))
            {
                result = NewFindPartialView(controllerContext, partialViewName, useCache, keyPath,area, controller);
            }
            // if still not found try base methods
            if (result == null || result.View == null)
            {
                result = base.FindPartialView(controllerContext, partialViewName, useCache);
            }
            return result;
        }
        /// <summary>
        /// Finds the specified view by using the specified controller context and master view name.
        /// </summary>
        /// <param name="controllerContext">The controller context.</param>
        /// <param name="viewName">The name of the view.</param>
        /// <param name="masterName">The name of the master view.</param>
        /// <param name="useCache">true to use the cached view.</param>
        /// <returns>
        /// The page view.
        /// </returns>
        public override ViewEngineResult FindView(ControllerContext controllerContext, string viewName,
                                                  string masterName, bool useCache)
        {          
            // Get the name of the controller from the path
            string area, controller;
            var keyPath = GetKeyPath(controllerContext, viewName, out area, out controller);

            if (fileExists(controllerContext, viewName))
            {
                return CacheLocation(controllerContext, keyPath, viewName, masterName, false);               
            }
            string overrideViewName = !string.IsNullOrEmpty(this.Theme)
                                          ? viewName + "." + this.Theme.ToLower()
                                          : viewName;
            ViewEngineResult result = NewFindView(controllerContext, overrideViewName, masterName, useCache,
                keyPath, area, controller);

            // If we're looking for a themed view and couldn't find it try again without modifying the viewname
            if (overrideViewName.Contains("." + this.Theme.ToLower()) && (result == null || result.View == null))
            {
                result = NewFindView(controllerContext, viewName, masterName, useCache, keyPath, area, controller);
            }
            // if still not found try base methods
            if (result == null || result.View == null)
            {
                result = base.FindView(controllerContext, viewName, masterName, useCache);
            }
            return result;
        }

        /// <summary>
        /// Gets a value that indicates whether a file exists in the specified virtual file system (path).
        /// </summary>
        /// <param name="controllerContext">The controller context.</param>
        /// <param name="virtualPath">The virtual path.</param>
        /// <returns>
        /// true if the file exists in the virtual file system; otherwise, false.
        /// </returns>
        private bool fileExists(ControllerContext controllerContext, string virtualPath)
        {
            try
            {
                return File.Exists(controllerContext.HttpContext.Server.MapPath(virtualPath));
            }
            catch (HttpException exception)
            {
                if (exception.GetHttpCode() != 404)
                {
                    throw;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
        private ViewEngineResult NewFindView(ControllerContext controllerContext, string viewName, string masterName,
             bool useCache, string keyPath, string area, string controller)
        {
            // Get the name of the controller from the path
            //string area, controller;
            //var keyPath = GetKeyPath(controllerContext, viewName, out area, out controller);

            // Try the cache           
            if (useCache)
            {
                var result = GetFromCache(controllerContext, keyPath, masterName, false);
                if (result != null) return result;
            }

            // Remember the attempted paths, if not found display the attempted paths in the error message.           
            var attempts = new List<string>();

            string[] locationFormats = string.IsNullOrEmpty(area) ? ViewLocationFormats : AreaViewLocationFormats;

            // for each of the paths defined, format the string and see if that path exists. When found, cache it.           
            foreach (string rootPath in locationFormats)
            {
                string currentPath = string.IsNullOrEmpty(area)
                                         ? string.Format(rootPath, viewName, controller)
                                         : string.Format(rootPath, viewName, controller, area);
                try
                {
                    if (fileExists(controllerContext, currentPath))
                    {
                        return CacheLocation(controllerContext, keyPath, currentPath, masterName, false);
                    }
                }
                catch
                {
                    attempts.Add(currentPath);
                }

                // If not found, add to the list of attempts.               
                attempts.Add(currentPath);
            }

            // if not found by now, simply return the attempted paths.           
            return new ViewEngineResult(attempts.Distinct().ToList());
        }
        private ViewEngineResult NewFindPartialView(ControllerContext controllerContext, string viewName,
            bool useCache, string keyPath, string area, string controller)
        {
            // Get the name of the controller from the path
            //string area, controller;
            //var keyPath = GetKeyPath(controllerContext, viewName, out area, out controller);

            // Try the cache           
            if (useCache)
            {
                var result = GetFromCache(controllerContext, keyPath, string.Empty, true);
                if (result != null) return result;
            }

            // Remember the attempted paths, if not found display the attempted paths in the error message.           
            var attempts = new List<string>();

            string[] locationFormats = string.IsNullOrEmpty(area) ? 
                PartialViewLocationFormats : 
                AreaPartialViewLocationFormats;

            // for each of the paths defined, format the string and see if that path exists. When found, cache it.           
            foreach (string rootPath in locationFormats)
            {
                string currentPath = string.IsNullOrEmpty(area)
                                         ? string.Format(rootPath, viewName, controller)
                                         : string.Format(rootPath, viewName, controller, area);
                try
                {
                    if (fileExists(controllerContext, currentPath))
                    {
                        return CacheLocation(controllerContext, keyPath, currentPath, string.Empty, true);
                    }
                }
                catch
                {
                    attempts.Add(currentPath);
                }

                // If not found, add to the list of attempts.               
                attempts.Add(currentPath);
            }

            // if not found by now, simply return the attempted paths.           
            return new ViewEngineResult(attempts.Distinct().ToList());
        }
        private ViewEngineResult GetFromCache(ControllerContext controllerContext, string keyPath, 
            string masterName, bool partial)
        {
            //If using the cache, check to see if the location is cached.               
            string cacheLocation = ViewLocationCache.GetViewLocation(controllerContext.HttpContext, keyPath);
            if (!string.IsNullOrWhiteSpace(cacheLocation))
            {
                return new ViewEngineResult(partial ? CreatePartialView(controllerContext,cacheLocation):
                    CreateView(controllerContext, cacheLocation, masterName), this);
            }
            return null;
        }
        private ViewEngineResult CacheLocation(ControllerContext controllerContext, string keyPath, 
            string currentPath, string masterName, bool partial)
        {
            ViewLocationCache.InsertViewLocation(controllerContext.HttpContext, keyPath, currentPath);
            return new ViewEngineResult(partial ? CreatePartialView(controllerContext,currentPath):
                CreateView(controllerContext, currentPath, masterName), this);
        }
        private string GetKeyPath(ControllerContext controllerContext, string viewName, out string area, out string controller)
        {
            controller = controllerContext.RouteData.Values["controller"].ToString();
            area = "";

            // if a relative URL path is given (e.g. ~/views/home/index.cshtml or /views/home/index.cshtml)
            // then convert that to the physical path, remove the application portion and return the rest
            if (viewName.StartsWith("~/") || viewName.StartsWith("/"))
            {
                return HostingEnvironment.MapPath(viewName)
                    .Replace(HostingEnvironment.ApplicationPhysicalPath, String.Empty);                
            }
            
            try
            {
                var a = controllerContext.RouteData.DataTokens["area"] ?? controllerContext.RouteData.Values["area"];
                area = a != null ? a.ToString() : string.Empty;
            }
            catch
            {
            }

            // Create the key for caching purposes           
            return Path.Combine(area, controller, viewName);
        }
    }
}