namespace Geocrest.Web.Mvc
{
    using System;
    using System.Linq;
    using System.Web;
    using System.Web.Routing;

    /// <summary>
    /// Specifies an MVC route contraint that should exclude routes that have parameter values starting
    /// with the specified values.
    /// </summary>
    public class DoesNotStartWithConstraint : IRouteConstraint
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Web.Mvc.DoesNotStartWithConstraint" /> class.
        /// </summary>
        /// <param name="values">The values.</param>
        public DoesNotStartWithConstraint(params string[] values)
        {
            _values = values;
        }

        private string[] _values;

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
        public bool Match(HttpContextBase httpContext, Route route,string parameterName,
          RouteValueDictionary values,RouteDirection routeDirection)
        {
            string value = values[parameterName].ToString();
            //return !_values.Contains(value, StringComparer.CurrentCultureIgnoreCase);
            return !_values.Any(x => value.ToLower().StartsWith(x.ToLower()));
        }
    }
}
