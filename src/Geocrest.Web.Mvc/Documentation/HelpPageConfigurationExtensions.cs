namespace Geocrest.Web.Mvc.Documentation
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Linq;
    using System.Net.Http.Headers;
    using System.Web.Http;
    using System.Web.Http.Controllers;
    using System.Web.Http.Description;
    using System.Web.Http.Dispatcher;
    using Ninject;
    using Geocrest.Web.Infrastructure;
    using Geocrest.Web.Mvc.Controllers;
    /// <summary>
    /// Configuration extensions for the API help pages
    /// </summary>
    public static class HelpPageConfigurationExtensions
    {
        private const string ApiModelPrefix = "MS_HelpPageApiModel_";

        /// <summary>
        /// Sets the documentation provider for help page.
        /// </summary>
        /// <param name="config">The <see cref="T:System.Web.Http.HttpConfiguration"/>.</param>
        /// <param name="documentationProvider">The documentation provider.</param>
        public static void SetDocumentationProvider(this HttpConfiguration config, IDocumentationProvider documentationProvider)
        {
            config.Services.Replace(typeof(IDocumentationProvider), documentationProvider);
        }
        /// <summary>
        /// Gets the API explorer for a specific version and area help page.
        /// </summary>
        /// <param name="services">The services container.</param>
        /// <returns>
        /// Returns an <see cref="T:Geocrest.Web.Mvc.Documentation.IVersionedApiExplorer" />
        /// or <see langword="null"/> if none has been set.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">services</exception>
        public static IVersionedApiExplorer GetVersionedApiExplorer(this ServicesContainer services)
        {
            Throw.IfArgumentNull(services, "services");
            var explorer = BaseApplication.Kernel.TryGet<IApiExplorer>();
            if (explorer is IVersionedApiExplorer)
                return (IVersionedApiExplorer)explorer;
            else
                return null;
        }
        /// <summary>
        /// Gets a <see cref="T:Geocrest.Web.Mvc.Controllers.IVersionedHttpControllerSelector" /> instance
        /// from Ninject's bound services.
        /// </summary>
        /// <param name="services">The services container.</param>
        /// <returns>
        /// Returns the default <see cref="IVersionedHttpControllerSelector"/> or <see langword="null"/> if none has been set.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">services</exception>
        public static IVersionedHttpControllerSelector GetVersionedHttpControllerSelector(this ServicesContainer services)
        {
            Throw.IfArgumentNull(services, "services");
            var selector = BaseApplication.Kernel.TryGet<IHttpControllerSelector>();
            if (selector is IVersionedHttpControllerSelector)
                return (IVersionedHttpControllerSelector)selector;
            else return null;
        }
        /// <summary>
        /// Gets the API explorer for the help page.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <returns>
        /// Returns the default <see cref="T:System.Web.Http.Description.IApiExplorer" />.
        /// </returns>
        public static IApiExplorer GetDefaultApiExplorer(this ServicesContainer services)
        {
            return BaseApplication.Kernel.TryGet<IApiExplorer>();
        }
        /// <summary>
        /// Sets the objects that will be used by the formatters to produce sample requests/responses.
        /// </summary>
        /// <param name="config">The <see cref="T:System.Web.Http.HttpConfiguration"/>.</param>
        /// <param name="sampleObjects">The sample objects.</param>
        public static void SetSampleObjects(this HttpConfiguration config, IDictionary<Type, object> sampleObjects)
        {
            var generator = config.GetHelpPageSampleGenerator();
            foreach (var kvp in sampleObjects)
            {
                if (!generator.SampleObjects.ContainsKey(kvp.Key))
                    generator.SampleObjects.Add(kvp);
            }
            //config.GetHelpPageSampleGenerator().SampleObjects = sampleObjects;
        }
        //public static T GetSampleRequest<T>(this HttpConfiguration config, MediaTypeHeaderValue mediaType, string controllerName, string actionName, params string[] parameterNames)
        //{
        //    return (T)config.GetHelpPageSampleGenerator().GetActionSample(controllerName, actionName, parameterNames, typeof(T), mediaType, SampleDirection.Request);
        //}
        /// <summary>
        /// Gets the HTML sample for the specified route combination.
        /// </summary>
        /// <param name="config">The <see cref="T:System.Web.Http.HttpConfiguration"/>.</param>
        /// <param name="controllerName">Name of the controller.</param>
        /// <param name="actionName">Name of the action.</param>
        /// <param name="parameterNames">The parameter names.</param>
        /// <returns>
        /// Returns the HTML samples associated with the specified controller and action.
        /// </returns>
        public static IEnumerable<HtmlSample> GetHtmlSamples(this HttpConfiguration config, string controllerName, string actionName, params string[] parameterNames)
        {
            return config.GetHelpPageSampleGenerator().GetHtmlSamples(controllerName, actionName, parameterNames);
        }
        /// <summary>
        /// Sets the sample request directly for the specified media type and action.
        /// </summary>
        /// <param name="config">The <see cref="T:System.Web.Http.HttpConfiguration"/>.</param>
        /// <param name="sample">The sample request.</param>
        /// <param name="mediaType">The media type.</param>
        /// <param name="controllerName">Name of the controller.</param>
        /// <param name="actionName">Name of the action.</param>
        public static void SetSampleRequest(this HttpConfiguration config, object sample, MediaTypeHeaderValue mediaType, string controllerName, string actionName)
        {
            config.GetHelpPageSampleGenerator().ActionSamples.Add(new HelpPageSampleKey(mediaType, SampleDirection.Request, controllerName, actionName, new string[] { }), sample);
        }

        /// <summary>
        /// Sets the sample request directly for the specified media type and action with parameters.
        /// </summary>
        /// <param name="config">The <see cref="T:System.Web.Http.HttpConfiguration"/>.</param>
        /// <param name="sample">The sample request.</param>
        /// <param name="mediaType">The media type.</param>
        /// <param name="controllerName">Name of the controller.</param>
        /// <param name="actionName">Name of the action.</param>
        /// <param name="parameterNames">The parameter names.</param>
        public static void SetSampleRequest(this HttpConfiguration config, object sample, MediaTypeHeaderValue mediaType, string controllerName, string actionName, params string[] parameterNames)
        {
            config.GetHelpPageSampleGenerator().ActionSamples.Add(new HelpPageSampleKey(mediaType, SampleDirection.Request, controllerName, actionName, parameterNames), sample);
        }
        /// <summary>
        /// Sets the HTML sample for a specified route value combination.
        /// </summary>
        /// <param name="config">The <see cref="HttpConfiguration"/>.</param>
        /// <param name="sample">The sample containing the HTML.</param>
        /// <param name="controllerName">Name of the controller.</param>
        /// <param name="actionName">Name of the action.</param>
        /// <param name="parameterNames">The parameter names.</param>
        public static void SetHtmlSample(this HttpConfiguration config, HtmlSample sample, string controllerName, string actionName, params string[] parameterNames)
        {
            config.GetHelpPageSampleGenerator().HtmlSamples.Add(new KeyValuePair<HelpPageSampleKey, HtmlSample>(
                new HelpPageSampleKey(controllerName, actionName, parameterNames), sample));
        }
        /// <summary>
        /// Sets the sample request directly for the specified media type of the action.
        /// </summary>
        /// <param name="config">The <see cref="T:System.Web.Http.HttpConfiguration"/>.</param>
        /// <param name="sample">The sample response.</param>
        /// <param name="mediaType">The media type.</param>
        /// <param name="controllerName">Name of the controller.</param>
        /// <param name="actionName">Name of the action.</param>
        public static void SetSampleResponse(this HttpConfiguration config, object sample, MediaTypeHeaderValue mediaType, string controllerName, string actionName)
        {
            config.GetHelpPageSampleGenerator().ActionSamples.Add(new HelpPageSampleKey(mediaType, SampleDirection.Response, controllerName, actionName, new string[] { }), sample);
        }

        /// <summary>
        /// Sets the sample response directly for the specified media type of the action with specific parameters.
        /// </summary>
        /// <param name="config">The <see cref="T:System.Web.Http.HttpConfiguration"/>.</param>
        /// <param name="sample">The sample response.</param>
        /// <param name="mediaType">The media type.</param>
        /// <param name="controllerName">Name of the controller.</param>
        /// <param name="actionName">Name of the action.</param>
        /// <param name="parameterNames">The parameter names.</param>
        public static void SetSampleResponse(this HttpConfiguration config, object sample, MediaTypeHeaderValue mediaType, string controllerName, string actionName, params string[] parameterNames)
        {
            config.GetHelpPageSampleGenerator().ActionSamples.Add(new HelpPageSampleKey(mediaType, SampleDirection.Response, controllerName, actionName, parameterNames), sample);
        }

        /// <summary>
        /// Sets the sample directly for all actions with the specified type and media type.
        /// </summary>
        /// <param name="config">The <see cref="T:System.Web.Http.HttpConfiguration"/>.</param>
        /// <param name="sample">The sample.</param>
        /// <param name="mediaType">The media type.</param>
        /// <param name="type">The parameter type or return type of an action.</param>
        public static void SetSampleForType(this HttpConfiguration config, object sample, MediaTypeHeaderValue mediaType, Type type)
        {
            config.GetHelpPageSampleGenerator().ActionSamples.Add(new HelpPageSampleKey(mediaType, type), sample);
        }
        /// <summary>
        /// Sets the sample directly for all actions with the specified type and for one or more media types.
        /// </summary>
        /// <param name="config">The <see cref="T:System.Web.Http.HttpConfiguration"/>.</param>
        /// <param name="sample">The sample.</param>
        /// <param name="mediaTypes">The media types.</param>
        /// <param name="type">The parameter type or return type of an action.</param>
        public static void SetSampleForTypes(this HttpConfiguration config, object sample, string[] mediaTypes, Type type)
        {
            var generator = config.GetHelpPageSampleGenerator();
            foreach (var media in mediaTypes)
            {
                generator.ActionSamples.Add(new HelpPageSampleKey(new MediaTypeHeaderValue(media), type), sample);
            }
        }
        /// <summary>
        /// Specifies the actual type of <see cref="T:System.Net.Http.ObjectContent`1"/> passed to the 
        /// <see cref="T:System.Net.Http.HttpRequestMessage"/> in an action. 
        /// The help page will use this information to produce more accurate request samples.
        /// </summary>
        /// <param name="config">The <see cref="T:System.Web.Http.HttpConfiguration"/>.</param>
        /// <param name="type">The type.</param>
        /// <param name="controllerName">Name of the controller.</param>
        /// <param name="actionName">Name of the action.</param>
        public static void SetActualRequestType(this HttpConfiguration config, Type type, string controllerName, string actionName)
        {
            config.GetHelpPageSampleGenerator().ActualHttpMessageTypes.Add(new HelpPageSampleKey(SampleDirection.Request, controllerName, actionName, new string[] { }), type);
        }

        /// <summary>
        /// Specifies the actual type of <see cref="T:System.Net.Http.ObjectContent`1"/> passed to the 
        /// <see cref="T:System.Net.Http.HttpRequestMessage"/> in an action. 
        /// The help page will use this information to produce more accurate request samples.
        /// </summary>
        /// <param name="config">The <see cref="T:System.Web.Http.HttpConfiguration"/>.</param>
        /// <param name="type">The type.</param>
        /// <param name="controllerName">Name of the controller.</param>
        /// <param name="actionName">Name of the action.</param>
        /// <param name="parameterNames">The parameter names.</param>
        public static void SetActualRequestType(this HttpConfiguration config, Type type, string controllerName, string actionName, params string[] parameterNames)
        {
            config.GetHelpPageSampleGenerator().ActualHttpMessageTypes.Add(new HelpPageSampleKey(SampleDirection.Request, controllerName, actionName, parameterNames), type);
        }

        /// <summary>
        /// Specifies the actual type of <see cref="T:System.Net.Http.ObjectContent`1"/> returned as part of 
        /// the <see cref="T:System.Net.Http.HttpRequestMessage"/> in an action. 
        /// The help page will use this information to produce more accurate response samples.
        /// </summary>
        /// <param name="config">The <see cref="T:System.Web.Http.HttpConfiguration"/>.</param>
        /// <param name="type">The type.</param>
        /// <param name="controllerName">Name of the controller.</param>
        /// <param name="actionName">Name of the action.</param>
        public static void SetActualResponseType(this HttpConfiguration config, Type type, string controllerName, string actionName)
        {
            config.GetHelpPageSampleGenerator().ActualHttpMessageTypes.Add(new HelpPageSampleKey(SampleDirection.Response, controllerName, actionName, new string[] { }), type);
        }

        /// <summary>
        /// Specifies the actual type of <see cref="T:System.Net.Http.ObjectContent`1"/> returned as part of 
        /// the <see cref="T:System.Net.Http.HttpRequestMessage"/> in an action. 
        /// The help page will use this information to produce more accurate response samples.
        /// </summary>
        /// <param name="config">The <see cref="T:System.Web.Http.HttpConfiguration"/>.</param>
        /// <param name="type">The type.</param>
        /// <param name="controllerName">Name of the controller.</param>
        /// <param name="actionName">Name of the action.</param>
        /// <param name="parameterNames">The parameter names.</param>
        public static void SetActualResponseType(this HttpConfiguration config, Type type, string controllerName, string actionName, params string[] parameterNames)
        {
            config.GetHelpPageSampleGenerator().ActualHttpMessageTypes.Add(new HelpPageSampleKey(SampleDirection.Response, controllerName, actionName, parameterNames), type);
        }

        /// <summary>
        /// Gets the help page sample generator.
        /// </summary>
        /// <param name="config">The <see cref="T:System.Web.Http.HttpConfiguration"/>.</param>
        /// <returns>The help page sample generator.</returns>
        public static HelpPageSampleGenerator GetHelpPageSampleGenerator(this HttpConfiguration config)
        {
            return (HelpPageSampleGenerator)config.Properties.GetOrAdd(
                typeof(HelpPageSampleGenerator),
                k => new HelpPageSampleGenerator());
        }

        /// <summary>
        /// Sets the help page sample generator.
        /// </summary>
        /// <param name="config">The <see cref="T:System.Web.Http.HttpConfiguration"/>.</param>
        /// <param name="sampleGenerator">The help page sample generator.</param>
        public static void SetHelpPageSampleGenerator(this HttpConfiguration config, HelpPageSampleGenerator sampleGenerator)
        {
            config.Properties.AddOrUpdate(
                typeof(HelpPageSampleGenerator),
                k => sampleGenerator,
                (k, o) => sampleGenerator);
        }

        /// <summary>
        /// Gets the model that represents an API displayed on the help page. The model is initialized on the first call and cached for subsequent calls.
        /// </summary>
        /// <param name="config">The <see cref="T:System.Web.Http.HttpConfiguration" />.</param>
        /// <param name="explorer">The API explorer containing all known controllers.</param>
        /// <param name="apiDescriptionId">The API description ID as specified in
        /// <see cref="M:Geocrest.Web.Mvc.Documentation.ApiDescriptionExtensions.GetFriendlyId(System.Web.Http.Description.ApiDescription,System.String)"/>.</param>
        /// <param name="homePage">The home page url.</param>
        /// <param name="version">The version number to display.</param>
        /// <returns>
        /// An <see cref="HelpPageApiModel" />
        /// </returns>
        public static HelpPageApiModel GetHelpPageApiModel(this HttpConfiguration config,
            IApiExplorer explorer, string apiDescriptionId, string homePage = "", string version = "")
        {
            object model;
            apiDescriptionId += !string.IsNullOrEmpty(version) ? "-" + version : "";
            string modelId = ApiModelPrefix + apiDescriptionId;
            if (!config.Properties.TryGetValue(modelId, out model))
            {
                Collection<System.Web.Http.Description.ApiDescription> apiDescriptions = explorer.ApiDescriptions;
                System.Web.Http.Description.ApiDescription apiDescription =
                    apiDescriptions.FirstOrDefault(api => String.Equals(api.GetFriendlyId(version),
                        apiDescriptionId, StringComparison.OrdinalIgnoreCase));
                if (apiDescription != null)
                {
                    HelpPageSampleGenerator sampleGenerator = config.GetHelpPageSampleGenerator();
                    model = GenerateApiModel(apiDescription, sampleGenerator, config, homePage);
                    config.Properties.TryAdd(modelId, model);
                    if (string.IsNullOrEmpty(((HelpPageApiModel)model).HomePageUrl) && !string.IsNullOrEmpty(homePage))
                        ((HelpPageApiModel)model).HomePageUrl = homePage;
                }
            }
            return (HelpPageApiModel)model;
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "The exception is recorded as ErrorMessages.")]
        private static HelpPageApiModel GenerateApiModel(System.Web.Http.Description.ApiDescription apiDescription,
            HelpPageSampleGenerator sampleGenerator, HttpConfiguration configuration, string homePage)
        {
            HelpPageApiModel apiModel = new HelpPageApiModel(homePage);
            apiModel.ApiDescription = apiDescription;
            if (apiModel.ApiDescription.RelativePath.EndsWith("/"))
                apiModel.ApiDescription.RelativePath = apiModel.ApiDescription.RelativePath.Substring(0,
                apiModel.ApiDescription.RelativePath.Length - 1);
            try
            {
                IResponseDocumentationProvider responseDocProvider = (IResponseDocumentationProvider)configuration.Services.GetDocumentationProvider();
                if (responseDocProvider != null)
                    apiModel.ResponseDocumentation = responseDocProvider.GetResponseDocumentation(apiDescription.ActionDescriptor);

                foreach (var item in sampleGenerator.GetSampleRequests(apiDescription))
                {
                    apiModel.SampleRequests.Add(item.Key, item.Value);
                    LogInvalidSampleAsError(apiModel, item.Value);
                }

                foreach (var item in sampleGenerator.GetSampleResponses(apiDescription))
                {
                    apiModel.SampleResponses.Add(item.Key, item.Value);
                    LogInvalidSampleAsError(apiModel, item.Value);
                }
                foreach (var item in sampleGenerator.GetHtmlSamples(apiDescription))
                {
                    apiModel.HtmlSamples.AddIfNotNull<HtmlSample>(item);
                    LogInvalidSampleAsError(apiModel, item);
                }
            }
            catch (Exception e)
            {
                apiModel.ErrorMessages.Add(String.Format(CultureInfo.CurrentCulture, "An exception has occurred while generating the sample. Exception Message: {0}", e.Message));
            }

            return apiModel;
        }

        private static void LogInvalidSampleAsError(HelpPageApiModel apiModel, object sample)
        {
            InvalidSample invalidSample = sample as InvalidSample;
            if (invalidSample != null)
            {
                apiModel.ErrorMessages.Add(invalidSample.ErrorMessage);
            }
        }
    }
}
