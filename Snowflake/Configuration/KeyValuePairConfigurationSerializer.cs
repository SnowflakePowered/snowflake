using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Configuration.Input;
using Snowflake.Configuration.Input.Hotkey;
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
            foreach (var config in configurationSection.Options.Values)
            {
                stringBuilder.AppendLine(this.SerializeLine(config.OptionName, config.Value));
            }
            return stringBuilder.ToString();
        }

        public override string Serialize(IHotkeyTemplate hotkeyTemplate, IInputMapping inputMapping)
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var config in hotkeyTemplate.ConfigurationOptions)
            {
                stringBuilder.AppendLine(this.SerializeLine(config.OptionName, config.Value));
            }

            foreach (var input in hotkeyTemplate.HotkeyOptions)
            {
                stringBuilder.AppendLine(this.SerializeHotkeyInput(input.OptionName, input.Value, hotkeyTemplate.TemplateType, inputMapping));
            }
            return stringBuilder.ToString();
        }

        public override string Serialize(IInputTemplate inputTemplate, IInputMapping inputMapping)
        {
            StringBuilder stringBuilder = new StringBuilder();

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

        public KeyValuePairConfigurationSerializer(IBooleanMapping booleanMapping, string nullSerializer, string separator)
            : base(booleanMapping, nullSerializer)
        {
            this.separator = separator;
        }
    }
}
