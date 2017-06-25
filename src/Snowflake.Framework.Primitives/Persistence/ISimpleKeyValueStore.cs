using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Persistence
{
    public interface ISimpleKeyValueStore
    {
        /// <summary>
        /// Gets an object from the key value store
        /// </summary>
        /// <typeparam name="T">The type of the object</typeparam>
        /// <param name="key">The key stored</param>
        /// <returns>The object</returns>
        T GetObject<T>(string key);

        /// <summary>
        /// Gets multiple objects of identical type given a list of keys
        /// </summary>
        /// <typeparam name="T">The type of the objects</typeparam>
        /// <param name="keys">The key stored</param>
        /// <returns>The keyed dictionary of the returned objects</returns>
        IDictionary<string, T> GetObjects<T>(IEnumerable<string> keys);

        /// <summary>
        /// Gets multiple objects of a identical type
        /// </summary>
        /// <typeparam name="T">The type of the objects</typeparam>
        /// <param name="keys">The key stored</param>
        /// <returns>The keyed dictionary of the returned objects</returns>

        IDictionary<string, T> GetAllObjects<T>();
        /// <summary>
        /// Inserts or updates an object into the key value store
        /// </summary>
        /// <typeparam name="T">The type of object to insert</typeparam>
        /// <param name="key">The key to insert with</param>
        /// <param name="value">The object to insert</param>
        /// <param name="ignoreIfExistent">Quietly ignore the insert if the object already exists</param>
        void InsertObject<T>(string key, T value, bool ignoreIfExistent = false);

        /// <summary>
        /// Inserts multiple objects of type T into the store
        /// </summary>
        /// <typeparam name="T">The type of the objects to insert</typeparam>
        /// <param name="keyValuePairs">A dictionary with the objects to insert</param>
        /// <param name="ignoreIfExistent">Quietly ignore the insert if the object already exists</param>
        void InsertObjects<T>(IDictionary<string, T> keyValuePairs, bool ignoreIfExistent = false);

        /// <summary>
        /// Deletes an object
        /// </summary>
        /// <param name="key">The key to delete</param>
        void DeleteObject(string key);

        /// <summary>
        /// Deletes multiple objects
        /// </summary>
        /// <param name="keys">The keys to delete</param>
        void DeleteObjects(IEnumerable<string> keys);

        /// <summary>
        /// Deletes multiple objects of a type with the keys
        /// </summary>
        /// <param name="keys">The keys to delete</param>
        void DeleteObjects<T>(IEnumerable<string> keys);

        /// <summary>
        /// Deletes all objects of a type
        /// </summary>
        /// <param name="keys">The keys to delete</param>
        void DeleteAllObjects<T>();
    }
}
