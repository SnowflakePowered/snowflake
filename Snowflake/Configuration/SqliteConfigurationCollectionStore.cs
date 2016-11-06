using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Configuration.Attributes;
using Snowflake.Configuration.Input;
using Snowflake.Records.Game;
using Snowflake.Utility;
using Dapper;
using FastMember;
using Newtonsoft.Json;

namespace Snowflake.Configuration
{
    public class SqliteConfigurationCollectionStore
    {
        private readonly SqliteDatabase backingDatabase;

        public SqliteConfigurationCollectionStore(SqliteDatabase sqliteDatabase)
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
                "uuid UUID PRIMARY KEY",
                "game UUID",
                "value TEXT",
                "option TEXT",
                "section TEXT",
                "emulator TEXT",
                "profile TEXT");
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
                from value in section.Values
                select new
                {
                    uuid = value.Value.Guid,
                    game = gameRecord,
                    value = value.Value.Value.ToString(), //so i put a value in your value so you can value values
                    option = value.Key,
                    section = (from descript in configuration.Descriptor.SectionDescriptors
                              where descript.Value == section.Descriptor
                              select descript.Key).First(), //todo this feels really gross.
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
