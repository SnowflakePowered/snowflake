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
            return @this.QueryConnectedDevices().Any(d => d.DeviceName == portDevice.DeviceName
                && d.VendorID == portDevice.VendorID
                && d.Instances.Any(i => i.Driver == portDevice.Driver &&
                    i.NameEnumerationIndex == portDevice.ProductEnumerationIndex));
        }

        public static IInputDeviceInstance? GetPortDeviceInstance(this IDeviceEnumerator @this,
            IEmulatedPortDeviceEntry portDevice)
        {
            return @this.QueryConnectedDevices()
                .Where(d => d.DeviceName == portDevice.DeviceName && d.VendorID == portDevice.VendorID)
                .SelectMany(d => d.Instances)
                .FirstOrDefault(i => i.Driver == portDevice.Driver &&
                    i.NameEnumerationIndex == portDevice.ProductEnumerationIndex);
        }

        public static IInputDevice? GetPortDevice(this IDeviceEnumerator @this,
            IEmulatedPortDeviceEntry portDevice)
        {
            return @this.QueryConnectedDevices()
                .FirstOrDefault(d => d.DeviceName == portDevice.DeviceName
                    && d.VendorID == portDevice.VendorID
                    && d.Instances.Any(i => i.Driver == portDevice.Driver &&
                    i.NameEnumerationIndex == portDevice.ProductEnumerationIndex));
        }


    }
}
