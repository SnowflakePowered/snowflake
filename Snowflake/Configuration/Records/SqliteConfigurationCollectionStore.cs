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
using Snowflake.Configuration.Records;

namespace Snowflake.Configuration.Records
{
    public class SqliteConfigurationCollectionStore : IConfigurationCollectionStore
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
        /// 
        /// </summary>
        private void CreateDatabase()
        {
            this.backingDatabase.CreateTable("configuration", 
                    "uuid UUID PRIMARY KEY",
                    "game UUID",
                    "section UUID",
                    "value TEXT");
        }

        public void Set(IConfigurationCollection collection, Guid gameRecord)
        {
            this.Set(from section in collection
                from option in section.Options.Values
                select option.GetValue(gameRecord));
        }

        private class ConfigurationCollectionStoreRecord
        {
            internal byte[] game { get; set; }
            internal byte[] uuid { get; set; }
            internal byte[] section { get; set; }
            internal string value { get; set; }
            
        }

       
        private static Guid GetKeyedGuid(Guid record, Guid sectionGuid, string keyName) => GuidUtility.Create(record, $"{sectionGuid}::{keyName}");

        public void Set(IConfigurationValue record)
        {
            this.backingDatabase.Execute(dbConnection =>
            {
                dbConnection.Execute(@"INSERT OR REPLACE INTO configuration(uuid, game, section, value) 
                                         VALUES (@uuid, @game, @section, @value)", new
                {
                    uuid = record.Guid,
                    game = record.Record,
                    section = record.Section,
                    value = record.Value.ToString()
                });
            });
        }

        public T Get<T>(Guid gameRecord) where T : IConfigurationCollection, new()
        {
            const string sql = @"SELECT * FROM configuration WHERE game = @gameRecord AND section IN @sectionGuids";
            var configurationCollection = new T();
            var accessor = TypeAccessor.Create(typeof(T));
            foreach (var section in
                from sectionInfo in accessor.GetMembers()
                let sectionType = sectionInfo.Type
                where typeof(IConfigurationSection).IsAssignableFrom(sectionType)
                where sectionType.GetConstructor(Type.EmptyTypes) != null
                let type = Instantiate.CreateInstance(sectionInfo.Type)
                select new  {sectionInfo,  type})
            {
                ((ConfigurationSection) section.type).ConfigFilename = configurationCollection.FileName;
                accessor[configurationCollection, section.sectionInfo.Name] = section.type;
            }

            IList<Guid> sectionGuids = (from section in configurationCollection select section.SectionGuid).ToList();
            var values = this.backingDatabase.Query(dbConnection =>
            {
                return dbConnection.Query<ConfigurationCollectionStoreRecord>(sql, new
                {
                    gameRecord,
                    sectionGuids
                }).ToLookup(c => new Guid(c.section), c => c);
            });

            //create each section individually through reflection

                foreach (var option in
                   from section in configurationCollection
                   from optionInfo in section.GetType().GetProperties()
                   let optionType = optionInfo.PropertyType
                   let optionAttr = (ConfigurationOptionAttribute)optionInfo.GetCustomAttribute(typeof(ConfigurationOptionAttribute))
                   where optionAttr != null
                   let keyName = optionInfo.Name
                   let sectionName = section.SectionName
                   let sectionGuid = section.SectionGuid
                   where values[sectionGuid].Any()
                   let strValue = (from value in values[sectionGuid] where new Guid(value.uuid) == GetKeyedGuid(gameRecord, sectionGuid, keyName)
                                  select value).FirstOrDefault()?.value
                   where strValue != null
                   let value = optionType == typeof(string) ? strValue //return string value if string
                    : optionType.IsEnum ? Enum.Parse(optionType, strValue, true) //return parsed enum if enum
                    : TypeDescriptor.GetConverter(optionType).ConvertFromInvariantString(strValue)
                   select new {optionInfo, value, section})
                {
                    option.optionInfo.SetValue(option.section, option.value);
                }
            
            return configurationCollection;
        }

        public void Set(IEnumerable<IConfigurationValue> records)
        {
            this.backingDatabase.Execute(dbConnection =>
            {
                dbConnection.Execute(@"INSERT OR REPLACE INTO configuration(uuid, game, section, value) 
                                         VALUES (@uuid, @game, @section, @value)", 
                                         records.Select(record => new
                {
                    uuid = record.Guid,
                    game = record.Record,
                    section = record.Section,
                    value = record.Value.ToString()
                }));
            });
        }

        public void Remove(IConfigurationValue record) => this.Remove(record.Guid);

        public void Remove(IEnumerable<IConfigurationValue> records) => this.Remove(records.Select(r => r.Guid));

        public void Remove(Guid guid)
        {
            this.backingDatabase.Execute(dbConnection =>
            {
                dbConnection.Execute(@"DELETE FROM configuration WHERE uuid = @guid", new {guid});
            });
        }

        public void Remove(IEnumerable<Guid> guids)
        {
            this.backingDatabase.Execute(dbConnection =>
            {
                dbConnection.Execute(@"DELETE FROM configuration WHERE uuid IN @guids", new { guids });
            });
        }

        /// <summary>
        /// Getting an individual configuration value does not make sense, so this method is not supported.
        /// </summary>
        /// <param name="guid">/param>
        /// <returns></returns>
        [Obsolete("Not Supported")]
        public IConfigurationValue Get(Guid guid)
        {
            throw new NotSupportedException("Getting individual configuration values without associated section are meaningless.");
        }

        /// <summary>
        /// Getting an individual configuration value does not make sense, so this method is not implemented.
        /// </summary>
        /// <param name="guid">/param>
        /// <returns></returns>
        [Obsolete("Not Supported")]
        public IEnumerable<IConfigurationValue> Get(IEnumerable<Guid> guids)
        {
            throw new NotSupportedException("Getting individual configuration values without associated section are meaningless.");
        }

        /// <summary>
        /// Getting an individual configuration value does not make sense, so this method is not implemented.
        /// </summary>
        /// <param name="guid">/param>
        /// <returns></returns>
        [Obsolete("Not Supported")]
        public IEnumerable<IConfigurationValue> GetAllRecords()
        {
            throw new NotSupportedException("Getting individual configuration values without associated section are meaningless.");
        }
    }
}
