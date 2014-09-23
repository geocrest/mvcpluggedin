
namespace Geocrest.Data.Contracts.Gis
{
    using System;
    using System.Collections.Generic;
    using Geocrest.Model;
    using Geocrest.Model.ArcGIS;
    using Geocrest.Model.ArcGIS.Geometry;
    using Geocrest.Model.ArcGIS.Tasks;
    /// <summary>
    /// Represents a specific geoprocessing task.
    /// </summary>
    public interface IGeoprocessingTask
    {
        /// <summary>
        /// Gets or sets the spatial reference to use for the output.
        /// </summary>
        /// <value>
        /// The output spatial reference.
        /// </value>
        WKID OutSpatialReference { get; set; }
        /// <summary>
        /// Gets or sets the spatial reference to use during processing.
        /// </summary>
        /// <value>
        /// The process spatial reference.
        /// </value>
        WKID ProcessSpatialReference { get; set; }
        /// <summary>
        /// Gets or sets the time interval in milliseconds between job status requests. The default is 1000.
        /// </summary>
        /// <value>
        /// The update delay.
        /// </value>
        int UpdateDelay { get; set; }
        /// <summary>
        /// Gets or sets the URL through which all request will be proxied.
        /// </summary>
        /// <value>
        /// The fully-qualified proxy URL.
        /// </value>
        string ProxyUrl { get; set; }
        /// <summary>
        /// Gets the Url to the specific task.
        /// </summary>
        /// <value>
        /// The Url.
        /// </value>
        string Url { get; }
        /// <summary>
        /// Gets or sets the name of the task.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        string Name { get; set; }
        /// <summary>
        /// Gets or sets the display name of the task.
        /// </summary>
        /// <value>
        /// The display name.
        /// </value>
        string DisplayName { get; set; }
        /// <summary>
        /// Gets or sets the category.
        /// </summary>
        /// <value>
        /// The category.
        /// </value>
        string Category { get; set; }
        /// <summary>
        /// Gets or sets the help URL.
        /// </summary>
        /// <value>
        /// The help URL.
        /// </value>
        string HelpUrl { get; set; }
        /// <summary>
        /// Gets or sets how the task executes.
        /// </summary>
        /// <value>
        /// The type of execution.
        /// </value>
        esriExecutionType ExecutionType { get; set; }
        /// <summary>
        /// Gets or sets information about the various parameters.
        /// </summary>
        /// <value>
        /// The parameters.
        /// </value>
        GPParameterInfo[] Parameters { get; set; }
        /// <summary>
        /// Gets or sets the rest helper used for hydration of objects.
        /// </summary>
        /// <value>
        /// The rest helper.
        /// </value>
        IRestHelper RestHelper { get;  set; }
        /// <summary>
        /// Gets the last result retrieved from calling GetResultDataAsync.
        /// </summary>
        GPParameter GetResultDataLastResult { get; }
        /// <summary>
        /// Gets the last result image retrieved from calling GetResultImageAsync.
        /// </summary>
        MapImage GetResultImageLastResult { get; }
        /// <summary>
        /// Executes the task.
        /// </summary>
        /// <param name="parameters">A <see cref="T:System.Collections.Generic.List`1"/> of input 
        /// <see cref="T:Geocrest.Model.ArcGIS.Tasks.GPParameter"/>s.</param>
        void ExecuteAsync(List<GPParameter> parameters);
        /// <summary>
        /// Submits the job.
        /// </summary>
        /// <param name="parameters">A <see cref="T:System.Collections.Generic.List`1"/> of input 
        /// <see cref="T:Geocrest.Model.ArcGIS.Tasks.GPParameter"/>s.</param>
        void SubmitJobAsync(List<GPParameter> parameters);
        /// <summary>
        /// Gets a result data parameter.
        /// </summary>
        /// <param name="jobId">A string that uniquely identifies a job on the server.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        void GetResultDataAsync(string jobId, string parameterName);
        /// <summary>
        /// Gets a map image that displays the results of the task.
        /// </summary>
        /// <param name="jobId">A string that uniquely identifies a job on the server.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        void GetResultImageAsync(string jobId, string parameterName);
        /// <summary>
        /// Occurs when a submit job operation has successfully completed.
        /// </summary>
        event EventHandler<JobCompletedEventArgs> JobCompleted;
        /// <summary>
        /// Occurs when an execute task operation has successfully completed.
        /// </summary>
        event EventHandler<ExecuteCompletedEventArgs> ExecuteCompleted;
        /// <summary>
        /// Occurs when the tasks fails.
        /// </summary>
        event EventHandler<TaskFailedEventArgs> Failed;
        /// <summary>
        /// Occurs when a result parameter has been retrieved as data.
        /// </summary>
        event EventHandler<GPParameterEventArgs> GetResultDataCompleted;
        /// <summary>
        /// Occurs when a result parameter has been retrieved as an image.
        /// </summary>
        event EventHandler<GetResultImageEventArgs> GetResultImageCompleted;
    }
}
