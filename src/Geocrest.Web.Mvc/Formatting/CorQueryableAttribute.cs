
namespace Geocrest.Web.Mvc.Formatting
{
    using Microsoft.OData.Edm;
    using System.Net.Http;
    using System.Web.Http;
    using System.Web.OData;
    using System.Web.OData.Extensions;

    /// <summary>
    /// This class defines an attribute that can be applied to an action to enable 
    /// querying using the OData query syntax. Additionally, the class enables the 
    /// web framework to access the underlying EDM model throught the request object.
    /// To avoid processing unexpected or malicious queries, use the validation settings 
    /// on System.Web.Http.QueryableAttribute to validate incoming queries. 
    /// For more information, visit <see href="http://go.microsoft.com/fwlink/?LinkId=279712"/>.
    /// </summary>
    public class CorQueryableAttribute : EnableQueryAttribute //QueryableAttribute (WebApi2 Upgrade)
    {
        /// <summary>
        /// Gets the EDM model for the given type and request.
        /// </summary>
        /// <param name="elementClrType">The CLR type for which to retrieve a model.</param>
        /// <param name="request">The request message used to hold a reference to the model.</param>
        /// <param name="actionDescriptor">The action descriptor for the action being queried.</param>
        /// <returns>
        /// The EDM model for the given type and request.
        /// </returns>
        /// <remarks>This override allows an <see cref="T:System.Web.Http.ApiController"/> that is not a type of
        /// <see cref="T:System.Web.Http.OData.ODataController"/> to set the <see cref="T:Microsoft.Data.Edm.IEdmModel"/>
        /// associated with an OData request as a property on the request. Ultimately, this allows
        /// us to call <see cref="M:System.Net.Http.ODataHttpRequestMessageExtensions.GetNextPageLink(System.Net.Http.HttpRequestMessage)"/></remarks>
        public override IEdmModel GetModel(System.Type elementClrType, HttpRequestMessage request, System.Web.Http.Controllers.HttpActionDescriptor actionDescriptor)
        {
            var model = base.GetModel(elementClrType, request, actionDescriptor);

            //
            // WebApi2 Upgrade: 
            //
            //original: request.SetEdmModel(model);
            //updated might be something like: request.ODataProperties().Model....
            //
            // TODO: Evaluate if the above or any of this override is really needed.
            //

            return model;
        }                
    }
}