namespace Geocrest.Data.Contracts.Gis
{
    using System.Collections.Generic;
    using Geocrest.Model.ArcGIS.Tasks;
    /// <summary>
    /// Provides access to geoprocessing operations.
    /// </summary>
    public interface IGeoprocessingServer : IArcGISService
    {
        /// <summary>
        /// Gets or sets the type of the execution.
        /// </summary>
        /// <value>
        /// The type of the execution.
        /// </value>
        esriExecutionType ExecutionType { get; set; }
        /// <summary>
        /// Gets or sets the name of the result map server.
        /// </summary>
        /// <value>
        /// The name of the result map server.
        /// </value>
        string ResultMapServerName { get; set; }
        /// <summary>
        /// Gets or sets the tasks associated with this service.
        /// </summary>
        /// <value>
        /// The tasks.
        /// </value>
        string[] TaskNames { get; set; }
        /// <summary>
        /// Gets the tasks associated with the service.
        /// </summary>
        List<IGeoprocessingTask> Tasks { get; }       
    }
}
