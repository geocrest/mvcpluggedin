namespace Geocrest.Web.Mvc.Documentation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    /// <summary>
    /// Provides extension methods for the API documentation
    /// </summary>
    public static class ApiDescriptionExtensions
    {
        /// <summary>
        /// Generates an URI-friendly ID for the <see cref="T:System.Web.Http.Description.ApiDescription" />. E.g. "Get-Values-id" instead of "GetValues/{id}?name={name}"
        /// </summary>
        /// <param name="description">The <see cref="T:System.Web.Http.Description.ApiDescription" />.</param>
        /// <param name="version">The version of the API controller, if any.</param>
        /// <returns>
        /// Returns a URL-friendly id of the API.
        /// </returns>
        /// <remarks>The current version removes all optional parameters.</remarks>
        public static string GetFriendlyId(this System.Web.Http.Description.ApiDescription description, string version = "")
        {
            string path = description.RelativePath;
            string[] urlParts = path.Split('?');
            string localPath = urlParts[0].EndsWith("/") ? 
                urlParts[0].Substring(0,urlParts[0].Length -1).ToLower() :
                urlParts[0].ToLower();
            string queryKeyString = null;

            StringBuilder friendlyPath = new StringBuilder();
            friendlyPath.AppendFormat("{0}-{1}",
                description.HttpMethod.Method,
                localPath.Replace("/", "-").Replace("{", String.Empty).Replace("}", String.Empty));
            if (queryKeyString != null)
            {
                friendlyPath.AppendFormat("_{0}", queryKeyString);
            }
            if (!string.IsNullOrEmpty(version))
                friendlyPath.AppendFormat("-{0}", version);
            return friendlyPath.ToString();
        }
        /// <summary>
        /// Generates an URI-friendly ID for the <see cref="T:Geocrest.Web.Mvc.Documentation.ApiDescription" />. E.g. "Get-Values-id" instead of "GetValues/{id}?name={name}"
        /// </summary>
        /// <param name="description">The <see cref="T:Geocrest.Web.Mvc.Documentation.ApiDescription"/>.</param>
        /// <param name="version">The version of the API controller, if any.</param>
        /// <returns>
        /// Returns a URL-friendly id of the API.
        /// </returns>
        /// <remarks>The current version removes all optional parameters.</remarks>
        public static string GetFriendlyId(this ApiDescription description, string version = "")
        {
            string path = description.RelativePath;
            string[] urlParts = path.Split('?');
            string localPath = urlParts[0].EndsWith("/") ?
                urlParts[0].Substring(0, urlParts[0].Length - 1).ToLower() :
                urlParts[0].ToLower();
            string queryKeyString = null;           

            StringBuilder friendlyPath = new StringBuilder();
            friendlyPath.AppendFormat("{0}-{1}",
                description.HttpMethod,
                localPath.Replace("/", "-").Replace("{", String.Empty).Replace("}", String.Empty));
            if (queryKeyString != null)
            {
                friendlyPath.AppendFormat("_{0}", queryKeyString);
            }
            if (!string.IsNullOrEmpty(version))
                friendlyPath.AppendFormat("-{0}", version);
            return friendlyPath.ToString();
        }
        /// <summary>
        /// Converts each <see cref="T:System.Web.Http.Description.ApiDescription"/> to an 
        /// <see cref="T:Geocrest.Web.Mvc.Documentation.ApiDescription"/> that allows for serialization.
        /// </summary>
        /// <param name="descriptions">The collection of API descriptions to serialize.</param>
        /// <returns>
        /// Returns a collection of ApiDescriptions that can be serialized to the response.
        /// </returns>
        public static IEnumerable<ApiDescription> AsSerializable(this IEnumerable<System.Web.Http.Description.ApiDescription> descriptions)
        {
            if (descriptions == null) yield return null;
            foreach (System.Web.Http.Description.ApiDescription api in descriptions)
            {
                yield return new ApiDescription()
                {
                    RelativePath = api.RelativePath,
                    HttpMethod = api.HttpMethod.Method,
                    Documentation = api.Documentation,
                    ReturnType = api.ActionDescriptor.ReturnType != null ? api.ActionDescriptor.ReturnType.FullName : "none",
                    ParameterDescriptions = api.ParameterDescriptions.Select(x =>
                        new ApiParameterDescription()
                        {
                            Documentation = x.Documentation,
                            Name = x.Name,
                            Type = x.ParameterDescriptor.ParameterType.FullName,
                            Source = x.Source.ToString()
                        }).ToArray()
                };
            }
        }
    }
}