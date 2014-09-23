namespace Geocrest.Web.Hypermedia.Formatting
{
    using Geocrest.Model;
    using Geocrest.Web.Mvc.Formatting;
    using Newtonsoft.Json;
    using Ninject;
    using System.Collections;
    using System.Net.Http.Formatting;
    using System.Net.Http.Headers;
    /// <summary>
    /// An JSON formatter for returning data model entities as Hypertext Application Language representations.
    /// </summary>
    public class JsonHalFormatter : JsonReferenceLoopFormatter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Web.Hypermedia.Formatting.JsonHalFormatter"/> class.
        /// </summary>
        /// <param name="formatter">The formatter.</param>
        public JsonHalFormatter([Named("GeodataFormatter")]IEntityFormatter formatter) 
            : base(formatter)
        {
            this.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/hal+json"));
            this.AddQueryStringMapping("f", "haljson", "application/hal+json");
            this.SerializerSettings = new JsonSerializerSettings
            {
                Formatting = Newtonsoft.Json.Formatting.Indented,
                DefaultValueHandling = DefaultValueHandling.Include
            };
            this.SerializerSettings.Converters.Add(new ResourceCollectionConverter());
            this.SerializerSettings.Converters.Add(new LinksConverter());
            this.SerializerSettings.Converters.Add(new ResourceConverter());
        }
        /// <summary>
        /// Determines whether this <see cref="T:System.Net.Http.Formatting.JsonMediaTypeFormatter" /> can read objects of the specified <paramref name="type" />.
        /// </summary>
        /// <param name="type">The type of object that will be read.</param>
        /// <returns>
        /// true if objects of this <paramref name="type" /> can be read, otherwise false.
        /// </returns>
        public override bool CanReadType(System.Type type)
        {
            return typeof(IHalResource).IsAssignableFrom(type);
        }
        /// <summary>
        /// Determines whether this <see cref="T:System.Net.Http.Formatting.JsonMediaTypeFormatter" /> can write objects of the specified <paramref name="type" />.
        /// </summary>
        /// <param name="type">The type of object that will be written.</param>
        /// <returns>
        /// true if objects of this <paramref name="type" /> can be written, otherwise false.
        /// </returns>
        public override bool CanWriteType(System.Type type)
        {
            return typeof(IHalResource).IsAssignableFrom(type) || 
                (type.IsGenericType && typeof(IEnumerable).IsAssignableFrom(type)
                && typeof(IHalResource).IsAssignableFrom(type.GetGenericArguments()[0]));
        }
    }
}