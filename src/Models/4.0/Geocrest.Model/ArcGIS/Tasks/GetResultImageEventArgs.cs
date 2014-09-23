namespace Geocrest.Model.ArcGIS.Tasks
{
    using System;
    using Geocrest.Model.ArcGIS;

    /// <summary>
    /// Provides the map image returned from a request to an asynchronous geoprocessing task that returns map images.
    /// </summary>
    public sealed class GetResultImageEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the map image.
        /// </summary>
        /// <value>
        /// The map image.
        /// </value>
        public MapImage MapImage { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Model.ArcGIS.Tasks.GetResultImageEventArgs" /> class.
        /// </summary>
        /// <param name="mapimage">The mapimage.</param>
        public GetResultImageEventArgs(MapImage mapimage)
        {
            this.MapImage = mapimage;
        }
    }
}
