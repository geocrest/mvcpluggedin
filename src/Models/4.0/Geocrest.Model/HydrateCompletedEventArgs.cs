namespace Geocrest.Model
{
    using System;

    /// <summary>
    /// Event arguments containing the result of a hydrate operation.
    /// </summary>
    public class HydrateCompletedEventArgs : WrappedAsyncCompletedEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Model.HydrateCompletedEventArgs" /> class.
        /// </summary>
        /// <param name="type">The type of model being hydrated.</param>
        /// <param name="result">The hydrated object.</param>
        /// <param name="error">Any error that occurred during the asynchronous operation.</param>
        /// <param name="cancelled">A value indicating whether the asynchronous operation was canceled.</param>
        /// <param name="userState">The optional user-supplied state object passed to the
        /// <see cref="T:System.ComponentModel.BackgroundWorker.RunWorkerAsync()" /> method.</param>
        /// <param name="url">The URL of the resource requested.</param>
        /// <param name="response">The response string from the web request.</param>
        public HydrateCompletedEventArgs(Type type, object result, Exception error, 
            bool cancelled,object userState,  Uri url, string response)
            : base(type, result, error, cancelled, userState, url)
        {
            this.Response = response;
        }        
        /// <summary>
        /// Gets the string response returned from the request.
        /// </summary>
        /// <value>
        /// The originial response returned from the web request.
        /// </value>
        public string Response { get; private set; }       
    }
}
