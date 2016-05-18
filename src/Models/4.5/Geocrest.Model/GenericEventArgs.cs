
namespace Geocrest.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    /// <summary>
    /// Provides generic event data for returning results to an event handler.
    /// </summary>    
    public abstract class GenericEventArgs : AsyncCompletedEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Model.GenericEventArgs" /> class.
        /// </summary>
        /// <param name="type">The type of data being returned from an operation.</param>
        /// <param name="result">The result of an operation.</param>
        /// <param name="error">Any error that occurred during the asynchronous operation.</param>
        /// <param name="cancelled">A value indicating whether the asynchronous operation was canceled.</param>
        /// <param name="userState">The optional user-supplied state object passed to the
        /// <see cref="T:System.ComponentModel.BackgroundWorker.RunWorkerAsync()" /> method.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="type"/> is null.</exception>
        protected GenericEventArgs(Type type, object result, Exception error, bool cancelled, object userState)
            :base(error, cancelled, userState)
        {
            if (type == null) throw new ArgumentNullException("type");
            this.Result = error == null ? result : null;   
            this.HasError = error != null ? true : false;
        }
        /// <summary>
        /// Gets the result of an operation to provide to an event handler
        /// </summary>
        /// <value>
        /// The event data provided from the operation.
        /// </value>
        public object Result { get; private set; }
        /// <summary>
        /// The type of data being returned from an operation.
        /// </summary>
        /// <value>
        /// The type of object represented by <see cref="P:Geocrest.Model.GenericEventArgs.Result"/>.
        /// </value>
        public Type Type { get; private set; }
        /// <summary>
        /// Gets or sets a value indicating whether the operation resulted in an error.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the operation resulted in an error; otherwise, <c>false</c>.
        /// </value>
        public bool HasError { get; private set; }
    }
}
