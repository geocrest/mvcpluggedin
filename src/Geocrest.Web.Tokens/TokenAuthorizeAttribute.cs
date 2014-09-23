namespace Geocrest.Web.Tokens
{
    using System.Linq;
    using System.Net.Http;
    using System.Web.Http;
    using System.Web.Http.Controllers;
    using Geocrest.Web.Infrastructure;
    /// <summary>
    /// Provides token-based authorization for use within an ASP.NET Web API.
    /// </summary>
    public class TokenAuthorizeAttribute : AuthorizeAttribute
    {
        string message = "";        

        /// <summary>
        /// Calls when an action is being authorized.
        /// </summary>
        /// <param name="actionContext">The context.</param>
        /// <exception cref="T:System.ArgumentNullException">actionContext</exception>
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            Throw.IfArgumentNull(actionContext, "actionContext");
            if (AuthorizationDisabled(actionContext) || AuthorizeRequest())
                return;
            this.HandleUnauthorizedRequest(actionContext);
        }
        /// <summary>
        /// Processes requests that fail authorization.
        /// </summary>
        /// <param name="actionContext">The context.</param>
        /// <exception cref="T:System.ArgumentNullException">actionContext</exception>
        protected override void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
            Throw.IfArgumentNull(actionContext, "actionContext");
            actionContext.Response = new HttpResponseMessage
            {
                StatusCode = System.Net.HttpStatusCode.Forbidden,
                RequestMessage = actionContext.ControllerContext.Request,
                Content = new StringContent(string.IsNullOrWhiteSpace(message) ? "Unauthorized": message)
            };
        }
        /// <summary>
        /// Checks to see if the <see cref="T:System.Web.Http.AllowAnonymousAttribute"/>
        /// has been applied to the action.
        /// </summary>
        /// <param name="actionContext">The action context.</param>
        /// <returns>
        /// Returns <c>true</c> if anonymous access should be granted.
        /// </returns>
        private static bool AuthorizationDisabled(HttpActionContext actionContext)
        {
            //support new AllowAnonymousAttribute
            if (!actionContext.ActionDescriptor
                .GetCustomAttributes<AllowAnonymousAttribute>().Any())
                return actionContext.ControllerContext
                    .ControllerDescriptor
                    .GetCustomAttributes<AllowAnonymousAttribute>().Any();
            else
                return true;
        }
        /// <summary>
        /// Returns a <c>true</c> value if the request is authorized.
        /// </summary>
        /// <returns>
        /// Returns an instance of <see cref="System.Boolean"/>.
        /// </returns>
        private bool AuthorizeRequest()
        {
            Token token = new Token();
            message = "";
            if (!token.IsValid)
            {               
                message = "Invalid token";
                return false;
            }
            string[] roles = this.Roles.Split(',')
                .Where(s => !string.IsNullOrWhiteSpace(s.Trim())).ToArray();
            //string[] users = this.Users.Split(',')
            //    .Where(s => !string.IsNullOrWhiteSpace(s.Trim())).ToArray();
            bool authorizedRoles = roles.Any(r => token.GroupName== r);
            //bool authorizedUsers = users.Any(u => u == token.UserName);
            if (!authorizedRoles)
            {
                message = "Unauthorized for the service";
                return false;
            }

            return true;
        }
    }
}
