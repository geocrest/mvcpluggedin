namespace Geocrest.Web.Infrastructure
{
    using System.Collections.Generic;
    using System.Collections.Specialized;

    /// <summary>
    /// Provides extension methods for use with <see cref="T:System.Collections.Generic.IDictionary`2"/> instances.
    /// </summary>
    public static class DictionaryExtensions
    {
        /// <summary>
        /// Converts a dictionary of values to a name/value collection.
        /// </summary>
        /// <typeparam name="TKey">The type of key for the input dictionary.</typeparam>
        /// <typeparam name="TValue">The type of value for the input dictionary.</typeparam>
        /// <param name="dictionary">The dictionary instance to convert.</param>
        /// <returns>
        /// A name/value collection of items from the dictionary.
        /// </returns>
        public static NameValueCollection ToNameValueCollection<TKey,TValue>(this IDictionary<TKey, TValue> dictionary)
        {
            Throw.IfArgumentNull(dictionary, "dictionary");
            var nameValueCollection = new NameValueCollection();

            foreach (var kvp in dictionary)
            {
                string value = null;
                if (kvp.Value != null)
                    value = kvp.Value.ToString();

                nameValueCollection.Add(kvp.Key.ToString(), value);
            }

            return nameValueCollection;
        }
    }
}
