using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowflake.Input.Device
{
    public sealed class PassthroughDeviceInstance : IInputDriverInstance
    {
        public InputDriverType Driver => InputDriverType.Passthrough;

        public int EnumerationIndex => 0;

        public int ClassEnumerationIndex => 0;

        public IEnumerable<DeviceCapability> Capabilities => Enumerable.Empty<DeviceCapability>();

        public IDeviceLayoutMapping DefaultLayout => DeviceLayoutMapping.EmptyMapping;

        public int UniqueNameEnumerationIndex => 0;
    }
}
