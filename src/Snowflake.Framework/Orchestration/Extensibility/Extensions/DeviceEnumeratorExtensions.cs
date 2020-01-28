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
        /// <summary>
        /// Checks if the <see cref="IInputDeviceInstance"/> specified by the given
        /// <see cref="IEmulatedPortDeviceEntry"/> is connected to the system.
        /// </summary>
        /// <param name="this">The <see cref="IDeviceEnumerator"/> to use.</param>
        /// <param name="portDevice">The <see cref="IEmulatedPortDeviceEntry"/> to check if connected.</param>
        /// <returns>Whether or not the given <see cref="IEmulatedPortDeviceEntry"/> is connected.</returns>
        public static bool IsPortDeviceConnected(this IDeviceEnumerator @this,
            IEmulatedPortDeviceEntry portDevice)
        {
            return @this.QueryConnectedDevices().Any(d => d.InstanceGuid == portDevice.InstanceGuid);
        }

        /// <summary>
        /// Gets the <see cref="IInputDeviceInstance"/> specifid by the given <see cref="IEmulatedPortDeviceEntry"/> 
        /// if it is connected to the system, or null if it does not.
        /// </summary>
        /// <param name="this">The <see cref="IDeviceEnumerator"/> to use.</param>
        /// <param name="portDevice">The <see cref="IEmulatedPortDeviceEntry"/> to check if connected.</param>
        /// <returns>The <see cref="IInputDeviceInstance"/> specifid by the given <see cref="IEmulatedPortDeviceEntry"/> 
        /// if it is connected to the system, or null if it does not.</returns>
        public static IInputDeviceInstance? GetPortDeviceInstance(this IDeviceEnumerator @this,
            IEmulatedPortDeviceEntry portDevice)
        {
            return @this.QueryConnectedDevices()
                .FirstOrDefault(d => d.InstanceGuid == portDevice.InstanceGuid)?.Instances?
                .FirstOrDefault(i => i.Driver == portDevice.Driver);
        }

        /// <summary>
        /// Gets the <see cref="IInputDevice"/> specifid by the given <see cref="IEmulatedPortDeviceEntry"/> 
        /// if it is connected to the system, or null if it does not.
        /// </summary>
        /// <param name="this">The <see cref="IDeviceEnumerator"/> to use.</param>
        /// <param name="portDevice">The <see cref="IEmulatedPortDeviceEntry"/> to check if connected.</param>
        /// <returns>The <see cref="IInputDevice"/> specifid by the given <see cref="IEmulatedPortDeviceEntry"/> 
        /// if it is connected to the system, or null if it does not.</returns>
        public static IInputDevice? GetPortDevice(this IDeviceEnumerator @this,
            IEmulatedPortDeviceEntry portDevice)
        {
            return @this.QueryConnectedDevices()
                .FirstOrDefault(d => d.InstanceGuid == portDevice.InstanceGuid);
        }
    }
}
