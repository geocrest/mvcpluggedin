namespace Geocrest.Model
{
    using System;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Net;
    using System.Web;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using Geocrest.Model.ArcGIS;
    /// <summary>
    /// Provides deserialization helper methods for working with RESTful web services.
    /// </summary>    
    public class RestHelper : IRestHelper
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Model.RestHelper"/> class.
        /// </summary>
        public RestHelper()
        {
            this.WebHelper = new WrappedWebClient();
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Model.RestHelper" /> class.
        /// </summary>
        /// <param name="webHelper">The web helper used to makes web requests.</param>
        public RestHelper(IWebHelper webHelper)
        {
            if (webHelper == null) throw new ArgumentNullException("webHelper");
            this.WebHelper = webHelper;
        }        

        #region IRestHelper Members
        /// <summary>
        /// Gets the web helper used for retrieving web resources.
        /// </summary>
        public virtual IWebHelper WebHelper { get; private set; }
        /// <summary>
        /// Occurs when the hydrate operation has completed.
        /// </summary>
        public event EventHandler<HydrateCompletedEventArgs> HydrateCompleted;

        /// <summary>
        /// Hydrates an object using the string response from the specified URL resource.
        /// </summary>
        /// <typeparam name="T">The type of object to hydrate.</typeparam>
        /// <param name="url">The web service URL.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="url" /> is null or empty</exception>
        public void HydrateAsync<T>(string url)
        {
            this.HydrateAsync<T>(url, (object)null);
        }
        /// <summary>
        /// Hydrates an object using the string response from the specified URL resource.
        /// </summary>
        /// <typeparam name="T">The type of object to hydrate.</typeparam>
        /// <param name="url">The web service URL.</param>
        /// <param name="userState">A user-defined object that is passed to the method invoked when the asynchronous
        /// operation completes.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="url" /> is null or empty</exception>
        public void HydrateAsync<T>(string url, object userState)
        {
            if (string.IsNullOrEmpty(url)) throw new ArgumentNullException("url", "Url parameter is required!");
            Uri uri = null;
            try
            {
                uri = new Uri(RestHelper.ForceJsonFormat(url));
                this.WebHelper.DownloadStringCompleted += WebHelper_DownloadStringCompleted<T>;
                this.WebHelper.DownloadStringAsync(uri, userState);
            }
            catch (Exception ex)
            {
                OnHydrateCompleted<T>(new HydrateCompletedEventArgs(typeof(T),default(T), ex, false,
                    null, uri, ex.Message));
            }
        }
        /// <summary>
        /// Hydrates an object using the string response from the specified URL resource.
        /// This method allows data to be uploaded to the server.
        /// </summary>
        /// <typeparam name="T">The type of object to hydrate.</typeparam>
        /// <param name="url">The web service URL.</param>
        /// <param name="data">A string representing data to upload to the server.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="url"/> is null or empty</exception>
        public void HydrateAsync<T>(string url, string data)
        {
            this.HydrateAsync<T>(url, data, "POST");
        }
        
        /// <summary>
        /// Hydrates an object using the string response from the specified URL resource.
        /// This method allows data to be uploaded to the server.
        /// </summary>
        /// <typeparam name="T">The type of object to hydrate.</typeparam>
        /// <param name="url">The web service URL.</param>
        /// <param name="data">A string representing data to upload to the server.</param>
        /// <param name="method">The HTTP method to use when uploading.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="url"/> is null or empty</exception>
        public void HydrateAsync<T>(string url, string data, string method)
        {
            this.HydrateAsync<T>(url, data, method, null);
        }
        /// <summary>
        /// Hydrates an object using the string response from the specified URL resource.
        /// This method allows data to be uploaded to the server.
        /// </summary>
        /// <typeparam name="T">The type of object to hydrate.</typeparam>
        /// <param name="url">The web service URL.</param>
        /// <param name="data">A string representing data to upload to the server.</param>
        /// <param name="method">The HTTP method to use when uploading.</param>
        /// <param name="userState">A user-defined object that is passed to the method invoked when the asynchronous
        /// operation completes.</param>
        public void HydrateAsync<T>(string url, string data, string method, object userState)
        {
            if (string.IsNullOrEmpty(url)) throw new ArgumentNullException("url", "Url parameter is required!");
            Uri uri = null;
            try
            {
                uri = new Uri(RestHelper.ForceJsonFormat(url));
                this.WebHelper.UploadStringCompleted += WebHelper_UploadStringCompleted<T>;
                this.WebHelper.UploadStringAsync(uri, method, data, userState);
            }
            catch (Exception ex)
            {
                OnHydrateCompleted<T>(new HydrateCompletedEventArgs(typeof(T),default(T), ex, false,
                    userState,uri,ex.Message));
            }
        }
        /// <summary>
        /// Hydrates an object using the string response from the specified URL resource.
        /// This method allows data to be uploaded to the server.
        /// </summary>
        /// <typeparam name="T">The type of object to hydrate.</typeparam>
        /// <param name="url">The web service URL.</param>
        /// <param name="data">The <see cref="T:System.Collections.Specialized.NameValueCollection" />
        /// to send to the resource.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="url"/> is null
        /// or
        /// <paramref name="data"/> is null</exception>
        public void HydrateAsync<T>(string url, NameValueCollection data)
        {
            this.HydrateAsync<T>(url, data, "POST");
        }

        /// <summary>
        /// Hydrates an object using the string response from the specified URL resource.
        /// This method allows data to be uploaded to the server.
        /// </summary>
        /// <typeparam name="T">The type of object to hydrate.</typeparam>
        /// <param name="url">The web service URL.</param>
        /// <param name="data">The <see cref="T:System.Collections.Specialized.NameValueCollection" />
        /// to send to the resource.</param>
        /// <param name="method">The HTTP method to use when uploading.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="url"/> is null
        /// or
        /// <paramref name="data"/> is null</exception>
        public void HydrateAsync<T>(string url, NameValueCollection data, string method)
        {
            this.HydrateAsync<T>(url, data, method, null);
        }

        /// <summary>
        /// Hydrates an object using the string response from the specified URL resource.
        /// This method allows data to be uploaded to the server.
        /// </summary>
        /// <typeparam name="T">The type of object to hydrate.</typeparam>
        /// <param name="url">The web service URL.</param>
        /// <param name="data">The <see cref="T:System.Collections.Specialized.NameValueCollection" />
        /// to send to the resource.</param>
        /// <param name="method">The HTTP method to use when uploading.</param>
        /// <param name="userState">A user-defined object that is passed to the method invoked when the asynchronous
        /// operation completes.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="url"/> is null
        /// or
        /// <paramref name="data"/> is null</exception>
        public void HydrateAsync<T>(string url, NameValueCollection data, string method, object userState)
        {
            if (string.IsNullOrEmpty(url)) throw new ArgumentNullException("url", "Url parameter is required!");
            if (data == null) throw new ArgumentNullException("data");
            Uri uri = null;
            try
            {
                uri = new Uri(url);
                if (!data.AllKeys.Contains("f")) data.Add("f", "json");
                this.WebHelper.UploadValuesCompleted += WebHelper_UploadValuesCompleted<T>;
                this.WebHelper.UploadValuesAsync(uri, method,data, userState);
            }
            catch (Exception ex)
            {
                OnHydrateCompleted<T>(new HydrateCompletedEventArgs(typeof(T),default(T), ex, false,
                    userState,uri, ex.Message));
            }
        }
        /// <summary>
        /// Allows hydration of an object from a json string representation
        /// </summary>
        /// <typeparam name="T">The type of object to hydrate.</typeparam>
        /// <param name="json">The string representation of json used to deserialize.</param>
        /// <returns>
        /// The hydrated object.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">json parameter is required.</exception>
        public virtual T HydrateFromJson<T>(string json)
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
        /// Allows hydration of an object from a json string representation
        /// </summary>
        /// <param name="json">The json to deserialize.</param>
        /// <param name="type">The type of object being deserialized.</param>
        /// <returns>
        /// The hydrated object.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">json parameter is required.</exception>
        public object HydrateFromJson(string json, Type type)
        {
            if (!string.IsNullOrEmpty(json))
            {
                return JsonConvert.DeserializeObject(json, type);
            }
            else
            {
                throw new ArgumentNullException("json parameter is required.");
            }
        }
        /// <summary>
        /// Hydrates an object using the string response from the specified URL resource.
        /// </summary>
        /// <typeparam name="T">The type of object to hydrate.</typeparam>
        /// <param name="url">The web service URL.</param>
        /// <returns>
        /// The hydrated object.
        /// </returns>
        /// <exception cref="T:System.Web.HttpException">If the web service call to the resource fails,
        /// the exception will contain the status code and reason.</exception>
        /// <exception cref="T:System.ArgumentNullException">url</exception>
        public virtual T Hydrate<T>(string url)
        {
            if (!string.IsNullOrEmpty(url))
            {
                string json = string.Empty;
                Uri uri = null;
                try
                {
                    uri = new Uri(ForceJsonFormat(url));
                    json = this.WebHelper.DownloadString(uri);
                    if (url.ToLower().Contains("arcgis/rest/services"))
                    {
                        ESRIException error = JsonConvert.DeserializeObject<ESRIException>(json);
                        if (error.Error != null)
                        {
                            throw new HttpException((int)error.Error.Code,
                                string.Format("The underlying ArcGIS service located at " +
                            "{0} returned the following error: {1}: {2}",
                            url, error.Error.Message, string.Join("; ", error.Error.Details)));
                        }
                    }
                }
                catch (WebException ex)
                {
                    if (ex.Response is HttpWebResponse)
                    {
                        switch ((ex.Response as HttpWebResponse).StatusCode)
                        {
                            case HttpStatusCode.NotFound:
                                throw new HttpException((int)HttpStatusCode.NotFound,
                                    string.Format("The underlying web service located at {0} could not be found.",
                                    uri.GetLeftPart(UriPartial.Path)));
                            case HttpStatusCode.BadGateway:
                                throw new HttpException((int)HttpStatusCode.BadGateway,
                                    string.Format("The underlying web service domain could not be resolved: {0}.",
                                    uri.Host));
                        }
                    }
                    else throw;
                }
                if (string.IsNullOrEmpty(json))
                {
                    throw new HttpException((int)HttpStatusCode.NoContent,
                        string.Format("The underlying web service response from {0} was null.", url));
                }
                return JsonConvert.DeserializeObject<T>(json, new StringEnumConverter());
            }
            else
            {
                throw new ArgumentNullException("url", "Url parameter is required!");
            }
        }
        #endregion

        #region Static
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
        /// and without requiring a <see cref="T:Geocrest.Model.RestHelper"/>.
        /// </summary>
        /// <typeparam name="T">The type of object to hydrate.</typeparam>
        /// <param name="url">The web service URL.</param>
        /// <returns>The hydrated object.</returns>
        public static T HydrateObject<T>(string url)
        {
            RestHelper helper = new RestHelper();
            return helper.Hydrate<T>(ForceJsonFormat(url));
        }
        /// <summary>
        /// Forces the json format using the query string 'f' parameter.
        /// </summary>
        /// <param name="url">The URL to check.</param>
        /// <returns>
        /// The input URL but with a query parameter of 'f=json'.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">url</exception>
        internal static string ForceJsonFormat(string url)
        {
            if (string.IsNullOrEmpty(url)) throw new ArgumentNullException("url");            
            string json = "json";
            string f = "f";
            string query = string.Empty;
            string root = string.Empty;
            if (url.Contains(f + "=" + json)) return url;

            // get root url using string manipulation instead of building a Uri so items like
            // port number and a trailing slash are not added. Want to return url as it was entered
            root = url.Contains("?") ? url.Replace(url.Substring(url.IndexOf("?")), "") : url;

            // set the 'f' parameter of query string
            Uri uri = new Uri(url);
            NameValueCollection queryparams = HttpUtility.ParseQueryString(uri.Query);
            if (queryparams.AllKeys.Contains(f))
            {
                queryparams[f] = json;
            }
            else
            {
                queryparams.Add(f, json);
            }
            query = "?" + queryparams.ToString();

            // build the new Uri            
            return root + query;
        }
        #endregion

        #region Protected
        /// <summary>
        /// Raises the <see cref="E:Geocrest.Model.RestHelper.HydrateCompleted"/> event.
        /// </summary>
        /// <param name="e">The event arguments containing the hydrated object or an exception if the operation failed.</param>
        protected void OnHydrateCompleted<T>(HydrateCompletedEventArgs e)
        {
            this.WebHelper.DownloadStringCompleted -= WebHelper_DownloadStringCompleted<T>;
            this.WebHelper.UploadStringCompleted -= WebHelper_UploadStringCompleted<T>;
            this.WebHelper.UploadValuesCompleted -= WebHelper_UploadValuesCompleted<T>;
            EventHandler<HydrateCompletedEventArgs> handler = HydrateCompleted;
            if (handler != null)
                handler(this, e);
        }
        #endregion

        #region Private
        private void WebHelper_UploadStringCompleted<TModel>(object sender, WrappedUploadStringCompletedEventArgs e)
        {
            ValidateStringResult<TModel>(sender, e, e.Result);
        }
        private void WebHelper_DownloadStringCompleted<TModel>(object sender, WrappedDownloadStringCompletedEventArgs e)
        {
            ValidateStringResult<TModel>(sender, e, e.Result);
        }
        private void WebHelper_UploadValuesCompleted<TModel>(object sender, WrappedUploadValuesCompletedEventArgs e)
        {
            ValidateByteArrayResult<TModel>(sender, e, e.Result);
        }
        private void ValidateStringResult<TModel>(object sender, WrappedAsyncCompletedEventArgs e, string result)
        {
            // check for error
            if (e.Error != null)
            {
                OnHydrateCompleted<TModel>(new HydrateCompletedEventArgs<TModel>(default(TModel), e.Error,
                    e.Cancelled,e.UserState,e.OriginalUrl, result));
            }

            // check response type
            var headers = ((IWebHelper)sender).ResponseHeaders != null ? 
                ((IWebHelper)sender).ResponseHeaders.GetValues("Content-Type") : null;
            if (headers != null && headers.Any(x => x.Contains("text/html")))
            {
                OnHydrateCompleted<TModel>(new HydrateCompletedEventArgs<TModel>(default(TModel), 
                    new HttpException((int)HttpStatusCode.UnsupportedMediaType, string.Format(
                        "The underlying web service located at {0} returned HTML and only JSON or XML is supported.",
                        e.OriginalUrl)),e.Cancelled,e.UserState, e.OriginalUrl, result));
            }

            // check for empty response
            string json = result;
            if (string.IsNullOrEmpty(json))
            {
                OnHydrateCompleted<TModel>(new HydrateCompletedEventArgs<TModel>(default(TModel), 
                    new HttpException((int)HttpStatusCode.NoContent, string.Format(
                        "The underlying web service response from {0} was null.",
                        e.OriginalUrl)),e.Cancelled,e.UserState, e.OriginalUrl, result));
            }
            else if (e.OriginalUrl != null && e.OriginalUrl.AbsoluteUri.ToString().ToLower().Contains("arcgis/rest/services"))
            {
                ESRIException error = JsonConvert.DeserializeObject<ESRIException>(json);
                if (error.Error != null)
                {
                    OnHydrateCompleted<TModel>(new HydrateCompletedEventArgs<TModel>(default(TModel), 
                        new HttpException((int)error.Error.Code, string.Format(
                            "The underlying ArcGIS service located at {0} returned the following error: {1}: {2}",
                        e.OriginalUrl, error.Error.Message, string.Join("; ", error.Error.Details))),
                        e.Cancelled,e.UserState,e.OriginalUrl, result));
                }
                else
                {
                    Success<TModel>(json, e);
                }
            }
            else
            {
                Success<TModel>(json,e);
            }
        }
        private void Success<TModel>(string response, WrappedAsyncCompletedEventArgs e)
        {
            var value = JsonConvert.DeserializeObject<TModel>(response, new StringEnumConverter());
            HttpException exception = value != null ? null : new HttpException((int)HttpStatusCode.NoContent, 
                    string.Format("An error occurred while deserializing the response: {0}", response));
            HydrateCompletedEventArgs args = new HydrateCompletedEventArgs(typeof(TModel), value,
                exception, false, e.UserState, e.OriginalUrl, response);
            OnHydrateCompleted<TModel>(args);
        }
        private void ValidateByteArrayResult<TModel>(object sender, WrappedAsyncCompletedEventArgs e, byte[] result)
        {
            var strResult = result == null ? string.Empty : ((IWebHelper)sender).Encoding.GetString(result);
            ValidateStringResult<TModel>(sender, e, strResult);
        }
        #endregion
    }
}