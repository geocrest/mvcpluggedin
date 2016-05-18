namespace Geocrest.Model.Extensions
{
    using System;
    using System.Collections.Generic;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    /// <summary>
    /// Provides an explicit override to the <see cref="T:Newtonsoft.Json.Serialization.DefaultContractResolver"/> such 
    /// that all public members are serialized by default and are only excluded if decorated with <see cref="T:Newtonsoft.Json.JsonIgnoreAttribute"/> 
    /// or <see cref="T:System.NonSerializedAttribute"/> attributes.
    /// </summary>
    /// <remarks>
    /// Typically, if a class is decorated with <see cref="T:System.Runtime.Serialization.DataContractAttribute"/> then 
    /// serialization only occurs if a member has a <see cref="T:System.Runtime.Serialization.DataMemberAttribute"/>. This is the 
    /// <see cref="F:Newtonsoft.Json.MemberSerialization.OptIn"/> behavior. This class forces the use of the 
    /// <see cref="F:Newtonsoft.Json.MemberSerialization.OptOut"/> behavior regardless of whether the class is marked 
    /// with the <see cref="T:System.Runtime.DataContractAttribute"/> attribute.
    /// </remarks>
    public class OptOutContractResolver : DefaultContractResolver
    {
        /// <summary>
        /// Creates properties for the given <see cref="T:Newtonsoft.Json.Serialization.JsonContract" />.
        /// </summary>
        /// <param name="type">The type to create properties for.</param>
        /// <param name="memberSerialization">The member serialization mode for the type.</param>
        /// <returns>
        /// Properties for the given <see cref="T:Newtonsoft.Json.Serialization.JsonContract" />.
        /// </returns>
        /// <remarks>
        /// This override forces the use of <see cref="F:Newtonsoft.Json.MemberSerialization.OptOut"/> regardless of the input value.
        /// </remarks>
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            var list = base.CreateProperties(type, MemberSerialization.OptOut);
            return list;
        }
    }
}
