namespace Geocrest
{
    /// <summary>
    /// Provides XML namespaces used throughout the framework.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This class should be added as a linked class (shared from one solution file). By convention it is added to the project Properties folder. 
    /// Scope is internal to prevent name collisions across the solution.
    /// </para>
    /// </remarks>
    internal static class XmlNamespaces
    {
        #region Data Contracts
        /// <summary>
        /// The XML namespace for enterprise model data contracts, current version.
        /// </summary>
        internal const string CurrentModelVersion = ModelVersion1;
        /// <summary>
        /// The XML namespace for enterprise model data contracts, version 1.
        /// This value is <i>http://www.geocrest.co/gis/model/1</i>.
        /// </summary>
        internal const string ModelVersion1 = "http://www.geocrest.co/gis/model/1";      

        /// <summary>
        /// The XML namespace for infrastructure data contracts, version 1.
        /// These contracts are provided for internal consumption of GIS services.
        /// This value is <i>http://www.geocrest.co/gis/infrastructure/1</i>.
        /// </summary>
        internal const string InfrastructureVersion1 = "http://www.geocrest.co/gis/infrastructure/1";
        #endregion

        #region Common Binding, Service Contracts, and Service Behaviors (these must match)
        /// <summary>
        /// The XML namespace for all service contracts and behaviors defined in API modules, version 1. 
        /// This namespace is also used in the API service route binding set programmatically in the 
        /// <see cref="N:Geocrest.Wcf.Business"/> library.
        /// This value is <i>http://www.geocrest.co/gis/api/1</i>.
        /// </summary>
        /// <remarks>
        /// Each WCF service must have a common XML namespace for the binding, the contract, and the implementation.
        /// See class-level remarks for more on this topic.
        /// </remarks>
        internal const string ApiVersion1 = "http://www.geocrest.co/gis/api/1";
        #endregion
    }
}
