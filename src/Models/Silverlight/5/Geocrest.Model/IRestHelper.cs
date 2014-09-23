namespace Geocrest.Model
{
    using System;
    /// <summary>
    /// Provides methods for hydrating .NET types using JSON representations located at the specified url endpoints.
    /// </summary>
    public interface IRestHelper
    {       
        /// <summary>
        /// Hydrates an object using the string response from the specified URL resource.
        /// </summary>
        /// <typeparam name="T">The type of object to hydrate.</typeparam>
        /// <param name="url">The web service URL.</param>
        /// <param name="callback">A callback function that receives the result.</param>
        void HydrateAsync<T>(string url, Action<T> callback);
        /// <summary>
        /// Hydrates an object using the string response from the specified URL resource.
        /// </summary>
        /// <typeparam name="T">The type of object to hydrate.</typeparam>
        /// <param name="url">The web service URL.</param>
        /// <param name="callback">A callback function that receives the result.</param>
        /// <param name="userState">A user-defined object that is passed to the method invoked when the asynchronous operation completes.</param>
        void HydrateAsync<T>(string url, Action<T, object> callback, object userState);
        /// <summary>
        /// Allows hydration of an object from a json string representation
        /// </summary>
        /// <typeparam name="T">The type of object to hydrate.</typeparam>
        /// <param name="json">The string representation of json used to deserialize.</param>
        /// <returns>
        /// The hydrated object.
        /// </returns>
        T HydrateFromJson<T>(string json);
    }
}
