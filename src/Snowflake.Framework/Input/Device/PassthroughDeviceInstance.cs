using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowflake.Input.Device
{
    public sealed class PassthroughDeviceInstance : IInputDeviceInstance
    {
        public InputDriver Driver => InputDriver.Passthrough;

        public int EnumerationIndex => 0;

        public int ClassEnumerationIndex => 0;

        public IEnumerable<DeviceCapability> Capabilities => Enumerable.Empty<DeviceCapability>();

        public IDeviceLayoutMapping DefaultLayout => DeviceLayoutMapping.EmptyMapping;

        public int NameEnumerationIndex => 0;

        public int ProductEnumerationIndex => 0;

        public IDeviceCapabilityLabels CapabilityLabels => DefaultDeviceCapabilityLabels.DefaultLabels;
    }
}
