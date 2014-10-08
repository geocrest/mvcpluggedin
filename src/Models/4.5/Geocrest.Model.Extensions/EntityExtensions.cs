namespace Geocrest.Model
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data.Entity.Core.Objects;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.Serialization;
#if NET45 || SILVERLIGHT
    using Microsoft.CSharp.RuntimeBinder;
#endif
    using Newtonsoft.Json;
    using Geocrest.Model.ArcGIS.Tasks;
    /// <summary>
    /// Extension methods used with model entities.
    /// </summary>
    public static class EntityExtensions
    {
        /// <summary>
        /// The maximum items in object graph. This is set to 1,000,000.
        /// </summary>
        private const int MAX_ITEMS_IN_OBJECT_GRAPH = 1000000;
#if NET45 || SILVERLIGHT
        /// <summary>
        /// Determines whether the specified entity is a proxy.
        /// </summary>
        /// <param name="entity">An object to evaluate.</param>
        /// <returns>
        /// <b>true</b>, if the specified object is a proxy; otherwise, <b>false</b>.
        /// </returns>
        public static bool IsProxy(object entity)
        {
            return entity != null && ObjectContext.GetObjectType(entity.GetType()) != entity.GetType();
        }
#endif

        private static Dictionary<Type, IEnumerable<Type>> allTypes = new Dictionary<Type, IEnumerable<Type>>();

        /// <summary>
        /// Gets all known types that derive from the specified type and are loaded into the current domain.
        /// </summary>
        /// <param name="type">The type of base class for which to evaluate.</param>
        /// <returns>
        /// Returns an instance of <see cref="T:System.Collections.Generic.IEnumerable`1"/>.
        /// </returns>
        public static IEnumerable<Type> GetAllKnownTypes(this Type type)
        {
            IEnumerable<Type> types;
            if (allTypes.TryGetValue(type, out types))
                return types;
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var knownTypes = new List<Type>();
            foreach (var assembly in assemblies.Where(x => x.GetTypes()
                .Count(t => t.IsSubclassOf(type)) > 0))
            {
                knownTypes.AddRange(assembly.GetTypes().Where(x => x.IsSubclassOf(type) && !x.IsGenericType));
            }
            allTypes.Add(type, knownTypes);
            return (IEnumerable<Type>)knownTypes;
        }
        /// <summary>
        /// Gets a JSON structure that represents the resource.
        /// </summary>
        /// <typeparam name="T">A class deriving from <see cref="T:Geocrest.Model.ArcGIS.Tasks.FeatureSet"/>.</typeparam>
        /// <param name="resource">The resource to serialize.</param>
        /// <returns>
        /// Returns a JSON structure of the resource.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">resource</exception>
        public static string ToJson<T>(this T resource)
        {
            return resource.ToJson<T>(Formatting.None);
        }
        /// <summary>
        /// Gets a JSON structure that represents the resource with an option for the formatting.
        /// </summary>
        /// <typeparam name="T">A class deriving from <see cref="T:Geocrest.Model.ArcGIS.Tasks.FeatureSet" />.</typeparam>
        /// <param name="resource">The resource to serialize.</param>
        /// <param name="formatting">The formatting to use when serializing.</param>
        /// <returns>
        /// Returns a JSON structure of the resource.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">resource</exception>
        public static string ToJson<T>(this T resource, Formatting formatting)
        {
            return resource.ToJson<T>(formatting, null);
        }
        /// <summary>
        /// Gets a JSON structure that represents the resource.
        /// </summary>
        /// <typeparam name="T">A class deriving from <see cref="T:Geocrest.Model.ArcGIS.Tasks.FeatureSet" />.</typeparam>
        /// <param name="resource">The resource to serialize.</param>
        /// <param name="formatting">The formatting to use when serializing.</param>
        /// <param name="propertiesToExclude">The properties to exclude from serialization.</param>
        /// <returns>
        /// Returns a JSON structure of the resource.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">resource</exception>
        public static string ToJson<T>(this T resource, Formatting formatting,
            OptionalPropertiesContractResolver propertiesToExclude)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = formatting,
                ContractResolver = propertiesToExclude
            };
            return resource.ToJson<T>(settings);
        }
        /// <summary>
        /// Gets a JSON structure that represents the resource.
        /// </summary>
        /// <typeparam name="T">A class deriving from <see cref="T:Geocrest.Model.ArcGIS.Tasks.FeatureSet" />.</typeparam>
        /// <param name="resource">The resource to serialize.</param>
        /// <param name="settings">The settings to use when serializing.</param>
        /// <returns>
        /// Returns a JSON structure of the resource.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">resource</exception>
        public static string ToJson<T>(this T resource, JsonSerializerSettings settings)
        {
            if (resource == null) throw new ArgumentNullException("resource");
            return JsonConvert.SerializeObject(resource, settings);
        }
