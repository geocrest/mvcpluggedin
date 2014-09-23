namespace Geocrest.Web.Mvc.Documentation
{
    using System.Web.Http;
    using System.Web.Http.Description;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Ninject;
    /// <summary>
    /// Use this class to customize the Help Page. For example you can set a custom 
    /// <see cref="T:System.Web.Http.Description.IDocumentationProvider"/> to supply the documentation
    /// or you can provide the samples for the requests/responses.
    /// </summary>
    public static class HelpPageConfig
    {
        /// <summary>
        /// Registers the help page routes.
        /// </summary>
        /// <param name="routes">The routes collection.</param>
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.MapRoute(
                name: "HelpCss",
                url: "help/css/{id}",
                defaults: new
                {
                    controller = "help",
                    action = "contentfile",
                    area = "",
                    id = UrlParameter.Optional
                },
                namespaces: new[] { "Geocrest.Web.Mvc.Controllers" }
                );
            routes.MapRoute(
                name: "Help",
                url: "help/{apiId}",
                defaults: new
                {
                    controller = "help",
                    action = "index",
                    apiId = UrlParameter.Optional,
                    area = ""
                },
                namespaces: new [] { "Geocrest.Web.Mvc.Controllers" }
            );
            routes.MapRoute(
                 name: "Area_Help",
                 url: "{id}/help/{apiId}",
                 defaults: new
                 {
                     controller = "help",
                     action = "api",
                     apiId = UrlParameter.Optional,
                     area = ""
                 },
                 namespaces: new[] { "Geocrest.Web.Mvc.Controllers" }
             );            
        }
        /// <summary>
        /// Registers the help page component of the application, including sample objects/requests.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public static void Register(HttpConfiguration configuration)
        {
            //// Uncomment the following to use the documentation from XML documentation file.           
            //config.SetDocumentationProvider(BaseApplication.Kernel.TryGet<IDocumentationProvider>());
           
            //// Uncomment the following to use "sample string" as the sample for all actions that have string as the body parameter or return type.
            //// Also, the string arrays will be used for IEnumerable<string>. The sample objects will be serialized into different media type 
            //// formats by the available formatters.
            //config.SetSampleObjects(new Dictionary<Type, object>
            //{
            //    {typeof(string), "sample string"},
            //    {typeof(IEnumerable<string>), new string[]{"sample 1", "sample 2"}}
            //});

            //// Uncomment the following to use "[0]=foo&[1]=bar" directly as the sample for all actions that support form URL encoded format
            //// and have IEnumerable<string> as the body parameter or return type.
            //config.SetSampleForType("[0]=foo&[1]=bar", new MediaTypeHeaderValue("application/x-www-form-urlencoded"), typeof(IEnumerable<string>));

            //// Uncomment the following to use "1234" directly as the request sample for media type "text/plain" on the controller named "Values"
            //// and action named "Put".
            //config.SetSampleRequest(new LinkSample(new Dictionary<string, string> 
            //    {
            //        {"http://www.google.com", "Google"},
            //        {"http://www.bing.com", "Bing"}
            //    }),
            //    new MediaTypeHeaderValue("application/json"), "Sample", "Get");
            //config.SetSampleRequest(new LinkSample(new Dictionary<string, string> 
            //    {
            //        {"http://www.google.com", "Google"},
            //        {"http://www.bing.com", "Bing"}
            //    }),
            //    new MediaTypeHeaderValue("text/json"), "Sample", "Get");

            //// Uncomment the following to use the image on "../images/aspNetHome.png" directly as the response sample for media type "image/png"
            //// on the controller named "Values" and action named "Get" with parameter "id".
            //config.SetSampleResponse(new ImageSample("../images/aspNetHome.png"), new MediaTypeHeaderValue("image/png"), "Values", "Get", "id");

            //// Uncomment the following to correct the sample request when the action expects an HttpRequestMessage with ObjectContent<string>.
            //// The sample will be generated as if the controller named "Values" and action named "Get" were having string as the body parameter.
            //config.SetActualRequestType(typeof(string), "Values", "Get");

            //// Uncomment the following to correct the sample response when the action returns an HttpResponseMessage with ObjectContent<string>.
            //// The sample will be generated as if the controller named "Values" and action named "Post" were returning a string.
            //config.SetActualResponseType(typeof(string), "Values", "Post");
        }
    }
}