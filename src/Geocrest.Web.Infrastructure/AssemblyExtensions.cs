namespace Geocrest.Web.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Extensions method used for reflection.
    /// </summary>
    public static class AssemblyExtensions
    {
        /// <summary>
        /// Gets all types with the specified attribute.
        /// </summary>
        /// <typeparam name="T">An attribute type.</typeparam>
        /// <param name="assembly">The assembly within which to search for types.</param>
        /// <returns>
        /// Returns an instance of <see cref="T:System.Collections.Generic.IEnumerable`1"/>
        /// </returns>       
        public static IEnumerable<Type> GetTypesWithAttribute<T>(this Assembly assembly) where T : Attribute
        {
            foreach (Type type in assembly.GetTypes())
            {
                if (type.GetCustomAttributes(typeof(T), true).Length > 0)
                {
                    yield return type;
                }
            }
        }

        /// <summary>
        /// Gets actual instances of the specified type from the assembly.
        /// </summary>
        /// <param name="assembly">The assembly within which to search for types.</param>
        /// <param name="type">The type of the class to retrieve.</param>
        /// <returns>
        /// Returns an instance of <see cref="T:System.Collections.Generic.IList`1"/>
        /// </returns>
        public static IEnumerable<object> GetInstances(this Assembly assembly, Type type)
        {
            return (from t in assembly.GetTypes()
                    where t.BaseType == (type) && t.GetConstructor(Type.EmptyTypes) != null
                    select Activator.CreateInstance(t));
        }
        /// <summary>
        /// Gets actual instances of the specified type from the assembly.
        /// </summary>
        /// <typeparam name="T">The type of class to retrieve.</typeparam>
        /// <param name="assembly">The assembly within which to search for types.</param>
        /// <returns>
        /// Returns an instance of <see cref="T:System.Collections.Generic.IEnumerable`1"/>
        /// </returns>
        public static IEnumerable<T> GetInstances<T>(this Assembly assembly)
        {
            return (from t in assembly.GetTypes()
                    where t.GetConstructor(Type.EmptyTypes) != null && t.BaseType == (typeof(T))
                    select (T)Activator.CreateInstance(t));            
        }
        /// <summary>
        /// Gets actual instances of types that implement the specified interface.
        /// </summary>
        /// <typeparam name="Interface">The type of interface used to locate types.</typeparam>
        /// <param name="assembly">The assembly within which to search for types.</param>
        /// <returns>
        /// Returns an instance of <see cref="T:System.Collections.Generic.IEnumerable`1"/>.
        /// </returns>
        public static IEnumerable<Interface> GetInterfaceInstances<Interface>(this Assembly assembly)
        {
            return (from t in assembly.GetTypes()
                    where t.GetConstructor(Type.EmptyTypes) != null && !t.IsAbstract
                    && typeof(Interface).IsAssignableFrom(t)
                    select (Interface)Activator.CreateInstance(t));
        }
    }
}
