namespace Geocrest.Web.Hypermedia.Formatting
{
    using Geocrest.Model;
    using Geocrest.Web.Infrastructure;
    using Geocrest.Web.Mvc.Formatting;
    using Ninject;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http.Formatting;
    using System.Net.Http.Headers;
    using System.Reflection;
    using System.Runtime.Serialization;
    using System.Threading.Tasks;
    using System.Xml;
    /// <summary>
    /// An XML formatter for returning data model entities as Hypertext Application Language representations.
    /// </summary>
    public class XmlHalFormatter : XmlReferenceLoopFormatter
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Web.Hypermedia.Formatting.XmlHalFormatter"/> class.
        /// </summary>
        /// <param name="formatter">The formatter.</param>
        public XmlHalFormatter([Named("GeodataFormatter")]IEntityFormatter formatter)
            : base(formatter)
        {
            this.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/hal+xml"));
            this.AddQueryStringMapping("f", "halxml", "application/hal+xml");
        }
        /// <summary>
        /// Queries whether the <see cref="T:System.Net.Http.Formatting.XmlMediaTypeFormatter" /> can deserialize an object of the specified type.
        /// </summary>
        /// <param name="type">The type to deserialize.</param>
        /// <returns>
        /// true if the <see cref="T:System.Net.Http.Formatting.XmlMediaTypeFormatter" /> can deserialize the type; otherwise, false.
        /// </returns>
        public override bool CanReadType(System.Type type)
        {
            return typeof(IHalResource).IsAssignableFrom(type);
        }
        /// <summary>
        /// Queries whether the <see cref="T:System.Net.Http.Formatting.XmlMediaTypeFormatter" /> can serialize an object of the specified type.
        /// </summary>
        /// <param name="type">The type to serialize.</param>
        /// <returns>
        /// true if the <see cref="T:System.Net.Http.Formatting.XmlMediaTypeFormatter" /> can serialize the type; otherwise, false.
        /// </returns>
        public override bool CanWriteType(System.Type type)
        {
            return typeof(IHalResource).IsAssignableFrom(type) ||
                (type.IsGenericType && typeof(IEnumerable).IsAssignableFrom(type)
                && typeof(IHalResource).IsAssignableFrom(type.GetGenericArguments()[0]));
        }
        /// <summary>
        /// Called during deserialization to read an object of the specified type from the specified readStream.
        /// </summary>
        /// <param name="type">The type of object to read.</param>
        /// <param name="readStream">The <see cref="T:System.IO.Stream" /> from which to read.</param>
        /// <param name="content">The <see cref="T:System.Net.Http.HttpContent" /> for the content being read.</param>
        /// <param name="formatterLogger">The <see cref="T:System.Net.Http.Formatting.IFormatterLogger" /> to log events to.</param>
        /// <returns>
        /// A <see cref="T:System.Threading.Tasks.Task" /> whose result will be the object instance that has been read.
        /// </returns>
        public override System.Threading.Tasks.Task<object> ReadFromStreamAsync(System.Type type, System.IO.Stream readStream, System.Net.Http.HttpContent content, IFormatterLogger formatterLogger)
        {
            return Task<object>.Factory.StartNew(() =>
                {
                    var value = (Resource)Activator.CreateInstance(type);
                    var reader = XmlReader.Create(readStream);
                    value = DeserializeInnerResource(reader, type);
                    reader.Close();
                    return value;
                }); ;
        }

