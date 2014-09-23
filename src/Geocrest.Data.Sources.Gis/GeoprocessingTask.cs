namespace Geocrest.Data.Sources.Gis
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Net;
    using System.Runtime.Serialization;
    using System.Timers;
    using System.Web;
    using Geocrest.Data.Contracts.Gis;
    using Geocrest.Web.Infrastructure;
    using Geocrest.Model;
    using Geocrest.Model.ArcGIS;
    using Geocrest.Model.ArcGIS.Geometry;
    using Geocrest.Model.ArcGIS.Tasks;

    /// <summary>
    /// Represents an ArGIS Server geoprocessing service task.
    /// </summary>
    [DataContract(Namespace = Geocrest.XmlNamespaces.InfrastructureVersion1)]
    public sealed class GeoprocessingTask : IGeoprocessingTask
    {
        private Dictionary<string, GeoprocessingTask.JobParams> jobs = new Dictionary<string, GeoprocessingTask.JobParams>();
        private int updateDelay = 1000;
        private const int URLMAX = 2000;
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Data.Sources.Gis.GeoprocessingTask" /> class.
        /// </summary>
        internal GeoprocessingTask()
        {
            this.OutSpatialReference = WKID.NotSpecified;
            this.ProcessSpatialReference = WKID.NotSpecified;
        }

        #region IGeoprocessingTask Members
        #region Methods
        /// <summary>
        /// Executes the task using the synchronous ArcGIS Server geoprocessing task.
        /// </summary>
        /// <param name="parameters">A <see cref="T:System.Collections.Generic.List`1" /> of input
        /// <see cref="T:Geocrest.Model.ArcGIS.Tasks.GPParameter" />s.</param>
        public void ExecuteAsync(List<GPParameter> parameters)
        {
            try
            {
                ValidateParameters(parameters);
                Uri endpoint;
                var paramDict = GetParameters(parameters);
                RestHelper.HydrateCompleted += RestHelper_ExecuteComplete;
                if (TryParseUrl("execute", paramDict, out endpoint))
                {
                    RestHelper.HydrateAsync<GPResultSet>(endpoint.ToString());
                }
                else
                {
                    RestHelper.HydrateAsync<GPResultSet>(endpoint.ToString(), GetParametersAsNVC(paramDict));
                }
            }
            catch (Exception ex)
            {
                OnFailed(new TaskFailedEventArgs(ex));
            }
        }

        /// <summary>
        /// Executes the task using the asynchronous ArcGIS Server geoprocessing task.
        /// </summary>
        /// <param name="parameters">A <see cref="T:System.Collections.Generic.List`1"/> of input 
        /// <see cref="T:Geocrest.Model.ArcGIS.Tasks.GPParameter"/>s.</param>
        public void SubmitJobAsync(List<GPParameter> parameters)
        {
            if (this.UpdateDelay <= 0) Throw.InvalidOperation("Update Delay must be greater than zero.");
            JobParams jobParams = new JobParams();
            jobParams.updateDelay = this.UpdateDelay;
            try
            {
                ValidateParameters(parameters);
                Uri endpoint;
                var paramDict = GetParameters(parameters);
                RestHelper.HydrateCompleted += RestHelper_SubmitJobComplete;
                if (TryParseUrl("submitJob", paramDict, out endpoint))
                {
                    RestHelper.HydrateAsync<JobInfo>(endpoint.ToString(), jobParams);
                }
                else
                {
                    RestHelper.HydrateAsync<JobInfo>(endpoint.ToString(),
                        GetParametersAsNVC(paramDict), "POST", jobParams);
                }
            }
            catch (Exception ex)
            {
                OnFailed(new TaskFailedEventArgs(ex));
            }
        }

        /// <summary>
        /// Gets a result data parameter.
        /// </summary>
        /// <param name="jobId">A string that uniquely identifies a job on the server.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        public void GetResultDataAsync(string jobId, string parameterName)
        {
            Throw.IfArgumentNullOrEmpty(jobId, "jobId");
            Throw.IfArgumentNullOrEmpty(parameterName, "parameterName");
            Throw.IfArgumentNullOrEmpty(this.Url, "Url");
            if (this.Parameters.Count(x => x.Name == parameterName) == 0)
                Throw.InvalidOperation(parameterName + " does not exist in the list of parameters.");
            try
            {
                Uri endpoint = GetUrl(string.Format("jobs/{0}/results/{1}", jobId, parameterName),
                    new Dictionary<string, string> { { "returnType", "data" } });
                RestHelper.HydrateCompleted += RestHelper_GetResultDataComplete;
                RestHelper.HydrateAsync<GPResult>(endpoint.ToString(),(object)parameterName);
            }
            catch (Exception ex)
            {
                OnFailed(new TaskFailedEventArgs(ex));
            }
        }

        /// <summary>
        /// Gets a map image that displays the results of the task.
        /// </summary>
        /// <param name="jobId">A string that uniquely identifies a job on the server.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        public void GetResultImageAsync(string jobId, string parameterName)
        {
            Throw.IfArgumentNullOrEmpty(jobId, "jobId");
            Throw.IfArgumentNullOrEmpty(parameterName, "parameterName");
            Throw.IfArgumentNullOrEmpty(this.Url, "Url");
            if (this.Parameters.Count(x => x.Name == parameterName) == 0)
                Throw.InvalidOperation(parameterName + " does not exist in the list of parameters.");
            try
            {
                Uri endpoint = GetUrl(string.Format("jobs/{0}/results/{1}", jobId, parameterName), null);
                RestHelper.HydrateCompleted += RestHelper_GetResultImageComplete;
                RestHelper.HydrateAsync<GPResult>(endpoint.ToString());               
            }
            catch (Exception ex)
            {
                OnFailed(new TaskFailedEventArgs(ex));
            }
        }
        #endregion

        #region Events
        /// <summary>
        /// Occurs when a submit job operation has successfully completed.
        /// </summary>
        public event System.EventHandler<JobCompletedEventArgs> JobCompleted;

        /// <summary>
        /// Occurs when an execute task operation has successfully completed.
        /// </summary>
        public event System.EventHandler<ExecuteCompletedEventArgs> ExecuteCompleted;

        /// <summary>
        /// Occurs when the tasks fails.
        /// </summary>
        public event System.EventHandler<TaskFailedEventArgs> Failed;

        /// <summary>
        /// Occurs when a result parameter has been retrieved as data.
        /// </summary>
        public event System.EventHandler<GPParameterEventArgs> GetResultDataCompleted;

        /// <summary>
        /// Occurs when a result parameter has been retrieved as an image.
        /// </summary>
        public event System.EventHandler<GetResultImageEventArgs> GetResultImageCompleted;
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the URL through which all request will be proxied.
        /// </summary>
        /// <value>
        /// The fully-qualified proxy URL.
        /// </value>
        public string ProxyUrl { get; set; }
        /// <summary>
        /// Gets or sets the spatial reference to use for the output.
        /// </summary>
        /// <value>
        /// The output spatial reference.
        /// </value>
        public WKID OutSpatialReference { get; set; }

        /// <summary>
        /// Gets or sets the spatial reference to use during processing.
        /// </summary>
        /// <value>
        /// The process spatial reference.
        /// </value>
        public WKID ProcessSpatialReference { get; set; }

        /// <summary>
        /// Gets or sets the time interval in milliseconds between job status requests. The default is 1000.
        /// </summary>
        /// <value>
        /// The update delay.
        /// </value>
        public int UpdateDelay
        {
            get { return this.updateDelay; }
            set { this.updateDelay = value; }
        }

        /// <summary>
        /// Gets the Url to the specific task.
        /// </summary>
        /// <value>
        /// The Url.
        /// </value>
        public string Url { get; internal set; }

        /// <summary>
        /// Gets the last result retrieved from calling GetResultDataAsync.
        /// </summary>
        public GPParameter GetResultDataLastResult { get; private set; }

        /// <summary>
        /// Gets the last result image retrieved from calling GetResultImageAsync.
        /// </summary>
        public MapImage GetResultImageLastResult { get; private set; }

        /// <summary>
        /// Gets or sets the name of the task.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the display name of the task.
        /// </summary>
        /// <value>
        /// The display name.
        /// </value>
        [DataMember]
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the category.
        /// </summary>
        /// <value>
        /// The category.
        /// </value>
        [DataMember]
        public string Category { get; set; }

        /// <summary>
        /// Gets or sets the help URL.
        /// </summary>
        /// <value>
        /// The help URL.
        /// </value>
        [DataMember]
        public string HelpUrl { get; set; }

        /// <summary>
        /// Gets or sets how the task executes.
        /// </summary>
        /// <value>
        /// The type of execution.
        /// </value>
        [DataMember]
        public esriExecutionType ExecutionType { get; set; }

        /// <summary>
        /// Gets or sets information about the various parameters.
        /// </summary>
        /// <value>
        /// The parameters.
        /// </value>
        [DataMember]
        public GPParameterInfo[] Parameters { get; set; }

        /// <summary>
        /// Gets or sets the rest helper used for hydration of objects.
        /// </summary>
        /// <value>
        /// The rest helper.
        /// </value>
        public IRestHelper RestHelper { get; set; }
        #endregion

        #endregion

        #region Private
        /// <summary>
        /// Gets the parameterized Url to submit to the server.
        /// </summary>
        /// <param name="operation">The operation to perform.</param>
        /// <param name="parameters">A collection of input parameters to submit to the server. Argument can be null.</param>
        /// <returns>
        /// Returns a URL with the task's parameters included in the query string.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        /// <see cref="P:Geocrest.Data.Contracts.Gis.IGeoprocessingTask.Url"/> is null
        /// </exception>
        private Uri GetUrl(string operation, IDictionary<string, string> parameters)
        {
            Throw.IfArgumentNullOrEmpty(this.Url, "url");
            string baseurl = new Uri(this.Url).GetLeftPart(UriPartial.Path);
            if (baseurl.EndsWith("/")) baseurl = baseurl.Substring(0, baseurl.Length - 1);
            baseurl = (baseurl + "/" + operation).ForceJsonFormat();

            if (parameters != null)
            {
                string query = string.Empty;
                string pair = "&{0}={1}";
                foreach (var kvp in parameters)
                {
                    string value = string.IsNullOrEmpty(kvp.Value) ? null : kvp.Value;
                    query += string.Format(pair, kvp.Key, Uri.EscapeUriString(value));
                }
                return new Uri(baseurl + query);
            }
            return new Uri(!string.IsNullOrEmpty(this.ProxyUrl)
                ? this.ProxyUrl + "?" + baseurl : baseurl);
        }
        /// <summary>
        /// Gets the base Url to submit to the server without parameters.
        /// </summary>
        /// <param name="operation">The operation to perform.</param>
        /// <returns>
        /// Returns a URL without the task's parameters included in the query string.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        /// <see cref="P:Geocrest.Data.Contracts.Gis.IGeoprocessingTask.Url"/> is null</exception>
        private Uri GetUrl(string operation)
        {
            return GetUrl(operation, null);
        }
        /// <summary>
        /// Gets the parameters.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <returns>
        /// Returns an instance of <see cref="T:System.Collections.Generic.Dictionary`2"/>.
        /// </returns>
        private IDictionary<string, string> GetParameters(List<GPParameter> parameters)
        {
            IDictionary<string, string> inputs = new Dictionary<string, string>();
            try
            {
                this.Parameters
                    .Where(x => x.Direction == esriDirection.esriGPParameterDirectionInput)
                    .ForEach(p =>
                    {
                        GPParameter param = parameters.FirstOrDefault(x => x.Name == p.Name);
                        inputs.Add(p.Name, param != null ? param.ToJson() : "");
                    });
                if (this.OutSpatialReference != WKID.NotSpecified)
                    inputs.Add("env:outSR", this.OutSpatialReference.ToString());
                if (this.ProcessSpatialReference != WKID.NotSpecified)
                    inputs.Add("env:processSR", this.ProcessSpatialReference.ToString());
            }
            catch (Exception ex)
            {
                Throw.InvalidOperation("Failed to set input geoprocessing parameters.", x =>
                    new InvalidOperationException(x.Message, ex));
            }
            return inputs;
        }
        /// <summary>
        /// Gets the parameters as a collection of name/value pairs.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <returns>
        /// Returns an instance of <see cref="System.Collections.Specialized.NameValueCollection"/> with an
        /// additional entry for f=json.
        /// </returns>
        private NameValueCollection GetParametersAsNVC(IDictionary<string, string> parameters)
        {
            var nvc = parameters.ToNameValueCollection<string, string>();
            nvc.Add("f", "json");
            return nvc;
        }
        /// <summary>
        /// Validates the parameters.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <exception cref="T:System.InvalidOperationException">if any required input parameters are not specified.</exception>
        private void ValidateParameters(List<GPParameter> parameters)
        {
            // validate required parameters
            if (this.Parameters.Where(x => x.ParameterType == esriParameterType.esriGPParameterTypeRequired)
                .All(x => parameters.Count(p => p.Name == x.Name &&
                    x.Direction == esriDirection.esriGPParameterDirectionInput) > 0))
            {
                Throw.InvalidOperation("Missing required parameter.");
            }
        }

        /// <summary>
        /// Raises the <see cref="E:Geocrest.Data.Sources.Gis.GeoprocessingTask.JobCompleted"/> event.
        /// </summary>
        /// <param name="args">The <see cref="T:Geocrest.Model.ArcGIS.Tasks.JobCompletedEventArgs"/> instance containing the event data.</param>
        private void OnJobCompleted(JobCompletedEventArgs args)
        {
            EventHandler<JobCompletedEventArgs> handler = JobCompleted;
            if (handler != null)
                handler(this, args);
        }

        /// <summary>
        /// Raises the <see cref="E:Geocrest.Data.Sources.Gis.GeoprocessingTask.ExecuteCompleted"/> event.
        /// </summary>
        /// <param name="args">The <see cref="T:Geocrest.Model.ArcGIS.Tasks.ExecuteCompletedEventArgs"/> instance containing the event data.</param>
        private void OnExecuteComplete(ExecuteCompletedEventArgs args)
        {
            EventHandler<ExecuteCompletedEventArgs> handler = ExecuteCompleted;
            if (handler != null)
                handler(this, args);
        }

        /// <summary>
        /// Raises the <see cref="E:Geocrest.Data.Sources.Gis.GeoprocessingTask.GetResultDataCompleted"/> event.
        /// </summary>
        /// <param name="args">The <see cref="T:Geocrest.Model.ArcGIS.Tasks.GPParameterEventArgs"/> instance containing the event data.</param>
        private void OnGetResultDataComplete(GPParameterEventArgs args)
        {
            EventHandler<GPParameterEventArgs> handler = GetResultDataCompleted;
            if (handler != null)
                handler(this, args);
        }

        /// <summary>
        /// Raises the <see cref="E:Geocrest.Data.Sources.Gis.GeoprocessingTask.Failed"/> event.
        /// </summary>
        /// <param name="args">The <see cref="T:Geocrest.Model.ArcGIS.Tasks.TaskFailedEventArgs"/> instance containing the event data.</param>
        private void OnFailed(TaskFailedEventArgs args)
        {
            UnhookEvents();
            EventHandler<TaskFailedEventArgs> handler = Failed;
            if (handler != null)
                handler(this, args);
        }

        /// <summary>
        /// Raises the <see cref="E:Geocrest.Data.Sources.Gis.GeoprocessingTask.GetResultImageCompleted"/> event.
        /// </summary>
        /// <param name="args">The <see cref="T:Geocrest.Model.ArcGIS.Tasks.GetResultImageEventArgs"/> instance containing the event data.</param>
        private void OnGetResultImageComplete(GetResultImageEventArgs args)
        {
            EventHandler<GetResultImageEventArgs> handler = GetResultImageCompleted;
            if (handler != null)
                handler(this, args);
        }

        /// <summary>
        /// Attempts to create a new Uri for the given operation and parameter list. If the Uri can
        /// be created with under 2000 characters the result will be true (e.g. GET) else it will be 
        /// false (e.g. POST)
        /// </summary>
        /// <param name="operation">The operation.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="endpoint">The endpoint.</param>
        /// <returns>
        /// Returns an instance of <see cref="System.Boolean"/>.
        /// </returns>
        private bool TryParseUrl(string operation, IDictionary<string, string> parameters, out Uri endpoint)
        {
            try
            {
                var uri = GetUrl(operation, parameters);
                endpoint = uri.ToString().Length < URLMAX ? uri : GetUrl(operation, null);                
                return uri.ToString().Length < URLMAX ? true : false;
            }
            catch (UriFormatException)
            {
                endpoint = GetUrl(operation, null);
                return false;
            }
        }
        private void UpdateJobs(string result, string jobId)
        {
            if (!this.jobs.ContainsKey(jobId))
                return;
            try
            {
                JobParams jobParams = this.jobs[jobId];
                if (jobParams.cancelled)
                {
                    this.jobs.Remove(jobId);
                    jobParams.timer.Stop();
                    jobParams.timer = null;
                }
                else
                {
                    ESRIException error = RestHelper.HydrateFromJson<ESRIException>(result);
                    if (error != null && error.Error != null)
                    {
                        this.jobs.Remove(jobId);
                        jobParams.timer.Stop();
                        jobParams.timer.Dispose();
                        jobParams.timer = null;
                        OnFailed(new TaskFailedEventArgs(new HttpException((int)error.Error.Code,
                            error.Error.Message)));
                        return;
                    }
                    JobInfo jobInfo = RestHelper.HydrateFromJson<JobInfo>(result);
                    JobCompletedEventArgs args = new JobCompletedEventArgs(jobInfo);
                    switch (jobInfo.JobStatus)
                    {
                        case esriJobStatus.esriJobSucceeded:
                        case esriJobStatus.esriJobFailed:
                        case esriJobStatus.esriJobTimedOut:
                        case esriJobStatus.esriJobCancelled:
                        case esriJobStatus.esriJobDeleted:
                            this.jobs.Remove(jobId);
                            jobParams.timer.Stop();
                            jobParams.timer.Dispose();
                            jobParams.timer = null;
                            this.OnJobCompleted(args);
                            break;
                        default:
                            jobParams.timer.Start();
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                OnFailed(new TaskFailedEventArgs(ex));
            }
        }

        private void RestHelper_SubmitJobComplete(object sender, HydrateCompletedEventArgs args)
        {
            try
            {
                RestHelper.HydrateCompleted -= RestHelper_SubmitJobComplete;
                JobInfo info = args.Result as JobInfo;
                if (args.HasError)
                {
                    OnFailed(new TaskFailedEventArgs(args.Error));
                }
                else if (info != null)
                {
                    JobParams jp = args.UserState as JobParams;
                    this.jobs[info.JobId] = jp;
                    jp.timer = new Timer((double)this.UpdateDelay);
                    jp.timer.Elapsed += (s, e) =>
                    {
                        jp.timer.Stop();
                        if (!jp.cancelled)
                        {
                            Dictionary<string, string> temp_91 = new Dictionary<string, string>();
                            if (jp.webClient == null)
                            {
                                jp.webClient = this.RestHelper.WebHelper.GetWebHelper();
                            }
                            string result = jp.webClient.DownloadString(GetUrl(string.Format("jobs/{0}", info.JobId)));
                            this.UpdateJobs(result, info.JobId);
                        }
                        else
                            this.jobs.Remove(info.JobId);
                    };
                    jp.timer.Start();
                }
                else
                {
                    OnFailed(new TaskFailedEventArgs(new HttpException((int)HttpStatusCode.NoContent,
                    string.Format("The response from '{0}' was empty.", this.Url))));
                }
            }
            catch (Exception ex)
            {
                OnFailed(new TaskFailedEventArgs(ex));
            }
        }
        private void RestHelper_ExecuteComplete(object sender, HydrateCompletedEventArgs args)
        {
            RestHelper.HydrateCompleted -= RestHelper_ExecuteComplete;
            GPResultSet results = args.Result as GPResultSet;
            if (args.HasError)
            {
                OnFailed(new TaskFailedEventArgs(args.Error));
            }
            else if (results != null)
            {
                OnExecuteComplete(new ExecuteCompletedEventArgs(results));
            }
            else
            {
                OnFailed(new TaskFailedEventArgs(new HttpException((int)HttpStatusCode.NoContent,
                string.Format("The response from '{0}' was empty.", this.Url))));
            }
        }
        private void RestHelper_GetResultDataComplete(object sender, HydrateCompletedEventArgs args)
        {
            GPResult result = args.Result as GPResult;
            if (args.UserState as string == result.ParamName)
            {
                RestHelper.HydrateCompleted -= RestHelper_GetResultDataComplete;
                if (args.HasError)
                {
                    OnFailed(new TaskFailedEventArgs(args.Error));
                }
                else if (result != null)
                {
                    object parameter = args.UserState as object;
                    this.GetResultDataLastResult = result.Value;
                    this.OnGetResultDataComplete(new GPParameterEventArgs(this.GetResultDataLastResult));
                }
                else
                {
                    OnFailed(new TaskFailedEventArgs(new HttpException((int)HttpStatusCode.NoContent,
                    string.Format("The response from '{0}' was empty.", this.Url))));
                }
            }
        }
        private void RestHelper_GetResultImageComplete(object sender, HydrateCompletedEventArgs args)
        {
            RestHelper.HydrateCompleted -= RestHelper_GetResultImageComplete;
            GPResult result = args.Result as GPResult;
            if (args.HasError)
            {
                OnFailed(new TaskFailedEventArgs(args.Error));
            }
            else if (result != null)
            {
                this.GetResultImageLastResult = result.Value;
                this.OnGetResultImageComplete(new GetResultImageEventArgs(this.GetResultImageLastResult));
            }
            else
            {
                OnFailed(new TaskFailedEventArgs(new HttpException((int)HttpStatusCode.NoContent,
                string.Format("The response from '{0}' was empty.", this.Url))));
            }
        }

        private void UnhookEvents()
        {
            this.RestHelper.HydrateCompleted -= RestHelper_ExecuteComplete;
            this.RestHelper.HydrateCompleted -= RestHelper_GetResultDataComplete;
            this.RestHelper.HydrateCompleted -= RestHelper_GetResultImageComplete;
            this.RestHelper.HydrateCompleted -= RestHelper_SubmitJobComplete;
        }
        private class JobParams
        {
            public int updateDelay { get; set; }

            public Timer timer { get; set; }

            public IWebHelper webClient { get; set; }

            public bool cancelled { get; set; }
        }
        #endregion
    }
}
