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
using Newtonsoft.Json;

namespace Snowflake.Configuration
{
    public class ConfigurationCollectionStore
    {
        private readonly SqliteDatabase backingDatabase;

        public ConfigurationCollectionStore()
        {
          
        }

        /// <summary>
        /// Enums are stored as their string representation
        /// Strings are stored as strings
        /// Primitives are stored as primitive
        /// </summary>
        private void CreateDatabase()
        {
            this.backingDatabase.CreateTable("configuration", 
                    "ConfigurationFilename TEXT",
                    "ConfigurationId TEXT",
                    "GameRecordId TEXT",
                    "OptionKey TEXT",
                    "OptionValue TEXT",
                    "PRIMARY KEY (ConfigurationFilename, ConfigurationId, GameRecordId, OptionKey)");
        }

        private IDictionary<string, IDictionary<string, string>> GetValues(string configurationCollectionName, IGameRecord record)
        {
            this.backingDatabase.Query<IDictionary<string, IDictionary<string, string>>>(dbConnection =>
            {
                dbConnection.Query("SELECT ");
                return null;
            });
            return null;
        }

        /// <summary>
        /// Builds a configuration collection from the ground up using a set of keyed values.
        /// </summary>
        /// <typeparam name="T">The type to build</typeparam>
        /// <param name="values">The values</param>
        /// <returns>The configuration collection</returns>
        private T BuildConfigurationCollection<T>(IDictionary<string, IDictionary<string, string>> values) where T : new()
        {
            var configurationSection = new T();
            foreach (var setter in
                from sectionInfo in typeof(T).GetProperties()
                    let sectionType = sectionInfo.PropertyType
                    where typeof(IConfigurationSection).IsAssignableFrom(sectionType)
                    where sectionType.GetConstructor(Type.EmptyTypes) != null
                    let type = Instantiate.CreateInstance(sectionInfo.PropertyType)
                    select new {setter =  new Action<object>(t => sectionInfo.SetValue(configurationSection, t)), section = type})
            {
                foreach (var optionSetter in 
                    from optionInfo in setter.section.GetType().GetProperties()
                    let optionType = optionInfo.PropertyType
                    where optionInfo.GetCustomAttribute(typeof(ConfigurationOptionAttribute)) != null
                    let strValue = values[setter.section.GetType().Name][setter.section.GetType().Name]
                    let value = optionType == typeof(String) ? strValue //return string value if string
                    : optionType.IsEnum ? Enum.Parse(optionType, strValue, true) //return parsed enum if enum
                    : TypeDescriptor.GetConverter(optionType).ConvertFromInvariantString(strValue)
                    select new Action(() => optionInfo.SetValue(setter.section, value)))
                {
                    optionSetter();
                }
                setter.setter(setter.section);
            }
            return configurationSection;
        }

    }
}
