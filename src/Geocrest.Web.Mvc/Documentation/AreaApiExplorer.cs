namespace Geocrest.Web.Mvc.Documentation
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Web.Http;
    using System.Web.Http.Controllers;
    using System.Web.Http.Description;
    using System.Web.Http.Dispatcher;
    using System.Web.Http.Routing;
    using Geocrest.Web.Infrastructure;
    /// <summary>
    /// Explores the URI space of the service based on routes, controllers and actions available in the system.
    /// This class provides the unique ability to explore specific areas within an application.
    /// </summary>
    public class AreaApiExplorer : ApiExplorer
    {
        private const string ControllerVariableName = "controller";
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Web.Mvc.Documentation.AreaApiExplorer" /> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public AreaApiExplorer(HttpConfiguration configuration) : base(configuration) { }
        /// <summary>
        /// Gets or sets the area within which to explore. If this property is not set,
        /// the entire space will be searched.
        /// </summary>
        /// <value>
        /// The area name.
        /// </value>
        public string Area { get; set; }
        /// <summary>
        /// Determines whether the controller should be considered for <see cref="P:System.Web.Http.Description.ApiExplorer.ApiDescriptions" /> generation. 
        /// Called when initializing the <see cref="P:System.Web.Http.Description.ApiExplorer.ApiDescriptions" />.
        /// </summary>
        /// <param name="controllerVariableValue">The controller variable value from the route.</param>
        /// <param name="controllerDescriptor">The controller descriptor.</param>
        /// <param name="route">The route.</param>
        /// <returns>
        /// true if the controller should be considered for <see cref="P:System.Web.Http.Description.ApiExplorer.ApiDescriptions" /> generation, false otherwise.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">controllerDescriptor</exception>
        /// <exception cref="T:System.ArgumentNullException">route</exception>
        public override bool ShouldExploreController(string controllerVariableValue, 
            HttpControllerDescriptor controllerDescriptor, IHttpRoute route)
        {
            //if (string.IsNullOrEmpty(this.Area))
            //    return false;
            Throw.IfArgumentNull(controllerDescriptor, "controllerDescriptor");
            Throw.IfArgumentNull(route, "route");
            
            ApiExplorerSettingsAttribute setting = controllerDescriptor.GetCustomAttributes<ApiExplorerSettingsAttribute>().FirstOrDefault();
            var controllerType = controllerDescriptor.ControllerType.FullName;
            string area = route.Defaults.ContainsKey("area") ? route.Defaults["area"].ToString().ToLower() : string.Empty;
            bool shouldExplore = (setting == null || !setting.IgnoreApi) && 
                (string.IsNullOrEmpty(area) || string.IsNullOrEmpty(this.Area) ||
                area == this.Area.ToLower()) &&
                (controllerType.ToLower().Contains(string.Format(".{0}.", area)) &&
                controllerType.ToLower().EndsWith(string.Format(".{0}{1}", controllerDescriptor.ControllerName.ToLower(), 
                DefaultHttpControllerSelector.ControllerSuffix.ToLower()))) &&
                MatchRegexConstraint(route, ControllerVariableName, controllerVariableValue);
            return shouldExplore;
        }
        private static bool MatchRegexConstraint(IHttpRoute route, string parameterName, string parameterValue)
        {
            IDictionary<string, object> constraints = route.Constraints;
            if (constraints != null)
            {
                object constraint;
                if (constraints.TryGetValue(parameterName, out constraint))
                {
                    // treat the constraint as a string which represents a Regex.
                    // note that we don't support custom constraint (IHttpRouteConstraint) because it might rely on the request and some runtime states
                    string constraintsRule = constraint as string;
                    if (constraintsRule != null)
                    {
                        string constraintsRegEx = "^(" + constraintsRule + ")$";
                        return parameterValue != null && Regex.IsMatch(parameterValue, constraintsRegEx, RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
                    }
                }
            }

            return true;
        }
    }
}
