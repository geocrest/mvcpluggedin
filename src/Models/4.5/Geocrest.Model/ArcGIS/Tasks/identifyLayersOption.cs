namespace Geocrest.Model.ArcGIS.Tasks
{
    /// <summary>
    /// Available options for identifying map service layers.
    /// </summary>
    public enum IdentifyLayersOption
    {
        /// <summary>
        /// Only the top-most layer at the specified location.
        /// </summary>
        Top,

        /// <summary>
        /// All visible layers at the specified location.
        /// </summary>
        Visible,

        /// <summary>
        /// All layers at the specified location.
        /// </summary>
        All
    }
}
