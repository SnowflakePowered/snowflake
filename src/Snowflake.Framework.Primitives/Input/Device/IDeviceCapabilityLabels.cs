using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Input.Device
{
    /// <summary>
    /// Represents a mapping from <see cref="DeviceCapability"/> to a label on the real device.
    /// When enumerated, 
    /// </summary>
    public interface IDeviceCapabilityLabels : IEnumerable<KeyValuePair<DeviceCapability, string>>
    {
        /// <summary>
        /// Gets a friendly string representation of the provided device capability.
        /// </summary>
        /// <param name="capability">The <see cref="DeviceCapability"/> to get a name for.</param>
        /// <returns>A string representation of the device capbility</returns>
        string this[DeviceCapability capability] { get; }
    }
}
