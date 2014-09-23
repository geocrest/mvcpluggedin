[assembly: WebActivatorEx.PostApplicationStartMethod(typeof(Geocrest.Web.Mvc.RazorGeneratorMvcStart), "Start")]

namespace Geocrest.Web.Mvc
{
    using System;
    using System.Reflection;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.WebPages;
    using RazorGenerator.Mvc;

    /// <summary>
    /// Provides registration of the <see cref="T:RazorGenerator.Mvc.PrecompiledMvcEngine"/> with the application.
    /// </summary>
    public static class RazorGeneratorMvcStart
    {
        /// <summary>
        /// Registers the <see cref="T:RazorGenerator.Mvc.PrecompiledMvcEngine"/> with the application.
        /// </summary>
        public static void Start()
        {
            PrecompiledMvcEngine engine = null;
            try
            {
                engine = new PrecompiledMvcEngine(typeof(RazorGeneratorMvcStart).Assembly)
                {
                    UsePhysicalViewsIfNewer = HttpContext.Current.Request.IsLocal
                };
            }
            catch (ReflectionTypeLoadException e)
            {
                string exceptions = "The following DLL load exceptions occurred:";
                foreach (var x in e.LoaderExceptions)
                {
                    exceptions += x.Message + ",\n\n";
                }
                throw new Exception("Error loading Razor Generator Stuff:\n" + exceptions);
            }
            ViewEngines.Engines.Insert(0, engine);

            // StartPage lookups are done by WebPages. 
            VirtualPathFactoryManager.RegisterVirtualPathFactory(engine);
        }
    }
}
