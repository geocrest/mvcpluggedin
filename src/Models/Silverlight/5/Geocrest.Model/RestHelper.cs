namespace Geocrest.Model
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using Geocrest.Model.ArcGIS;
    /// <summary>
    /// Provides deserialization helper methods for working with RESTful web services.
    /// </summary>
    public class RestHelper : IRestHelper
    {
        /// <summary>
        /// Allows hydration of an object from a json string representation
        /// </summary>
        /// <typeparam name="T">The type of object to hydrate.</typeparam>
        /// <param name="json">The string representation of json used to deserialize.</param>
        /// <returns>
        /// The hydrated object.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">json parameter is required.</exception>
        public T HydrateFromJson<T>(string json)
        {
            if (!string.IsNullOrEmpty(json))
            {
                return JsonConvert.DeserializeObject<T>(json);
            }
            else
            {
                throw new ArgumentNullException("json parameter is required.");
            }
        }
        /// <summary>
        /// Helper method to hydrate an object from a json string representation
        /// and without requiring a <see cref="T:Geocrest.Model.RestHelper"/>.
        /// </summary>
        /// <typeparam name="T">The type of object to hydrate.</typeparam>
        /// <param name="json">The string representation of json used to deserialize.</param>
        /// <returns>The hydrated object.</returns>
        public static T HydrateObjectFromJson<T>(string json)
        {
            RestHelper helper = new RestHelper();
            return helper.HydrateFromJson<T>(json);
        }
        /// <summary>
        /// Helper method to hydrate an object using the string response from the specified URL resource
        /// and without requiring a <see cref="T:Geocrest.Model.RestHelper" />.
        /// </summary>
        /// <typeparam name="T">The type of object to hydrate.</typeparam>
        /// <param name="url">The web service URL.</param>
        /// <param name="callback">A callback function that receives the result.</param>
        /// <returns>
        /// The hydrated object.
        /// </returns>
        public static void HydrateObject<T>(string url, Action<T> callback)
        {
            RestHelper helper = new RestHelper();
            helper.HydrateAsync<T>(ForceJsonFormat(url), callback);
        }
        /// <summary>
        /// Helper method to hydrate an object using the string response from the specified URL resource
        /// and without requiring a <see cref="T:Geocrest.Model.RestHelper" />.
        /// </summary>
        /// <typeparam name="T">The type of object to hydrate.</typeparam>
        /// <param name="url">The web service URL.</param>
        /// <param name="callback">A callback function that receives the result.</param>
        /// <param name="userState">A user-defined object that is passed to the method invoked when the asynchronous operation completes.</param>
        public static void HydrateObject<T>(string url, Action<T, object> callback, object userState)
        {
            RestHelper helper = new RestHelper();
            helper.HydrateAsync<T>(ForceJsonFormat(url), callback, userState);
        }
        /// <summary>
        /// Downloads a resource string resource asynchronously.
        /// </summary>
        /// <param name="uri">The <see cref="T:System.Uri" /> of the resource to download.</param>
        /// <param name="callback">An action that will be invoked when the string has been downloaded.</param>
        /// <param name="userState">A user-defined object that is passed to the method invoked when the asynchronous operation completes.</param>
        public virtual void DownloadStringAsync(Uri uri, Action<string, object> callback, object userState)
        {
            WebClient wc = GetWebClient();
            wc.UseDefaultCredentials = true;
            wc.DownloadStringCompleted += (o, e) =>
            {
                if (e.Error != null)
                {
                    callback(null, e.UserState);
                    return;
                }
                callback(e.Result, e.UserState);
            };
            wc.DownloadStringAsync(uri, userState);
        }
        /// <summary>
        /// Creates a new web client.
        /// </summary>
        /// <returns>
        /// Returns an instance of <see cref="WebClient"/>.
        /// </returns>
        protected internal virtual WebClient GetWebClient()
        {
            return new WebClient();
        }
        /// <summary>
        /// Hydrates an object using the string response from the specified URL resource.
        /// </summary>
        /// <typeparam name="T">The type of object to hydrate.</typeparam>
        /// <param name="url">The web service URL.</param>
        /// <param name="callback">A callback function that receives the result.</param>
        public void HydrateAsync<T>(string url, Action<T> callback)
        {
            HydrateAsync<T>(ForceJsonFormat(url), delegate(T cb, object token)
            {
                callback(cb);
            }, null);
        }
        /// <summary>
        /// Hydrates an object using the string response from the specified URL resource.
        /// </summary>
        /// <typeparam name="T">The type of object to hydrate.</typeparam>
        /// <param name="url">The web service URL.</param>
        /// <param name="callback">A callback function that receives the result.</param>
        /// <param name="userState">A user-defined object that is passed to the method invoked when the asynchronous operation completes.</param>
        /// <exception cref="System.InvalidOperationException">If the web service call to the resource fails.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">url</exception>
        public void HydrateAsync<T>(string url, Action<T, object> callback, object userState)
        {
            if (!string.IsNullOrEmpty(url))
            {
                string json = string.Empty;
                Uri uri = null;
                try
                {
                    uri = new Uri(ForceJsonFormat(url), UriKind.RelativeOrAbsolute);
                    DownloadStringAsync(uri, delegate(string cb, object token)
                    {
                        json = cb;
                        if (string.IsNullOrEmpty(json))
                        {
                            callback(default(T), token);
                            return;
                            //throw new InvalidProgramException(
                            //    string.Format("The underlying web service response from {0} was null.", url));
                        }
                        else if (url.ToLower().Contains("arcgis/rest/services"))
                        {
                            ESRIException error = JsonConvert.DeserializeObject<ESRIException>(json);
                            if (error.Error != null)
                            {
                                throw new InvalidOperationException(
                                    string.Format("The underlying ArcGIS service located at " +
                                    "{0} returned the following error: {1}: {2}",
                                    url, error.Error.Message, string.Join("; ", error.Error.Details)));
                            }
                        }
                        callback((T)JsonConvert.DeserializeObject<T>(json, new StringEnumConverter()), token);
                    }, userState);
                }
                catch (WebException ex)
                {
                    switch ((ex.Response as HttpWebResponse).StatusCode)
                    {
                        case HttpStatusCode.NotFound:
                            throw new InvalidOperationException(
                                string.Format("The underlying web service located at {0} could not be found.",
                                url));
                        case HttpStatusCode.BadGateway:
                            throw new InvalidOperationException(
                                string.Format("The underlying web service domain could not be resolved: {0}.",
                                uri.Host));
                    }
                }
            }
            else
            {
                throw new ArgumentNullException("url", "Url parameter is required!");
            }
        }
        /// <summary>
        /// Forces the json format using the query string 'f' parameter.
        /// </summary>
        /// <param name="url">The URL to check.</param>
        internal static string ForceJsonFormat(string url)
        {
            if (string.IsNullOrEmpty(url)) throw new ArgumentNullException("url");
            string json = "json";
            string f = "f";
            string query = "?";
            string root = string.Empty;

            // get root url using string manipulation instead of building a Uri so items like
            // port number and a trailing slash are not added. Want to return url as it was entered
            root = url.Contains("?") ? url.Replace(url.Substring(url.IndexOf("?")), "") : url;

            // set the 'f' parameter of query string
            Uri uri = new Uri(url);
            IDictionary<string, string> queryparams = ParseQueryString(uri.Query);
            if (queryparams.Keys.Contains(f))
            {
                queryparams[f] = json;
            }
            else
            {
                queryparams.Add(f, json);
            }
            foreach (var kvp in queryparams)
                query += kvp.Key + "=" + kvp.Value + "&";

            // build the new Uri            
            return root + query.Substring(0, query.Length - 1);
        }
        /// <summary>
        /// Parses the query string into a key/value dictionary.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns>
        /// Returns an instance of <see cref="T:System.Collections.Generic.IDictionary`2"/>.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">url</exception>
        private static IDictionary<string, string> ParseQueryString(string query)
        {
            IDictionary<string, string> values = new Dictionary<string, string>();
            if (string.IsNullOrEmpty(query) || !query.Contains("=")) return values;

            if (query.StartsWith("?"))
            {
                query = query.Substring(1);
            }
            var args = query.Split(new[] { "&" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var arg in args)
            {
                var pairs = arg.Split(new[] { "=" }, StringSplitOptions.None);
                if (pairs.Length == 2)
                {
                    values.Add(pairs[0], pairs[1] == null ? string.Empty : pairs[1]);
                }
            }
            return values;
        }
    }
}
