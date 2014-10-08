namespace Geocrest.Model
{
    using System;
    using System.ComponentModel;
    using System.Net;

    /// <summary>
    /// Wraps the <see cref="T:System.Net.UploadValuesCompletedEventArgs"/> class to enable unit testing.
    /// </summary>
    public class WrappedUploadValuesCompletedEventArgs : WrappedAsyncCompletedEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Model.WrappedUploadStringCompletedEventArgs" /> class.
        /// </summary>
        /// <param name="result">A <see cref="T:System.Byte" /> array containing the server reply.</param>
        /// <param name="error">Any error that occurred during the asynchronous operation.</param>
        /// <param name="cancelled">A value indicating whether the asynchronous operation was canceled.</param>
        /// <param name="userState">The optional user-supplied state object passed to the
        /// <see cref="T:System.ComponentModel.BackgroundWorker.RunWorkerAsync()" /> method.</param>
        /// <param name="url">The URL of the resource requested.</param>
        public WrappedUploadValuesCompletedEventArgs(byte[] result, Exception error, bool cancelled, object userState, Uri url)
            : base(typeof(byte[]),result, error, cancelled, userState, url)
        {
            this.Result = error == null ? result : null;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Model.WrappedUploadStringCompletedEventArgs" /> class.
        /// </summary>
        /// <param name="args">The <see cref="T:System.Net.UploadStringCompletedEventArgs"/> instance containing the event data.</param>
        public WrappedUploadValuesCompletedEventArgs(UploadValuesCompletedEventArgs args)
            :base(args, typeof(byte[]), args.Error == null ? args.Result : null)
        {
            this.Result = args.Error == null ? args.Result : null;
        }
        /// <summary>
        /// Gets the result of an operation to provide to an event handler
        /// </summary>
        /// <value>
        /// The event data provided from the operation.
        /// </value>
        public new byte[] Result { get; private set; }
    }
}
