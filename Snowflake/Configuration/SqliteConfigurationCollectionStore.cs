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
                    "SELECT * FROM configuration WHERE game == @gameRecord AND emulator == @emulator AND profile == @profile");
            });
            var defs = records.GroupBy(p => p.section)
                .ToDictionary(p => p.Key, p => p
                .ToDictionary(o => o.option, o => new ConfigurationValue(o.value, o.uuid) as IConfigurationValue) as IDictionary<string, IConfigurationValue>); //how to restore without guid?

                //IDictionary<string, IDictionary<string, IConfigurationOption>>
            var template = new ConfigurationCollection<T>(defs);
            return null;
        }
        

    }

    class ConfigurationRecord
    {
        public Guid uuid;
        public Guid game;
        public string value;
        public string option;
        public string section;
        public string emulator;
        public string profile;
    }
}
