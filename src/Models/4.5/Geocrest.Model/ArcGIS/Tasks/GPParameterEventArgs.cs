namespace Geocrest.Model.ArcGIS.Tasks
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Provides a single parameter returned from an event.
    /// </summary>
    [DataContract(Namespace = Geocrest.XmlNamespaces.ModelVersion1)]
    public sealed class GPParameterEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Model.ArcGIS.Tasks.GPParameterEventArgs" /> class.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        public GPParameterEventArgs(GPParameter parameter)
        {
            this.Parameter = parameter;
        }

        /// <summary>
        /// Gets the parameter.
        /// </summary>
        public GPParameter Parameter { get; private set; }
    }
}
