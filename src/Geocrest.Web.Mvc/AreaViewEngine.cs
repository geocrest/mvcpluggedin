namespace Geocrest.Web.Mvc
{
    using System.Web.Mvc;

    /// <summary>
    /// Allows for a custom set of View locations to be defined for MVC Areas.
    /// </summary>
    public class AreaViewEngine : RazorViewEngine
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Web.Mvc.AreaViewEngine"/> class.
        /// </summary>
        public AreaViewEngine() 
        {
            AreaViewLocationFormats = new []{
                 "~/Areas/{2}/Views/{1}/{0}.cshtml",
                "~/Areas/{2}/Views/{1}/{0}.vbhtml",
                "~/Areas/{2}/Shared/{0}.cshtml",
                "~/Areas/{2}/Shared/{0}.vbhtml",
            };
            ViewLocationFormats = new[] {                  
                "~/Views/{1}/{0}.cshtml",
                "~/Views/{1}/{0}.vbhtml",
                "~/Views/Shared/{0}.cshtml",
                "~/Views/Shared/{0}.vbhtml"
            };
            PartialViewLocationFormats = ViewLocationFormats;          
        }
    }
}
