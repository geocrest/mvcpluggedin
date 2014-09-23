
namespace Geocrest.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.Serialization;
    /// <summary>
    /// Provides all extension methods in this assembly.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Converts an object's public <see cref="T:System.Runtime.Serialization.DataMemberAttribute"/>
        /// properties and their values to a dictionary of property names and values.
        /// </summary>
        /// <param name="obj">The object whose properties will be added to the dictionary.</param>
        /// <returns>
        /// Returns an <see cref="T:System.Collections.Generic.IDictionary`2"/> of 
        /// <see cref="T:System.Collections.Generic.KeyValuePair`2"/>s of <see cref="T:System.String"/>
        /// and <see cref="T:System.Object"/>.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">obj</exception>
        public static IDictionary<string, object> ToDictionary(this object obj)
        {
            if (obj == null) throw new ArgumentNullException("obj");
            IDictionary<string, object> dict = new Dictionary<string, object>();
            var props = (from p in obj.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public)
                         where p.GetCustomAttributes(typeof(DataMemberAttribute), true).Length > 0
                         select p);
            if (props.Count() > 0)
            {
                foreach (var prop in props)
                {
                    if (prop.GetGetMethod() != null)
                    {
                        dict.Add(prop.Name, prop.GetValue(obj, null));
                    }
                }
            }
            return dict;
        }
        /// <summary>
        /// Returns the public <see cref="T:System.Runtime.Serialization.DataMemberAttribute"/>
        /// properties of an object.
        /// </summary>
        /// <param name="type">The type of object whose properties will be returned.</param>
        /// <returns>
        /// Returns a list of property names.
        /// </returns>
        public static IList<string> ToPropertyList(this Type type)
        {
            IList<string> list = new List<string>();
            var props = (from p in type.GetProperties(BindingFlags.Instance | BindingFlags.Public)
                         where p.GetCustomAttributes(typeof(DataMemberAttribute), true).Length > 0
                         select p);
            if (props.Count() > 0)
            {
                foreach (var prop in props)
                {
                    if (prop.GetGetMethod() != null)
                    {
                        list.Add(prop.Name);
                    }
                }
            }
            return list;
        }
    }
}
