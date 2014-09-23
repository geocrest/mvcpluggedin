namespace Geocrest.Web.Mvc.Documentation
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Formatting;
    using System.Net.Http.Headers;
    using System.Web;
    using System.Web.Http;
    using System.Web.Http.Description;
    using System.Web.Http.Routing;
    using System.Xml.Linq;
    using Newtonsoft.Json;
    using Geocrest.Web.Infrastructure;
    using Geocrest.Web.Mvc;
    /// <summary>
    /// This class will generate the samples for the help page.
    /// </summary>
    public class HelpPageSampleGenerator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Web.Mvc.Documentation.HelpPageSampleGenerator" /> class.
        /// </summary>
        public HelpPageSampleGenerator() :this(true)
        {           
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Web.Mvc.Documentation.HelpPageSampleGenerator" /> class.
        /// </summary>
        /// <param name="useDataMembersOnly">if set to <b>true</b> use data members only.</param>
        public HelpPageSampleGenerator(bool useDataMembersOnly)
        {
            ActualHttpMessageTypes = new Dictionary<HelpPageSampleKey, Type>();
            ActionSamples = new Dictionary<HelpPageSampleKey, object>();
            SampleObjects = new Dictionary<Type, object>();
            HtmlSamples = new List<KeyValuePair<HelpPageSampleKey, HtmlSample>>();
            DataMembersOnly = useDataMembersOnly;
        }
        /// <summary>
        /// Gets a value indicating whether to use only those properties that are marked as 
        /// <see cref="T:System.Runtime.Serialization.DataMemberAttribute"/>s.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance uses data members only; otherwise, <c>false</c>.
        /// </value>
        public bool DataMembersOnly { get; private set; }
        /// <summary>
        /// Gets CLR types that are used as the content of <see cref="T:System.Net.Http.HttpRequestMessage"/> or 
        /// <see cref="T:System.Net.Http.HttpResponseMessage"/>.
        /// </summary>
        public IDictionary<HelpPageSampleKey, Type> ActualHttpMessageTypes { get; internal set; }

        /// <summary>
        /// Gets the objects that are used directly as samples for certain actions.
        /// </summary>
        public IDictionary<HelpPageSampleKey, object> ActionSamples { get; internal set; }

        /// <summary>
        /// Gets the objects that are serialized as samples by the supported formatters.
        /// </summary>
        public IDictionary<Type, object> SampleObjects { get; internal set; }

        /// <summary>
        /// Gets the objects used to render HTML on the sample page.
        /// </summary>
        public List<KeyValuePair<HelpPageSampleKey, HtmlSample>> HtmlSamples { get; internal set; }

        ///// <summary>
        ///// Gets the HTML samples for a given <see cref="T:System.Web.Http.Description.ApiDescription" />.
        ///// </summary>
        ///// <param name="api">The <see cref="T:System.Web.Http.Description.ApiDescription" />.</param>
        ///// <returns>
        ///// Returns an instance of <see cref="List&lt;Geocrest.Web.Mvc.Documentation.HtmlSample&gt;">
        ///// List&lt;Geocrest.Web.Mvc.Documentation.HtmlSample&gt;</see>.
        ///// </returns>
        ///// <exception cref="System.ArgumentNullException">api</exception>
        //public List<HtmlSample> GetLiveSamples(System.Web.Http.Description.ApiDescription api)
        //{
        //    Throw.IfArgumentNull(api, "api");
        //    string controllerName = api.ActionDescriptor.ControllerDescriptor.ControllerName;
        //    string actionName = api.ActionDescriptor.ActionName;
        //    IEnumerable<string> parameterNames = api.ParameterDescriptions.Select(p => p.Name);
        //    var samples = new List<HtmlSample>();

        //    var actionSamples = GetAllLiveSamples(api, parameterNames);
        //    foreach (var actionSample in actionSamples)
        //    {
        //        samples.Add(actionSample.Value);
        //    }
        //    return samples;            
        //}
        /// <summary>
        /// Gets the request body samples for a given <see cref="T:System.Web.Http.Description.ApiDescription"/>.
        /// </summary>
        /// <param name="api">The <see cref="T:System.Web.Http.Description.ApiDescription"/>.</param>
        /// <returns>The samples keyed by media type.</returns>
        public IDictionary<MediaTypeHeaderValue, object> GetSampleRequests(System.Web.Http.Description.ApiDescription api)
        {
            return GetSample(api, SampleDirection.Request);
        }

        /// <summary>
        /// Gets the response body samples for a given <see cref="T:System.Web.Http.Description.ApiDescription"/>.
        /// </summary>
        /// <param name="api">The <see cref="System.Web.Http.Description.ApiDescription"/>.</param>
        /// <returns>The samples keyed by media type.</returns>
        public IDictionary<MediaTypeHeaderValue, object> GetSampleResponses(System.Web.Http.Description.ApiDescription api)
        {
            return GetSample(api, SampleDirection.Response);
        }
        /// <summary>
        /// Gets the HTML samples associated with the given route parameters
        /// </summary>
        /// <param name="controllerName">Name of the controller.</param>
        /// <param name="actionName">Name of the action.</param>
        /// <param name="parameterNames">The parameter names.</param>
        /// <returns>
        /// The associated <see cref="T:Geocrest.Web.Mvc.Documentation.HtmlSample"/>.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">controllerName</exception>
        /// <exception cref="System.ArgumentNullException">actionName</exception>
        /// <exception cref="System.ArgumentNullException">parameterNames</exception>
        public virtual IEnumerable<HtmlSample> GetHtmlSamples(string controllerName, string actionName, IEnumerable<string> parameterNames)
        {
            Throw.IfArgumentNullOrEmpty(controllerName, "controllerName");
            Throw.IfArgumentNullOrEmpty(actionName, "actionName");
            Throw.IfArgumentNull(parameterNames, "parameterNames");
            var samples = new List<HtmlSample>();
            HashSet<string> parameterNamesSet = new HashSet<string>(parameterNames, StringComparer.OrdinalIgnoreCase);
            foreach (var sample in HtmlSamples)
            {
                HelpPageSampleKey sampleKey = sample.Key;
                if (String.Equals(controllerName, sampleKey.ControllerName, StringComparison.OrdinalIgnoreCase) &&
                    String.Equals(actionName, sampleKey.ActionName, StringComparison.OrdinalIgnoreCase) &&
                    parameterNamesSet.Count == sampleKey.ParameterNames.Count &&
                    parameterNamesSet.All(x => sampleKey.ParameterNames.Contains(x)))
                {
                    samples.Add(sample.Value);
                }
            }
            return samples.AsEnumerable();
        }
        ///// <summary>
        ///// Gets the HTML sample associated with a given <see cref="T:System.Web.Http.Description.ApiDescription" />.
        ///// </summary>
        ///// <param name="api">The API description containing the route values.</param>
        ///// <returns>
        ///// The associated <see cref="T:Geocrest.Web.Mvc.Documentation.HtmlSample" />.
        ///// </returns>
        ///// <exception cref="System.ArgumentNullException">api</exception>
        //public virtual HtmlSample GetHtmlSample(System.Web.Http.Description.ApiDescription api)
        //{
        //    Throw.IfArgumentNull(api, "api");
        //    string controllerName = api.ActionDescriptor.ControllerDescriptor.ControllerName;
        //    string actionName = api.ActionDescriptor.ActionName;
        //    IEnumerable<string> parameterNames = api.ParameterDescriptions.Select(p => p.Name);
        //    return GetHtmlSample(controllerName, actionName, parameterNames);
        //}
        /// <summary>
        /// Gets the HTML samples associated with a given <see cref="T:System.Web.Http.Description.ApiDescription" />.
        /// </summary>
        /// <param name="api">The API description containing the route values.</param>
        /// <returns>
        /// Returns a collection of <see cref="T:Geocrest.Web.Mvc.Documentation.HtmlSample"/>s.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">api</exception>
        public virtual IEnumerable<HtmlSample> GetHtmlSamples(System.Web.Http.Description.ApiDescription api)
        {
            Throw.IfArgumentNull(api, "api");
            string controllerName = api.ActionDescriptor.ControllerDescriptor.ControllerName;
            string actionName = api.ActionDescriptor.ActionName;
            IEnumerable<string> parameterNames = api.ParameterDescriptions.Select(p => p.Name);
            return GetHtmlSamples(controllerName, actionName, parameterNames);
        }
        /// <summary>
        /// Gets the request or response body samples.
        /// </summary>
        /// <param name="api">The <see cref="T:System.Web.Http.Description.ApiDescription"/>.</param>
        /// <param name="sampleDirection">The value indicating whether the sample is for a request or for a response.</param>
        /// <returns>The samples keyed by media type.</returns>
        /// <exception cref="System.ArgumentNullException">api</exception>
        public virtual IDictionary<MediaTypeHeaderValue, object> GetSample(System.Web.Http.Description.ApiDescription api, SampleDirection sampleDirection)
        {
            Throw.IfArgumentNull(api, "api");
            string controllerName = api.ActionDescriptor.ControllerDescriptor.ControllerName;
            string actionName = api.ActionDescriptor.ActionName;
            IEnumerable<string> parameterNames = api.ParameterDescriptions.Select(p => p.Name);
            Collection<MediaTypeFormatter> formatters;
            Type type = ResolveType(api, controllerName, actionName, parameterNames, sampleDirection, out formatters);
            var samples = new Dictionary<MediaTypeHeaderValue, object>();

            // Use the samples provided directly for actions
            var actionSamples = GetAllActionSamples(api, parameterNames, sampleDirection);
            foreach (var actionSample in actionSamples)
            {
                samples.Add(actionSample.Key.MediaType, WrapSampleIfString(actionSample.Value));
            }

            // Do the sample generation based on formatters only if an action doesn't return an HttpResponseMessage.
            // Here we cannot rely on formatters because we don't know what's in the HttpResponseMessage, it might not even use formatters.
            if (type != null && !typeof(HttpResponseMessage).IsAssignableFrom(type))
            {
                object sampleObject = GetSampleObject(type);
                foreach (var formatter in formatters)
                {
                    foreach (MediaTypeHeaderValue mediaType in formatter.SupportedMediaTypes)
                    {
                        if (!samples.ContainsKey(mediaType))
                        {
                            object sample = GetActionSample(controllerName, actionName, parameterNames, type, mediaType, sampleDirection);

                            // If no sample found, try generate sample using formatter and sample object
                            if (sample == null && sampleObject != null)
                            {
                                sample = WriteSampleObjectUsingFormatter(api,formatter, sampleObject, type, mediaType);
                            }

                            samples.Add(mediaType, WrapSampleIfString(sample, formatter));
                        }
                    }
                }
            }

            return samples;
        }

        /// <summary>
        /// Search for samples that are provided directly through 
        /// <see cref="P:Geocrest.Web.Mvc.Documentation.HelpPageSampleGenerator.ActionSamples"/>.
        /// </summary>
        /// <param name="controllerName">Name of the controller.</param>
        /// <param name="actionName">Name of the action.</param>
        /// <param name="parameterNames">The parameter names.</param>
        /// <param name="type">The CLR type.</param>
        /// <param name="mediaType">The media type.</param>
        /// <param name="sampleDirection">The value indicating whether the sample is for a request or for a response.</param>
        /// <returns>The sample that matches the parameters.</returns>
        public virtual object GetActionSample(string controllerName, string actionName, IEnumerable<string> parameterNames, Type type, MediaTypeHeaderValue mediaType, SampleDirection sampleDirection)
        {
            object sample;

            // First, try get sample provided for a specific mediaType, controllerName, actionName and parameterNames.
            // If not found, try get the sample provided for a specific mediaType, controllerName and actionName regardless of the parameterNames
            // If still not found, try get the sample provided for a specific type and mediaType 
            if (ActionSamples.TryGetValue(new HelpPageSampleKey(mediaType, sampleDirection, controllerName, actionName, parameterNames), out sample) ||
                // The following line was removed so that a sample is required to match the parameters
                //ActionSamples.TryGetValue(new HelpPageSampleKey(mediaType, sampleDirection, controllerName, actionName, new string[] {  }), out sample) ||
                ActionSamples.TryGetValue(new HelpPageSampleKey(mediaType, type), out sample))
            {
                return sample;
            }

            return null;
        }

        /// <summary>
        /// Gets the sample object that will be serialized by the formatters. 
        /// First, it will look at the <see cref="P:Geocrest.Web.Mvc.Documentation.HelpPageSampleGenerator.SampleObjects"/>.
        /// If no sample object is found, it will try to create one using <see cref="T:Geocrest.Web.Mvc.Documentation.ObjectGenerator"/>.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>The sample object.</returns>
        public virtual object GetSampleObject(Type type)
        {
            object sampleObject;

            if (!SampleObjects.TryGetValue(type, out sampleObject))
            {
                // Try create a default sample object
                ObjectGenerator objectGenerator = new ObjectGenerator();
                sampleObject = objectGenerator.GenerateObject(type,this.DataMembersOnly);
            }

            return sampleObject;
        }

        /// <summary>
        /// Resolves the type of the action parameter or return value when
        /// <see cref="T:System.Net.Http.HttpRequestMessage" /> or <see cref="T:System.Net.Http.HttpResponseMessage" /> is used.
        /// </summary>
        /// <param name="api">The API description.</param>
        /// <param name="controllerName">Name of the controller.</param>
        /// <param name="actionName">Name of the action.</param>
        /// <param name="parameterNames">The parameter names.</param>
        /// <param name="sampleDirection">The value indicating whether the sample is for a request or a response.</param>
        /// <param name="formatters">The formatters.</param>
        /// <returns>
        /// Returns the return type for the action, the request body parameter type, or <see langword="null"/>.
        /// </returns>
        /// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException"><paramref name="sampleDirection" /> is not defined.</exception>
        /// <exception cref="T:System.ArgumentNullException">api</exception>
        [SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", Justification = "This is only used in advanced scenarios.")]
        public virtual Type ResolveType(System.Web.Http.Description.ApiDescription api, string controllerName, string actionName, IEnumerable<string> parameterNames, SampleDirection sampleDirection, out Collection<MediaTypeFormatter> formatters)
        {
            Throw.IfEnumNotDefined<SampleDirection>(sampleDirection, "sampleDirection");
            Throw.IfArgumentNull(api, "api");
            Type type;
            if (ActualHttpMessageTypes.TryGetValue(new HelpPageSampleKey(sampleDirection, controllerName, actionName, parameterNames), out type) ||
                ActualHttpMessageTypes.TryGetValue(new HelpPageSampleKey(sampleDirection, controllerName, actionName, new string[] {  }), out type))
            {
                // Re-compute the supported formatters based on type
                Collection<MediaTypeFormatter> newFormatters = new Collection<MediaTypeFormatter>();
                foreach (var formatter in api.ActionDescriptor.Configuration.Formatters)
                {
                    if (IsFormatSupported(sampleDirection, formatter, type))
                    {
                        newFormatters.Add(formatter);
                    }
                }
                formatters = newFormatters;
            }
            else
            {
                switch (sampleDirection)
                {
                    case SampleDirection.Request:
                        System.Web.Http.Description.ApiParameterDescription requestBodyParameter = api.ParameterDescriptions.FirstOrDefault(p => p.Source == ApiParameterSource.FromBody);
                        type = requestBodyParameter == null ? null : requestBodyParameter.ParameterDescriptor.ParameterType;
                        formatters = api.SupportedRequestBodyFormatters;
                        break;
                    case SampleDirection.Response:
                    default:
                        type = api.ActionDescriptor.ReturnType;
                        formatters = api.SupportedResponseFormatters;
                        break;
                }
            }

            return type;
        }

        /// <summary>
        /// Writes the sample object using formatter.
        /// </summary>
        /// <param name="api">The API.</param>
        /// <param name="formatter">The formatter.</param>
        /// <param name="value">The value.</param>
        /// <param name="type">The type.</param>
        /// <param name="mediaType">Type of the media.</param>
        /// <returns>
        /// Returns an instance of <see cref="T:System.Object"/>.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        /// formatter
        /// or
        /// mediaType
        /// </exception>
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "The exception is recorded as InvalidSample.")]
        public virtual object WriteSampleObjectUsingFormatter(System.Web.Http.Description.ApiDescription api,
            MediaTypeFormatter formatter, object value, Type type, MediaTypeHeaderValue mediaType)
        {
            Throw.IfArgumentNull(formatter, "formatter");
            Throw.IfArgumentNull(mediaType, "mediaType");

            object sample = String.Empty;
            MemoryStream ms = null;
            HttpContent content = null;
            try
            {
                if (formatter.CanWriteType(type))
                {
                    ms = new MemoryStream();
                    content = new ObjectContent(type, value, formatter, mediaType);
                    var request = new HttpRequestMessage(HttpMethod.Get, api.RelativePath.ToAbsoluteUrl());
                    request.Properties["MS_HttpConfiguration"] = GlobalConfiguration.Configuration;
                    request.Properties["MS_HttpRouteData"] = api.Route.GetRouteData(
                        GlobalConfiguration.Configuration.VirtualPathRoot,request);
                    request.Properties["MS_HttpContext"] = new HttpContextWrapper(HttpContext.Current);
                    UrlHelper helper = new UrlHelper(request);
                   
                    var enrichers = GlobalConfiguration.Configuration.GetResponseEnrichers();
                    var enriched = enrichers.Where(e => e.CanEnrich(type,helper,mediaType))
                        .Aggregate(value, (url, enricher) => enricher.Enrich(api.RelativePath.ToAbsoluteUrl(),helper,value)); 
                    formatter.WriteToStreamAsync(type, enriched, ms, content, null).Wait();
                    ms.Position = 0;
                    StreamReader reader = new StreamReader(ms);
                    string serializedSampleString = reader.ReadToEnd();
                    if (mediaType.MediaType.ToUpperInvariant().Contains("XML"))
                    {
                        serializedSampleString = TryFormatXml(serializedSampleString);
                    }
                    else if (mediaType.MediaType.ToUpperInvariant().Contains("JSON"))
                    {
                        serializedSampleString = TryFormatJson(serializedSampleString);
                    }

                    sample = new TextSample(serializedSampleString,formatter.SupportedMediaTypes);
                }
                else
                {
                    sample = new InvalidSample(String.Format(
                        CultureInfo.CurrentCulture,
                        "Failed to generate the sample for media type '{0}'. Cannot use formatter '{1}' to write type '{2}'.",
                        mediaType,
                        formatter.GetType().Name,
                        type.Name));
                }
            }
            catch (Exception e)
            {
                sample = new InvalidSample(String.Format(
                    CultureInfo.CurrentCulture,
                    "An exception has occurred while using the formatter '{0}' to generate sample for media type '{1}'. Exception message: {2}",
                    formatter.GetType().Name,
                    mediaType.MediaType,
                    e.GetExceptionMessages()));
            }
            finally
            {
                if (ms != null)
                {
                    ms.Dispose();
                }
                if (content != null)
                {
                    content.Dispose();
                }
            }

            return sample;
        }
        
        #region Helper Methods
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Handling the failure by returning the original string.")]
        private static string TryFormatJson(string str)
        {
            try
            {
                object parsedJson = JsonConvert.DeserializeObject(str);
                JsonSerializerSettings settings = GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings;
                settings.Formatting = Formatting.Indented;
                return JsonConvert.SerializeObject(parsedJson, settings);
            }
            catch
            {
                // can't parse JSON, return the original string
                return str;
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Handling the failure by returning the original string.")]
        private static string TryFormatXml(string str)
        {
            try
            {
                XDocument xml = XDocument.Parse(str);
                return xml.ToString();
            }
            catch
            {
                // can't parse XML, return the original string
                return str;
            }
        }

        private static bool IsFormatSupported(SampleDirection sampleDirection, MediaTypeFormatter formatter, Type type)
        {
            switch (sampleDirection)
            {
                case SampleDirection.Request:
                    return formatter.CanReadType(type);
                case SampleDirection.Response:
                    return formatter.CanWriteType(type);
            }
            return false;
        }

        private IEnumerable<KeyValuePair<HelpPageSampleKey, object>> GetAllActionSamples(System.Web.Http.Description.ApiDescription api,
            IEnumerable<string> parameterNames, SampleDirection sampleDirection)
        {
            HashSet<string> parameterNamesSet = new HashSet<string>(parameterNames, StringComparer.OrdinalIgnoreCase);
            foreach (var sample in ActionSamples)
            {
                HelpPageSampleKey sampleKey = sample.Key;
                if (String.Equals(api.ActionDescriptor.ControllerDescriptor.ControllerName, sampleKey.ControllerName, StringComparison.OrdinalIgnoreCase) &&
                    String.Equals(api.ActionDescriptor.ActionName, sampleKey.ActionName, StringComparison.OrdinalIgnoreCase) &&
                    api.ParameterDescriptions.Count == sampleKey.ParameterNames.Count &&
                    api.ParameterDescriptions.All(x => sampleKey.ParameterNames.Contains(x.Name)) &&
                    //(sampleKey.ParameterNames.SetEquals(new string[] { }) || parameterNamesSet.SetEquals(sampleKey.ParameterNames)) &&
                    sampleDirection == sampleKey.SampleDirection)
                {
                    yield return sample;
                }
            }
        }
        //private IEnumerable<KeyValuePair<HelpPageSampleKey, HtmlSample>> GetAllLiveSamples(System.Web.Http.Description.ApiDescription api,
        //    IEnumerable<string> parameterNames)
        //{
        //    HashSet<string> parameterNamesSet = new HashSet<string>(parameterNames, StringComparer.OrdinalIgnoreCase);
        //    foreach (var sample in HtmlSamples)
        //    {
        //        HelpPageSampleKey sampleKey = sample.Key;
        //        if (String.Equals(api.ActionDescriptor.ControllerDescriptor.ControllerName, sampleKey.ControllerName, StringComparison.OrdinalIgnoreCase) &&
        //            String.Equals(api.ActionDescriptor.ActionName, sampleKey.ActionName, StringComparison.OrdinalIgnoreCase) &&
        //            api.ParameterDescriptions.Count == sampleKey.ParameterNames.Count &&
        //            api.ParameterDescriptions.All(x => sampleKey.ParameterNames.Contains(x.Name)))                    
        //        {
        //            yield return sample;
        //        }
        //    }
        //}
        private static object WrapSampleIfString(object sample, MediaTypeFormatter formatter = null)
        {
            string stringSample = sample as string;
            if (stringSample != null)
            {
                if (formatter == null)
                    return new TextSample(stringSample);
                else
                    return new TextSample(stringSample, formatter.SupportedMediaTypes);
            }

            return sample;
        }
        #endregion
    }
}