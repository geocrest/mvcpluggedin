namespace Geocrest.Web.Mvc.Formatting
{
    using System.Net.Http.Formatting;
    using System.Web.Http;
    using Newtonsoft.Json;
    using Geocrest.Web.Infrastructure;
    /// <summary>
    /// A JSON formatter for returning model entities that do not have circular references.
    /// </summary>
    public class JsonReferenceLoopFormatter : JsonMediaTypeFormatter
    {
        private readonly IEntityFormatter formatter;
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Web.Mvc.Formatting.JsonReferenceLoopFormatter"/> class.
        /// </summary>
        /// <param name="formatter">The formatter.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="formatter"/></exception>
        public JsonReferenceLoopFormatter(IEntityFormatter formatter)
        {
            Throw.IfArgumentNull(formatter, "formatter");
            this.formatter = formatter;
            this.SerializerSettings = new JsonSerializerSettings
            {
                Formatting = Newtonsoft.Json.Formatting.Indented,
                DefaultValueHandling = DefaultValueHandling.Include
            };
        }

        /// <summary>
        /// Writes an object of the specified <paramref name="type" /> to the specified <paramref name="writeStream" />. 
        /// This method is called during serialization.
        /// </summary>
        /// <param name="type">The type of object to write.</param>
        /// <param name="value">The object to write.</param>
        /// <param name="writeStream">The <see cref="T:System.IO.Stream" /> to which to write.</param>
        /// <param name="content">The <see cref="T:System.Net.Http.HttpContent" /> where the content is being written.</param>
        /// <param name="transportContext">The <see cref="T:System.Net.TransportContext" />.</param>
        /// <returns>
        /// A <see cref="T:System.Threading.Tasks.Task" /> that will write the value to the stream.
        /// </returns>
        public override System.Threading.Tasks.Task WriteToStreamAsync(System.Type type, object value, System.IO.Stream writeStream, System.Net.Http.HttpContent content, System.Net.TransportContext transportContext)
        {
            object cleaned = value;
            if (type != typeof(HttpError))
                cleaned = this.formatter.Format(value);
            return base.WriteToStreamAsync(type, cleaned, writeStream, content, transportContext);           
        }

        /// <summary>
        /// Gets the formatter used to handle circular references.
        /// </summary>
        /// <value>
        /// The formatter.
        /// </value>
        public IEntityFormatter Formatter { get { return this.formatter; } }
    }
}