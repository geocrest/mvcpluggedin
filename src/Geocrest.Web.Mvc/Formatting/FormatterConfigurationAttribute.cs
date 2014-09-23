namespace Geocrest.Web.Mvc.Formatting
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http.Formatting;
    using System.Web.Http.Controllers;
    using Ninject;
    using Ninject.Parameters;
    using Geocrest.Web.Infrastructure;

    /// <summary>
    /// Provides a way to specify per-controller media formatters.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class FormatterConfigurationAttribute : Attribute, IControllerConfiguration
    {
        #region IControllerConfiguration Members

        /// <summary>
        /// Callback invoked to set per-controller overrides for this controllerDescriptor.
        /// </summary>
        /// <param name="controllerSettings">The controller settings to initialize.</param>
        /// <param name="controllerDescriptor">The controller descriptor. Note that the
        /// <see cref="T:System.Web.Http.Controllers.HttpControllerDescriptor" /> can be associated with the derived
        /// controller type given that <see cref="T:System.Web.Http.Controllers.IControllerConfiguration" /> is inherited.</param>
        public void Initialize(HttpControllerSettings controllerSettings, HttpControllerDescriptor controllerDescriptor)
        {
            // clear formatters from controller
            controllerSettings.Formatters.Clear();
            if (this.XmlFormatter != null)
            {
                controllerSettings.Formatters.Remove(controllerSettings.Formatters.XmlFormatter);
                if (this.AddQueryStringMapping)
                    this.Formatters.Single(x => x == this.XmlFormatter)
                        .AddQueryStringMapping(BaseApplication.FormatParameter, "xml", "application/xml");
            }
            if (this.JsonFormatter != null)
            {
                controllerSettings.Formatters.Remove(controllerSettings.Formatters.JsonFormatter);
                if (this.AddQueryStringMapping)
                    this.Formatters.Single(x => x == this.JsonFormatter)
                        .AddQueryStringMapping(BaseApplication.FormatParameter, "json", "application/json");
            }
            foreach (var formatter in this.Formatters)
                controllerSettings.Formatters.Add(formatter);
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Web.Mvc.Formatting.FormatterConfigurationAttribute" /> class.
        /// </summary>
        /// <param name="entityFormatterName">The name of the entity formatter instance to use when injecting 
        /// an <see cref="T:Geocrest.Web.Mvc.Formatting.IEntityFormatter"/> instance.</param>
        /// <param name="formatterTypes">The list of formatter types to use.</param>
        /// <exception cref="T:System.NotSupportedException">if the specified type(s) are not a type of
        /// <see cref="T:System.Net.Http.Formatting.MediaTypeFormatter" /></exception>
        /// <exception cref="T:System.NotSupportedException">if the specified type(s) are not a type of
        /// <see cref="T:System.Net.Http.Formatting.MediaTypeFormatter" /></exception>
        /// <exception cref="T:System.InvalidOperationException">if a valid formatter could not be found
        /// using the named <see cref="T:Geocrest.Web.Mvc.Formatting.IEntityFormatter"/></exception>
        public FormatterConfigurationAttribute(string entityFormatterName, params Type[] formatterTypes)
        {
            Throw.IfArgumentNullOrEmpty(entityFormatterName, "entityFormatterName");
            this.Formatters = new List<MediaTypeFormatter>();
            if (formatterTypes != null)
            {
                foreach (var type in formatterTypes)
                {
                    // Must be a MediaTypeFormatter
                    Throw.If<Type>(type, x => !typeof(MediaTypeFormatter).IsAssignableFrom(x),
                        string.Format("The type '{0}' must be a 'MediaTypeFormatter'", type.FullName));

                    // Must also not be an abstract class
                    Throw.If<Type>(type, x => x.IsAbstract, string.Format("The type '{0}' must not be abstract.", type.FullName));

                    // Get IEntityFormatter instance if provided
                    MediaTypeFormatter formatter = GetFormatter(type, entityFormatterName);
                    if (formatter == null) Throw.InvalidOperation(
                        string.Format("Could not find an appropriate formatter for the type '{0}'.", type.Name));

                    // Add query string mappings and add to collection
                    if (HandleJsonFormatter(formatter)) continue;
                    if (HandleXmlFormatter(formatter)) continue;
                    this.Formatters.Add(formatter);
                }
            }
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Web.Mvc.Formatting.FormatterConfigurationAttribute" /> class.
        /// </summary>
        /// <param name="formatterTypes">The list of formatter types to use.</param>
        /// <exception cref="T:System.NotSupportedException">if the specified type(s) are not a type of
        /// <see cref="T:System.Net.Http.Formatting.MediaTypeFormatter" /></exception>
        /// <exception cref="T:System.NotSupportedException">if the specified type(s) are not a type of
        /// <see cref="T:System.Net.Http.Formatting.MediaTypeFormatter" /></exception>
        /// <exception cref="T:System.InvalidOperationException">if a valid formatter could not be found</exception>
        public FormatterConfigurationAttribute(params Type[] formatterTypes)
        {
            this.Formatters = new List<MediaTypeFormatter>();
            if (formatterTypes != null)
            {
                foreach (var type in formatterTypes)
                {
                    // Must be a MediaTypeFormatter
                    Throw.If<Type>(type, x => !typeof(MediaTypeFormatter).IsAssignableFrom(x),
                        string.Format("The type '{0}' must be a 'MediaTypeFormatter'", type.FullName));

                    // Must also not be an abstract class
                    Throw.If<Type>(type, x => x.IsAbstract, string.Format("The type '{0}' must not be abstract.", type.FullName));

                    // Get IEntityFormatter instance if provided
                    MediaTypeFormatter formatter = GetFormatter(type);
                    if (formatter == null) Throw.InvalidOperation(
                        string.Format("Could not find an appropriate formatter for the type '{0}'.", type.Name));

                    // Add query string mappings and add to collection
                    if (HandleJsonFormatter(formatter)) continue;
                    if (HandleXmlFormatter(formatter)) continue;
                    this.Formatters.Add(formatter);
                }
            }
        }

        /// <summary>
        /// Gets the XML formatter to use for the current controller.
        /// </summary>
        /// <value>
        /// The XML formatter.
        /// </value>
        public MediaTypeFormatter XmlFormatter { get; private set; }

        /// <summary>
        /// Gets the json formatter to use for the current controller.
        /// </summary>
        /// <value>
        /// The json formatter.
        /// </value>
        public MediaTypeFormatter JsonFormatter { get; private set; }

        /// <summary>
        /// Gets or sets the formatters to use with this controller.
        /// </summary>
        /// <value>
        /// The formatters.
        /// </value>
        public ICollection<MediaTypeFormatter> Formatters { get; private set; }

        /// <summary>
        /// If set to <b>true</b>, then formatters for JSON and XML will have the 
        /// following query string mappings for parameter name 'f':
        /// <list type="bullet">
        /// <item>json : "application/json"</item>
        /// <item>xml : "application/xml"</item>
        /// </list>
        /// </summary>
        /// <value>
        /// <b>true</b>, if this instance add query string mapping; otherwise, <b>false</b>.
        /// </value>
        public bool AddQueryStringMapping { get; set; }

        /// <summary>
        /// Handles the json formatter.
        /// </summary>
        /// <param name="formatter">The formatter.</param>
        /// <returns></returns>
        private bool HandleJsonFormatter(MediaTypeFormatter formatter)
        {
            if (this.JsonFormatter == null && formatter.SupportedMediaTypes
                .Count(x => x.MediaType.ToLower() == "application/json" ||
                    x.MediaType.ToLower() == "text/json") > 0)
            {
                this.JsonFormatter = formatter;
                this.Formatters.Add(formatter);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Gets a formatter of the specified type with an optional IEntityFormatter parameter.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="formatterName">Name of the formatter.</param>
        /// <returns>
        /// Returns an instance of <see cref="MediaTypeFormatter"/>.
        /// </returns>
        private MediaTypeFormatter GetFormatter(Type type, string formatterName = "")
        {
            MediaTypeFormatter formatter;
            if (!string.IsNullOrEmpty(formatterName))
            {
                IEntityFormatter ef = BaseApplication.Kernel.Get<IEntityFormatter>(formatterName);
                formatter = (MediaTypeFormatter)BaseApplication.Kernel.Get(type, new IParameter[] 
                        { 
                            new ConstructorArgument("formatter", ef, true) 
                        }) ?? (MediaTypeFormatter)Activator.CreateInstance(type);
            }
            else
            {
                formatter = (MediaTypeFormatter)Activator.CreateInstance(type);
            }
            return formatter;
        }
        /// <summary>
        /// Handles the XML formatter.
        /// </summary>
        /// <param name="formatter">The formatter.</param>
        /// <returns></returns>
        private bool HandleXmlFormatter(MediaTypeFormatter formatter)
        {
            if (this.XmlFormatter == null && formatter.SupportedMediaTypes
                .Count(x => x.MediaType.ToLower() == "application/xml" ||
                    x.MediaType.ToLower() == "text/xml") > 0)
            {
                this.XmlFormatter = formatter;
                if (this.AddQueryStringMapping) this.XmlFormatter
                    .AddQueryStringMapping(BaseApplication.FormatParameter, "xml", "application/xml");
                this.Formatters.Add(formatter);
                return true;
            }

            return false;
        }
    }
}