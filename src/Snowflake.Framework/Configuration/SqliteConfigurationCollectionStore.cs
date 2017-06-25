using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Dapper;
using EnumsNET.NonGeneric;
using Snowflake.Persistence;
using System.Data.Common;

namespace Snowflake.Configuration
{
    internal class SqliteConfigurationCollectionStore : IConfigurationCollectionStore
    {
        private readonly ISqlDatabase backingDatabase;

        internal SqliteConfigurationCollectionStore(ISqlDatabase sqliteDatabase)
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
                "game UUID",
                "value TEXT",
                "option TEXT",
                "section TEXT",
                "emulator TEXT",
                "profile TEXT",
                "PRIMARY KEY (game, option, section, emulator, profile)");
        }

        public IConfigurationCollection<T> Get<T>(Guid gameRecord, string emulator, string profile) where T: class, IConfigurationCollection<T>
        {
            var records = this.backingDatabase.Query(dbConnection =>
            {
                return dbConnection.Query<ConfigurationRecord>(
                    "SELECT * FROM configuration WHERE game == @gameRecord AND emulator == @emulator AND profile == @profile", new
                    {
                        gameRecord,
                        emulator,
                        profile
                    });
            });
            
            var defs = records.GroupBy(p => p.section)
                .ToDictionary(p => p.Key, p => p
                .ToDictionary(o => o.option, o => 
                new ValueTuple<string, Guid>(o.value, new Guid(o.uuid))) 
                as IDictionary<string, ValueTuple<string, Guid>>);
            return new ConfigurationCollection<T>(defs);
        }

        public void Set<T>(IConfigurationCollection<T> configuration, Guid gameRecord, string emulator, string profile) 
            where T: class, IConfigurationCollection<T>
        {
            var values = from section in configuration
                from value in section.Value.Values
                select new
                {
                    uuid = value.Value.Guid,
                    game = gameRecord,
                    value = value.Value.Value.GetType().GetTypeInfo().IsEnum ? 
                            NonGenericEnums.GetName(value.Value.Value.GetType(), value.Value.Value) : //optimized path for enums
                            Convert.ToString(value.Value.Value), //so i put a value in your value so you can value values
                    option = value.Key,
                    section = section.Key, 
                    emulator,
                    profile
                };
            this.backingDatabase.Execute(dbConnection =>
            {
                dbConnection.Execute(
                    @"INSERT OR REPLACE INTO configuration (uuid, game, value, option, section, emulator, profile) VALUES
                      (@uuid, @game, @value, @option, @section, @emulator, @profile)", values);
            });
        }

        public void Set(IConfigurationValue value)
        {
            try
            {
                this.backingDatabase.Execute(dbConnection =>
                {
                    dbConnection.Execute(
                        @"UPDATE configuration SET value = @Value WHERE uuid == @Guid", new
                        {
                            value.Value,
                            value.Guid
                        });
                });
            }
            catch (DbException)
            {
                throw new KeyNotFoundException("Value GUID was not found in store.");
            }
        }

    }

    class ConfigurationRecord
    {
        public byte[] uuid;
        public byte[] game;
        public string value;
        public string option;
        public string section;
        public string emulator;
        public string profile;
    }
}
