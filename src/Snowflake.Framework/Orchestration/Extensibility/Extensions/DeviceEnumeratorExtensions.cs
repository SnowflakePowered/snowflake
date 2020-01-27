using Snowflake.Input.Device;
using Snowflake.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowflake.Orchestration.Extensibility.Extensions
{
    public static class DeviceEnumeratorExtensions
    {
        public static bool IsPortDeviceConnected(this IDeviceEnumerator @this,
            IEmulatedPortDeviceEntry portDevice)
        {
            return @this.QueryConnectedDevices().Any(d => d.InstanceGuid == portDevice.InstanceGuid);
        }

        public static IInputDeviceInstance? GetPortDeviceInstance(this IDeviceEnumerator @this,
            IEmulatedPortDeviceEntry portDevice)
        {
            return @this.QueryConnectedDevices()
                .FirstOrDefault(d => d.InstanceGuid == portDevice.InstanceGuid)?.Instances?
                .FirstOrDefault(i => i.Driver == portDevice.Driver);
        }

        public static IInputDevice? GetPortDevice(this IDeviceEnumerator @this,
            IEmulatedPortDeviceEntry portDevice)
        {
            return @this.QueryConnectedDevices()
                .FirstOrDefault(d => d.InstanceGuid == portDevice.InstanceGuid);
        }
    }
}
