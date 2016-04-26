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
    /// <summary>
    /// A configuration serializer that uses INI format
    /// 
    /// [Header]
    /// Key = Value
    /// </summary>
    public class IniConfigurationSerializer : ConfigurationSerializer
    {
       
        public IniConfigurationSerializer(IBooleanMapping booleanMapping, string nullSerializer)
            : base(booleanMapping, nullSerializer)
        {
        }

        public override string SerializeLine<T>(string key, T value)
        {
            return $"{key} = {this.SerializeValue(value)}";
        }

        public override string Serialize(IConfigurationSection configurationSection)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($"[{configurationSection.SectionName}]");

            foreach (var config in configurationSection.Options.Values)
            {
                stringBuilder.AppendLine(this.SerializeLine(config.OptionName, config.Value));
            }
            return stringBuilder.ToString();
        }
    }
}
