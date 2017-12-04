using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Configuration.Input;
using Snowflake.Input.Controller;
using Snowflake.Input.Controller.Mapped;

namespace Snowflake.Configuration
{
    /// <summary>
    /// Provides a generic key value pair serializer with a 
    /// configuration separator:
    /// 
    /// key separator value
    /// </summary>
    public class KeyValuePairConfigurationSerializer : ConfigurationSerializer
    {
        private readonly string separator;
        public override string SerializeLine<T>(string key, T value)
        {
            return $"{key} {this.separator} \"{this.SerializeValue(value)}\"";
        }

        public override string Serialize(IConfigurationSection configurationSection)
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var config in from option in configurationSection.Descriptor.Options where !option.Flag select option)
            {
                stringBuilder.AppendLine(this.SerializeLine(config.OptionName, configurationSection.Values[config.OptionKey].Value));
            }
            return stringBuilder.ToString();
        }

        public KeyValuePairConfigurationSerializer(IBooleanMapping booleanMapping, string nullSerializer, string separator)
            : base(booleanMapping, nullSerializer)
        {
            this.separator = separator;
        }
    }
}
