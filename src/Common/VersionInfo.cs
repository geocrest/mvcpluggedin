namespace Geocrest
{
    /// <summary>
    /// Provides assembly versioning metadata to be reused amongst the various framework projects.
    /// </summary>
    internal class VersionInfo
    {
        /// <summary>
        /// Prevents a default instance of the <see cref="T:Geocrest.VersionInfo" /> class from being created.
        /// </summary>
        private VersionInfo() { }

        /// <summary>
        /// Specifies the assembly version for use when the .NET runtime loads an assembly. Use a
        /// fixed version for this so developers do not have to rebuild. This is the important one!
        /// </summary>
        /// <remarks>
        /// Use this for the enterprise model framework libraries.</remarks>
        internal const string ModelAssemblyVersion = "3.0.0";
        /// <summary>
        /// Specifies the file version for use in the file system (e.g. Windows Explorer). Does not
        /// affect the loading of the assembly in the runtime. This version is visible if you hover
        /// over the assembly in Windows Explorer. Use number of days since 01/01/2000 for build number 
        /// and the number of seconds since midnight divided by 2 for revision.
        /// </summary>
        /// <remarks>
        /// Use this for the enterprise model framework libraries.
        /// </remarks>
        internal const string ModelFileVersion = "3.0.6573.22612";
        /// <summary>
        /// Specifies the product version to display in the Details tab of Windows Explorer.
        /// </summary>
        /// <remarks>
        /// Use this for the enterprise model framework libraries.
        /// </remarks>
        internal const string ModelProductVersion = "3.0.0";
        /// <summary>
        /// Specifies the assembly version for use when the .NET runtime loads an assembly. Use a
        /// fixed version for this so developers do not have to rebuild. This is the important one!
        /// </summary>
        /// <remarks>
        /// Use this for the core framework libraries.</remarks>
        internal const string GlobalAssemblyVersion = "3.0.0";
        /// <summary>
        /// Specifies the file version for use in the file system (e.g. Windows Explorer). Does not
        /// affect the loading of the assembly in the runtime. This version is visible if you hover
        /// over the assembly in Windows Explorer. Use number of days since 01/01/2000 for build number 
        /// and the number of seconds since midnight divided by 2 for revision.
        /// </summary>
        /// <remarks>
        /// Use this for the core framework libraries.
        /// </remarks>
        internal const string GlobalFileVersion = "3.0.6573.22612";
        /// <summary>
        /// Specifies the product version to display in the Details tab of Windows Explorer.
        /// </summary>
        /// <remarks>
        /// Use this for the core framework libraries.
        /// </remarks>
        internal const string GlobalProductVersion = "3.0.0";
        /// <summary>
        /// Use this for all assemblies throughout the solution.
        /// </summary>
        internal const string CompanyName = "Geocrest Mapping, LLC";
        /// <summary>
        /// Use this for all assemblies throughout the solution.
        /// </summary>
        internal const string Copyright = "Copyright © 2014-2017 Geocrest Mapping, LLC";
    }
}
