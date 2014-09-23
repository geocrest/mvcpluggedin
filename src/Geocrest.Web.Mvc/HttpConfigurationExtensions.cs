namespace Geocrest.Web.Mvc
{
    using System;
    using System.Linq;
    using System.Collections.ObjectModel;
    using System.Net.Http;
    using System.Web;
    using System.Web.Http;
    using System.Web.Http.Hosting;
    using Geocrest.Web.Infrastructure;
    using Geocrest.Web.Mvc.Formatting;
    /// <summary>
    /// Provides extension methods on <see cref="T:System.Web.Http.HttpConfiguration"/> objects.
    /// </summary>
    public static class HttpConfigurationExtensions
    {
        /// <summary>
        /// Adds the specified response enrichers to the configuration.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="enrichers">The enrichers.</param>
        /// <exception cref="T:System.ArgumentNullException">configuration</exception>
        public static void AddResponseEnrichers(this HttpConfiguration configuration, params IResponseEnricher[] enrichers)
        {
            Throw.IfArgumentNull(configuration, "configuration");
            var collection = configuration.GetResponseEnrichers();
            foreach (var enricher in enrichers.Distinct())
            {
                if (!collection.Contains(enricher)) 
                    configuration.GetResponseEnrichers().Add(enricher);
            }
        }

        /// <summary>
        /// Gets the response enrichers from the configuration object.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <returns>
        /// Returns an instance of <see cref="T:System.Collections.ObjectModel.Collection`1"/>.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">configuration</exception>
        public static Collection<IResponseEnricher> GetResponseEnrichers(this HttpConfiguration configuration)
        {
            Throw.IfArgumentNull(configuration, "configuration");
            return (Collection<IResponseEnricher>)configuration.Properties.GetOrAdd(
                    typeof(Collection<IResponseEnricher>),
                    k => new Collection<IResponseEnricher>()
                );
        }
        /// <summary>
        /// Gets a value that indicates if error details should be included given a specific request message.
        /// </summary>
        /// <param name="configuration">The configuration object.</param>
        /// <param name="request">The request message.</param>
        /// <returns>
        ///   <c>true</c>, if the application should include error details in the response; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">configuration</exception>
        public static bool ShouldIncludeErrorDetail(this HttpConfiguration configuration, HttpRequestMessage request)
        {
            Throw.IfArgumentNull(configuration, "configuration");
            switch (configuration.IncludeErrorDetailPolicy)
            {
                case IncludeErrorDetailPolicy.Default:
                    object includeErrorDetail;
                    if (request.Properties.TryGetValue(HttpPropertyKeys.IncludeErrorDetailKey, out includeErrorDetail))
                    {
                        // If we are on webhost and the user hasn't changed the IncludeErrorDetailPolicy
                        // look up into the ASP.NET CustomErrors property else default to LocalOnly.
                        return ((Lazy<bool>)includeErrorDetail).Value;
                    }

                    goto case IncludeErrorDetailPolicy.LocalOnly;

                case IncludeErrorDetailPolicy.LocalOnly:
                    if (request == null)
                    {
                        return false;
                    }

                    object isLocal;
                    if (request.Properties.TryGetValue(HttpPropertyKeys.IsLocalKey, out isLocal))
                    {
                        return ((Lazy<bool>)isLocal).Value;
                    }
                    return false;

                case IncludeErrorDetailPolicy.Always:
                    return true;

                case IncludeErrorDetailPolicy.Never:
                default:
                    return false;
            }
        }
        /// <summary>
        /// Gets a value that indicates if error details should be included given a specific request.
        /// </summary>
        /// <param name="configuration">The configuration object.</param>
        /// <param name="request">The request sent to the server.</param>
        /// <returns>
        ///   <c>true</c>, if the application should include error details in the response; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="request"/> is null, or
        /// <paramref name="configuration"/> is null</exception>
        public static bool ShouldIncludeErrorDetail(this HttpConfiguration configuration, HttpRequest request)
        {
            Throw.IfArgumentNull(configuration, "configuration");
            Throw.IfArgumentNull(request, "request");
            HttpMethod method = new HttpMethod(request.HttpMethod);
            var requestMessage = new HttpRequestMessage(method, request.Url);
            requestMessage.Properties["MS_HttpConfiguration"] = configuration;
            requestMessage.Properties.Add(HttpPropertyKeys.IncludeErrorDetailKey, 
                new Lazy<bool>(() => !request.RequestContext.HttpContext.IsCustomErrorEnabled));

            return configuration.ShouldIncludeErrorDetail(requestMessage);
        }
    }
}
