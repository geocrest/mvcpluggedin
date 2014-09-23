namespace Geocrest.Data.Sources.Gis
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using Geocrest.Data.Contracts.Gis;
    using Geocrest.Model.ArcGIS.Tasks;

    /// <summary>
    /// Represents an ArcGIS Server geoprocessing service.
    /// </summary>
    [DataContract(Namespace = Geocrest.XmlNamespaces.InfrastructureVersion1)]
    public class GeoprocessingServer: ArcGISService, IGeoprocessingServer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Data.Sources.Gis.GeoprocessingServer"/> class.
        /// </summary>
        internal GeoprocessingServer() { }

        /// <summary>
        /// Gets or sets the type of the execution.
        /// </summary>
        /// <value>
        /// The type of the execution.
        /// </value>
        [DataMember]
        public esriExecutionType ExecutionType { get; set; }

        /// <summary>
        /// Gets or sets the name of the result map server.
        /// </summary>
        /// <value>
        /// The name of the result map server.
        /// </value>
        [DataMember]
        public string ResultMapServerName { get; set; }

        /// <summary>
        /// Gets or sets the tasks associated with this service.
        /// </summary>
        /// <value>
        /// The tasks.
        /// </value>
        [DataMember(Name="tasks")]
        public string[] TaskNames { get; set; }

        #region IGeoprocessingServer Members
        /// <summary>
        /// Gets the tasks associated with the service.
        /// </summary>
        public List<IGeoprocessingTask> Tasks { get; internal set; }
        #endregion
    }
}
