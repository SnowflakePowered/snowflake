using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Extensions;
using Newtonsoft.Json;

namespace Snowflake.Emulator.Configuration.Flags
{
    public class ConfigurationFlag
    {
        public string Key { get; private set; }
        public ConfigurationFlagTypes Type { get; private set; }
        public string DefaultValue { get; private set; }
        public string Description { get; private set; }
        public IReadOnlyDictionary<string, string> SelectValues { get; private set; }
        public ConfigurationFlag(string key, ConfigurationFlagTypes type, string defaultValue, string description, IDictionary<string, string> selectValues = null)
        {
            this.Key = key;
            this.Type = type;
            this.DefaultValue = defaultValue;
            this.Description = description;
            if (selectValues != null)
            {
                this.SelectValues = selectValues.AsReadOnly();
            }
            else
            {
                this.SelectValues = null;
            }
        }

        public static ConfigurationFlag FromDictionary(IDictionary<string, dynamic> protoTemplate){
            string key = protoTemplate["key"];
            ConfigurationFlagTypes type;
            if (!Enum.TryParse<ConfigurationFlagTypes>(protoTemplate["type"], out type))
                throw new ArgumentException("type can be one of BOOLEAN_FLAG, INTEGER_FLAG, SELECT_FLAG. Fix your emulator plugin.");
            string description = protoTemplate["description"];
            string defaultValue = protoTemplate["default"].ToString();
            try
            {
                IDictionary<string, string> selectTypes = protoTemplate["values"].ToObject(typeof(IDictionary<string, string>));
                return new ConfigurationFlag(key, type, defaultValue, description, selectTypes);

            }
            catch (KeyNotFoundException)
            {
                return new ConfigurationFlag(key, type, defaultValue, description);

            }
        }
        public static IList<ConfigurationFlag> FromManyDictionaries(IList<IDictionary<string, dynamic>> protoTemplates)
        {
            return protoTemplates.Select(protoTemplate => ConfigurationFlag.FromDictionary(protoTemplate)).ToList();
        }

    }
    public enum ConfigurationFlagTypes
    {
        BOOLEAN_FLAG,
        INTEGER_FLAG,
        SELECT_FLAG
    }
}
