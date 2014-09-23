
namespace Geocrest.Model
{
    using System;
    using System.Collections.Specialized;

    /// <summary>
    /// Provides methods for hydrating .NET types using JSON representations located at the specified url endpoints.
    /// </summary>
    public interface IRestHelper
    {
        /// <summary>
        /// Allows hydration of an object from a json string representation
        /// </summary>
        /// <typeparam name="T">The type of object to hydrate.</typeparam>
        /// <param name="json">The string representation of json used to deserialize.</param>
        /// <returns>The hydrated object.</returns>
        T HydrateFromJson<T>(string json);
        /// <summary>
        /// Allows hydration of an object from a json string representation
        /// </summary>
        /// <param name="json">The json to deserialize.</param>
        /// <param name="type">The type of object being deserialized.</param>
        /// <returns>The hydrated object.</returns>
        /// <exception cref="T:System.ArgumentNullException">json parameter is required.</exception>
        object HydrateFromJson(string json, Type type);
        /// <summary>
        /// Gets the web helper used for retrieving web resources.
        /// </summary>
        IWebHelper WebHelper { get; }
        /// <summary>
        /// Occurs when the hydrate operation has completed.
        /// </summary>
        event EventHandler<HydrateCompletedEventArgs> HydrateCompleted;
        /// <summary>
        /// Hydrates an object using the string response from the specified URL resource.
        /// </summary>
        /// <typeparam name="T">The type of object to hydrate.</typeparam>
        /// <param name="url">The web service URL.</param>
        /// <returns>The hydrated object.</returns>
        T Hydrate<T>(string url);
        /// <summary>
        /// Hydrates an object using the string response from the specified URL resource.
        /// </summary>
        /// <typeparam name="T">The type of object to hydrate.</typeparam>
        /// <param name="url">The web service URL.</param>
        void HydrateAsync<T>(string url);
        /// <summary>
        /// Hydrates an object using the string response from the specified URL resource.
        /// </summary>
        /// <typeparam name="T">The type of object to hydrate.</typeparam>
        /// <param name="url">The web service URL.</param>
        /// <param name="userState">A user-defined object that is passed to the method invoked when the asynchronous
        /// operation completes.</param>
        void HydrateAsync<T>(string url, object userState);
        /// <summary>
        /// Hydrates an object using the string response from the specified URL resource.
        /// This method allows data to be uploaded to the server.
        /// </summary>
        /// <typeparam name="T">The type of object to hydrate.</typeparam>
        /// <param name="url">The web service URL.</param>
        /// <param name="data">A string representing data to upload to the server.</param>
        void HydrateAsync<T>(string url, string data);
        /// <summary>
        /// Hydrates an object using the string response from the specified URL resource.
        /// This method allows data to be uploaded to the server.
        /// </summary>
        /// <typeparam name="T">The type of object to hydrate.</typeparam>
        /// <param name="url">The web service URL.</param>
        /// <param name="data">A string representing data to upload to the server.</param>
        /// <param name="method">The HTTP method to use when uploading.</param>
        void HydrateAsync<T>(string url, string data, string method);
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
        void HydrateAsync<T>(string url, string data, string method, object userState);
        /// <summary>
        /// Hydrates an object using the string response from the specified URL resource.
        /// This method allows data to be uploaded to the server.
        /// </summary>
        /// <typeparam name="T">The type of object to hydrate.</typeparam>
        /// <param name="url">The web service URL.</param>
        /// <param name="data">The <see cref="T:System.Collections.Specialized.NameValueCollection" /> 
        /// to send to the resource.</param>
        void HydrateAsync<T>(string url, NameValueCollection data);
        /// <summary>
        /// Hydrates an object using the string response from the specified URL resource.
        /// This method allows data to be uploaded to the server.
        /// </summary>
        /// <typeparam name="T">The type of object to hydrate.</typeparam>
        /// <param name="url">The web service URL.</param>
        /// <param name="data">The <see cref="T:System.Collections.Specialized.NameValueCollection" /> 
        /// to send to the resource.</param>
        /// <param name="method">The HTTP method to use when uploading.</param>
        void HydrateAsync<T>(string url, NameValueCollection data, string method);
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
        void HydrateAsync<T>(string url, NameValueCollection data, string method, object userState);
    }
}
