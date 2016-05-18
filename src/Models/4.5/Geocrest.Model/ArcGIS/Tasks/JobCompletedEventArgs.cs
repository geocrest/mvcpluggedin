namespace Geocrest.Model.ArcGIS.Tasks
{
    using System;

    /// <summary>
    /// Event arguments raised when a geoprocessing job has completed.
    /// </summary>
    public sealed class JobCompletedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the job information pertaining to the execution of an asynchronous geoprocessing task on the server.
        /// </summary>
        /// <value>
        /// The job info object.
        /// </value>
        public JobInfo JobInfo { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Model.ArcGIS.Tasks.JobCompletedEventArgs" /> class.
        /// </summary>
        /// <param name="info">The information about the submitted job.</param>
        public JobCompletedEventArgs(JobInfo info)
        {
            this.JobInfo = info;
        }
    }
}
