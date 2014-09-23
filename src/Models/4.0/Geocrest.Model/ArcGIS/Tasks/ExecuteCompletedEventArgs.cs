
#if NET40 || SILVERLIGHT
namespace Geocrest.Model.ArcGIS.Tasks
{
    using System;
    using Geocrest.Model;

    /// <summary>
    /// Event arguments raised when a geoprocessing task has completed.
    /// </summary>
    public sealed class ExecuteCompletedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the results associated with the execution of a geoprocessing task.
        /// </summary>
        public GPResultSet Results { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Model.ArcGIS.Tasks.ExecuteCompletedEventArgs" /> class.
        /// </summary>
        /// <param name="results">The results of the task.</param>
        public ExecuteCompletedEventArgs(GPResultSet results)
        {
            this.Results = results;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Model.ArcGIS.Tasks.ExecuteCompletedEventArgs" /> class.
        /// </summary>
        /// <param name="resultsJson">The results as a json structure.</param>
        /// <exception cref="T:System.ArgumentNullException">resultsJson</exception>
        public ExecuteCompletedEventArgs(string resultsJson)
        {
            if (string.IsNullOrEmpty(resultsJson)) throw new ArgumentNullException("resultsJson");
            this.Results = RestHelper.HydrateObjectFromJson<GPResultSet>(resultsJson);
        }
    }
}
#endif