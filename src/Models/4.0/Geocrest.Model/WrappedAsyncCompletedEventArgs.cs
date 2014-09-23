namespace Geocrest.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    /// <summary>
    /// Provides generic event data for returning results to an event handler when
    /// making web requests with the <see cref="T:System.Net.WebClient"/>.
    /// </summary>
    public abstract class WrappedAsyncCompletedEventArgs : GenericEventArgs
    {
        private static bool urlSet;
        private static Uri originalUrl;
        private static bool stateSet;
        private static object userState;
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Model.WrappedAsyncCompletedEventArgs" /> class.
        /// </summary>
        /// <param name="type">The type of data being returned from an operation.</param>
        /// <param name="result">The result of an operation.</param>
        /// <param name="error">Any error that occurred during the asynchronous operation.</param>
        /// <param name="cancelled">A value indicating whether the asynchronous operation was canceled.</param>
        /// <param name="userState">The optional user-supplied state object passed to the
        /// <see cref="T:System.ComponentModel.BackgroundWorker.RunWorkerAsync()" /> method.</param>
        /// <param name="url">The URL of the resource requested.</param>
        protected WrappedAsyncCompletedEventArgs(Type type, object result, Exception error, bool cancelled, 
            object userState, Uri url) : base(type, result, error, cancelled, userState)
        {
            originalUrl = url;
            urlSet = true;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Model.WrappedAsyncCompletedEventArgs" /> class.
        /// </summary>
        /// <param name="args">The <see cref="T:System.ComponentModel.AsyncCompletedEventArgs" /> instance containing the event data.</param>
        /// <param name="type">The type of data being returned from an operation.</param>
        /// <param name="result">The result of an operation.</param>
        public WrappedAsyncCompletedEventArgs(AsyncCompletedEventArgs args,Type type, object result)
            : base(type, result, args.Error, args.Cancelled, HandleUserState(args.UserState))
        {            
        }
        /// <summary>
        /// Gets the original URL of the resource requested.
        /// </summary>
        /// <value>
        /// The original URL of the remote resource.
        /// </value>
        public Uri OriginalUrl
        {
            get
            {
                if (!urlSet)
                {
                    HandleUrl(base.UserState);
                }
                return originalUrl;
            }
        }

        /// <summary>
        /// Extracts the first item of UserState if UserState is an array and
        /// if there is more than one item and if that item is a Uri.
        /// </summary>
        /// <param name="userstate">An object reference that uniquely identifies the asynchronous task</param>
        protected static void HandleUrl(object userstate)
        {
            object[] objArray = userstate as object[];
            if (objArray != null && objArray.Length > 0 && objArray[0] is Uri)
            {
                originalUrl = (Uri)objArray[0];
            }
            urlSet = true;
        }
        /// <summary>
        /// Gets the unique identifier for the asynchronous task.
        /// </summary>
        /// <returns>An object reference that uniquely identifies the asynchronous task; otherwise, null if no value has been set.</returns>
        public new object UserState
        {
            get
            {
                if (!stateSet)
                {
                    HandleUserState(base.UserState);
                }
                return userState;
            }
        }
        /// <summary>
        /// Removes the first item of UserState if UserState is an array and
        /// if there is more than one item and if that item is a Uri.
        /// </summary>
        /// <param name="userstate">An object reference that uniquely identifies the asynchronous task</param>
        /// <returns></returns>
        protected static object HandleUserState(object userstate)
        {
            HandleUrl(userstate);
            object[] objArray = userstate as object[];
            if (objArray != null && objArray.Length > 1 && objArray[0] is Uri)
            {
                var token = new List<object>(objArray);
                token.RemoveAt(0);
                userState = token.Count > 1 ? token.ToArray() : token[0];
            }
            else
            {
                userState = userstate;
            }
            stateSet = true;
            return userState;
        }
    }
}
