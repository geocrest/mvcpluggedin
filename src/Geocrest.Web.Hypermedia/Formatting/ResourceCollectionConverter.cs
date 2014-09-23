
namespace Geocrest.Web.Hypermedia.Formatting
{
    using Geocrest.Model;
    using Newtonsoft.Json;
    using System;
    using System.Collections;
    /// <summary>
    /// Provides methods for converting collections of entities formatted with 
    /// Hypermedia Application Language to and from JSON. 
    /// </summary>
    public class ResourceCollectionConverter : ResourceConverter
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
            return typeof(IHalResourceCollection).IsAssignableFrom(objectType) || 
                (objectType.IsGenericType && typeof(IEnumerable).IsAssignableFrom(objectType) &&
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
        /// <exception cref="T:System.NotImplementedException"></exception>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var list = (IHalResourceCollection)value;

            // Open the object
            writer.WriteStartObject();

            // Write the links collection into the reserved _links JSON property        
            WriteLinks(writer, list, serializer);

            // Write each non-HAL property (i.e. Resource State)
            WriteNonHalProperties(writer, list, serializer);

            writer.WritePropertyName("_embedded");
            writer.WriteStartObject();
            writer.WritePropertyName(list.Rel);
            writer.WriteStartArray();
            foreach (IHalResource halResource in list)
            {
                serializer.Serialize(writer, halResource);
            }
            writer.WriteEndArray();
            writer.WriteEndObject();
            
            // End the object
            writer.WriteEndObject();
        }
    }
}