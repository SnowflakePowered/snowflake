using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Snowflake.Configuration.Input;
using Snowflake.Input.Controller;

namespace Snowflake.Configuration.Hotkey
{
    public class HotkeyOption : IHotkeyOption
    {
        public string DisplayName { get; }
        public string HotkeyName { get; }
        public string KeyName { get; }
        public bool Private { get; }
        public InputOptionType InputType { get; }

        [JsonIgnore]
        public HotkeyTrigger Value
        {
            get { return (HotkeyTrigger)this.propertyInfo.GetValue(this.instance); }
            set { this.propertyInfo.SetValue(this.instance, value); }
        }

        public string KeyboardConfigurationKey { get; }
        public string ControllerConfigurationKey { get; }
        public string ControllerAxisConfigurationKey { get; }

        private readonly PropertyInfo propertyInfo;
        private readonly object instance;
        internal HotkeyOption(PropertyInfo propertyInfo, IHotkeyTemplate instance)
        {
            this.propertyInfo = propertyInfo;
            this.instance = instance;
            var attribute = propertyInfo.GetCustomAttribute<HotkeyOptionAttribute>();
            this.HotkeyName = attribute.HotkeyName;
            this.DisplayName = attribute.DisplayName;
            this.Private = attribute.Private;
            this.KeyName = this.propertyInfo.Name;
            this.InputType = attribute.InputType;
            this.KeyboardConfigurationKey = attribute.KeyboardConfigurationKey ?? this.HotkeyName;
            this.ControllerConfigurationKey = attribute.ControllerConfigurationKey ?? this.HotkeyName;
            this.ControllerAxisConfigurationKey = attribute.AxisConfigurationKey ??
                                                  this.ControllerConfigurationKey;
        }
    }
}
