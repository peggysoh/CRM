using System;
using System.Collections.Specialized;
using System.Linq;

namespace OgsysCRM.Extensions
{
    public static class NameValueCollectionDataTableExtension
    {
        /// <summary>
        /// Gets a typed item from the collection using the provided key.
        /// If there's no corresponding item on the collection, returns default(T).
        /// </summary>
        /// <typeparam name="T">The type to cast the collection item.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="key">The key to access the item inside the collection.</param>
        /// <returns>The typed item.</returns>
        public static T G<T>(this NameValueCollection collection, string key)
        {
            return G<T>(collection, key, default(T));
        }

        /// <summary>
        /// Gets a typed item from the collection using the provided key.
        /// If there's no corresponding item on the collection, returns the provided defaultValue.
        /// </summary>
        /// <typeparam name="T">The type to cast the collection item.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="key">The key to access the item inside the collection.</param>
        /// <param name="defaultValue">The default value to return if there's no corresponding item on the collection.</param>
        /// <returns>The typed item.</returns>
        public static T G<T>(this NameValueCollection collection, string key, object defaultValue)
        {
            if (collection == null) throw new ArgumentNullException("collection", "The provided collection cannot be null.");
            if (String.IsNullOrWhiteSpace(key)) throw new ArgumentException("The provided key cannot be null or empty.", "key");

            var collectionItem = collection[key];
            if (collectionItem == null) return (T)defaultValue;
            return (T)Convert.ChangeType(collectionItem, typeof(T));
        }
        /// <summary>
        /// Sets or updates a value inside the provided collection.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="key">The key to access the item inside the collection.</param>
        /// <param name="value">The value to be set or updated.</param>
        public static void S(this NameValueCollection collection, string key, object value)
        {
            if (collection == null) throw new ArgumentNullException("collection", "The provided collection cannot be null.");
            if (String.IsNullOrWhiteSpace(key)) throw new ArgumentException("The provided key cannot be null or empty.", "key");
            if (value == null) throw new ArgumentNullException("value", "The provided value cannot be null.");

            if (collection.Keys.Cast<string>().Any(_k => _k.Equals(key)))
                collection[key] = value.ToString();
            else
                collection.Add(key, value.ToString());
        }
    }
}