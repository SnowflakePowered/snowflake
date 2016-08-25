using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Snowflake.Configuration.Input;
using Snowflake.Configuration.Input.Hotkey;
using Snowflake.Input.Controller;

namespace Snowflake.Configuration.Hotkey
{
    public class HotkeyOption : IHotkeyOption
    {
        public string DisplayName { get; }
        public string OptionName { get; }
        public string KeyName { get; }
        public bool Private { get; }
        public InputOptionType InputType { get; }

        [JsonIgnore]
        public IHotkeyTrigger Value
        {
            get
            {
                var value = this.propertyInfo.GetValue(this.instance);
                var type = this.propertyInfo.PropertyType;
                if(type == typeof(KeyboardKey)) //todo pattern matching would be wonderful here
                    return new HotkeyTrigger((KeyboardKey)value);
                if(type == typeof(ControllerElement))
                    return new HotkeyTrigger((ControllerElement)value);
                return new HotkeyTrigger();               
            }
            set
            {
                var type = this.propertyInfo.PropertyType;
                if (type == typeof(KeyboardKey)) //todo pattern matching would be wonderful here
                    this.propertyInfo.SetValue(this.instance, value.KeyboardTrigger);
                if (type == typeof(ControllerElement)) 
                    this.propertyInfo.SetValue(this.instance, value.ControllerTrigger);
            }
        }


        private readonly PropertyInfo propertyInfo;
        private readonly object instance;
        internal HotkeyOption(PropertyInfo propertyInfo, IHotkeyTemplate instance)
        {
            this.propertyInfo = propertyInfo;
            this.instance = instance;
            var attribute = propertyInfo.GetCustomAttribute<HotkeyOptionAttribute>();
            this.OptionName = attribute.OptionName;
            this.DisplayName = attribute.DisplayName;
            this.Private = attribute.Private;
            this.KeyName = this.propertyInfo.Name;
            this.InputType = attribute.InputType;
        }
    }
}
