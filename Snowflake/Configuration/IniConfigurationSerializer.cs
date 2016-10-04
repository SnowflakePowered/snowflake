using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Snowflake.Configuration.Attributes;
using Snowflake.Configuration.Input;
using Snowflake.Input.Controller;
using Snowflake.Input.Controller.Mapped;
using Snowflake.Input.Device;

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

        public override string SerializeHeader(string headerString) => $"[{headerString}]{Environment.NewLine}";

        public override string Serialize(IConfigurationSection configurationSection)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(this.SerializeHeader(configurationSection.SectionName));
            foreach (var config in from option in configurationSection.Options.Values where !option.Flag select option)
            {
                stringBuilder.AppendLine(this.SerializeLine(config.OptionName, config.GetValue(Guid.Empty).Value));
            }
            return stringBuilder.ToString();
        }
    }
}
