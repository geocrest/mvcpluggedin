namespace Geocrest.Web.Mvc.Formatting
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Formatting;
    using System.Net.Http.Headers;
    using System.ServiceModel.Syndication;
    using System.Xml;
    using Geocrest.Web.Infrastructure;
    using Geocrest.Web.Mvc.Syndication;
    /// <summary>
    /// A buffered media type formatter for handling Atom feeds and entries. 
    /// </summary>
    public class AtomPubCategoryMediaTypeFormatter : BufferedMediaTypeFormatter
    {
        private const string AtomCategoryMediaType = "application/atomcat+xml";
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Web.Mvc.Formatting.AtomPubCategoryMediaTypeFormatter" /> class.
        /// </summary>
        public AtomPubCategoryMediaTypeFormatter()
        {
            SupportedMediaTypes.Add(new MediaTypeHeaderValue(AtomCategoryMediaType));
            this.AddQueryStringMapping("format", "atomcat", AtomCategoryMediaType);
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
            return type.Implements<IPublicationCategoriesDocument>();
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
            var document = value as IPublicationCategoriesDocument;

            var atomDoc = new InlineCategoriesDocument(
                document.Categories.Select(c => new SyndicationCategory(c.Name) { Label = c.Label }),
                document.IsFixed,
                document.Scheme
            );

            var formatter = new AtomPub10CategoriesDocumentFormatter(atomDoc);

            using (writeStream)
            {
                using (var writer = XmlWriter.Create(writeStream))
                {
                    formatter.WriteTo(writer);
                }
            }
        }
    }
}
