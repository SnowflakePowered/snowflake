using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Extensions;
using Newtonsoft.Json;

namespace Snowflake.Emulator.Configuration
{
    public class ConfigurationFlag : IConfigurationFlag
    {
        public string Key { get; }
        public ConfigurationFlagTypes Type { get; }
        public string DefaultValue { get; }
        public string Description { get; }
        public int RangeMin { get; }
        public int RangeMax { get; }
        public IList<IConfigurationFlagSelectValue> SelectValues { get; }

        public ConfigurationFlag(string key, ConfigurationFlagTypes type, string defaultValue, string description, int rangeMin = 0, int rangeMax = 0, IList<IConfigurationFlagSelectValue> selectValues = null)
        {
            this.Key = key;
            this.Type = type;
            this.DefaultValue = defaultValue;
            this.Description = description;
            this.RangeMin = rangeMin;
            this.RangeMax = rangeMax;
            this.SelectValues = selectValues;
        }

        public static IConfigurationFlag FromJsonProtoTemplate(IDictionary<string, dynamic> protoTemplate){
            string key = protoTemplate["key"];
            ConfigurationFlagTypes type;
            if (!Enum.TryParse<ConfigurationFlagTypes>(protoTemplate["type"], out type))
                throw new ArgumentException("type can be one of BOOLEAN_FLAG, INTEGER_FLAG, SELECT_FLAG. Fix your emulator plugin.");
            string description = protoTemplate["description"];
            string defaultValue = protoTemplate["default"].ToString();
            int max = 0;
            int min = 0;
            IList<IConfigurationFlagSelectValue> selectTypes = null;
            if (protoTemplate.ContainsKey("max"))
            {
                max = (int)protoTemplate["max"];
            }
            if (protoTemplate.ContainsKey("min"))
            {
                min = (int)protoTemplate["min"];
            }
            if (protoTemplate.ContainsKey("values"))
            {
                selectTypes = ((IList<ConfigurationFlagSelectValue>)protoTemplate["values"].ToObject(typeof(IList<ConfigurationFlagSelectValue>)))
                    .Select(x => (IConfigurationFlagSelectValue)x).ToList();
            }
           
            return new ConfigurationFlag(key, type, defaultValue, description, (int)max, (int)min, selectTypes);

        }
    }
}
