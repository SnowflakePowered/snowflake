using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using Dapper;
using EnumsNET.NonGeneric;
using Snowflake.Configuration;
using Snowflake.Persistence;

namespace Snowflake.Extensibility.Configuration
{
    internal class SqlitePluginConfigurationStore : IPluginConfigurationStore
    {
        private readonly ISqlDatabase backingDatabase;

        internal SqlitePluginConfigurationStore(ISqlDatabase sqliteDatabase)
        {
            this.backingDatabase = sqliteDatabase;
            this.CreateDatabase();
        }

        /// <summary>
        /// Enums are stored as their string representation
        /// Strings are stored as strings
        /// Primitives are stored as primitive
        /// </summary>
        private void CreateDatabase()
        {
            this.backingDatabase.CreateTable("configuration",
                "uuid UUID",
                "value TEXT",
                "option TEXT",
                "configurationType TEXT",
                "PRIMARY KEY (uuid)");
        }

        /// <inheritdoc/>
        public IConfigurationSection<T> Get<T>()
            where T : class, IConfigurationSection<T>
        {
            string typeName = typeof(T).FullName;
            var records = this.backingDatabase.Query(dbConnection =>
            {
                return dbConnection.Query<ConfigurationRecord>(
                    "SELECT * FROM configuration WHERE configurationType == @typeName", new { typeName });
            });

            var defs = records.ToDictionary(p => p.option, p => (p.value, new Guid(p.uuid)));
            return new ConfigurationSection<T>(defs);
        }

        /// <inheritdoc/>
        public void Set<T>(IConfigurationSection<T> configuration)
            where T : class, IConfigurationSection<T>
        {
            var values = from value in configuration.Values
                select new
                {
                    uuid = value.Value.Guid,
                    value = value.Value.Value.GetType().GetTypeInfo().IsEnum ?
                            NonGenericEnums.GetName(value.Value.Value.GetType(), value.Value.Value) : // optimized path for enums
                            Convert.ToString(value.Value.Value), // so i put a value in your value so you can value values
                    option = value.Key,
                    typeName = typeof(T).FullName,
                };
            this.backingDatabase.Execute(dbConnection =>
            {
                dbConnection.Execute(
                    @"INSERT OR REPLACE INTO configuration (uuid, value, option, configurationType) VALUES
                      (@uuid, @value, @option, @typeName)", values);
            });
        }

        /// <inheritdoc/>
        public void Set(IConfigurationValue value)
        {
            try
            {
                this.backingDatabase.Execute(dbConnection =>
                {
                    dbConnection.Execute(
                        @"UPDATE configuration SET value = @Value WHERE uuid == @Guid", new
                        {
                            Value = value.Value.GetType().GetTypeInfo().IsEnum ?
                            NonGenericEnums.GetName(value.Value.GetType(), value.Value) : // optimized path for enums
                            Convert.ToString(value.Value), // so i put a value in your value so you can value values,
                            Guid = value.Guid,
                        });
                });
            }
            catch (DbException)
            {
                throw new KeyNotFoundException("Value GUID was not found in store.");
            }
        }

        public void Set(IEnumerable<IConfigurationValue> values)
        {
            try
            {
                this.backingDatabase.Execute(dbConnection =>
                {
                    dbConnection.Execute(
                        @"UPDATE configuration SET value = @Value WHERE uuid == @Guid", values.Select(value => new
                        {
                            Value = value.Value.GetType().GetTypeInfo().IsEnum ?
                            NonGenericEnums.GetName(value.Value.GetType(), value.Value) : // optimized path for enums
                            Convert.ToString(value.Value), // so i put a value in your value so you can value values,
                            Guid = value.Guid,
                        }));
                });
            }
            catch (DbException)
            {
                throw new KeyNotFoundException("Value GUIDS was not found in store.");
            }
        }
    }

    internal class ConfigurationRecord
    {
        public byte[] uuid;
        public string value;
        public string option;
    }
}
