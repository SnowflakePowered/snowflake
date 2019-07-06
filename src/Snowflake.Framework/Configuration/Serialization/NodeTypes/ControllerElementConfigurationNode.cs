using System;
using System.Collections.Generic;
using System.Text;
using Snowflake.Configuration.Input;
using Snowflake.Input.Controller;

namespace Snowflake.Configuration.Serialization
{
    public sealed class ControllerElementConfigurationNode
        : AbstractConfigurationNode<ControllerElement>
    {
        internal ControllerElementConfigurationNode(string key, ControllerElement value, 
            string mappedValue, 
            InputOptionDeviceType type)
            : base(key, value)
        {
            this.DeviceType = type;
            this.Value = mappedValue;
        }

        public new string Value { get; }
        public InputOptionDeviceType DeviceType { get; }
    }
}