#if NET45 || SILVERLIGHT
        /// <summary>
        /// Converts a dynamic proxy representing an entity to the actual entity.
        /// </summary>
        /// <typeparam name="T">The actual entity type needed.</typeparam>
        /// <param name="entity">An object of the type <typeparamref name="T"/>.</param>
        /// <returns>
        /// Returns a non-proxied instance of <typeparamref name="T"/>.
        /// </returns>
        public static T ToObject<T>(this T entity)
        {
            if (!IsProxy(entity)) return entity;

            Type type = entity.GetType(); /* proxy type */
            Type baseType = type.BaseType; /* actual type (proxies inherit from actual) */

            var s = new JsonSerializerSettings
            {
                ContractResolver = new Geocrest.Model.Extensions.OptOutContractResolver()
            };

            var json = JsonConvert.SerializeObject(entity, s);
            var value = JsonConvert.DeserializeObject(json, baseType, s);
            return (T)value;

            /*******************************************************************************/
            /*The following legacy code performs opt-in serialization so any member that's
             * not marked as DataMember will not be serialized. This becomes an issue when
             * a DataMember property is populated from a non DataMember property. In this
             * case, the DataMember property will not be set correctly since the populating
             * property is set to the property's default value instead of the correct value.
             * The example of this is the Mailable property on Address. The following code
             * sets the Mailable property back to it's default (in this case null) and 
             * subsequent calls to this method will incorrectly set IsMailable to false since
             * that is the default value of Mailable.
            /*******************************************************************************/
            //var knownTypes = typeof(Resource).GetAllKnownTypes();
            //var resolver = new ProxyDataContractResolver();
            //var serializer = new DataContractSerializer(
            //               type,
            //               type.Name,
            //               string.Empty,
            //               knownTypes,
            //               MAX_ITEMS_IN_OBJECT_GRAPH,
            //               true, true,
            //               null, resolver);
            //var stream = new MemoryStream();
            //serializer.WriteObject(stream, entity);

            //stream.Seek(0, SeekOrigin.Begin);
            //return (T)serializer.ReadObject(stream);
        }

        /// <summary>
        /// Converts a collection of entity proxies to a collection of actual entities. 
        /// </summary>
        /// <typeparam name="T">The actual entity type needed.</typeparam>
        /// <param name="entities">An <see cref="T:System.Collections.Generic.IEnumerable`1"/> of 
        /// <typeparamref name="T"/> objects.</param>
        /// <returns>
        /// Returns an <see cref="T:System.Collections.Generic.IEnumerable`1"/> of non-proxied 
        /// <typeparamref name="T"/> objects.
        /// </returns>
        public static IEnumerable<T> ToObject<T>(this IEnumerable<T> entities)
        {
            if (entities.Count() == 0 || !IsProxy(entities.First())) return entities;
            IList<T> values = new List<T>();

            Type type = entities.GetType().GetGenericArguments()[0]; /* proxy type */
            Type baseType = type.BaseType; /* actual type (proxies inherit from actual) */

            var s = new JsonSerializerSettings
            {
                ContractResolver = new Geocrest.Model.Extensions.OptOutContractResolver()
            };

            foreach (T entity in entities)
            {
                var json = JsonConvert.SerializeObject(entity, s);
                var value = JsonConvert.DeserializeObject(json, baseType, s);
                values.Add((T)value);
            }
            return values.AsEnumerable<T>();
        }

        /// <summary>
        /// Cleans the specified entity by removing circular references within child properties.
        /// </summary>
        /// <typeparam name="T">The type of entity to clean.</typeparam>
        /// <param name="entity">The entity to clean.</param>
        /// <returns>
        /// Returns an entity that has circular references set to null.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="entity" /> is null.</exception>
        /// <exception cref="T:Microsoft.CSharp.RuntimeBinder.RuntimeBinderException">If a
        /// Clean method could not be found for the type <typeparamref name="T" />.</exception>
        /// <remarks>
        /// If the specified entity is a proxy, the return type will be converted to <typeparamref name="T"/>.
        /// </remarks>
        public static T Clean<T>(this T entity)
        {
            if (entity == null) throw new ArgumentNullException("entity");

            Type entityType = null;
            try
            {   /* Collections */
                if (entity.GetType().IsGenericType && typeof(IEnumerable).IsAssignableFrom(entity.GetType()))
                {
                    entityType = entity.GetType().GetGenericArguments()[0];
                    if (!entityType.IsSubclassOf(typeof(Resource)) ||
                        typeof(IHalResourceCollection).IsAssignableFrom(entity.GetType()))
                        return entity;
                    else
                    {
                        var list = (IList)CreateGeneric(typeof(List<>), entityType);
                        foreach (var e in (IEnumerable<dynamic>)entity)
                            list.Add(CleanEntity((dynamic)e, new List<Resource>()));
                        return (T)list.AsQueryable();
                    }
                }
                /* Single entities */
                else
                {
                    entityType = entity.GetType();
                    if (!entityType.IsSubclassOf(typeof(Resource)))
                        return entity;
                    else
                        return CleanEntity((dynamic)entity, new List<Resource>());
                }
            }
            catch (RuntimeBinderException ex)
            {
                throw new NotImplementedException(string.Format(
                    "No applicable 'CleanEntity' method could be found for the type '{0}'",
                    entityType), ex);
            }
        }

        /// <summary>
        /// Provides a generic cleaning method for removing self-referencing objects
        /// </summary>
        /// <param name="resource">The resource to clean.</param>        
        /// <param name="referencedObjects">The objects that have already been traversed in the resource's
        /// heirarchy.</param>
        /// <returns>
        /// Returns a cleaned <see cref="T:Geocrest.Model.Resource"/> instance.
        /// </returns>
        /// <remarks>
        /// This method also calls <see cref="M:Geocrest.Model.EntityExtensions.UpdateAddressAttributes(Address.Address)"/>
        /// used to set Address attributes in order to avoid multiple iterations through all navigable entity relationships.
        /// </remarks>
        private static Resource CleanEntity(Resource resource, IList<Resource> referencedObjects)
        {
            // If the type is an EF proxy then we need to get the properties from the 
            // base type instead of from the proxy because the proxy does not have any
            // knowledge of custom attributes.
            PropertyInfo[] properties = IsProxy(resource) ?
                resource.GetType().BaseType.GetProperties(BindingFlags.Public | BindingFlags.Instance) :
                resource.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            referencedObjects.Add(resource);                       

            foreach (PropertyInfo property in properties)
            {
                var propValue = property.GetValue(resource, null);
                var propType = property.PropertyType;
                if (propValue == null) continue;
                if (propType.IsGenericType && typeof(IEnumerable).IsAssignableFrom(propType))
                {
                    var entityType = propType.GetGenericArguments()[0];
                    if (entityType.IsSubclassOf(typeof(Resource)))
                    {
                        var list = (IList)CreateGeneric(typeof(List<>), entityType);
                        foreach (var item in (IEnumerable)propValue)
                        {
                            if (referencedObjects.Contains(item))
                                list.Add(item);
                            else
                                CleanEntity((Resource)item, referencedObjects);
                        }

                        var removeMethod = propType.GetMethod("Remove");
                        if (removeMethod != null)
                        {
                            foreach (var item in list)
                                removeMethod.Invoke(propValue, new object[] { item });
                        }
                    }
                }
                else if (propType.IsSubclassOf(typeof(Resource)))
                {
                    var exists = referencedObjects.Contains(propValue);
                    var isDataMember = property.GetCustomAttributes(typeof(DataMemberAttribute), false).Length == 0 ? false : true;
                    if (!exists && property.CanWrite && isDataMember)
                        property.SetValue(resource, CleanEntity((Resource)propValue, referencedObjects), null);
                    else
                        property.SetValue(resource, null, null);
                }
            }
            return resource == referencedObjects[0] ? resource.ToObject() : resource;
        }
#endif
        /// <summary>
        /// Creates a generic object with the specified inner generic set as the object's type parameter.
        /// Use this method when you need to create a generic from a <see cref="T:System.Type"/>.
        /// </summary>
        /// <param name="generic">The generic type to create.</param>
        /// <param name="innerType">The inner type of the generic object.</param>
        /// <param name="args">Arguments for the constructor of the type to create.</param>
        /// <returns>
        /// Returns an instance of <see cref="T:System.Object"/> that can be
        /// cast to the requested generic type.
        /// </returns>
        public static object CreateGeneric(Type generic, Type innerType, params object[] args)
        {
            System.Type specificType = generic.MakeGenericType(new System.Type[] { innerType });
            return Activator.CreateInstance(specificType, args);
        }

        /// <summary>
        /// Returns an unmodified entity to prevent Runtime Binder Exceptions in the generic
        /// Clean method.
        /// </summary>
        /// <param name="value">The input value to clean.</param>
        /// <returns>
        /// The original object.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="value" /> is null.</exception>        
        private static object CleanEntity(object value)
        {
            if (value == null) throw new ArgumentNullException("value");
            return value;
        }         
    }
}
