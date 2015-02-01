using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Extensions;
using Newtonsoft.Json;

namespace Snowflake.Emulator.Configuration
{
    public class ConfigurationFlag : Snowflake.Emulator.Configuration.IConfigurationFlag
    {
        public string Key { get; private set; }
        public ConfigurationFlagTypes Type { get; private set; }
        public string DefaultValue { get; private set; }
        public string Description { get; private set; }
        public int RangeMin { get; private set; }
        public int RangeMax { get; private set; }
        public IReadOnlyDictionary<string, string> SelectValues { get; private set; }
        
        public ConfigurationFlag(string key, ConfigurationFlagTypes type, string defaultValue, string description, int rangeMin = 0, int rangeMax = 0, IDictionary<string, string> selectValues = null)
        {
            this.Key = key;
            this.Type = type;
            this.DefaultValue = defaultValue;
            this.Description = description;
            this.RangeMin = rangeMin;
            this.RangeMax = rangeMax;
            if (selectValues != null)
            {
                this.SelectValues = selectValues.AsReadOnly();
            }
            else
            {
                this.SelectValues = null;
            }
        }

        public static IConfigurationFlag FromJsonProtoTemplate(IDictionary<string, dynamic> protoTemplate){
            string key = protoTemplate["key"];
            ConfigurationFlagTypes type;
            if (!Enum.TryParse<ConfigurationFlagTypes>(protoTemplate["type"], out type))
                throw new ArgumentException("type can be one of BOOLEAN_FLAG, INTEGER_FLAG, SELECT_FLAG. Fix your emulator plugin.");
            string description = protoTemplate["description"];
            string defaultValue = protoTemplate["default"].ToString();
            dynamic max = 0;
            dynamic min = 0;
            dynamic selectTypes;
            protoTemplate.TryGetValue("max", out max);
            protoTemplate.TryGetValue("min", out min);
            protoTemplate.TryGetValue("values", out selectTypes);
            try
            {
                selectTypes.ToObject(typeof(IDictionary<string, string>));
            }
            catch (NullReferenceException)
            {
                selectTypes = null;

            }
            
            return new ConfigurationFlag(key, type, defaultValue, description, max, min, selectTypes);

        }
    }
}
