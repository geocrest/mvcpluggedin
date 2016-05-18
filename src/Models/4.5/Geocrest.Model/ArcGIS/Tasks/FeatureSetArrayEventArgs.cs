namespace Geocrest.Model.ArcGIS.Tasks
{
    using System;
    /// <summary>
    /// Provides event argument data when the result of a geoprocessing task is an
    /// array of featuresets. This is a custom event argument created for the 
    /// bulk geoprocessing model.
    /// </summary>
    public class FeatureSetArrayEventArgs : GenericEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Model.ArcGIS.Tasks.FeatureSetArrayEventArgs" /> class.
        /// </summary>
        /// <param name="result">The features to return as the result.</param>
        /// <param name="error">Any error that occurred during the asynchronous operation.</param>
        /// <param name="cancelled">A value indicating whether the asynchronous operation was canceled.</param>
        /// <param name="userState">The optional user-supplied state object passed to the 
        /// <see cref="T:System.ComponentModel.BackgroundWorker.RunWorkerAsync()"/> method.</param>
        public FeatureSetArrayEventArgs(FeatureSet[] result, Exception error, bool cancelled, object userState)
            : base(typeof(FeatureSet[]), result, error,cancelled,userState)
        {
            this.Result = error == null ? result : null;
        }
        /// <summary>
        /// Gets the result of an operation to provide to an event handler
        /// </summary>
        /// <value>
        /// The event data provided from the operation.
        /// </value>
        public new FeatureSet[] Result { get; private set; }
    }
}
