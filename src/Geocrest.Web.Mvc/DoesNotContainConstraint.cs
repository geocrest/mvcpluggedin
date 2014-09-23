namespace Geocrest.Web.Mvc
{
    using System.Linq;
    using System.Web.Routing;
    /// <summary>
    /// Specifies an MVC route contraint that should exclude routes that have parameter values containing
    /// the specified values.
    /// </summary>
    public class DoesNotContainConstraint : IRouteConstraint
    {
        private string[] _matches = new string[]{};
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Web.Mvc.DoesNotContainConstraint"/> class.
        /// </summary>
        /// <param name="values">An array of <see cref="T:System.String"/> to exclude if a route parameter value contains them.</param>
        public DoesNotContainConstraint(params string[] values)
        {
            _matches = values.Select(x => x.ToLower()).ToArray();
        }
        #region IRouteConstraint Members

        /// <summary>
        /// Determines whether the URL parameter contains a valid value for this constraint.
        /// </summary>
        /// <param name="httpContext">An object that encapsulates information about the HTTP request.</param>
        /// <param name="route">The object that this constraint belongs to.</param>
        /// <param name="parameterName">The name of the parameter that is being checked.</param>
        /// <param name="values">An object that contains the parameters for the URL.</param>
        /// <param name="routeDirection">An object that indicates whether the constraint check is being performed when an incoming request is being handled or when a URL is being generated.</param>
        /// <returns>
        /// true if the URL parameter contains a valid value; otherwise, false.
        /// </returns>
        public bool Match(System.Web.HttpContextBase httpContext, Route route, string parameterName, 
            RouteValueDictionary values, RouteDirection routeDirection)
        {
            return (!_matches.Contains(values[parameterName].ToString().ToLower()));
        }

        #endregion
    }
}
