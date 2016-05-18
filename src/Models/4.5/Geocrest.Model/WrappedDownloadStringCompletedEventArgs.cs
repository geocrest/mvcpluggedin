namespace Geocrest.Model
{
    using System;
    using System.Net;

    /// <summary>
    /// Wraps the <see cref="T:System.Net.DownloadStringCompletedEventArgs"/> class to enable unit testing.
    /// </summary>
    public class WrappedDownloadStringCompletedEventArgs : WrappedAsyncCompletedEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Model.WrappedDownloadStringCompletedEventArgs" /> class.
        /// </summary>
        /// <param name="result">The string result of the operation.</param>
        /// <param name="error">Any error that occurred during the asynchronous operation.</param>
        /// <param name="cancelled">A value indicating whether the asynchronous operation was canceled.</param>
        /// <param name="userState">The optional user-supplied state object passed to the
        /// <see cref="T:System.ComponentModel.BackgroundWorker.RunWorkerAsync()" /> method.</param>
        /// <param name="url">The URL of the resource requested.</param>
        public WrappedDownloadStringCompletedEventArgs(string result, Exception error, bool cancelled, 
            object userState, Uri url) : base(typeof(string),result, error, cancelled, userState, url)
        {
            this.Result = error == null ? result : null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Model.WrappedDownloadStringCompletedEventArgs" /> class.
        /// </summary>
        /// <param name="args">The <see cref="T:System.Net.DownloadStringCompletedEventArgs" /> instance containing the event data.</param>
        public WrappedDownloadStringCompletedEventArgs(DownloadStringCompletedEventArgs args)
            : base(args, typeof(string), args.Error == null ? args.Result : string.Empty)
        {
            this.Result = args.Error == null ? args.Result : null;
        }
        /// <summary>
        /// Gets the result of an operation to provide to an event handler
        /// </summary>
        /// <value>
        /// The event data provided from the operation.
        /// </value>
        public new string Result { get; private set; }
    }
}
