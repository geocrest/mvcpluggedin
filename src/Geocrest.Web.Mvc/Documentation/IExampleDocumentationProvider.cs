namespace Geocrest.Web.Mvc.Documentation
{
    using System.Web.Http.Controllers;

    /// <summary>
    /// Provides a single method for returning example code.
    /// </summary>
    public interface IExampleDocumentationProvider
    {
        /// <summary>
        /// Gets example code for a specific method.
        /// </summary>
        /// <param name="actionDescriptor">An <see cref="T:System.Web.Http.Controllers.HttpActionDescriptor">HttpActionDescriptor</see> 
        /// containing information about the method.</param>
        /// <returns>
        /// Returns the content of the <c>example</c> tag from inline XML documentation.
        /// </returns>
        string GetExampleDocumentation(HttpActionDescriptor actionDescriptor);
    }
}