        /// <summary>
        /// Called during serialization to write an object of the specified type to the specified writeStream.
        /// </summary>
        /// <param name="type">The type of object to write.</param>
        /// <param name="value">The object to write.</param>
        /// <param name="writeStream">The <see cref="T:System.IO.Stream" /> to which to write.</param>
        /// <param name="content">The <see cref="T:System.Net.Http.HttpContent" /> for the content being written.</param>
        /// <param name="transportContext">The <see cref="T:System.Net.TransportContext" />.</param>
        /// <returns>
        /// A <see cref="T:System.Threading.Tasks.Task" /> that will write the value to the stream.
        /// </returns>
        public override System.Threading.Tasks.Task WriteToStreamAsync(System.Type type, object value, System.IO.Stream writeStream, System.Net.Http.HttpContent content, System.Net.TransportContext transportContext)
        {
            return Task.Factory.StartNew(() =>
                {
                    var settings = new XmlWriterSettings();
                    settings.Indent = false;
                    settings.OmitXmlDeclaration = false;

                    var writer = XmlWriter.Create(writeStream, settings);

                    IHalResource resource = null;
                                        
                    SerializeInnerResource(writer, (IHalResource)value);
                    
                    writer.Flush();
                    writer.Close();
                    return resource;
                });
        }
        private Resource DeserializeInnerResource(XmlReader reader, Type innerType)
        {
            var resource = (Resource)Activator.CreateInstance(innerType);

            reader.ReadStartElement("resource");
            resource.HRef = reader.GetAttribute("href");

            var properties = resource.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
               .Where(p =>
                   p.Name != "HRef" &&
                   p.Name != "Links")
               .ToArray();

            while (reader.Read())
            {
                if (reader.LocalName == "link")
                {
                    var link = new Link(reader.GetAttribute("rel"), reader.GetAttribute("href"));
                    resource.AddLink(link);
                }
                else if (reader.IsStartElement("resource"))
                {
                    var rel = reader.GetAttribute("rel");
                    var property = properties.FirstOrDefault(p => p.Name == rel);
                    if (property != null)
                    {
                        var propertyValue = DeserializeInnerResource(reader, property.PropertyType);
                        property.SetValue(resource, propertyValue, null);
                    }
                }
                else if (reader.IsStartElement())
                {
                    var property = properties.FirstOrDefault(p => p.Name == reader.LocalName);
                    if (property != null)
                    {
                        var propertyValue = reader.ReadElementContentAs(property.PropertyType, null);
                        property.SetValue(resource, propertyValue, null);
                    }
                }
            }

            return resource;
        }
        /// <summary>
        /// Write the HAL links collection into the output stream
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Xml.XmlWriter" /> to write to.</param>
        /// <param name="value">The value.</param>
        protected virtual void WriteLinks(XmlWriter writer, IHalResource value)
        {
            Throw.IfArgumentNull(writer, "writer");
            if (value != null && value.Links != null)
            {
                foreach (var link in value.Links.Where(x => !(x is SelfLink)))
                {
                    writer.WriteStartElement("link");
                    writer.WriteAttributeString("rel", link.Rel);
                    writer.WriteAttributeString("href", link.HRef);
                    writer.WriteEndElement();
                }
            }
        }
        /// <summary>
        /// Writes the properties that are not HAL properties (i.e. are not of the type 
        /// <see cref="T:Geocrest.Model.IHalResource"/> or 
        /// <see cref="T:Geocrest.Model.IHalResourceCollection"/>) 
        /// to the output stream. This is the resource state.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Xml.XmlWriter" /> to write to.</param>
        /// <param name="value">The value.</param>
        protected virtual void WriteNonHalProperties(XmlWriter writer, IHalResource value)
        {
            Throw.IfArgumentNull(writer, "writer");
            Throw.IfArgumentNull(value, "value");

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

                // classes can exclude serialization of properties that contain default 
                // values by adding [DataMember(EmitDefaultValue = false)]
                var emitDefault = property.GetCustomAttributes(typeof(DataMemberAttribute), false).Length == 0 ?
                    true : // <-default behavior
                    ((DataMemberAttribute)property.GetCustomAttributes(typeof(DataMemberAttribute), false)
                    .Single()).EmitDefaultValue;

                // classes can also exclude serialization of properties regardless of value
                // by adding [IgnoreDataMember] to the property
                var ignore = property.GetCustomAttributes(typeof(IgnoreDataMemberAttribute), false).Length == 0 ? false : true;

                // and serialize
                if (dataMember && !ignore && (emitDefault == true || !IsDefault(propertyValue)))
                {
                    var dataContract = (DataContractAttribute)property.PropertyType.GetCustomAttributes(typeof(DataContractAttribute),
                        false).SingleOrDefault();
                    var ns = dataContract != null ? dataContract.Namespace : string.Empty;
                    var serializer = new DataContractSerializer(property.PropertyType, GetDataMemberName(property), ns);
                    serializer.WriteObject(writer, propertyValue);
                }
            }
        }
        /// <summary>
        /// Writes each property of the type <see cref="T:Geocrest.Model.IHalResource"/> or 
        /// <see cref="T:Geocrest.Model.IHalResourceCollection"/> into the output stream
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Xml.XmlWriter" /> to write to.</param>
        /// <param name="value">The value.</param>
        protected virtual void WriteHalProperties(XmlWriter writer, IHalResource value)
        {
            Throw.IfArgumentNull(writer, "writer");
            Throw.IfArgumentNull(value, "value");

            // add all HalResources to a dictionary for embedding.
            var properties = value.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(x => typeof(IHalResource).IsAssignableFrom(x.PropertyType) && x.GetIndexParameters()
                .Length == 0);
            foreach (var property in properties)
            {
                var item = property.GetValue(value, null);
                SerializeInnerResource(writer, (IHalResource)item, GetDataMemberName(property));
            }
        }
        /// <summary>
        /// Opens the resource element node.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Xml.XmlWriter" /> to write to.</param>
        /// <param name="value">The value.</param>
        protected virtual void OpenResourceElement(XmlWriter writer, IHalResource value, string rel = "")
        {
            writer.WriteStartElement("resource");
            if (value != null)
            {
                writer.WriteAttributeString("rel", !string.IsNullOrEmpty(rel) ? rel : value.Rel);
                writer.WriteAttributeString("href", value.HRef);
            }
        }
        /// <summary>
        /// Closes the resource element node.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Xml.XmlWriter" /> to write to.</param>
        protected virtual void CloseResourceElement(XmlWriter writer)
        {
            writer.WriteEndElement();
        }
        private bool IsDefault<T>(T value)
        {
            return EqualityComparer<T>.Default.Equals(value, default(T));
        }
        /// <summary>
        /// Recursively serializes the HAL resource to the output stream.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Xml.XmlWriter" /> to write to.</param>
        /// <param name="resource">The resource being written.</param>
        protected virtual void SerializeInnerResource(XmlWriter writer, IHalResource resource, string rel = "")
        {
            if (resource == null) return;

            // Open the node
            OpenResourceElement(writer, resource, rel);

            // Write the links collection into the output stream
            WriteLinks(writer, resource);

            // Write each non-HAL property (i.e. Resource State)
            WriteNonHalProperties(writer, resource);

            // Write each HAL property
            WriteHalProperties(writer, resource);

            if (typeof(IHalResourceCollection).IsAssignableFrom(resource.GetType()))
            {
                foreach (IHalResource innerResource in (IHalResourceCollection)resource)
                {
                    SerializeInnerResource(writer, innerResource);
                }
            }

            // Close out the node
            CloseResourceElement(writer);
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