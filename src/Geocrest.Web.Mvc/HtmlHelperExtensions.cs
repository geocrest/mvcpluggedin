namespace Geocrest.Web.Mvc
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq.Expressions;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Mvc.Ajax;
    using System.Web.Mvc.Html;
    using System.Web.Routing;
    using Geocrest.Web.Infrastructure;
    /// <summary>
    /// Provides extension methods for rendering HTML markup
    /// </summary>
    public static class HtmlHelperExtensions
    {
        /// <summary>
        /// Creates a listitem with an anchor element for display within a bootstrap navbar.
        /// This method will determine if the link should have a CSS class called 'active' depending
        /// on the current url.
        /// </summary>
        /// <param name="helper">The helper object.</param>
        /// <param name="linkText">The text to display.</param>
        /// <param name="actionName">Name of the action.</param>
        /// <param name="controllerName">Name of the controller.</param>
        /// <param name="routeValues">Optional route values.</param>
        /// <param name="htmlAttributes">Optional HTML attributes.</param>
        /// <returns>
        /// Returns an instance of <see cref="T:System.Web.Mvc.MvcHtmlString">MvcHtmlString</see>.
        /// </returns>
        public static MvcHtmlString MenuLink(
            this HtmlHelper helper,
            string linkText,
            string actionName,
            string controllerName,
            object routeValues = null,
            object htmlAttributes = null)
        {
            var currentAction = helper.ViewContext.RouteData.GetRequiredString("action").ToLower();
            var currentController = helper.ViewContext.RouteData.GetRequiredString("controller").ToLower();
            var currentArea = (string)helper.ViewContext.RouteData.DataTokens["area"] ?? string.Empty;
            var area = routeValues != null && routeValues.GetType().GetProperty("area") != null ?
                (string)routeValues.GetType().GetProperty("area").GetValue(routeValues,null) : string.Empty;
            var builder = new TagBuilder("li")
            {
                InnerHtml = helper.ActionLink(linkText, actionName.ToLower(), controllerName.ToLower(),
                routeValues, htmlAttributes).ToHtmlString()
            };
            if (controllerName.ToLower() == currentController && 
                actionName.ToLower() == currentAction &&
                area.ToLower() == currentArea)
                builder.AddCssClass("active");
            return new MvcHtmlString(builder.ToString());
        }
        /// <summary>
        /// Returns the display name of a model property contained within an generic enumerable.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="helper">The helper.</param>
        /// <param name="expression">The expression.</param>
        /// <returns>
        /// Returns an instance of <see cref="T:System.Web.Mvc.MvcHtmlString">MvcHtmlString</see>.
        /// </returns>
        public static MvcHtmlString DisplayNameForEnumerable<TModel, TProperty>(
            this HtmlHelper<IEnumerable<TModel>> helper,
            Expression<Func<TModel, TProperty>> expression)
        {
            var name = ExpressionHelper.GetExpressionText(expression);
            name = helper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(name);
            var metadata = ModelMetadataProviders.Current.GetMetadataForProperty(() =>
                Activator.CreateInstance<TModel>(), typeof(TModel), name);
            return new MvcHtmlString(!string.IsNullOrEmpty(metadata.DisplayName) ? metadata.DisplayName : name);
        }
        /// <summary>
        /// Add a stylesheet CSS link to the markup that points to the specified resource.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="resourceName">Name of the resource.</param>
        /// <returns>
        /// Returns an instance of <see cref="T:System.Web.Mvc.MvcHtmlString"/>.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="htmlHelper"/> is null
        /// or
        /// <paramref name="resourceName"/> is null</exception>
        public static MvcHtmlString CssLink(this HtmlHelper htmlHelper, string resourceName)
        {
            Throw.IfArgumentNullOrEmpty(resourceName, "resourceName");
            Throw.IfArgumentNull(htmlHelper, "htmlHelper");
            var builder = new TagBuilder("link");
            builder.Attributes.Add("type", "text/css");
            builder.Attributes.Add("rel", "stylesheet");
            UrlHelper helper = new UrlHelper(htmlHelper.ViewContext.RequestContext);
            var route = new RouteValueDictionary
            {                
                { "controller", "help" },
                { "action", "contentfile" },
                { "area", "" },
                { "id", resourceName }
            };

            builder.Attributes.Add("href", helper.RouteUrl(route));
            //using (StreamReader reader = new StreamReader(Assembly.GetExecutingAssembly()
            //    .GetManifestResourceStream(resourceName)))
            //{
            //    builder.SetInnerText(reader.ReadToEnd());
            //}
            return new MvcHtmlString(builder.ToString());
        }
        /// <summary>
        /// Registers a script bundle from a partial page for inclusion in a master page where the 
        /// <see cref="M:Geocrest.Web.Mvc.HtmlHelperExtensions.RenderPartialScriptBundles(System.Web.Mvc.HtmlHelper)"/> 
        /// method has been called.
        /// </summary>
        /// <param name="htmlhelper">The htmlhelper.</param>
        /// <param name="bundles">The bundles to render.</param>
        /// <returns>
        /// Returns an instance of <see cref="T:System.Web.IHtmlString">IHtmlString</see>.
        /// </returns>
        public static IHtmlString ScriptBundle(this HtmlHelper htmlhelper, IHtmlString bundles)
        {
            if (htmlhelper.ViewContext.HttpContext.Items["scriptbundles"] != null)
                ((List<IHtmlString>)htmlhelper.ViewContext.HttpContext.Items["scriptbundles"]).AddIfNotNull<IHtmlString>(bundles);
            else
                htmlhelper.ViewContext.HttpContext.Items["scriptbundles"] = new List<IHtmlString>() { bundles };
            return new HtmlString(string.Empty);
        }
        /// <summary>
        /// Renders script bundles that have been registered in partial pages using the 
        /// <see cref="M:Geocrest.Web.Mvc.HtmlHelperExtensions.ScriptBundle(System.Web.Mvc.HtmlHelper,System.Web.IHtmlString)"/> method.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <returns>
        /// Returns an instance of <see cref="T:System.Web.IHtmlString">IHtmlString</see>.
        /// </returns>
        public static IHtmlString RenderPartialScriptBundles(this HtmlHelper htmlHelper)
        {
            IHtmlString html = new HtmlString(string.Empty);
            if (htmlHelper.ViewContext.HttpContext.Items["scriptbundles"] != null)
            {
                List<IHtmlString> bundles = (List<IHtmlString>)htmlHelper.ViewContext.HttpContext.Items["scriptbundles"];
                bundles.ForEach(b => 
                    {
                        if (b != null)
                        {
                            IHtmlString htmlstring = new HtmlString(html.ToHtmlString() + "\r\n" + b.ToHtmlString());
                            html = new HtmlString(htmlstring.ToHtmlString());
                        }
                    });               
            }
            return html;
        }
        /// <summary>
        /// Generates an anchor tag from the portions of a string that contain an http:// or https:// segment.
        /// </summary>
        /// <typeparam name="TModel">The type of model being displayed.</typeparam>
        /// <typeparam name="TProperty">The type of property containing the string to linkify.</typeparam>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="expression">An expression that represents the value to linkify.</param>
        /// <returns>The value of the model property with anchor tags where there are http segments.</returns>
        public static MvcHtmlString Linkify<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {
            var metadata = ModelMetadataProviders.Current.GetMetadataForProperty(() => Activator.CreateInstance<TModel>(), typeof(TModel),
                ExpressionHelper.GetExpressionText(expression));
            if (metadata.ModelType != typeof(string)) return htmlHelper.DisplayFor(expression);
            var value = htmlHelper.DisplayFor(expression).ToString();
            if (value.ToLower().Contains("http://") || value.ToLower().Contains("https://"))
            {
                string[] segments = value.Split(' ');
                List<string> newsegments = new List<string>();
                foreach (string segment in segments)
                {
                    string text = segment;
                    if (segment.ToLower().StartsWith("http://") || segment.ToLower().StartsWith("https://"))
                    {
                        text = string.Format("<a href='{0}' target='_blank' title='{0}' rel='tooltip'>{0}</a>", segment);
                    }
                    newsegments.Add(text);
                }
                value = string.Join(" ", newsegments);
            }
            var result = new MvcHtmlString(value);
            return result;
        }
        /// <summary>
        /// Generates an anchor tag wraped around an image.
        /// </summary>
        /// <param name="helper">The Ajax helper.</param>
        /// <param name="imageUrl">The image URL.</param>
        /// <param name="altText">The alternate text.</param>
        /// <param name="actionName">Name of the action.</param>
        /// <param name="controllerName">Name of the controller.</param>
        /// <param name="routeValues">The route values.</param>
        /// <param name="ajaxOptions">The ajax options.</param>
        /// <returns>
        /// Returns an anchor tag with an image.
        /// </returns>
        public static MvcHtmlString ImageActionLink(this AjaxHelper helper, string imageUrl, string altText, string actionName,
             string controllerName, object routeValues, AjaxOptions ajaxOptions)
        {
            var builder = new TagBuilder("img");
            builder.MergeAttribute("src", imageUrl);
            builder.MergeAttribute("alt", altText);
            builder.MergeAttribute("style", "border:none");
            var link = helper.ActionLink("[replaceme]", actionName, controllerName, routeValues, ajaxOptions);
            return new MvcHtmlString(link.ToHtmlString().Replace("[replaceme]", builder.ToString(TagRenderMode.SelfClosing)));
        }        
        /// <summary>
        /// Renders the partial view as a string.
        /// </summary>
        /// <param name="controller">The controller.</param>
        /// <param name="viewName">Name of the view.</param>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static string RenderPartialViewToString(Controller controller, string viewName, object model)
        {
            Throw.IfArgumentNull(controller, "controller");
            controller.ViewData.Model = model;
            try
            {
                using (StringWriter sw = new StringWriter())
                {
                    ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(controller.ControllerContext, viewName);
                    ViewContext viewContext = new ViewContext(controller.ControllerContext, viewResult.View, controller.ViewData, controller.TempData, sw);
                    viewResult.View.Render(viewContext, sw);

                    return sw.GetStringBuilder().ToString();
                }
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
    }
}