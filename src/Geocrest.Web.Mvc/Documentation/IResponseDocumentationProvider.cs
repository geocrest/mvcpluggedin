namespace Geocrest.Web.Mvc.Documentation
{
    using System.Web.Http.Controllers;

    /// <summary>
    /// Provides a single method for returning documentation about return values.
    /// </summary>
    public interface IResponseDocumentationProvider
    {
        /// <summary>
        /// Gets the documentation for a specific method's return value.
        /// </summary>
        /// <param name="actionDescriptor">An <see cref="T:System.Web.Http.Controllers.HttpActionDescriptor">HttpActionDescriptor</see> 
        /// containing information about the method.</param>
        /// <returns>
        /// Returns documentation about the return value of the method.
        /// </returns>
        string GetResponseDocumentation(HttpActionDescriptor actionDescriptor);
    }
}
