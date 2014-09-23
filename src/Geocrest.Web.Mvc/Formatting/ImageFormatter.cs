namespace Geocrest.Web.Mvc.Formatting
{
    using System;
    using System.IO;
    using System.Net.Http.Formatting;
    using System.Net.Http.Headers;

    /// <summary>
    /// A formatter for writing image stream's to the output response.
    /// </summary>
    public class ImageFormatter : BufferedMediaTypeFormatter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Web.Mvc.Formatting.ImageFormatter" /> class.
        /// </summary>
        public ImageFormatter()
        {
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("image/png"));
            MediaTypeMappings.Add(new QueryStringMapping(BaseApplication.FormatParameter, "image", "image/png"));
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
            return false;
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
            return type == typeof(Stream);
        }
        /// <summary>
        /// Writes synchronously to the buffered stream.
        /// </summary>
        /// <param name="type">The type of the object to serialize.</param>
        /// <param name="value">The object value to write. Can be null.</param>
        /// <param name="writeStream">The stream to which to write.</param>
        /// <param name="content">The <see cref="T:System.Net.Http.HttpContent" />, if available. Can be null.</param>
        public override void WriteToStream(Type type, object value, Stream writeStream, System.Net.Http.HttpContent content)
        {
            using (writeStream)
            {
                ((Stream)value).CopyTo(writeStream);
            }
        }
    }
}
