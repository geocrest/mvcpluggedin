namespace Geocrest.Web.Mvc.Documentation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Http;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Geocrest.Web.Infrastructure;
    using Geocrest.Web.Mvc.Controllers;

    /// <summary>
    /// This represents a preformatted sample with links on the help page.
    /// </summary>
    public class LinkSample : HtmlSample
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Web.Mvc.Documentation.LinkSample" /> class.
        /// </summary>
        /// <param name="urls">A dictionary of Urls (key) and display text (value).</param>
        /// <exception cref="T:System.ArgumentNullException">url</exception>
        public LinkSample(IDictionary<string, string> urls)
        {
            Throw.IfArgumentNull(urls, "urls");
            Urls = urls;
            Routes = new List<KeyValuePair<RouteValueDictionary, string>>();
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Web.Mvc.Documentation.LinkSample" /> class.
        /// </summary>
        /// <param name="routes">An enumerable of KeyValuePairs: Routes (key) and display text (value).</param>
        /// <exception cref="T:System.ArgumentNullException">routes</exception>
        public LinkSample(List<KeyValuePair<RouteValueDictionary, string>> routes)
        {
            Throw.IfArgumentNull(routes, "routes");
            Routes = routes;
            Urls = new Dictionary<string, string>();
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Web.Mvc.Documentation.LinkSample" /> class.
        /// </summary>
        /// <param name="routes">An enumerable of KeyValuePairs: Routes (key) and display text (value).</param>
        /// <param name="urls">A dictionary of Urls (key) and display text (value).</param>
        /// <exception cref="T:System.ArgumentNullException">urls</exception>
        /// <exception cref="T:System.ArgumentNullException">routes</exception>
        public LinkSample(List<KeyValuePair<RouteValueDictionary, string>> routes, IDictionary<string, string> urls)
        {
            Throw.IfArgumentNull(routes, "routes");
            Throw.IfArgumentNull(urls, "urls");
            Routes = routes;
            Urls = urls;
        }
        /// <summary>
        /// Gets the routes to display.
        /// </summary>
        /// <value>
        /// The collection of routes and their display labels.
        /// </value>
        public List<KeyValuePair<RouteValueDictionary, string>> Routes { get; private set; }
        /// <summary>
        /// Gets the urls to display.
        /// </summary>
        /// <value>
        /// The urls and their display labels.
        /// </value>
        public IDictionary<string, string> Urls { get; private set; }
        /// <summary>
        /// Gets the HTML to render in the page.
        /// </summary>
        public override string Html
        {
            get
            {
                var ctx = new HttpContextWrapper(HttpContext.Current);
                UrlHelper helper = new UrlHelper(new RequestContext(ctx, RouteTable.Routes.GetRouteData(ctx)));
                string html = string.Empty;                
                string version = string.Empty;

                // grab the version from the query string
                var qs = ctx.Request.QueryString.GetValues("version");
                if (qs != null && qs.Any() && GlobalConfiguration.Configuration.Services
                    .GetHttpControllerSelector().GetType() == typeof(AcceptHeaderVersionedControllerSelector))
                {
                    version = string.Format("{0}={1}", AcceptHeaderVersionedControllerSelector.QueryStringArgument, qs.First());
                }
               
                foreach (var uri in this.Urls)
                {
                    var url = CreateUrl(ctx, uri.Key, version);                    
                    html += string.Format("<a class='btn btn-primary sample' href=\"{0}\">{1}</a> ", url, uri.Value);
                }
                foreach (var route in this.Routes)
                {
                    var url = CreateUrl(ctx, helper.HttpRouteUrl(route.Key), version);
                    html += string.Format("<a class='btn btn-primary sample' href=\"{0}\">{1}</a> ", url, route.Value);
                }
                return HttpUtility.UrlDecode(html);
            }
        }

        private static string CreateUrl(HttpContextWrapper ctx, string url, string version)
        {
            string query = string.Empty;
            UriBuilder builder;
            if (url.Contains("?"))
            {
                query = url.Substring(url.IndexOf("?"));
                url = url.Replace(query, "");
                builder = new UriBuilder(ctx.Request.Url.Scheme, ctx.Request.Url.Host, ctx.Request.Url.Port, url, query);
            }
            else
                builder = new UriBuilder(ctx.Request.Url.Scheme, ctx.Request.Url.Host, ctx.Request.Url.Port, url);
            if (!string.IsNullOrEmpty(version))
                builder.Query = !string.IsNullOrEmpty(builder.Query) ? builder.Query.Substring(1) +
                    "&" + version : builder.Query = version;
            return builder.Uri.AbsoluteUri.ToString();
        }
        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            int hashCode = Urls.GetHashCode();
            hashCode ^= Routes.GetHashCode();
            return hashCode;
        }
    }
}