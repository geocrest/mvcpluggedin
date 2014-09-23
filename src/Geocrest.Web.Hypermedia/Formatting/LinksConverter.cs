
namespace Geocrest.Web.Hypermedia.Formatting
{
    using System;
    using System.Collections.Generic;
    using Newtonsoft.Json;
    using Geocrest.Model;

    /// <summary>
    /// Provides methods for converting links from entities formatted with 
    /// Hypermedia Application Language to and from JSON. 
    /// </summary>
    public class LinksConverter :JsonConverter
    {
        /// <summary>
        /// Determines whether this instance can convert the specified object type.
        /// </summary>
        /// <param name="objectType">Type of the object.</param>
        /// <returns>
        /// <b>true</b>, if this instance can convert the specified object type; otherwise, <b>false</b>.
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public override bool CanConvert(Type objectType)
        {
            return typeof(IList<Link>).IsAssignableFrom(objectType);
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
        /// <exception cref="System.NotImplementedException"></exception>
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
        /// <exception cref="System.NotImplementedException"></exception>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var links = (IList<Link>)value;
            writer.WriteStartObject();

            foreach (var link in links)
            {
                writer.WritePropertyName(link.Rel);
                writer.WriteStartObject();
                writer.WritePropertyName("href");
                writer.WriteValue(link.HRef);

                if (link.Templated)
                {
                    writer.WritePropertyName("isTemplated");
                    writer.WriteValue(true);
                }

                writer.WriteEndObject();
            }
            writer.WriteEndObject();
        }
    }
}
