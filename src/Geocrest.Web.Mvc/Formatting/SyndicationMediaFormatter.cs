namespace Geocrest.Web.Mvc.Formatting
{
    using System;
    using System.IO;
    using System.Net.Http;
    using System.Net.Http.Formatting;
    using System.Net.Http.Headers;
    using System.ServiceModel.Syndication;
    using System.Xml;
    using Geocrest.Web.Infrastructure;
    using System.Linq;
    using Geocrest.Web.Mvc.Syndication;
    /// <summary>
    /// A formatter for syndication feeds (Atom and RSS).
    /// </summary>
    public class SyndicationMediaFormatter : BufferedMediaTypeFormatter
    {
        private const string AtomMediaType = "application/atom+xml";
        private const string RssMediaType = "application/rss+xml";
        private const string JpegMediaType = "image/jpeg";
        private const string PngMediaType = "image/png";
        private const string GifMediaType = "image/gif";
        private const string BmpMediaType = "image/bmp";
        private const string MultipartMediaType = "multipart/form-data";
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Web.Mvc.Formatting.SyndicationMediaFormatter" /> class.
        /// </summary>
        public SyndicationMediaFormatter()
        {
            SupportedMediaTypes.Add(new MediaTypeHeaderValue(AtomMediaType));
            SupportedMediaTypes.Add(new MediaTypeHeaderValue(RssMediaType));
            SupportedMediaTypes.Add(new MediaTypeHeaderValue(JpegMediaType));
            SupportedMediaTypes.Add(new MediaTypeHeaderValue(PngMediaType));
            SupportedMediaTypes.Add(new MediaTypeHeaderValue(GifMediaType));
            SupportedMediaTypes.Add(new MediaTypeHeaderValue(BmpMediaType));
            this.AddQueryStringMapping("f", "atom", AtomMediaType);
            this.AddQueryStringMapping("f", "rss", RssMediaType);
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
            return type.Implements<IPublication>() || type.Implements<IMediaResourceCommand>();
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
            return type.Implements<IPublication>() || type.Implements<IPublicationFeed>() || type.Implements<IMediaResource>();
        }

        /// <summary>
        /// Reads synchronously from the buffered stream.
        /// </summary>
        /// <param name="type">The type of the object to deserialize.</param>
        /// <param name="readStream">The stream from which to read</param>
        /// <param name="content">The <see cref="T:System.Net.Http.HttpContent" />, if available. Can be null.</param>
        /// <param name="formatterLogger">The <see cref="T:System.Net.Http.Formatting.IFormatterLogger" /> to log events to.</param>
        /// <returns>
        /// An object of the given <paramref name="type" />.
        /// </returns>
        public override object ReadFromStream(Type type, Stream readStream, HttpContent content, IFormatterLogger formatterLogger)
        {
            Throw.IfArgumentNull(type, "type");
            Throw.IfArgumentNull(readStream, "readStream");
           
            HttpContentHeaders contentHeaders = content == null ? null : content.Headers;

            // If content length is 0 then return default value for this type
            if (contentHeaders != null && contentHeaders.ContentLength == 0)
            {
                return GetDefaultValueForType(type);
            }

            try
            {
                using (readStream)
                {                    
                    if (type.Implements<IPublication>())
                    {
                        return ReadEntry(type,readStream, content.Headers.ContentType.MediaType);                        
                    }
                    else
                    {                        
                        var command = Activator.CreateInstance(type);
                        ((IMediaResourceCommand)command).ContentType = content.Headers.ContentType.MediaType;
                        ((IMediaResourceCommand)command).Content = ReadFully(readStream);
                        ((IMediaResourceCommand)command).Summary = "Added using Web Api";
                        return command;
                    }
                }
            }
            catch (Exception e)
            {
                if (formatterLogger == null)
                {
                    throw;
                }
                formatterLogger.LogError(String.Empty, e);
                return GetDefaultValueForType(type);
            }
        }

        /// <summary>
        /// Writes synchronously to the buffered stream.
        /// </summary>
        /// <param name="type">The type of the object to serialize.</param>
        /// <param name="value">The object value to write. Can be null.</param>
        /// <param name="writeStream">The stream to which to write.</param>
        /// <param name="content">The <see cref="T:System.Net.Http.HttpContent" />, if available. Can be null.</param>
        public override void WriteToStream(Type type, object value, Stream writeStream, HttpContent content)
        {
            Throw.IfArgumentNull(type, "type");

            using (writeStream)
            {
                if (value is IPublicationFeed)
                {
                    WriteFeed((IPublicationFeed)value, writeStream, content.Headers.ContentType.MediaType);
                }
                else if (value is IPublication)
                {
                    WriteEntry((IPublication)value, writeStream, content.Headers.ContentType.MediaType);
                }
                else
                {
                    WriteMediaEntry((IMediaResource)value, writeStream, content.Headers.ContentType.MediaType);
                }
            }
        }

        /// <summary>
        /// Writes the atom feed to the specified stream.
        /// </summary>
        /// <param name="feed">The publication feed containing the entries.</param>
        /// <param name="writeStream">The output write stream.</param>
        /// <param name="contentType">The media type header requested.</param>
        protected virtual void WriteFeed(IPublicationFeed feed, Stream writeStream, string contentType)
        {
            var syndicationfeed = feed.Syndicate();
            syndicationfeed.Items = feed.Items.Select(x => x.Syndicate());
            var formatter = GetFormatter(contentType, syndicationfeed);
            using (var writer = XmlWriter.Create(writeStream, new XmlWriterSettings() 
            {
                Indent=true
            }))
            {
                formatter.WriteTo(writer);
            }
        }
        /// <summary>
        /// Writes the atom entry to the specified stream.
        /// </summary>
        /// <param name="publication">The publication entry.</param>
        /// <param name="writeStream">The output write stream.</param>
        /// <param name="contentType">The media type header requested.</param>        
        protected virtual void WriteEntry(IPublication publication, Stream writeStream, string contentType)
        {
            var entry = publication.Syndicate();
            var formatter = GetFormatter(contentType, entry);

            using (var writer = XmlWriter.Create(writeStream))
            {
                formatter.WriteTo(writer);
            }
        }
        /// <summary>
        /// Writes the media entry to the specified stream.
        /// </summary>
        /// <param name="mediaResource">The media resource.</param>
        /// <param name="writeStream">The output write stream.</param>
        /// <param name="contentType">The media type header requested.</param>        
        protected virtual void WriteMediaEntry(IMediaResource mediaResource, Stream writeStream, string contentType)
        {
            var media = mediaResource.Syndicate();
            var formatter = GetFormatter(contentType, media);
            using (var writer = XmlWriter.Create(writeStream))
            {
                formatter.WriteTo(writer);
            }
        }
        /// <summary>
        /// Populates a publication entry based on the input stream and media type.
        /// </summary>
        /// <param name="type">The instance type of the entry.</param>
        /// <param name="readStream">The read stream.</param>
        /// <param name="contentType">Type of the content.</param>
        /// <returns>
        /// Returns a populated publication entry.
        /// </returns>
        /// <exception cref="T:System.InvalidOperationException">if <paramref name="type"/> 
        /// does not implement <see cref="T:Geocrest.Web.Mvc.Syndication.IPublication" /></exception>
        protected virtual object ReadEntry(Type type,Stream readStream, string contentType)
        {
            var publication = Activator.CreateInstance(type);
            if (!type.Implements<IPublication>()) Throw.InvalidOperation(string.Format(
                "The type '{0}' does not implement IPublication",type.Name));
            using (var reader = XmlReader.Create(readStream))
            {
                var formatter = GetFormatter(contentType);
                formatter.ReadFrom(reader);
                ((IPublication)publication).ReadSyndicationItem(formatter.Item);

            }
            readStream.Position = 0;
            using (var x = new StreamReader(readStream))
            {
                var r = x.ReadToEnd();
                var s = r;
            } 
            return publication;   
        }
        /// <summary>
        /// Gets a formatter for syndicating items based on the input media type.
        /// </summary>
        /// <param name="contentType">The content media type.</param>
        /// <param name="item">The syndication item to format.</param>
        /// <returns>
        /// Returns either an <see cref="T:System.ServiceModel.Syndication.Atom10ItemFormmater"/> or
        /// a <see cref="T:System.ServiceModel.Syndication.Rss20ItemFormatter"/>.
        /// </returns>
        /// <exception cref="T:System.NotSupportedException">if the <paramref name="contentType"/>
        /// is not a valid Atom or RSS media type.</exception>
        protected SyndicationItemFormatter GetFormatter(string contentType, SyndicationItem item = null)
        {
            Throw.IfArgumentNullOrEmpty(contentType, "contentType");
            Throw.IfArgumentNull(item,"item");
            SyndicationItemFormatter formatter = null;
            if (contentType.ToLower() == AtomMediaType)
            {
                formatter = item == null ? new Atom10ItemFormatter() : new Atom10ItemFormatter(item);
            }
            else if (contentType.ToLower() == RssMediaType)
            {
                formatter = item == null ? new Rss20ItemFormatter() : new Rss20ItemFormatter(item);
            }
            else
            {
                Throw.NotSupported(string.Format("Content type '{0}' is not supported.", contentType));
            }
            return formatter;
        }
        /// <summary>
        /// Gets a formatter for syndicating feeds based on the input media type.
        /// </summary>
        /// <param name="contentType">The content media type.</param>
        /// <param name="feed">The syndication feed to format.</param>
        /// <returns>
        /// Returns either an <see cref="T:System.ServiceModel.Syndication.Atom10FeedFormmater"/> or
        /// a <see cref="T:System.ServiceModel.Syndication.Rss20FeedFormatter"/>.
        /// </returns>
        /// <exception cref="T:System.NotSupportedException">if the <paramref name="contentType"/>
        /// is not a valid Atom or RSS media type.</exception>
        protected SyndicationFeedFormatter GetFormatter(string contentType, SyndicationFeed feed)
        {
            Throw.IfArgumentNullOrEmpty(contentType, "contentType");
            Throw.IfArgumentNull(feed, "feed");
            SyndicationFeedFormatter formatter = null;
            if (contentType.ToLower() == AtomMediaType)
            {
                formatter = new Atom10FeedFormatter(feed);
            }
            else if (contentType.ToLower() == RssMediaType)
            {
                formatter = new Rss20FeedFormatter(feed);
            }
            else
            {
                Throw.NotSupported(string.Format("Content type '{0}' is not supported.", contentType));
            }
            return formatter;
        }
        private byte[] ReadFully(Stream input)
        {
            var Buffer = new byte[16 * 1024];
            using (var Ms = new MemoryStream())
            {
                int Read;
                while ((Read = input.Read(Buffer, 0, Buffer.Length)) > 0)
                {
                    Ms.Write(Buffer, 0, Read);
                }
                return Ms.ToArray();
            }
        }
    }
}
