using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Globalization;
using Dapper;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Snowflake.Utility
{


    public class SqliteKeyValueStore : BaseDatabase, ISimpleKeyValueStore
    {
        private class SqliteKeyValueEntry
        {
            public string itemKey { get; set; }
            public string itemValue { get; set; }
        }

        private JsonSerializerSettings jsonSettings;
        public SqliteKeyValueStore(string fileName) : base(fileName)
        {
            this.CreateDatabase();
            this.jsonSettings = new JsonSerializerSettings()
            {
                Culture = CultureInfo.InvariantCulture,
                PreserveReferencesHandling = PreserveReferencesHandling.All,
                TypeNameHandling = TypeNameHandling.Auto
            };
        }

        private void CreateDatabase()
        {
            using (SQLiteConnection dbConnection = this.GetConnection())
            {
                dbConnection.Open();
                var sqlCommand = new SQLiteCommand(@"CREATE TABLE IF NOT EXISTS kvstore(
                                                                itemKey TEXT PRIMARY KEY,
                                                                itemValue TEXT
                                                                )", dbConnection);
                sqlCommand.ExecuteNonQuery();
                dbConnection.Close();
            }
        }

        public void DeleteObject(string key)
        {
            using (var dbConnection = this.GetConnection())
            {
                dbConnection.Execute("DELETE FROM kvstore WHERE itemKey = @itemKey",
                    new { itemKey = key });
            }
        }

        public void DeleteObjects(IEnumerable<string> keys)
        {
            using (var dbConnection = this.GetConnection())
            {
                dbConnection.Execute("DELETE FROM kvstore WHERE itemKey IN @itemKeys",
                   new { itemKeys = keys });
            }
        }

        public T GetObject<T>(string key)
        {
            string serializedObject;
            using (var dbConnection = this.GetConnection())
            {
                serializedObject = dbConnection.Query<string>("SELECT itemValue FROM kvstore WHERE itemKey = @itemKey",
                   new { itemKey = key }).FirstOrDefault();
            }
            return !String.IsNullOrWhiteSpace(serializedObject) ? 
                JsonConvert.DeserializeObject<T>(serializedObject, this.jsonSettings) : default(T);
        }

        public IDictionary<string, T> GetObjects<T>(IEnumerable<string> keys)
        {
            IEnumerable<SqliteKeyValueEntry> serializedObjects;
            using (var dbConnection = this.GetConnection())
            {
                serializedObjects = dbConnection.Query<SqliteKeyValueEntry>("SELECT * FROM kvstore WHERE itemKey IN @itemKeys",
                    new {itemKeys = keys});
            }

            IDictionary<string, T> deserializedObjects = serializedObjects
                .ToDictionary(serializedObject => serializedObject.itemKey,
                    serializedObject => !String.IsNullOrWhiteSpace(serializedObject.itemValue)
                        ? JsonConvert.DeserializeObject<T>(serializedObject.itemValue, this.jsonSettings)
                        : default(T));
            return deserializedObjects;

        }

        public void InsertObject<T>(string key, T value, bool ignoreIfExistent = false)
        {
            using (var dbConnection = this.GetConnection())
            {
                dbConnection.Execute($"INSERT OR {(ignoreIfExistent ? "IGNORE" : "REPLACE")} INTO kvstore(itemKey, itemValue) VALUES (@itemKey, @itemValue)",
                    new {itemKey = key, itemValue = JsonConvert.SerializeObject(value, this.jsonSettings)});
            }
        }

        public void InsertObjects<T>(IDictionary<string, T> keyValuePairs, bool ignoreIfExistent = false) 
        {
            using (var dbConnection = this.GetConnection())
            {
                dbConnection.Execute($"INSERT OR {(ignoreIfExistent ? "IGNORE" : "REPLACE")} INTO kvstore(itemKey, itemValue) VALUES (@itemKey, @itemValue)",
                    keyValuePairs
                    .Select(kvp => new
                    {
                        itemKey = kvp.Key,
                        itemValue = JsonConvert.SerializeObject(kvp.Value, this.jsonSettings)
                    }));
            }
        }
    }
}
