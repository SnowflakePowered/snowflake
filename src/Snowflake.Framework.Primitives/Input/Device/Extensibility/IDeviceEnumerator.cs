using Snowflake.Extensibility;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Input.Device.Extensibility
{
    /// <summary>
    /// Enumerates input devices and produces <see cref="IInputDevice"/> for each 
    /// enumerated hardware device, alongside their supported device.
    /// 
    /// <para>
    /// There is only one <see cref="IDeviceEnumerator"/> service per instance,
    /// which is unique to the OS, and must enumerate all devices and their input drivers (API)
    /// for that operating system.
    /// </para>
    /// 
    /// <para>
    /// All <see cref="IDeviceEnumerator"/> must provide a keyboard device that implements the
    /// driver instance for <see cref="InputDriverType.Keyboard"/> and a passthrough device that
    /// implements the <see cref="InputDriverType.Passthrough"/> driver instance at a minimum, otherwise
    /// emulators will be unable to map inputs.
    /// </para>
    /// </summary>
    public interface IDeviceEnumerator
    {
        /// <summary>
        /// Queries and returns all input devices connected to the computer.
        /// </summary>
        /// <returns>All currently connected input devices.</returns>
        IEnumerable<IInputDevice> QueryConnectedDevices();
    }
}
