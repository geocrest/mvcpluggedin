namespace Geocrest.Web.Mvc.Documentation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http.Controllers;
    using System.Web.Http.ModelBinding;
    using System.Web.Http.ValueProviders;
    using System.Web.Http.ValueProviders.Providers;
    using Geocrest.Web.Infrastructure;
    using Geocrest.Web.Mvc.Controllers;
    /// <summary>
    /// Provides extension methods for use in controller selection.
    /// </summary>
    internal static class HttpParameterBindingExtensions
    {
        /// <summary>
        /// Determines if a parameter will be read from the URI.
        /// </summary>
        /// <param name="parameterBinding">The parameter binding.</param>
        public static bool WillReadUri(this HttpParameterBinding parameterBinding)
        {
            if (parameterBinding == null) return false;

            IValueProviderParameterBinding valueProviderParameterBinding = parameterBinding as IValueProviderParameterBinding;
            if (valueProviderParameterBinding != null)
            {
                IEnumerable<ValueProviderFactory> valueProviderFactories = valueProviderParameterBinding.ValueProviderFactories;
                if (valueProviderFactories.Any() && valueProviderFactories.All(factory => factory is QueryStringValueProviderFactory || factory is RouteDataValueProviderFactory))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Extracts the version number from a controller's full name.
        /// </summary>
        /// <param name="controllerDescriptor">The controller descriptor.</param>
        public static string Version(this HttpControllerDescriptor controllerDescriptor)
        {
            string fullName = controllerDescriptor.ControllerType.FullName;
            fullName = fullName.Substring(0, fullName.Length - VersionedControllerSelector.ControllerSuffix.Length);

            // split by dot and find version
            string[] nameSplit = fullName.Split('.');

            string name = nameSplit[nameSplit.Length - 1]; // same as Type.Name
            string version = null;

            for (int i = nameSplit.Length - 2; i >= 0; i--)
            {
                string namePart = nameSplit[i];
                if (!namePart.StartsWith(VersionedControllerSelector.VersionPrefix, StringComparison.InvariantCultureIgnoreCase))
                {
                    continue;
                }

                string versionNumberStr = namePart.Substring(VersionedControllerSelector.VersionPrefix.Length);
                versionNumberStr = versionNumberStr.Replace("_", ".");
                if (versionNumberStr.IsValidVersionNumber())
                {
                    version = versionNumberStr;
                    break;
                }
            }
            return version;
        }
    }
}
