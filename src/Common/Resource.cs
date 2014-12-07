namespace Geocrest
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;
    using Newtonsoft.Json;
#if REPRESENTATIONS
    using Geocrest.Model;
#endif
    /// <summary>
    /// Represents the top-level base class for all model entities.
    /// </summary>    
    [DataContract(Namespace = Geocrest.XmlNamespaces.ModelVersion1)]
    public abstract class Resource 
#if REPRESENTATIONS
        : IHalResource
#endif
    {        
#if REPRESENTATIONS
        /// <summary>
        /// Gets or sets the links associated with this entity.
        /// </summary>
        [DataMember(Name = "_links", Order = 0, EmitDefaultValue = false)]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IList<Link> Links { get; set; }

        /// <summary>
        /// For identifying how the target URI relates to the resource. The value
        /// of this will either be 'self' or null. 
        /// </summary>
        /// <value>
        /// The relation of the target URI.
        /// </value>        
        public string Rel { get; set; }

        /// <summary>
        /// Gets or sets the URI for this resource.
        /// </summary>
        /// <value>
        /// The URI of this unique resource.
        /// </value>
        public string HRef { get; set; }

        /// <summary>
        /// Adds a link to the entity.
        /// </summary>
        /// <param name="link">The link.</param>
        /// <exception cref="T:System.ArgumentNullException">link</exception>
        public void AddLink(Link link)
        {
            if (link == null) throw new ArgumentNullException("link");
            if (Links == null) Links = new List<Link>();
            if (link is SelfLink)
            {
                this.HRef = link.HRef;
                if (this.GetType().IsGenericType && typeof(IEnumerable).IsAssignableFrom(this.GetType())
                    && typeof(Resource).IsAssignableFrom(this.GetType().GetGenericArguments()[0]))
                {
                    this.Rel = this.GetType().GetGenericArguments()[0].Name;
                }
                else
                {
                    this.Rel = this.GetType().Name;
                }
                if (Links.Count(x => x.Rel == link.Rel) > 0)
                    Links.Remove(Links.First(x => x.Rel == link.Rel));
            }
            if (Links.Count(x => x.Rel == link.Rel && x.HRef == link.HRef) == 0)
                Links.Add(link);
        }

        /// <summary>
        /// Creates a HAL-aware collection of objects.
        /// </summary>ResourceCollection
        /// <param name="values">The values.</param>
        /// <returns>
        /// Returns an instance of <see cref="T:Geocrest.Model.IHalResourceCollection"/>.
        /// </returns>
        /// <exception cref="System.ArgumentException">Input argument 'values' must be a generic enumerable type.</exception>
        public static IHalResourceCollection FromValues(object values)
        {
            var type = values.GetType();
            if (type.IsGenericType && values is IEnumerable)
                type = type.GetGenericArguments()[0];              
            else if (type.HasElementType && values is IEnumerable)
                type = type.GetElementType();            
            else
            {
                throw new ArgumentException("Input argument 'values' must be a generic enumerable type.");
            }
            var collection = typeof(ResourceCollection<>).MakeGenericType(type);
            var instance = (IHalResourceCollection)Activator.CreateInstance(collection);
            foreach (var value in (IEnumerable)values)
            {
                instance.Add((IHalResource)value);
            }
            return instance;
        }      
#endif


        #region JSON Conversion
        /// <summary>
        /// Returns a string that represents this instance as a JSON object.
        /// </summary>
        /// <returns>
        /// A JSON string representing this instance.
        /// </returns>
        public virtual string ToJson()
        {
            JsonSerializerSettings settings = new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.None,
                DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Ignore
            };
            return ToJson(settings, settings.Formatting, null);
        }
        /// <summary>
        /// Returns a string that represents this instance as a JSON object.
        /// </summary>
        /// <param name="settings">The serializer settings to use.</param>
        /// <returns>
        /// A JSON string representing this instance.
        /// </returns>
        public virtual string ToJson(JsonSerializerSettings settings)
        {
            return ToJson(settings, settings.Formatting, null);
        }
        /// <summary>
        /// Returns a string that represents this instance as a JSON object.
        /// </summary>
        /// <param name="converters">The JSON converters to use.</param>
        /// <returns>
        /// A JSON string representing this instance.
        /// </returns>
        public virtual string ToJson(params JsonConverter[] converters)
        {
            return ToJson(null, Formatting.None, converters);
        }
        /// <summary>
        /// Returns a string that represents this instance as a JSON object.
        /// </summary>
        /// <param name="formatting">The formatting to use for indentation.</param>
        /// <returns>
        /// A JSON string representing this instance.
        /// </returns>
        public virtual string ToJson(Formatting formatting)
        {
            return ToJson(null, formatting, null);
        }
        /// <summary>
        /// Returns a string that represents this instance as a JSON object.
        /// </summary>
        /// <param name="settings">The serializer settings to use.</param>
        /// <param name="formatting">The indentation formatting to use.</param>
        /// <param name="converters">The JSON converters to use.</param>
        /// <returns>
        /// A JSON string representing this instance.
        /// </returns>
        private string ToJson(JsonSerializerSettings settings, Formatting formatting = Formatting.None, params JsonConverter[] converters)
        {
            if (settings != null)
            {
                return JsonConvert.SerializeObject(this, formatting, settings);
            }
            else if (converters != null)
            {
                return JsonConvert.SerializeObject(this, formatting, converters);
            }
            else
            {
                return JsonConvert.SerializeObject(this, formatting);
            }
        }
        /// <summary>
        /// Returns an object deserialized from the input JSON string.
        /// </summary>
        /// <typeparam name="T">The type of object being deserialized.</typeparam>
        /// <param name="json">The JSON string containing the objects properties.</param>
        /// <returns>
        /// A .NET object representing the JSON string.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">json parameter is required and must not be empty.</exception>
        public static T FromJson<T>(string json)
        {
            return FromJson<T>(json, new JsonConverter[]{});
        }
        /// <summary>
        /// Returns an object deserialized from the input JSON string.
        /// </summary>
        /// <typeparam name="T">The type of object being deserialized.</typeparam>
        /// <param name="json">The JSON string containing the objects properties.</param>
        /// <param name="settings">The serializer settings to use.</param>
        /// <returns>
        /// A .NET object representing the JSON string.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">json parameter is required and must not be empty.</exception>
        public static T FromJson<T>(string json, JsonSerializerSettings settings)
        {
            return FromJson<T>(json, settings, null);
        }
        /// <summary>
        /// Returns an object deserialized from the input JSON string.
        /// </summary>
        /// <typeparam name="T">The type of object being deserialized.</typeparam>
        /// <param name="json">The JSON string containing the objects properties.</param>
        /// <param name="converters">The JSON converters to use during deserialization.</param>
        /// <returns>
        /// A .NET object representing the JSON string.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">json parameter is required and must not be empty.</exception>
        public static T FromJson<T>(string json, params JsonConverter[] converters)
        {
            return FromJson<T>(json, null, converters);
        }
        /// <summary>
        /// Returns an object deserialized from the input JSON string.
        /// </summary>
        /// <typeparam name="T">The type of object being deserialized.</typeparam>
        /// <param name="json">The JSON string containing the objects properties.</param>
        /// <param name="settings">The serializer settings to use.</param>
        /// <param name="converters">The JSON converters to use during deserialization.</param>
        /// <returns>
        /// A .NET object representing the JSON string.
        /// </returns>
        /// <exception cref="T:System.InvalidOperationException">Method can only use one of the following: settings or converters.</exception>
        /// <exception cref="T:System.ArgumentNullException">json parameter is required and must not be empty.</exception>
        private static T FromJson<T>(string json, JsonSerializerSettings settings, params JsonConverter[] converters)
        {
            if (!string.IsNullOrEmpty(json))
            {
                if (settings != null && converters != null)
                    throw new InvalidOperationException("Method can only use one of the following: settings or converters.");
                return settings != null ?
                    JsonConvert.DeserializeObject<T>(json, settings) :
                    converters != null ?
                    JsonConvert.DeserializeObject<T>(json, converters) :
                    JsonConvert.DeserializeObject<T>(json);
            }
            else
            {
                throw new ArgumentNullException("json parameter is required and must not be empty.");
            }
        }
        #endregion
    }
}
