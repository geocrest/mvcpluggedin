namespace Geocrest.Web.Mvc.Formatting
{
    using System;
    using System.IO;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Formatting;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using Geocrest.Web.Infrastructure;
    /// <summary>
    /// Formatter for serializing and deserializing plain text.
    /// </summary>
    public class PlainTextFormatter:MediaTypeFormatter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Web.Mvc.Formatting.PlainTextFormatter" /> class.
        /// </summary>
        public PlainTextFormatter()
        {
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/plain"));
            MediaTypeMappings.Add(new QueryStringMapping(BaseApplication.FormatParameter, "base64", "text/plain"));
        }

        /// <summary>
        /// Queries whether this <see cref="T:System.Net.Http.Formatting.MediaTypeFormatter" /> can deserializean object of the specified type.
        /// </summary>
        /// <param name="type">The type to deserialize.</param>
        /// <returns>
        /// true if the <see cref="T:System.Net.Http.Formatting.MediaTypeFormatter" /> can deserialize the type; otherwise, false.
        /// </returns>
        public override bool CanReadType(Type type)
        {
            return type == typeof(string);
        }

        /// <summary>
        /// Queries whether this <see cref="T:System.Net.Http.Formatting.MediaTypeFormatter" /> can serializean object of the specified type.
        /// </summary>
        /// <param name="type">The type to serialize.</param>
        /// <returns>
        /// true if the <see cref="T:System.Net.Http.Formatting.MediaTypeFormatter" /> can serialize the type; otherwise, false.
        /// </returns>
        public override bool CanWriteType(Type type)
        {
            return type == typeof(string) || type == typeof(Stream);
        }
        /// <summary>
        /// Asynchronously deserializes an object of the specified type.
        /// </summary>
        /// <param name="type">The type of the object to deserialize.</param>
        /// <param name="readStream">The <see cref="T:System.IO.Stream" /> to read.</param>
        /// <param name="content">The <see cref="T:System.Net.Http.HttpContent" />, if available. It may be null.</param>
        /// <param name="formatterLogger">The <see cref="T:System.Net.Http.Formatting.IFormatterLogger" /> to log events to.</param>
        /// <returns>
        /// A <see cref="T:System.Threading.Tasks.Task" /> whose result will be an object of the given type.
        /// </returns>
        public override Task<object> ReadFromStreamAsync(Type type, Stream readStream, HttpContent content, IFormatterLogger formatterLogger)
        {
            var reader = new StreamReader(readStream);
            string value = reader.ReadToEnd();

            var tcs = new TaskCompletionSource<object>();
            tcs.SetResult(value);
            return tcs.Task;
        }
        /// <summary>
        /// Asynchronously writes an object of the specified type.
        /// </summary>
        /// <param name="type">The type of the object to write.</param>
        /// <param name="value">The object value to write.  It may be null.</param>
        /// <param name="writeStream">The <see cref="T:System.IO.Stream" /> to which to write.</param>
        /// <param name="content">The <see cref="T:System.Net.Http.HttpContent" /> if available. It may be null.</param>
        /// <param name="transportContext">The <see cref="T:System.Net.TransportContext" /> if available. It may be null.</param>
        /// <returns>
        /// A <see cref="T:System.Threading.Tasks.Task" /> that will perform the write.
        /// </returns>
        public override Task WriteToStreamAsync(Type type, object value, Stream writeStream, HttpContent content, TransportContext transportContext)
        {
            var writer = new StreamWriter(writeStream);
            writer.Write(type == typeof(string) ? (string)value : ((Stream)value).ToBase64());
            writer.Flush();

            var tcs = new TaskCompletionSource<object>();
            tcs.SetResult(null);
            return tcs.Task;
        }       
    }
}
