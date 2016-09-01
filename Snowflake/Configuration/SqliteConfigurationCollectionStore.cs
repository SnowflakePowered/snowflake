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
        /// </summary>
        private void CreateDatabase()
        {
            this.backingDatabase.CreateTable("configuration", 
                    "CollectionFilename TEXT",
                    "SectionName TEXT",
                    "GameRecordId TEXT",
                    "OptionKey TEXT",
                    "OptionValue TEXT",
                    "PRIMARY KEY (CollectionFilename, SectionName, GameRecordId, OptionKey)");
        }

        public void SetConfiguration(IConfigurationCollection collection, Guid gameRecord)
        {
            var values = from section in collection
                from option in section.Options
                select new 
                {
                    CollectionFilename = collection.FileName,
                    SectionName = section.SectionName,
                    GameRecordId = gameRecord.ToString(),
                    OptionKey = option.Value.OptionName,
                    OptionValue = option.Value.Value.ToString()
                };
            this.backingDatabase.Execute(dbConnection =>
            {
                dbConnection.Execute(
                    @"INSERT OR REPLACE INTO configuration (CollectionFilename, SectionName, GameRecordId, OptionKey, OptionValue) VALUES 
                    (@CollectionFilename, @SectionName, @GameRecordId, @OptionKey, @OptionValue)", values);
            });
        }

        public T GetConfiguration<T>(string collectionFilename, Guid gameRecord) where T : IConfigurationCollection, new()
        {
            var values = this.GetValues(collectionFilename, gameRecord);
            return values.Any() ? this.BuildConfigurationCollection<T>(values) : ConfigurationCollection.MakeDefault<T>();
        }

        public T GetConfiguration<T>(Guid gameRecord) where T : IConfigurationCollection, new()
            => this.GetConfiguration<T>(new T().FileName, gameRecord);

        /// <summary>
        /// Maps values keyed on configuration values keyed on option keys
        /// </summary>
        /// <param name="collectionFilename"></param>
        /// <param name="record"></param>
        /// <returns></returns>
        private IDictionary<string, IDictionary<string, string>> GetValues(string collectionFilename, Guid record)
        {
            return this.backingDatabase.Query<IDictionary<string, IDictionary<string, string>>>(dbConnection =>
            {
                var retValues = new Dictionary<string, IDictionary<string, string>>();
                var values = dbConnection.Query<ConfigurationCollectionStoreRecord>("SELECT * FROM configuration WHERE CollectionFilename = @CollectionFilename AND GameRecordId = @GameRecordId", 
                    new {CollectionFilename = collectionFilename, GameRecordId = record.ToString()});
                foreach (var options in values)
                {
                    if(!retValues.ContainsKey(options.SectionName))
                        retValues[options.SectionName] = new Dictionary<string, string>();
                    retValues[options.SectionName][options.OptionKey] = options.OptionValue;
                }
               return retValues;
            });
        }

        /// <summary>
        /// Builds a configuration collection from the ground up using a set of keyed values.
        /// </summary>
        /// <typeparam name="T">The type to build</typeparam>
        /// <param name="values">The values</param>
        /// <returns>The configuration collection</returns>
        private T BuildConfigurationCollection<T>(IDictionary<string, IDictionary<string, string>> values) where T : IConfigurationCollection, new()
        {
            var configurationCollection = new T();
            var accessor = TypeAccessor.Create(typeof(T));
            foreach (var setter in
                from sectionInfo in accessor.GetMembers()
                    let sectionType = sectionInfo.Type
                    where typeof(IConfigurationSection).IsAssignableFrom(sectionType)
                    where sectionType.GetConstructor(Type.EmptyTypes) != null
                    let type = Instantiate.CreateInstance(sectionInfo.Type)
                    select new {setter =  new Action<object>(t => accessor[configurationCollection, sectionInfo.Name] = t),
                        section = type})
            {
                foreach (var optionSetter in 
                    from optionInfo in setter.section.GetType().GetProperties()
                    let optionType = optionInfo.PropertyType
                    let optionAttr = (ConfigurationOptionAttribute)optionInfo.GetCustomAttribute(typeof(ConfigurationOptionAttribute))
                    where optionAttr != null
                    let sectionName = ((IConfigurationSection)setter.section).SectionName
                    where values.ContainsKey(sectionName)
                    where values[sectionName].ContainsKey(optionAttr.OptionName)
                    let strValue = values[sectionName][optionAttr.OptionName]
                    let value = optionType == typeof(String) ? strValue //return string value if string
                    : optionType.IsEnum ? Enum.Parse(optionType, strValue, true) //return parsed enum if enum
                    : TypeDescriptor.GetConverter(optionType).ConvertFromInvariantString(strValue)
                    select new Action(() => optionInfo.SetValue(setter.section, value)))
                    optionSetter();
                
                setter.setter(setter.section);
            }
            return configurationCollection;
        }

        private class ConfigurationCollectionStoreRecord
        {

            internal string CollectionFilename { get; set; }
            internal string SectionName { get; set; }
            internal string GameRecordId { get; set; }
            internal string OptionKey { get; set; }
            internal string OptionValue { get; set; }
        }
    }
}
