namespace Geocrest.Web.Mvc
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Web;
    using System.Web.Hosting;
    using System.Web.Http;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Geocrest.Web.Infrastructure;
    using Geocrest.Web.Mvc.Documentation;

    /// <summary>
    /// Provides registration of a module's routes and samples with the application
    /// </summary>
    public abstract class ModuleRegistration : AreaRegistration, IModuleRegistration
    {
        /// <summary>
        /// The JSON accept header 'application/json'
        /// </summary>
        protected const string APPLICATIONJSON = "application/json";
        /// <summary>
        /// The XML accept header 'application/xml'
        /// </summary>
        protected const string APPLICATIONXML = "application/xml";
        /// <summary>
        /// The JSON accept header 'text/json'
        /// </summary>
        protected const string TEXTJSON = "text/json";
        /// <summary>
        /// The XML accept header 'text/xml'
        /// </summary>
        protected const string TEXTXML = "text/xml";
        /// <summary>
        /// The Hypermedia Application Language JSON accept header 'application/hal+json'
        /// </summary>
        protected const string APPLICATIONHALJSON = "application/hal+json";
        /// <summary>
        /// The Hypermedia Application Language XML accept header 'application/hal+xml'
        /// </summary>
        protected const string APPLICATIONHALXML = "application/hal+xml";

        /// <summary>
        /// Explicitly sets objects to use when rendering samples.
        /// </summary>
        /// <typeparam name="T">The type of object to use</typeparam>
        /// <param name="configuration">The configuration.</param>
        /// <param name="value">The object value.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// configuration
        /// or
        /// value
        /// </exception>
        protected virtual void SetSampleObjects<T>(HttpConfiguration configuration, T value)
        {
            Throw.IfArgumentNull(configuration, "configuration");
            Throw.IfArgumentNull(value, "value");
            configuration.SetSampleObjects(new Dictionary<Type, object>
            {
                { typeof(T), value},
                { typeof(IEnumerable<T>), new T[] { value}},
                { typeof(IQueryable<T>), new List<T> { value }.AsQueryable<T>()}
            });
        }
        
        /// <summary>
        /// Explicitly sets objects to use when rendering samples.
        /// </summary>
        /// <typeparam name="T">The type of object to use</typeparam>
        /// <param name="configuration">The configuration.</param>
        /// <param name="values">A list of object values.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// configuration
        /// or
        /// values
        /// </exception>
        protected virtual void SetSampleObjects<T>(HttpConfiguration configuration, IEnumerable<T> values)
        {
            Throw.IfArgumentNull(configuration, "configuration");
            Throw.IfArgumentNull(values, "values");
            configuration.SetSampleObjects(new Dictionary<Type, object>
            {
                { typeof(IEnumerable<T>), new List<T>(values).ToArray()},
                { typeof(IQueryable<T>), new List<T>(values).AsQueryable<T>()}
            });
        }

        ///// <summary>
        ///// Sets sample links for GET requests.
        ///// </summary>
        ///// <param name="config">The configuration.</param>
        ///// <param name="routename">The routename.</param>
        ///// <param name="controller">The controller.</param>
        ///// <param name="action">The action.</param>
        ///// <param name="label">The label.</param>
        ///// <param name="parameters">The parameters.</param>
        //protected virtual void SetSampleGet(HttpConfiguration config, string routename, string controller, string action,
        //     string label, IDictionary<string, object> parameters)
        //{
        //    Throw.IfArgumentNullOrEmpty(routename, "routename");
        //    Throw.IfArgumentNullOrEmpty(controller, "controller");
        //    Throw.IfArgumentNullOrEmpty(action, "action");
        //    Throw.IfArgumentNullOrEmpty(label, "label");

        //    RouteValueDictionary baseroute = new RouteValueDictionary(new
        //    {
        //        area = AreaName.ToLower(),
        //        controller = controller,
        //        routename = routename,
        //        action = action
        //    });
        //    RouteValueDictionary json = new RouteValueDictionary(baseroute);
        //    RouteValueDictionary xml = new RouteValueDictionary(baseroute);
        //    json.Add("f", "json");
        //    xml.Add("f", "xml");
        //}

        /// <summary>
        /// Sets up an HTML sample to use for a specific route.
        /// </summary>
        /// <param name="configuration">The <see cref="T:System.Web.Http.HttpConfiguration" />.</param>
        /// <param name="routename">The route name.</param>
        /// <param name="controller">The name of the controller.</param>
        /// <param name="action">The action method.</param>
        /// <param name="html">The HTML markup to include on the page.</param>
        /// <param name="ispartial">if set to <b>true</b> the HTML is a file path to a partial view.</param>
        /// <param name="parameters">The parameters of the action method.</param>
        /// <returns>
        /// Returns an instance of <see cref="T:Geocrest.Web.Mvc.Documentation.HtmlSample">HtmlSample</see>.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">configuration or routename or controller or action or html</exception>
        protected virtual HtmlSample SetHtmlSample(HttpConfiguration configuration, 
            string routename, string controller, string action,
            string html, bool ispartial, params string[] parameters)
        {
            Throw.IfArgumentNull(configuration, "configuration");
            Throw.IfArgumentNullOrEmpty(routename, "routename");
            Throw.IfArgumentNullOrEmpty(controller, "controller");
            Throw.IfArgumentNullOrEmpty(action, "action");
            Throw.IfArgumentNullOrEmpty(html, "html");

            RouteValueDictionary baseroute = new RouteValueDictionary(new
            {
                area = AreaName.ToLower(),
                controller = controller,
                routename = routename,
                action = action
            });
            return SetHtmlSample(configuration, baseroute, html, parameters,ispartial);
        }

        /// <summary>
        /// Sets up an HTML sample to use for a specific sample.
        /// </summary>
        /// <param name="configuration">The <see cref="T:System.Web.Http.HttpConfiguration" />.</param>
        /// <param name="routename">The route name.</param>
        /// <param name="controller">The name of the controller.</param>
        /// <param name="action">The action method.</param>
        /// <param name="htmlpath">The virtual path to the file containing the HTML markup to include on the page.</param>
        /// <param name="ispartial">if set to <b>true</b> the <paramref name="htmlpath"/> points to a file path that will render as a partial view.</param>
        /// <param name="parameters">The parameters of the action method.</param>
        /// <returns>
        /// Returns an instance of <see cref="T:Geocrest.Web.Mvc.Documentation.HtmlSample">HtmlSample</see>.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">configuration or routename or controller or action or htmlpath</exception>
        protected virtual HtmlSample SetHtmlSampleFromFile(HttpConfiguration configuration,
            string routename, string controller, string action,
            string htmlpath, bool ispartial, params string[] parameters)
        {            
            Throw.IfArgumentNull(configuration, "configuration");
            Throw.IfArgumentNullOrEmpty(routename, "routename");
            Throw.IfArgumentNullOrEmpty(controller, "controller");
            Throw.IfArgumentNullOrEmpty(action, "action");
            Throw.IfArgumentNullOrEmpty(htmlpath, "htmlpath");
            string path = HostingEnvironment.MapPath(htmlpath);
            //Throw.If<string>(path, x => !File.Exists(x), string.Format("The file '{0}' does not exist.", path));
            string html = string.Empty;
            if (!ispartial)
            {
                using (StreamReader reader = new StreamReader(path))
                {
                    html = reader.ReadToEnd();
                }
            }
            else
                html = htmlpath;
            return SetHtmlSample(configuration, routename, controller, action, html, ispartial,parameters);
        }

        /// <summary>
        /// Sets the HTML sample.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="route">The route.</param>
        /// <param name="html">The HTML.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="ispartial"><b>true</b>, if partial; otherwise, <b>false</b>.</param>
        /// <returns></returns>
        private HtmlSample SetHtmlSample(HttpConfiguration configuration, RouteValueDictionary route, 
            string html, string[] parameters = null, bool ispartial = false)
        {
            string action = route["action"].ToString();
            route.Remove("action");

            // finish route
            if (parameters != null)
            {
                foreach (var param in parameters)
                    route.Add(param, null);
            }
            HtmlSample sample = new HtmlSample(html, ispartial);
            if (parameters != null)
                configuration.SetHtmlSample(sample, route["controller"].ToString(), action, parameters);
            else
                configuration.SetHtmlSample(sample, route["controller"].ToString(), action);

            return sample;
        }

        /// <summary>
        /// Sets up a sample with anchor elements for a specific route.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="routename">The route name.</param>
        /// <param name="controller">The name of the controller.</param>
        /// <param name="action">The action method.</param>
        /// <param name="label">The label to use for the anchor element.</param>
        /// <param name="querystring">An optional querystring to append to the link's url.</param>
        /// <param name="parameters">The parameters of the action method.</param>
        /// <returns>
        /// Returns an instance of <see cref="T:Geocrest.Web.Mvc.Documentation.HtmlSample">HtmlSample</see>.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">configuration or routename or controller or action or label</exception>
        protected virtual HtmlSample SetLinkSample(HttpConfiguration configuration, string routename, string controller, string action,
            string label, string querystring = "", params IDictionary<string, object>[] parameters)
        {
            Throw.IfArgumentNull(configuration, "configuration");
            Throw.IfArgumentNullOrEmpty(routename, "routename");
            Throw.IfArgumentNullOrEmpty(controller, "controller");
            Throw.IfArgumentNullOrEmpty(action, "action");
            Throw.IfArgumentNullOrEmpty(label, "label");

            RouteValueDictionary baseroute = new RouteValueDictionary(new
            {
                area = AreaName.ToLower(),
                controller = controller,
                routename = routename,
                action = action
            });
            if (parameters == null || parameters.Length == 0)
                return SetLinkSample(configuration, baseroute, label,querystring, null);
            
            HtmlSample sample = null;
            foreach(var paramDict in parameters)
                sample = SetLinkSample(configuration, baseroute, label, querystring, paramDict);
            return sample;
        }

        /// <summary>
        /// Sets the link sample.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="route">The route.</param>
        /// <param name="label">The label.</param>
        /// <param name="querystring">The querystring.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        private HtmlSample SetLinkSample(HttpConfiguration configuration, RouteValueDictionary route, 
            string label, string querystring = "", IDictionary<string, object> parameters = null)
        {
            var baseroute = new RouteValueDictionary(route);
            string action = route["action"].ToString();
            baseroute.Remove("action");

            // finish route
            if (parameters != null)
            {
                foreach (var kvp in parameters)
                    baseroute.Add(kvp.Key, kvp.Value);                
            }
            if (!string.IsNullOrEmpty(querystring))
            {
                var query = HttpUtility.ParseQueryString(querystring);
                foreach (var q in query.AllKeys)
                    baseroute.Add(q, query[q]);
            }

            // get the existing link sample
            HtmlSample link = configuration.GetHtmlSamples(baseroute["controller"].ToString(), action,
                parameters != null ? parameters.Select(x => x.Key).ToArray() : new string[] { })
                .FirstOrDefault(x => x is LinkSample);

            // add if sample exists
            if (link != null && link is LinkSample)
            {
                (link as LinkSample).Routes.Add(new KeyValuePair<RouteValueDictionary, string>(baseroute, label));
            }
            else // create new sample
            {
                link = new LinkSample(new List<KeyValuePair<RouteValueDictionary, string>> 
                { { new KeyValuePair<RouteValueDictionary, string>(baseroute, label) } });
                if (parameters != null)
                    configuration.SetHtmlSample(link, baseroute["controller"].ToString(), action, parameters.Keys.ToArray());
                else
                    configuration.SetHtmlSample(link, baseroute["controller"].ToString(), action);
            }
            return link;
        }

        ///// <summary>
        ///// Sets sample links for GET requests.
        ///// </summary>
        ///// <param name="config">The configuration.</param>
        ///// <param name="routename">The routename.</param>
        ///// <param name="controller">The controller.</param>
        ///// <param name="action">The action.</param>
        ///// <param name="label">The label.</param>
        //protected void SetSampleGet(HttpConfiguration config, string routename, string controller, string action, string label)
        //{
        //    SetSampleGet(config, routename, controller, action, label, null);
        //}

        #region IModuleRegistration Members
        /// <summary>
        /// Gets the name of this area.
        /// </summary>
        public abstract string Area { get; }

        /// <summary>
        /// Gets the default action to perform.
        /// </summary>
        public abstract string DefaultAction { get; }

        /// <summary>
        /// Gets the default controller for this area.
        /// </summary>
        public abstract string DefaultController { get; }

        /// <summary>
        /// A description of the module's intended use and/or contents.
        /// </summary>
        /// <value>
        /// The summary description.
        /// </value>
        public abstract string Description { get; }

        /// <summary>
        /// Gets the label used for display purposes.
        /// </summary>
        public abstract string Label { get; }

        /// <summary>
        /// Registers any HTTP routes with the application including WebApi and OData routes.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public virtual void RegisterHttpRoutes(HttpConfiguration configuration) { }

        /// <summary>
        /// Registers sample requests and responses for an API.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public virtual void RegisterSamples(HttpConfiguration configuration) { }

        /// <summary>
        /// Registers any SOAP service routes available with the application. This method will typically be
        /// the last of the registration methods called in order to list the service routes last in the table.
        /// This is necessary to allow correct handling of URLs for both inbound and outbound routes.
        /// </summary>
        /// <param name="routes">The routes.</param>
        public virtual void RegisterServiceRoutes(RouteCollection routes) { }

        /// <summary>
        /// The order in which the item is displayed in the site menu.
        /// </summary>
        public abstract int ViewOrder { get; }
        
        /// <summary>
        /// Gets a value indicating whether to display this in the site menu.
        /// </summary>
        /// <value>
        /// <b>true</b>, if visible; otherwise, <b>false</b>.
        /// </value>
        public abstract bool Visible { get; }
        #endregion
    }
}
