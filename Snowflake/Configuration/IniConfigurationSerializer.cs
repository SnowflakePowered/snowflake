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

        public override string Serialize(IInputTemplate inputTemplate, IInputMapping inputMapping)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($"[{inputTemplate.SectionName.Replace("{N}", inputTemplate.PlayerIndex.ToString())}]");

            foreach (var config in inputTemplate.ConfigurationOptions)
            {
                stringBuilder.AppendLine(this.SerializeLine(config.OptionName.Replace("{N}", inputTemplate.PlayerIndex.ToString()), config.Value));
            }

            foreach (var input in inputTemplate.InputOptions)
            {
                stringBuilder.AppendLine(this.SerializeInput(input.OptionName.Replace("{N}", inputTemplate.PlayerIndex.ToString()), input.Value, inputMapping));
            }
            return stringBuilder.ToString();
        }
    }
}
