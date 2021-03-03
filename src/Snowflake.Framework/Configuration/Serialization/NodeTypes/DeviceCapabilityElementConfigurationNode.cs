using System;
using System.Collections.Generic;
using System.Text;
using Snowflake.Input.Controller;
using Snowflake.Input.Device;

namespace Snowflake.Configuration.Serialization
{
    public sealed record DeviceCapabilityElementConfigurationNode
        : AbstractConfigurationNode<DeviceCapability>
    {
        internal DeviceCapabilityElementConfigurationNode(string key, 
            ControllerElement element,
            DeviceCapability value, 
            string mappedValue, 
            DeviceCapabilityClass type)
            : base(key, value)
        {
            this.DeviceType = type;
            this.Value = mappedValue;
            this.VirtualElement = element;
        }

        public new string Value { get; }

        public ControllerElement VirtualElement { get; }
        public DeviceCapabilityClass DeviceType { get; }
    }
}
