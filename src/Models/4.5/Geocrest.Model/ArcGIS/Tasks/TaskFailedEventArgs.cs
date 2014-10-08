namespace Geocrest.Model.ArcGIS.Tasks
{
    using System;

    /// <summary>
    /// Event arguments raised when a geoprocessing task has failed to complete.
    /// </summary>
    public class TaskFailedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the error explaining why the task failed.
        /// </summary>
        public Exception Error { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Model.ArcGIS.Tasks.TaskFailedEventArgs" /> class.
        /// </summary>
        /// <param name="error">The error.</param>
        public TaskFailedEventArgs(Exception error)
        {
            this.Error = error;
        }
    }
}
