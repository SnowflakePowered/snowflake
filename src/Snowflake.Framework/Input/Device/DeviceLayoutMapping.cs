using System;
using System.Collections.Generic;
using System.Text;
using Snowflake.Input.Controller;

namespace Snowflake.Input.Device
{
    public class DeviceLayoutMapping : IDeviceLayoutMapping
    {
        private static readonly DeviceLayoutMapping _EmptyMapping = new DeviceLayoutMapping();

        internal static IDeviceLayoutMapping EmptyMapping => _EmptyMapping;

        public DeviceLayoutMapping() : this(new Dictionary<ControllerElement, DeviceCapability>()) { }

        public DeviceLayoutMapping(IDictionary<ControllerElement, DeviceCapability> mapping)
        {
            this.Mapping = mapping;
        }

        public DeviceCapability this[ControllerElement e]
            => this.Mapping.TryGetValue(e, out DeviceCapability d) ? d : DeviceCapability.None;
        
        private IDictionary<ControllerElement, DeviceCapability> Mapping { get; }
    }
}
