namespace Geocrest.Model
{
    using System;
    using System.Collections.Generic;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;
    using System.Linq;
    /// <summary>
    /// Use this class to exclude certain properties from JSON serialization.
    /// </summary>
    public class OptionalPropertiesContractResolver : DefaultContractResolver
    {
        //only the properties whose names are included in this list will be serialized
        List<string> _excludedProperties;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Model.OptionalPropertiesContractResolver"/> class.
        /// </summary>
        /// <param name="excludedProperties">The excluded properties.</param>
        public OptionalPropertiesContractResolver(params string[] excludedProperties)
        {
            _excludedProperties = new List<string>(excludedProperties);
        }

        /// <summary>
        /// Creates properties for the given <see cref="T:Newtonsoft.Json.Serialization.JsonContract" />.
        /// </summary>
        /// <param name="type">The type to create properties for.</param>
        /// <param name="memberSerialization">The member serialization mode for the type.</param>
        /// <returns>
        /// Properties for the given <see cref="T:Newtonsoft.Json.Serialization.JsonContract" />.
        /// </returns>
        /// <remarks>This override allows the creation of the properties to exclude the properties included in the 
        /// constructor for this class.</remarks>
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            return (from prop in base.CreateProperties(type, memberSerialization)
                    where !_excludedProperties.Contains(prop.PropertyName)
                    select prop).ToList();
        }
    }
}
