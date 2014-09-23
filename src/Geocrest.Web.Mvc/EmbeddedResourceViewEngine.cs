
namespace Geocrest.Web.Mvc
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Web;
    using System.Web.Mvc;
    using Geocrest.Web.Infrastructure;
    /// <summary>
    /// Allows embedded resources to be written out as views to their respective file locations on the host application
    /// </summary>
    public class EmbeddedResourceViewEngine: RazorViewEngine
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Web.Mvc.EmbeddedResourceViewEngine"/> class.
        /// </summary>
        public EmbeddedResourceViewEngine()
        {
            ViewLocationFormats = new[] {
                "~/Views/{1}/{0}.aspx",
                "~/Views/{1}/{0}.ascx",
                "~/Views/Shared/{0}.aspx",
                "~/Views/Shared/{0}.ascx",
                "~/Views/{1}/{0}.cshtml",
                "~/Views/{1}/{0}.vbhtml",
                "~/Views/Shared/{0}.cshtml",
                "~/Views/Shared/{0}.vbhtml",
                "~/tmp/Views/{0}.cshtml",
                "~/tmp/Views/{0}.vbhtml"
            };
            PartialViewLocationFormats = ViewLocationFormats;

            DumpOutViews();
        }
        private static void DumpOutViews()
        {
            IEnumerable<string> resources = typeof(EmbeddedResourceViewEngine).Assembly
                .GetManifestResourceNames().Where(
                    name => name.EndsWith(".cshtml") ||
                    name.EndsWith(".css") || 
                    name.EndsWith(".js") ||
                    name.EndsWith(".png") ||
                    name.EndsWith(".gif"));
            foreach (string res in resources) 
            { 
                DumpOutView(res); 
            }
        }
        private static void DumpOutView(string res)
        {
            string folder = GetFolderPath(res);
            string rootPath = HttpContext.Current.Server.MapPath(folder);
            if (!Directory.Exists(rootPath))
            {
                Directory.CreateDirectory(rootPath);
            }

            Stream resStream = typeof(EmbeddedResourceViewEngine).Assembly.GetManifestResourceStream(res);

            int lastSeparatorIdx = res.LastIndexOf('.');
            string extension = res.Substring(lastSeparatorIdx + 1);
            res = res.Substring(0, lastSeparatorIdx);
            lastSeparatorIdx = res.LastIndexOf('.');
            string fileName = res.Substring(lastSeparatorIdx + 1);

            resStream.SaveStreamToFile(rootPath + fileName + "." + extension);
        }
        private static string GetFileName(string res)
        {
            FileInfo fi = new FileInfo(res);
            return fi.Name;        
        }
        private static string GetFolderPath(string res)
        {
            //res looks something like "Geocrest.Web.Mvc.Views.Shared._Layout.cshtml"
            Assembly a = typeof(EmbeddedResourceViewEngine).Assembly;
            string assemblyname = a.ManifestModule.Name.Substring(0,a.ManifestModule.Name.Length - ".dll".Length);
            //assemblyname looks like "Geocrest.Web.Mvc"
            int assemblylength = assemblyname.Length + 1;
            string fullfile = res.Substring(assemblylength);
            //fullfile looks something like "Views.Shared._Layout.cshtml"
            int filestart = fullfile.Substring(0,fullfile.LastIndexOf('.')).LastIndexOf('.');
            string folder = fullfile.Substring(0, filestart);
            return "~/" + folder.Replace('.', '/') + "/";
        }
    }
    
}
