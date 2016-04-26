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
    public abstract class IniConfigurationSerializer : ConfigurationSerializer
    {
        private readonly bool outputHeader;
       
        protected IniConfigurationSerializer(IBooleanMapping booleanMapping, string nullSerializer, bool outputHeader)
            : base(booleanMapping, nullSerializer)
        {
            this.outputHeader = outputHeader;
        }

        public override string SerializeLine<T>(string key, T value)
        {
            return $"{key} = {this.SerializeValue(value)}";
        }

        public override string Serialize(IConfigurationSection configurationSection)
        {
            StringBuilder stringBuilder = new StringBuilder();
            if(this.outputHeader) stringBuilder.AppendLine($"[{configurationSection.SectionName}]");

            foreach (var config in configurationSection.Options.Values)
            {
                stringBuilder.AppendLine(this.SerializeLine(config.OptionName, config.Value));
            }
            return stringBuilder.ToString();
        }
    }
}
