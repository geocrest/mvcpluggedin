namespace Geocrest.Web.Hypermedia.Formatting
{
    using Geocrest.Model;
    using Geocrest.Web.Infrastructure;
    using Newtonsoft.Json;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.Serialization;

    /// <summary>
    /// Provides methods for converting single entities formatted as Hypermedia Application Language to and from JSON. 
    /// </summary>
    public class ResourceConverter : JsonConverter
    {
        /// <summary>
        /// Determines whether this instance can convert the specified object type.
        /// </summary>
        /// <param name="objectType">Type of the object.</param>
        /// <returns>
        /// <b>true</b>, if this instance can convert the specified object type; otherwise, <b>false</b>.
        /// </returns>
        /// <exception cref="T:System.NotImplementedException"></exception>
        public override bool CanConvert(Type objectType)
        {
            return typeof(IHalResource).IsAssignableFrom(objectType) &&
                !(objectType.IsGenericType && typeof(IEnumerable).IsAssignableFrom(objectType) &&
                typeof(IHalResource).IsAssignableFrom(objectType.GetGenericArguments()[0]));
        }

        /// <summary>
        /// Reads the JSON representation of the object.
        /// </summary>
        /// <param name="reader">The <see cref="T:Newtonsoft.Json.JsonReader" /> to read from.</param>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="existingValue">The existing value of object being read.</param>
        /// <param name="serializer">The calling serializer.</param>
        /// <returns>
        /// The object value.
        /// </returns>
        /// <exception cref="T:System.NotImplementedException"></exception>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return reader.Value;
        }

        /// <summary>
        /// Writes the JSON representation of the object.
        /// </summary>
        /// <param name="writer">The <see cref="T:Newtonsoft.Json.JsonWriter" /> to write to.</param>
        /// <param name="value">The value.</param>
        /// <param name="serializer">The calling serializer.</param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var resource = (IHalResource)value;
            SerializeInnerResource(writer, resource, serializer);
        }
        /// <summary>
        /// Writes the properties that are not HAL properties (i.e. are not of the type 
        /// <see cref="T:Geocrest.Model.IHalResource"/> or 
        /// <see cref="T:Geocrest.Model.IHalResourceCollection"/>) 
        /// to the output JSON. This is the resource state.
        /// </summary>
        /// <param name="writer">The <see cref="T:Newtonsoft.Json.JsonWriter" /> to write to.</param>
        /// <param name="value">The value.</param>
        /// <param name="serializer">The calling serializer.</param>
        protected virtual void WriteNonHalProperties(JsonWriter writer, IHalResource value, JsonSerializer serializer)
        {
            Throw.IfArgumentNull(writer, "writer");
            Throw.IfArgumentNull(value, "value");
            Throw.IfArgumentNull(serializer, "serializer");

            // exclude HalResource types and IList<Link> type
            var properties = value.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(x => !typeof(IHalResource).IsAssignableFrom(x.PropertyType) && !typeof(IList<Link>)
                .IsAssignableFrom(x.PropertyType));

            // serialize each property and it's value
            foreach (var property in properties)
            {
                var propertyValue = property.GetValue(value, null);

                // class members must opt in to be included in the ouput serialization by having a 
                // [DataMember] on the property
                var dataMember = property.GetCustomAttributes(typeof(DataMemberAttribute), false).Length == 0 ? false : true;

                // classes can exclude serialization of properties that contain null/default 
                // values by adding [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
                var emitDefault = property.GetCustomAttributes(typeof(JsonPropertyAttribute), false).Length == 0 ?
                    NullValueHandling.Include : // <-default behavior
                    ((JsonPropertyAttribute)property.GetCustomAttributes(typeof(JsonPropertyAttribute), false)
                    .Single()).NullValueHandling;

                // classes can also exclude serialization of properties regardless of value
                // by adding [JsonIgnore] to the property
                var ignore = property.GetCustomAttributes(typeof(JsonIgnoreAttribute), false).Length == 0 ? false : true;

                // and serialize
                if (dataMember && !ignore && emitDefault == NullValueHandling.Include)
                {
                    writer.WritePropertyName(GetDataMemberName(property));
                    serializer.Serialize(writer, propertyValue);
                }
            }
        }

        /// <summary>
        /// Writes each property of the type <see cref="T:Geocrest.Model.IHalResource"/> or 
        /// <see cref="T:Geocrest.Model.IHalResourceCollection"/> into the reserved 
        /// <i>_embedded</i> JSON property
        /// http://tools.ietf.org/html/draft-kelly-json-hal-06#section-4.1.2
        /// </summary>
        /// <param name="writer">The <see cref="T:Newtonsoft.Json.JsonWriter" /> to write to.</param>
        /// <param name="value">The value.</param>
        /// <param name="serializer">The calling serializer.</param>
        protected virtual void WriteHalProperties(JsonWriter writer, IHalResource value, JsonSerializer serializer)
        {
            Throw.IfArgumentNull(writer, "writer");
            Throw.IfArgumentNull(value, "value");
            Throw.IfArgumentNull(serializer, "serializer");
            writer.WritePropertyName("_embedded");
            writer.WriteStartObject();

            // add all HalResources to a dictionary for embedding.
            var properties = value.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(x => typeof(IHalResource).IsAssignableFrom(x.PropertyType));
            foreach (var property in properties)
            {
                writer.WritePropertyName(GetDataMemberName(property));
                var item = property.GetValue(value, null);
                serializer.Serialize(writer, item);
            }

            writer.WriteEndObject();
        }

        /// <summary>
        /// Write the HAL links collection into the reserved <i>_links</i> JSON property
        /// http://tools.ietf.org/html/draft-kelly-json-hal-06#section-4.1.1
        /// </summary>
        /// <param name="writer">The <see cref="T:Newtonsoft.Json.JsonWriter" /> to write to.</param>
        /// <param name="value">The value.</param>
        /// <param name="serializer">The calling serializer.</param>
        protected virtual void WriteLinks(JsonWriter writer, IHalResource value, JsonSerializer serializer)
        {
            Throw.IfArgumentNull(writer, "writer");
            Throw.IfArgumentNull(serializer, "serializer");

            var linksConverter = serializer.Converters.FirstOrDefault(x => x.CanConvert(typeof(IList<Link>)));
            if (linksConverter != null)
            {
                writer.WritePropertyName("_links");
                writer.WriteStartArray();
                if (value != null)
                {
                    linksConverter.WriteJson(writer, value.Links, serializer);
                }
                writer.WriteEndArray();
            }
        }

        /// <summary>
        /// Recursively serializes the HAL resource to the output JSON.
        /// </summary>
        /// <param name="writer">The <see cref="T:Newtonsoft.Json.JsonWriter" /> to write to.</param>
        /// <param name="resource">The resource being written.</param>
        /// <param name="serializer">The calling serializer.</param>
        protected virtual void SerializeInnerResource(JsonWriter writer, IHalResource resource, JsonSerializer serializer)
        {
            if (resource == null) return;

            // Open the object
            writer.WriteStartObject();

            // Write the links collection into the reserved _links JSON property        
            WriteLinks(writer, resource, serializer);

            // Write each non-HAL property (i.e. Resource State)
            WriteNonHalProperties(writer, resource, serializer);

            // Write each HAL property into the reserved _embedded JSON property
            // http://tools.ietf.org/html/draft-kelly-json-hal-06#section-4.1.2
            WriteHalProperties(writer, resource, serializer);

            // End the object
            writer.WriteEndObject();
        }

        private string GetDataMemberName(PropertyInfo property)
        {
            // get DataMember Name attribute if provided
            var dataMember = property.GetCustomAttributes(typeof(DataMemberAttribute), true).Length > 0 ?
                (DataMemberAttribute)property.GetCustomAttributes(typeof(DataMemberAttribute), true)
                .Single() : null;
            var name = dataMember != null && !string.IsNullOrEmpty(dataMember.Name) ? dataMember.Name : property.Name;
            return name;
        }
    }
}
