using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Configuration
{
    public abstract class KeyValuePairConfigurationSerializer : ConfigurationSerializer
    {
        private readonly string separator;
        public override string SerializeLine<T>(string key, T value)
        {
            return $"{key} {this.separator} \"{this.SerializeValue(value)}\"";
        }

        public override string Serialize(IConfigurationSection configurationSection)
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var config in configurationSection.Options.Values)
            {
                stringBuilder.AppendLine(this.SerializeLine(config.OptionName, config.Value));
            }
            return stringBuilder.ToString();
        }

        protected KeyValuePairConfigurationSerializer(IBooleanMapping booleanMapping, string nullSerializer, string separator)
            : base(booleanMapping, nullSerializer)
        {
            this.separator = separator;
        }
    }
}
