using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Input.Device
{
    public sealed class DirectInputDeviceInstance : IInputDeviceInstance
    {
        internal DirectInputDeviceInstance(int enumerationIndex, 
            int classEnumerationIndex, int uniqueNameEnumerationIndex, 
            int productEnumerationIndex,
            IReadOnlyDictionary<DeviceCapability, (int offset, int rawId)> capabilities,
            IDeviceLayoutMapping defaultLayout,
            IDeviceCapabilityLabels labels)
        {
            this.EnumerationIndex = enumerationIndex;
            this.ClassEnumerationIndex = classEnumerationIndex;
            this.NameEnumerationIndex = uniqueNameEnumerationIndex;
            this.ProductEnumerationIndex = productEnumerationIndex;
            this.DefaultLayout = defaultLayout;
            this.CapabilityMap = capabilities;
            this.CapabilityLabels = labels;
        }

        private IReadOnlyDictionary<DeviceCapability, (int offset, int rawId)> CapabilityMap { get; }
        public InputDriver Driver => InputDriver.DirectInput;

        public int EnumerationIndex { get; }

        public int ClassEnumerationIndex { get; }

        public int NameEnumerationIndex { get; }

        public IEnumerable<DeviceCapability> Capabilities => this.CapabilityMap.Keys;

        public IDeviceLayoutMapping DefaultLayout { get; }

        public int ProductEnumerationIndex { get; }

        public IDeviceCapabilityLabels CapabilityLabels { get; }

        public int GetObjectOffset(DeviceCapability capability) => 
            this.CapabilityMap.TryGetValue(capability, out (int offset, int _) val) ? val.offset : -1;

        public int GetObjectRawId(DeviceCapability capability) =>
            this.CapabilityMap.TryGetValue(capability, out (int _, int rawId) val) ? val.rawId : -1;
    }
}
