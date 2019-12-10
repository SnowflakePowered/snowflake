using Snowflake.Input.Device;
using System;
using System.Collections.Generic;

namespace Snowflake.Services
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
        /// The instance GUID for the required passthrough instance.
        /// </summary>
        static readonly Guid PassthroughInstanceGuid = new Guid("a98fb892-a33c-44c5-8252-a4b440b78902");

        /// <summary>
        /// The instance GUID for the required keyboard instance.
        /// </summary>
        static readonly Guid KeyboardInstanceGuid = new Guid("02937edc-59b4-48ba-ae89-73b681070a40");

        /// <summary>
        /// Null virtual vendor ID for virtual devices.
        /// </summary>
        static readonly short VirtualVendorID = 0x0000;


        /// <summary>
        /// Virtual product ID for passthrough device.
        /// 0x5054 = 'P' 'T'
        /// </summary>
        static readonly int PassthroughDevicePID = 0x5054;

        /// <summary>
        /// Virtual product ID for keyboard device.
        /// 0x4b42 = 'K' 'B'
        /// </summary>
        static readonly int KeyboardDevicePID = 0x4b42;

        /// <summary>
        /// Queries and returns all input devices connected to the computer.
        /// </summary>
        /// <returns>All currently connected input devices.</returns>
        IEnumerable<IInputDevice> QueryConnectedDevices();
    }
}
