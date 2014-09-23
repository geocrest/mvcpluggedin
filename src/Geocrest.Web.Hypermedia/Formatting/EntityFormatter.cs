
namespace Geocrest.Web.Hypermedia.Formatting
{
    using Geocrest.Model;
    using Geocrest.Web.Infrastructure;
    using Geocrest.Web.Mvc;
    using Geocrest.Web.Mvc.Formatting;
    using Microsoft.CSharp.RuntimeBinder;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Data.Entity.Design.PluralizationServices;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Formatting;
    using System.Net.Http.Headers;
    using System.Reflection;
    using System.Runtime.Serialization;
    using System.Text;
    using System.Web.Http;
    using System.Web.Http.Hosting;
    using System.Web.Http.OData.Query;
    using System.Web.Http.Routing;
    /// <summary>
    /// Provides formatting for specific Geocrest.Model entities by removing circular references
    /// and adding Hypertext Application Language formatting (when requested).
    /// </summary>
    public abstract class EntityFormatter : IEntityFormatter, IResponseEnricher
    {
        #region Fields
        private const string HALJSONACCEPT = "application/hal+json";
        private const string HALXMLACCEPT = "application/hal+xml";
        private const string HALJSONQUERY = "haljson";
        private const string HALXMLQUERY = "halxml";
        private static Dictionary<Type, string> configMappings;
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Web.Hypermedia.Formatting.EntityFormatter" /> class.
        /// </summary>
        /// <param name="routeName">The name of the route used to construct URLs.</param>
        /// <param name="area">The name of the area to use during URL construction.</param>
        protected EntityFormatter(string routeName, string area)
        {
            Throw.IfArgumentNullOrEmpty(routeName, "routeName");
            this.RouteName = routeName;
            this.Area = area ?? string.Empty;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Web.Hypermedia.Formatting.EntityFormatter" /> class.
        /// </summary>
        /// <param name="routeName">The name of the route used to construct URLs</param>
        protected EntityFormatter(string routeName) 
            : this(routeName, string.Empty)
        {
        }
        #endregion
        
        /// <summary>
        /// Sets the mappings to use when associating HAL resources with a self link.
        /// </summary>
        /// <param name="mappings">The mappings.</param>
        public static void SetMappings(Dictionary<Type, string> mappings)
        {
            Throw.IfArgumentNull(mappings, "mappings");
            configMappings = mappings;
        }

        #region IEntityFormatter members
        /// <summary>
        /// Formats the specified entity with custom logic.
        /// </summary>
        /// <param name="entity">The entity to format.</param>
        /// <returns>
        /// The formatted entity.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">entity</exception>
        public object Format(object entity)
        {
            Throw.IfArgumentNull(entity, "entity");
            var cleaned = entity.Clean();
            return cleaned;
        }
        #endregion

        #region IResponseEnricher Members
        /// <summary>
        /// Gets the name of the route used to construct URLs. This property is required.
        /// </summary>
        /// <value>
        /// The name of the route.
        /// </value>
        public string RouteName { get; protected set; }

        /// <summary>
        /// Gets the name of the area to use during URL construction. This property is optional.
        /// </summary>
        /// <value>
        /// The area name.
        /// </value>
        public string Area { get; protected set; }

        /// <summary>
        /// Determines whether this instance can enrich the specified response.
        /// </summary>
        /// <param name="response">The response.</param>
        /// <returns>
        /// <b>true</b>, if this instance can enrich the specified response; otherwise, <b>false</b>.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">response</exception>
        public bool CanEnrich(HttpResponseMessage response)
        {
            Throw.IfArgumentNull(response, "response");
            if (response.RequestMessage == null) return false;
            var content = response.Content as ObjectContent;
            object routeData;
            object area = null;
            if (response.RequestMessage.Properties.TryGetValue(HttpPropertyKeys.HttpRouteDataKey, out routeData))
            {
                if (routeData is IHttpRouteData)
                {
                    if (!((IHttpRouteData)routeData).Values.TryGetValue("area", out area))
                    {
                        return false;
                    }
                }
            }
            else return false;
            if (content == null || response.RequestMessage == null) return false;
            var hasAcceptHeader = response.RequestMessage.Headers.Accept.Count(x => x.MediaType.
                Contains(HALJSONACCEPT) || x.MediaType.Contains(HALXMLACCEPT)) > 0;
            var hasQueryString = response.RequestMessage.RequestUri.ParseQueryString()["f"] == HALJSONQUERY ||
                response.RequestMessage.RequestUri.ParseQueryString()["f"] == HALXMLQUERY;
            return content != null && ((string)area).ToLower() ==this.Area.ToLower()
                && (hasAcceptHeader || hasQueryString) &&
                (typeof(IHalResource).IsAssignableFrom(content.ObjectType) ||
                (content.ObjectType.IsGenericType && typeof(IEnumerable).IsAssignableFrom(content.ObjectType)
                && typeof(IHalResource).IsAssignableFrom(content.ObjectType.GetGenericArguments()[0])));
        }

        /// <summary>
        /// Enriches the specified response with additional HAL link information.
        /// </summary>
        /// <param name="response">The outgoing HTTP response.</param>
        /// <remarks>This method is used when generating actual responses.</remarks>
        /// <exception cref="T:System.ArgumentNullException">response</exception>
        public HttpResponseMessage Enrich(HttpResponseMessage response)
        {
            Throw.IfArgumentNull(response, "response");
            dynamic resource;
            if (response.TryGetContentValue<dynamic>(out resource))
            {
                var urlHelper = response.RequestMessage.GetUrlHelper();
                var mediaType = response.Content.Headers.ContentType;
                var outgoingType = mediaType.MediaType.Contains(HALJSONACCEPT) ? "application/json" : "application/xml";
                var config = response.RequestMessage.GetConfiguration();

                MediaTypeFormatter formatter = config.Formatters.FindWriter(typeof(IHalResource), mediaType);
                try
                {
                    resource = Format(resource);
                    Type type = resource.GetType();
                    if (type.IsGenericType && typeof(IEnumerable).IsAssignableFrom(type) &&
                        typeof(IHalResource).IsAssignableFrom(type.GetGenericArguments()[0]))
                    {
                        foreach (var entity in resource)
                            EnrichEntity((dynamic)entity, urlHelper);
                        var newValue = Resource.FromValues((IEnumerable)resource);
                        var next = response.RequestMessage.GetNextPageLink();
                        if (next != null)
                            newValue.AddLink(new NextLink(next.AbsoluteUri.ToString()));
                        newValue.AddLink(new SelfLink(response.RequestMessage.RequestUri.AbsoluteUri));
                        return response.RequestMessage.CreateResponse(response.StatusCode, newValue,
                            formatter, outgoingType);
                    }
                    else
                    {
                        EnrichEntity((dynamic)resource, urlHelper);
                        return response.RequestMessage.CreateResponse(response.StatusCode, (IHalResource)resource,
                            formatter, outgoingType);
                    }
                }
                catch (RuntimeBinderException ex)
                {
                    Throw.HttpResponse(HttpStatusCode.MethodNotAllowed, "Error enriching", string.Format(
                        "No applicable 'EnrichEntity' method could be found for the type '{0}'",
                        (Type)resource.GetType()), x => new NotImplementedException(x.Message, ex));
                }
            }
            return response;
        }

        /// <summary>
        /// Determines whether this instance can enrich the specified type based on the accept header.
        /// </summary>
        /// <param name="type">The type to enrich.</param>
        /// <param name="urlHelper">The URL helper.</param>
        /// <param name="acceptHeader">The accept header.</param>
        /// <returns>
        ///   <b>true</b>, if this instance can enrich the specified type; otherwise, <b>false</b>.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">type or acceptHeader</exception>
        public bool CanEnrich(Type type, UrlHelper urlHelper, MediaTypeHeaderValue acceptHeader)
        {
            Throw.IfArgumentNull(type, "type");
            Throw.IfArgumentNull(acceptHeader, "acceptHeader");
            Throw.IfArgumentNull(urlHelper, "urlHelper");
            object routeData;
            object area = null;
            if (urlHelper.Request.Properties.TryGetValue(HttpPropertyKeys.HttpRouteDataKey, out routeData))
            {
                if (routeData is IHttpRouteData)
                {
                    if (!((IHttpRouteData)routeData).Values.TryGetValue("area", out area))
                    {
                        return false;
                    }
                }
            }
            else return false;
            var hasAcceptHeader = acceptHeader.MediaType.Contains(HALJSONACCEPT) || acceptHeader.MediaType.Contains(HALXMLACCEPT);
            return hasAcceptHeader && ((string)area).ToLower() == this.Area.ToLower()
                && (typeof(IHalResource).IsAssignableFrom(type) ||
                (type.IsGenericType && typeof(IEnumerable).IsAssignableFrom(type)
                && typeof(IHalResource).IsAssignableFrom(type.GetGenericArguments()[0])));
        }

        /// <summary>
        /// Enriches the object with HAL links using the specified URL helper.
        /// </summary>
        /// <param name="baseUrl">The base URL for the entity.</param>
        /// <param name="urlHelper">The URL helper.</param>
        /// <param name="value">The value.</param>
        /// <remarks>This method is used when generating sample syntax.</remarks>
        /// <exception cref="System.ArgumentNullException">urlHelper or value</exception>
        public object Enrich(string baseUrl, UrlHelper urlHelper, object value)
        {
            Throw.IfArgumentNull(urlHelper, "urlHelper");
            Throw.IfArgumentNull(value, "value");
            dynamic resource = value;
            try
            {
                resource = Format(value);
                Type type = resource.GetType();
                if (type.IsGenericType && typeof(IEnumerable).IsAssignableFrom(type) &&
                    typeof(IHalResource).IsAssignableFrom(type.GetGenericArguments()[0]))
                {
                    foreach (var entity in resource)
                        EnrichEntity((IHalResource)entity, urlHelper);
                    var newValue = Resource.FromValues((IEnumerable)resource);
                    var filter = (QueryFilterProvider)GlobalConfiguration.Configuration.Services.
                        GetFilterProviders().FirstOrDefault(x => x is QueryFilterProvider &&
                            ((QueryFilterProvider)x).QueryFilter is QueryableAttribute);
                    var pagesize = filter != null ? ((QueryableAttribute)filter.QueryFilter).PageSize : 10;
                    var next = this.GetNextPageLink(baseUrl.ToLower(), 100);
                    if (next != null)
                        newValue.AddLink(new NextLink(next.AbsoluteUri.ToString().ToLower()));
                    newValue.AddLink(new SelfLink(baseUrl.ToLower()));
                    return newValue;
                }
                else
                {
                    EnrichEntity((IHalResource)resource, urlHelper);
                    return resource;
                }
            }
            catch (RuntimeBinderException ex)
            {
                Throw.NotImplemented(string.Format(
                    "No applicable 'EnrichEntity' method could be found for the type '{0}'",
                    (Type)resource.GetType()), x => new NotImplementedException(x.Message, ex));
            }

            return value;
        }

        #endregion

        /// <summary>
        /// Enriches the entity with HAL links by recursively enriching any property 
        /// that inherits from <see cref="T:Geocrest.Model.IHalResource"/>
        /// </summary>
        /// <param name="resource">The resource.</param>
        /// <param name="url">The URL.</param>
        protected virtual void EnrichEntity(IHalResource resource, UrlHelper url)
        {
            if (resource == null) return;
            var properties = resource.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(x => typeof(IHalResource).IsAssignableFrom(x.PropertyType));

            // add a self link to the resource
            AddLink(resource, resource, null, url);

            foreach (PropertyInfo property in properties)
            {
                var propValue = property.GetValue(resource, null);
                var propType = property.PropertyType;
                if (propValue == null) continue;
                if (typeof(IHalResourceCollection).IsAssignableFrom(propType))
                {
                    var child = (IHalResourceCollection)propValue;

                    // add a self link to the collection (will use oData to create the link)
                    AddCollectionLink(child, child, resource, property, url);
                    // add link to parent
                    AddCollectionLink(resource, child, resource, property, url);

                    foreach (IHalResource item in (IHalResourceCollection)propValue)
                    {
                        // continue recursive enriching
                        EnrichEntity(item, url);
                    }
                }
                else
                {
                    // add a link to the parent resource to the child resource
                    AddLink(resource, (IHalResource)propValue, property, url);
                    // continue recursive enriching
                    EnrichEntity((IHalResource)propValue, url);
                }
            }
        }

        /// <summary>
        /// Adds a link to a resource that points to a collection.
        /// </summary>
        /// <param name="resource">The resource to which the link is added.</param>
        /// <param name="collection">The collection that the link targets.</param>
        /// <param name="parent">The parent resource containing the key attribute that links the <paramref name="resource" /> to the <paramref name="collection" />.</param>
        /// <param name="property">The property information of the target resource.</param>
        /// <param name="url">The URL helper used to create an HREF.</param>
        /// <remarks>
        /// There are scenarios that define what the relation is between the <paramref name="resource" /> and the <paramref name="collection" />:
        /// <list type="bullet">
        /// <item><strong>The resource and the collection reference the same object:</strong> the relation is <em>self</em></item>
        /// <item><strong>The resource and the collection refer to different objects:</strong> the relation is the property name of the collection</item>
        /// </list>
        /// </remarks>
        protected virtual void AddCollectionLink(IHalResource resource, IHalResourceCollection collection, IHalResource parent, PropertyInfo property, UrlHelper url)
        {
            if (resource == null || collection == null || parent == null || property == null || url == null) return;

            string key = GetKey(resource, parent).ToString();
            string rel = object.ReferenceEquals(resource, collection) ? "self" : property.Name;
            string filter = string.Empty;
            var keys = parent.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(x => x.GetCustomAttributes(typeof(KeyAttribute), true).Length > 0);
            PropertyInfo keyInfo = null;
            if (keys.Count() > 1) // multiple key properties
            {
                // sort them by [DataMember(Order)] and take the first one
                var keysWithDataMember = keys.Where(x => x.GetCustomAttributes(typeof(DataMemberAttribute), true).Length > 0);
                keyInfo = keysWithDataMember.OrderBy(x => ((DataMemberAttribute)x.GetCustomAttributes(typeof(DataMemberAttribute), true).First())
                    .Order).First();
            }
            else
            {
                keyInfo = keys.First();
            }

            var navigationKey = GetNavigationProperty(collection, parent);
            if (navigationKey == null) return;
            string controller = GetControllerName(collection);
            if (typeof(IEnumerable).IsAssignableFrom(navigationKey.PropertyType))
            {
                filter = string.Format("{0}/any({1}: {1}/{2} eq {3})", navigationKey.Name, parent.GetType().Name,
                    keyInfo.Name, (keyInfo.PropertyType == typeof(string))
                    ? string.Format("'{0}'", key) : key);
            }
            else
            {
                filter = string.Format("{0}/{1} eq {2}", navigationKey.Name, keyInfo.Name,
                    (keyInfo.PropertyType == typeof(string)) ? string.Format("'{0}'", key) : key);
            }
            string href = System.Web.HttpUtility.UrlDecode(url.Link(this.RouteName,
                     new Dictionary<string, object>
                    {
                        {"controller", controller},
                        {"$filter", filter},
                        {"mainargument", null}
                    }));
            AddLink(resource, href, rel);
        }
        
        /// <summary>
        /// Adds a link to the HAL resource.
        /// </summary>
        /// <param name="resource">The resource to which the link is added.</param>
        /// <param name="target">The resource that the link represents.</param>
        /// <param name="property">The property information of the target resource.</param>
        /// <param name="url">The URL helper used to create an HREF.</param>
        protected virtual void AddLink(IHalResource resource, IHalResource target, PropertyInfo property, UrlHelper url)
        {
            if (resource == null || url == null || target == null) return;
            try
            {
                var controller = GetControllerName(target);
                if (string.IsNullOrEmpty(controller))
                    Throw.NotSupported("Generating a HAL URI without a controller name is not supported.");
                var key = GetKey(target, resource).ToString();
                string href = url.Link(this.RouteName, controller, key);
                AddLink(resource, href, object.ReferenceEquals(resource, target) ? "self" : property.Name);
            }
            catch (NotSupportedException ex)
            {
                if (ex.Message == "The resource type does not have a KeyAttribute defined.") return;
            }
        }
        
        /// <summary>
        /// Adds a link to the HAL resource.
        /// </summary>
        /// <param name="resource">The resource to which the link is added.</param>
        /// <param name="href">The HREF to the target resource.</param>
        /// <param name="rel">The relation of the target resource.</param>
        protected virtual void AddLink(IHalResource resource, string href, string rel)
        {
            if (resource == null) return;
            if (string.IsNullOrEmpty(href))
                Throw.NotSupported("Links must have a valid HREF.");
            if (string.IsNullOrEmpty(rel))
                Throw.NotSupported("Links must have a relation defined.");

            resource.AddLink(rel.ToLower() == "self" ?
                new SelfLink(href) :
                new Link(rel, href));
        }

        private static PropertyInfo GetNavigationProperty(IHalResource primary, IHalResource foreign)
        {
            Type type = null;
            if (typeof(IHalResourceCollection).IsAssignableFrom(primary.GetType()))
            {
                type = primary.GetType().GetGenericArguments()[0];
            }
            else
            {
                type = primary.GetType();
            }
            var property = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .SingleOrDefault(x => (typeof(IHalResourceCollection).IsAssignableFrom(x.PropertyType)
                && x.PropertyType.GetGenericArguments()[0] == foreign.GetType()) ||
                x.PropertyType == foreign.GetType());            
            return property;
        }

        /// <summary>
        /// Returns the value of the property decorated with a
        /// <see cref="T:System.ComponentModel.DataAnnotations.KeyAttribute" />.
        /// </summary>
        /// <param name="resource">A HAL resource containing a key attribute.</param>
        /// <param name="parent">The parent resource if <paramref name="resource"/> is a HAL collection.</param>
        /// <returns>
        /// Returns an object containing the value of the key.
        /// </returns>
        private static object GetKey(IHalResource resource, IHalResource parent)
        {
            Type type = null;
            PropertyInfo keyProp = null;
            IHalResource keyHolder = null;

            if (typeof(IHalResourceCollection).IsAssignableFrom(resource.GetType())) // if collection then get key from parent
            {
                type = parent.GetType();
                keyHolder = parent;
            }
            else // get key from resource itself
            {
                type = resource.GetType();
                keyHolder = resource;
            }
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var keys = properties.Where(x => x.GetCustomAttributes(typeof(KeyAttribute), true).Length > 0);
            if (keys.Count() > 1) // multiple key properties
            {
                // sort them by [DataMember(Order)] and take the first one
                var keysWithDataMember = keys.Where(x => x.GetCustomAttributes(typeof(DataMemberAttribute), true).Length > 0);
                keyProp = keysWithDataMember.OrderBy(x => ((DataMemberAttribute)x.GetCustomAttributes(typeof(DataMemberAttribute), true).First())
                    .Order).FirstOrDefault();
            }
            else
            {
                keyProp = keys.FirstOrDefault();
            }

            if (keyProp == null)
                Throw.NotSupported("The resource type does not have a KeyAttribute defined.");
            var key = keyProp.GetGetMethod().Invoke(keyHolder, null);

            if (key == null)
                Throw.NotSupported(string.Format("Unable to retrieve the key for resource type '{0}'.", type.FullName));
            return key;
        }

        /// <summary>
        /// Gets the name of the controller first by configuration then by convention.
        /// </summary>
        /// <param name="resource">The resource.</param>
        /// <returns></returns>
        private static string GetControllerName(IHalResource resource)
        {
            var type = resource.GetType();
            if (typeof(IHalResourceCollection).IsAssignableFrom(type))
            {
                type = type.GetGenericArguments()[0];
            }
            string controllerName;
            if (!configMappings.TryGetValue(type, out controllerName))
            {
                var p = PluralizationService.CreateService(new System.Globalization.CultureInfo("en-US"));
                controllerName = p.Pluralize(type.Name).ToLower();
            }
            return controllerName;
        }

        /// <summary>
        /// Gets the 'next' page link for a collection. This method is used exclusively for generating sample syntax.
        /// </summary>
        /// <param name="url">The base URL.</param>
        /// <param name="pageSize">Page size.</param>
        /// <returns>
        /// Returns an instance of <see cref="Uri">Uri</see>.
        /// </returns>
        private Uri GetNextPageLink(string url, int pageSize)
        {
            Throw.IfArgumentNullOrEmpty(url, "url");

            StringBuilder queryBuilder = new StringBuilder();

            int nextPageSkip = pageSize;
            queryBuilder.AppendFormat("$skip={0}", nextPageSkip);

            UriBuilder uriBuilder = new UriBuilder(url.ToLower())
            {
                Query = queryBuilder.ToString()
            };
            return uriBuilder.Uri;
        }                
    }
}