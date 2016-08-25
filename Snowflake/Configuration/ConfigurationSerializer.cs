using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Configuration.Attributes;
using Snowflake.Configuration.Input;
using Snowflake.Configuration.Input.Hotkey;
using Snowflake.Input.Controller;
using Snowflake.Input.Controller.Mapped;

namespace Snowflake.Configuration
{
    public abstract class ConfigurationSerializer : IConfigurationSerializer
    {
        protected const string IteratorKey = "{N}";

        protected ConfigurationSerializer(IConfigurationTypeMapper typeMapper)
        {
            this.TypeMapper = typeMapper;
        }

        protected ConfigurationSerializer(IBooleanMapping booleanMapping, string nullSerializer) 
            : this(new DefaultConfigurationTypeMapper(booleanMapping, nullSerializer))
        {
        }

        public IConfigurationTypeMapper TypeMapper { get; set; }

        public abstract string SerializeLine<T>(string key, T value);

        public virtual string SerializeInput(string key, IMappedControllerElement element, IInputMapping inputMapping)
        {
           
            return
                element == null ? this.SerializeLine(key, this.TypeMapper.ConvertValue((object) null))
                : this.SerializeLine(key, element.DeviceElement == ControllerElement.Keyboard
                    ? inputMapping[element.DeviceKeyboardKey]
                    : inputMapping[element.DeviceElement]);
        }

        public virtual string SerializeHotkeyInput(string key, IHotkeyTrigger trigger, HotkeyTemplateType templateType, IInputMapping inputMapping)
        {
            switch (templateType)
            {
                case HotkeyTemplateType.ControllerHotkeys:
                    return this.SerializeLine(key, inputMapping[trigger.ControllerTrigger]);
                case HotkeyTemplateType.KeyboardHotkeys:
                    return this.SerializeLine(key, inputMapping[trigger.KeyboardTrigger]);
                default:
                    return this.SerializeLine(key, this.TypeMapper.ConvertValue((object) null));
            }
        }

        public string SerializeValue(object value)
        {
            if (value == null) return this.TypeMapper.ConvertValue((object) null);
            Type valueType = value.GetType();
            var converter = typeof(IConfigurationTypeMapper).GetMethod("ConvertValue");
            return (string)converter.MakeGenericMethod(valueType).Invoke(this.TypeMapper, new[] { value });
        }

        public abstract string Serialize(IInputTemplate inputTemplate, IInputMapping inputMapping);
        public abstract string Serialize(IConfigurationSection configurationSection);
        public abstract string Serialize(IHotkeyTemplate hotkeyTemplate, IInputMapping inputMapping);

    }
}
