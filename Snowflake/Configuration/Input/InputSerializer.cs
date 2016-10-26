using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Input.Controller;
using Snowflake.Input.Controller.Mapped;

namespace Snowflake.Configuration.Input
{
    public class InputSerializer : IInputSerializer
    {
        protected IConfigurationSerializer ConfigurationSerializer { get; }
        public InputSerializer(IConfigurationSerializer configurationSerializer)
        {
            this.ConfigurationSerializer = configurationSerializer;
        }
        public virtual string Serialize(IInputTemplate inputTemplate, IInputMapping inputMapping)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(this.ConfigurationSerializer.SerializeHeader(inputTemplate.SectionName.Replace("{N}", inputTemplate.PlayerIndex.ToString())));

            IConfigurationSection inputOptions = inputTemplate;
            foreach (var config in inputOptions.Options)
            {
                stringBuilder.AppendLine(this.ConfigurationSerializer.SerializeLine(config.OptionName.Replace("{N}", inputTemplate.PlayerIndex.ToString()), inputOptions.Values[config.KeyName].Value));
            }

            foreach (var input in inputTemplate.Options)
            {
                stringBuilder.AppendLine(this.SerializeInput(input.OptionName.Replace("{N}", inputTemplate.PlayerIndex.ToString()), inputTemplate.Values[input.KeyName], inputMapping));
            }
            stringBuilder.Append(this.ConfigurationSerializer.SerializeFooter(inputTemplate.SectionName.Replace("{N}", inputTemplate.PlayerIndex.ToString())));

            return stringBuilder.ToString();
        }

        public virtual string SerializeInput(string key, ControllerElement element, IInputMapping inputMapping)
        {

            return
                element == ControllerElement.NoElement ? this.ConfigurationSerializer.SerializeLine(key, this.ConfigurationSerializer.TypeMapper.ConvertValue((object)null))
                : this.ConfigurationSerializer.SerializeLine(key, inputMapping[element]);
        }
    }
}
