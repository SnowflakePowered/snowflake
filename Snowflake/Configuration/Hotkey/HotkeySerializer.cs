using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Configuration.Input;

namespace Snowflake.Configuration.Hotkey
{
    public class HotkeySerializer : IHotkeySerializer
    {
        protected IConfigurationSerializer ConfigurationSerializer { get; }
        public HotkeySerializer(IConfigurationSerializer baseSerializer)
        {
            this.ConfigurationSerializer = baseSerializer;
        }

        public virtual string SerializeKeyboard(IHotkeyTemplate template, IInputMapping inputMapping, int playerIndex = 0)
        {
            var sb = new StringBuilder();
            sb.Append(this.ConfigurationSerializer.SerializeHeader(template.SectionName.Replace("{N}", playerIndex.ToString())));
            foreach (var config in template.ConfigurationOptions)
            {
                sb.AppendLine(this.ConfigurationSerializer.SerializeLine(config.OptionName.Replace("{N}", playerIndex.ToString()), config.GetValue(Guid.Empty)));
            }
            foreach (var option in template.HotkeyOptions)
            {
                sb.AppendLine(this.ConfigurationSerializer.SerializeLine(
                    option.KeyboardConfigurationKey.Replace("{N}", playerIndex.ToString()),
                    inputMapping[option.Value.KeyboardTrigger]));
            }
            sb.Append(this.ConfigurationSerializer.SerializeFooter(template.SectionName.Replace("{N}", playerIndex.ToString())));
            return sb.ToString();
        }

        public virtual string SerializeController(IHotkeyTemplate template, IInputMapping inputMapping,
            int playerIndex = 0)
        {
            var sb = new StringBuilder();
            sb.Append(this.ConfigurationSerializer.SerializeHeader(template.SectionName.Replace("{N}", playerIndex.ToString())));
            foreach (var config in template.ConfigurationOptions)
            {
                sb.AppendLine(this.ConfigurationSerializer.SerializeLine(config.OptionName.Replace("{N}", playerIndex.ToString()), config.GetValue(Guid.Empty)));
            }
            foreach (var option in template.HotkeyOptions)
            {
                sb.AppendLine(this.ConfigurationSerializer.SerializeLine(
                    option.ControllerConfigurationKey.Replace("{N}", playerIndex.ToString()),
                    inputMapping[option.Value.KeyboardTrigger]));
            }
            sb.Append(this.ConfigurationSerializer.SerializeFooter(template.SectionName.Replace("{N}", playerIndex.ToString())));
            return sb.ToString();
        }
    }
}
