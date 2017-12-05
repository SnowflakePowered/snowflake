using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Newtonsoft.Json;

namespace Snowflake.Persistence
{
    // todo use simple execute api
    public class SqliteKeyValueStore : SqliteDatabase, ISimpleKeyValueStore
    {
        private class SqliteKeyValueEntry
        {
            public string itemKey { get; set; }
            public string itemValue { get; set; }
        }

        private readonly JsonSerializerSettings jsonSettings;
        public SqliteKeyValueStore(string fileName)
            : base(fileName)
        {
            this.CreateDatabase();
            this.jsonSettings = new JsonSerializerSettings()
            {
                Culture = CultureInfo.InvariantCulture,
                PreserveReferencesHandling = PreserveReferencesHandling.All,
                TypeNameHandling = TypeNameHandling.All,
                ObjectCreationHandling = ObjectCreationHandling.Replace,
            };
        }

        private void CreateDatabase()
        {
            this.CreateTable("kvstore", "itemKey TEXT PRIMARY KEY", "itemValue TEXT", "itemType TEXT");
        }

        /// <inheritdoc/>
        public void DeleteObject(string key)
        {
            this.Execute("DELETE FROM kvstore WHERE itemKey = @key",
                new { key });
        }

        /// <inheritdoc/>
        public void DeleteObjects(IEnumerable<string> keys)
        {
            this.Execute("DELETE FROM kvstore WHERE itemKey IN @keys", new { keys });
        }

        /// <inheritdoc/>
        public void DeleteObjects<T>(IEnumerable<string> keys)
        {
            this.Execute("DELETE FROM kvstore WHERE itemKey IN @keys AND itemType = @itemType",
                   new { keys, itemType = typeof(T).Name });
        }

        /// <inheritdoc/>
        public void DeleteAllObjects<T>()
        {
                this.Execute("DELETE FROM kvstore WHERE itemType = @itemType",
                   new { itemType = typeof(T).Name });
        }

        /// <inheritdoc/>
        public T GetObject<T>(string key)
        {
            string serializedObject =
                this.QueryFirstOrDefault<string>("SELECT itemValue FROM kvstore WHERE itemKey = @itemKey LIMIT 1",
                    new { itemKey = key, itemType = typeof(T).Name });

            return serializedObject == null ? default(T) : JsonConvert.DeserializeObject<T>(serializedObject, this.jsonSettings);
        }

        /// <inheritdoc/>
        public IDictionary<string, T> GetObjects<T>(IEnumerable<string> keys)
        {
            IDictionary<string, T> deserializedObjects = this.Query<SqliteKeyValueEntry>(
                "SELECT * FROM kvstore WHERE itemKey IN @keys",
                    new { keys })
                .ToDictionary(serializedObject => serializedObject.itemKey,
                    serializedObject => (serializedObject.itemValue == null)
                        ? default(T)
                        : JsonConvert.DeserializeObject<T>(serializedObject.itemValue, this.jsonSettings));
            return deserializedObjects;
        }

        /// <inheritdoc/>
        public IDictionary<string, T> GetAllObjects<T>()
        {
            IDictionary<string, T> deserializedObjects = this.Query<SqliteKeyValueEntry>(
                "SELECT * FROM kvstore WHERE itemType = @itemType",
                    new { itemType = typeof(T).Name })
                .ToDictionary(serializedObject => serializedObject.itemKey,
                    serializedObject => (serializedObject.itemValue == null)
                        ? default(T)
                        : JsonConvert.DeserializeObject<T>(serializedObject.itemValue, this.jsonSettings));
            return deserializedObjects;
        }

        /// <inheritdoc/>
        public void InsertObject<T>(string key, T value, bool ignoreIfExistent = false)
        {
            this.Execute($@"INSERT OR {(ignoreIfExistent ? "IGNORE" : "REPLACE")} INTO 
                                  kvstore(itemKey, itemValue, itemType) 
                                  VALUES (@itemKey, @itemValue, @itemType)",
                    new
                    {
                        itemKey = key, itemValue = JsonConvert.SerializeObject(value, this.jsonSettings),
                        itemType = typeof(T).Name,
                    });
        }

        /// <inheritdoc/>
        public void InsertObjects<T>(IDictionary<string, T> keyValuePairs, bool ignoreIfExistent = false)
        {
            this.Execute($@"INSERT OR {(ignoreIfExistent ? "IGNORE" : "REPLACE")} INTO 
                                  kvstore(itemKey, itemValue, itemType) 
                                  VALUES (@itemKey, @itemValue, @itemType)",
                    keyValuePairs
                    .Select(kvp => new
                    {
                        itemKey = kvp.Key,
                        itemValue = JsonConvert.SerializeObject(kvp.Value, this.jsonSettings),
                        itemType = typeof(T).Name,
                    }));
        }
    }
}
