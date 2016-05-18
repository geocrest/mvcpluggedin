namespace Geocrest.Model
{
    using System;

    /// <summary>
    /// Event arguments containing the result of a hydrate operation.
    /// </summary>
    /// <typeparam name="T">The type of object being hydrated.</typeparam>
    public class HydrateCompletedEventArgs<T> : HydrateCompletedEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Model.HydrateCompletedEventArgs`1" /> class.
        /// </summary>
        /// <param name="result">The hydrated object.</param>
        /// <param name="error">Any error that occurred during the asynchronous operation.</param>
        /// <param name="cancelled">A value indicating whether the asynchronous operation was canceled.</param>
        /// <param name="userState">The optional user-supplied state object passed to the
        /// <see cref="T:System.ComponentModel.BackgroundWorker.RunWorkerAsync()" /> method.</param>
        /// <param name="url">The URL of the resource requested.</param>
        /// <param name="response">The response string from the web request.</param>
        public HydrateCompletedEventArgs(T result, Exception error, bool cancelled,
            object userState, Uri url, string response)
            : base(typeof(T), result, error,cancelled,userState, url,response)
        {
            this.Result = error == null ? result : default(T);
        }
        /// <summary>
        /// Gets the result of an operation to provide to an event handler
        /// </summary>
        /// <value>
        /// The event data provided from the operation.
        /// </value>
        public new T Result { get; private set; }
    }
}
