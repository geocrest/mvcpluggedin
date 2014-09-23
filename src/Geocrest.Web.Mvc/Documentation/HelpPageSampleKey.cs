namespace Geocrest.Web.Mvc.Documentation
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http.Headers;
    using Geocrest.Web.Infrastructure;

    /// <summary>
    /// This is used to identify the place where the sample should be applied.
    /// </summary>
    public class HelpPageSampleKey
    {
        /// <summary>
        /// Creates a new <see cref="T:Geocrest.Web.Mvc.Documentation.HelpPageSampleKey"/> 
        /// based on media type and CLR type.
        /// </summary>
        /// <param name="mediaType">The media type.</param>
        /// <param name="type">The CLR type.</param>
        /// <exception cref="T:System.ArgumentNullException">mediaType</exception>
        /// <exception cref="T:System.ArgumentNullException">type</exception>
        public HelpPageSampleKey(MediaTypeHeaderValue mediaType, Type type)
        {
            Throw.IfArgumentNull(mediaType, "mediaType");
            Throw.IfArgumentNull(type, "type");
            ControllerName = String.Empty;
            ActionName = String.Empty;
            ParameterNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            ParameterType = type;
            MediaType = mediaType;
        }

        /// <summary>
        /// Creates a new <see cref="T:Geocrest.Web.Mvc.Documentation.HelpPageSampleKey"/> 
        /// based on <see cref="T:Geocrest.Web.Mvc.Documentation.SampleDirection"/>, 
        /// controller name, action name and parameter names.
        /// </summary>
        /// <param name="sampleDirection">The <see cref="T:Geocrest.Web.Mvc.Documentation.SampleDirection"/>.</param>
        /// <param name="controllerName">Name of the controller.</param>
        /// <param name="actionName">Name of the action.</param>
        /// <param name="parameterNames">The parameter names.</param>
        /// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">sampleDirection</exception>
        /// <exception cref="T:System.ArgumentNullException">controllerName</exception>
        /// <exception cref="T:System.ArgumentNullException">actionName</exception>
        /// <exception cref="T:System.ArgumentNullException">parameterNames</exception>
        public HelpPageSampleKey(SampleDirection sampleDirection, string controllerName, string actionName, IEnumerable<string> parameterNames)
        {
            Throw.IfEnumNotDefined<SampleDirection>(sampleDirection, "sampleDirection");
            Throw.IfArgumentNullOrEmpty(controllerName, "controllerName");
            Throw.IfArgumentNullOrEmpty(actionName, "actionName");
            Throw.IfArgumentNull(parameterNames, "parameterNames");           
            ControllerName = controllerName;
            ActionName = actionName;
            ParameterNames = new HashSet<string>(parameterNames, StringComparer.OrdinalIgnoreCase);
            SampleDirection = sampleDirection;
        }

        /// <summary>
        /// Creates a new <see cref="T:Geocrest.Web.Mvc.Documentation.HelpPageSampleKey"/> 
        /// based on media type, <see cref="T:Geocrest.Web.Mvc.Documentation.SampleDirection"/>, 
        /// controller name, action name and parameter names.
        /// </summary>
        /// <param name="mediaType">The media type.</param>
        /// <param name="sampleDirection">The <see cref="T:Geocrest.Web.Mvc.Documentation.SampleDirection"/>.</param>
        /// <param name="controllerName">Name of the controller.</param>
        /// <param name="actionName">Name of the action.</param>
        /// <param name="parameterNames">The parameter names.</param>
        /// <exception cref="T:System.ArgumentNullException">mediaType</exception>
        /// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">sampleDirection</exception>
        /// <exception cref="T:System.ArgumentNullException">controllerName</exception>
        /// <exception cref="T:System.ArgumentNullException">actionName</exception>
        /// <exception cref="T:System.ArgumentNullException">parameterNames</exception>
        public HelpPageSampleKey(MediaTypeHeaderValue mediaType, SampleDirection sampleDirection, string controllerName, string actionName, IEnumerable<string> parameterNames)
        {
            Throw.IfEnumNotDefined<SampleDirection>(sampleDirection, "sampleDirection");
            Throw.IfArgumentNull(mediaType, "mediaType");
            Throw.IfArgumentNullOrEmpty(controllerName, "controllerName");
            Throw.IfArgumentNullOrEmpty(actionName, "actionName");
            Throw.IfArgumentNull(parameterNames, "parameterNames");  
            ControllerName = controllerName;
            ActionName = actionName;
            MediaType = mediaType;
            ParameterNames = new HashSet<string>(parameterNames, StringComparer.OrdinalIgnoreCase);
            SampleDirection = sampleDirection;
        }
        /// <summary>
        /// Creates a new <see cref="T:Geocrest.Web.Mvc.Documentation.HelpPageSampleKey" /> 
        /// based on controller name, action name and parameter names.
        /// </summary>
        /// <param name="controllerName">Name of the controller.</param>
        /// <param name="actionName">Name of the action.</param>
        /// <param name="parameterNames">The parameter names.</param>
        /// <exception cref="T:System.ArgumentNullException">controllerName</exception>
        /// <exception cref="T:System.ArgumentNullException">actionName</exception>
        /// <exception cref="T:System.ArgumentNullException">parameterNames</exception>
        public HelpPageSampleKey(string controllerName, string actionName, IEnumerable<string> parameterNames)
        {
            Throw.IfArgumentNullOrEmpty(controllerName, "controllerName");
            Throw.IfArgumentNullOrEmpty(actionName, "actionName");
            Throw.IfArgumentNull(parameterNames, "parameterNames");
            ControllerName = controllerName;
            ActionName = actionName;
            ParameterNames = new HashSet<string>(parameterNames, StringComparer.OrdinalIgnoreCase);
        }
        /// <summary>
        /// Gets the name of the controller.
        /// </summary>
        /// <value>
        /// The name of the controller.
        /// </value>
        public string ControllerName { get; private set; }

        /// <summary>
        /// Gets the name of the action.
        /// </summary>
        /// <value>
        /// The name of the action.
        /// </value>
        public string ActionName { get; private set; }

        /// <summary>
        /// Gets the media type.
        /// </summary>
        /// <value>
        /// The media type.
        /// </value>
        public MediaTypeHeaderValue MediaType { get; private set; }

        /// <summary>
        /// Gets the parameter names.
        /// </summary>
        public HashSet<string> ParameterNames { get; private set; }

        /// <summary>
        /// Gets the type of parameter.
        /// </summary>
        /// <value>
        /// The type of parameter.
        /// </value>
        public Type ParameterType { get; private set; }

        /// <summary>
        /// Gets the <see cref="T:Geocrest.Web.Mvc.Documentation.SampleDirection"/>, if set.
        /// </summary>
        public SampleDirection? SampleDirection { get; private set; }

        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object" /> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="T:System.Object" /> to compare with this instance.</param>
        /// <returns>
        /// <b>true</b>, if the specified <see cref="T:System.Object" /> is equal to this instance; otherwise, <b>false</b>.
        /// </returns>
        public override bool Equals(object obj)
        {
            HelpPageSampleKey otherKey = obj as HelpPageSampleKey;
            if (otherKey == null)
            {
                return false;
            }

            return String.Equals(ControllerName, otherKey.ControllerName, StringComparison.OrdinalIgnoreCase) &&
                String.Equals(ActionName, otherKey.ActionName, StringComparison.OrdinalIgnoreCase) &&
                (MediaType == otherKey.MediaType || (MediaType != null && MediaType.Equals(otherKey.MediaType))) &&
                ParameterType == otherKey.ParameterType &&
                SampleDirection == otherKey.SampleDirection &&
                ParameterNames.SetEquals(otherKey.ParameterNames);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            int hashCode = ControllerName.ToUpperInvariant().GetHashCode() ^ ActionName.ToUpperInvariant().GetHashCode();
            if (MediaType != null)
            {
                hashCode ^= MediaType.GetHashCode();
            }
            if (SampleDirection != null)
            {
                hashCode ^= SampleDirection.GetHashCode();
            }
            if (ParameterType != null)
            {
                hashCode ^= ParameterType.GetHashCode();
            }
            foreach (string parameterName in ParameterNames)
            {
                hashCode ^= parameterName.ToUpperInvariant().GetHashCode();
            }

            return hashCode;
        }
    }
}
