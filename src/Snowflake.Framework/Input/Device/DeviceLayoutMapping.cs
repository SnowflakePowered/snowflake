using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

        public IEnumerator<ControllerElementMapping> GetEnumerator()
        {
            return this.Mapping.Select(o => (ControllerElementMapping)o).GetEnumerator();
        }
    }
}
