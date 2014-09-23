namespace Geocrest.Data.Contracts.Gis
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Geocrest.Model.ArcGIS;

    /// <summary>
    /// Provides access to feature server operations
    /// </summary>
    public interface IFeatureServer : IArcGISService
    {
        /// <summary>
        /// Gets or sets the layers.
        /// </summary>
        /// <value>
        /// The layers.
        /// </value>
        LayerTableBase[] LayerInfos { get; set; }
        /// <summary>
        /// Gets or sets the table infos.
        /// </summary>
        /// <value>
        /// The table infos.
        /// </value>
        LayerTableBase[] TableInfos { get; set; }
    }
}
