using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Snowflake.Configuration.Attributes;

namespace Snowflake.Configuration
{
    public class IniConfigurationSerializer : ConfigurationSerializer
    {
        public bool OutputHeader { get; set; }
       
        public IniConfigurationSerializer(IBooleanMapping booleanMapping, string nullSerializer, bool outputHeader)
            : base(booleanMapping, nullSerializer)
        {
            this.OutputHeader = outputHeader;
        }

        public override string SerializeLine<T>(string key, T value)
        {
            return $"{key} = {this.SerializeValue(value)}";
        }

        public override string SerializeIterableLine<T>(string key, T value, int iteration)
        {
            return $"{key.Replace(ConfigurationSerializer.IteratorKey, iteration.ToString())} = {this.SerializeValue(value)}";
        }

        public override string Serialize(IConfigurationSection configurationSection)
        {
            StringBuilder stringBuilder = new StringBuilder();
            if(this.OutputHeader) stringBuilder.AppendLine($"[{configurationSection.SectionName}]");

            foreach (var config in configurationSection.Options.Values)
            {
                stringBuilder.AppendLine(this.SerializeLine(config.OptionName, config.Value));
            }
            return stringBuilder.ToString();
        }

        public override string Serialize(IIterableConfigurationSection iterableConfigurationSection)
        {
            StringBuilder stringBuilder = new StringBuilder();

            
            if (this.OutputHeader) stringBuilder.AppendLine($@"[{iterableConfigurationSection.SectionName
                .Replace(ConfigurationSerializer.IteratorKey, iterableConfigurationSection.InterationNumber.ToString())}]");

            foreach (var config in iterableConfigurationSection.Options.Values)
            {
              
                stringBuilder.AppendLine(config.Iterable
                    ? this.SerializeIterableLine(config.OptionName, config.Value, 
                    iterableConfigurationSection.InterationNumber)
                    : this.SerializeLine(config.OptionName, config.Value));
            }
            return stringBuilder.ToString();
        }
    }
}
