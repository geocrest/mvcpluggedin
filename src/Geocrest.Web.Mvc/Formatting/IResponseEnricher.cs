namespace Geocrest.Web.Mvc.Formatting
{
    using System;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Web.Http.Routing;

    /// <summary>
    /// Provides a way to enrich the outgoing response additional information
    /// </summary>
    public interface IResponseEnricher
    {
        /// <summary>
        /// Determines whether this instance can enrich the specified response.
        /// </summary>
        /// <param name="response">The response.</param>
        /// <returns>
        /// <b>true</b>, if this instance can enrich the specified response; otherwise, <b>false</b>.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">response</exception>
        bool CanEnrich(HttpResponseMessage response);

        /// <summary>
        /// Determines whether this instance can enrich the specified type based on the accept header.
        /// </summary>
        /// <param name="type">The type to enrich.</param>
        /// <param name="urlHelper">The URL helper.</param>
        /// <param name="acceptHeader">The accept header.</param>
        /// <returns>
        ///   <b>true</b>, if this instance can enrich the specified type; otherwise, <b>false</b>.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">type or acceptHeader</exception>
        bool CanEnrich(Type type, UrlHelper urlHelper, MediaTypeHeaderValue acceptHeader);

        /// <summary>
        /// Enriches the specified response with additional information.
        /// </summary>
        /// <param name="response">The outgoing HTTP response.</param>
        /// <returns>The object content contained within the response message but enriched with additional
        /// information.</returns>
        /// <exception cref="T:System.ArgumentNullException">response</exception>
        HttpResponseMessage Enrich(HttpResponseMessage response);

        /// <summary>
        /// Enriches the object using the specified URL helper.
        /// </summary>
        /// <param name="baseUrl">The base URL for the entity.</param>
        /// <param name="urlHelper">The URL helper.</param>
        /// <param name="value">The value.</param>
        /// <returns>The input object enriched with additional information.</returns>
        /// <exception cref="System.ArgumentNullException">urlHelper or value</exception>
        object Enrich(string baseUrl, UrlHelper urlHelper, object value);
        /// <summary>
        /// Gets the name of the route used to construct URLs. This property is required.
        /// </summary>
        /// <value>
        /// The name of the route.
        /// </value>
        string RouteName { get; }
        /// <summary>
        /// Gets the name of the area to use during URL construction. This property is optional.
        /// </summary>
        /// <value>
        /// The area name.
        /// </value>
        string Area { get; }
    }
}
