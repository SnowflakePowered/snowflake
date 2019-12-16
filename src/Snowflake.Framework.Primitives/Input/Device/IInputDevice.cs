using Snowflake.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Input.Device
{
    /// <summary>
    /// Represents the physical instance of a hardware peripheral agnostic of any
    /// input API. That is to mean that one device may implement multiple device APIs
    /// (driver instances).
    /// </summary>
    public interface IInputDevice
    {
        /// <summary>
        /// The USB VID of this device.
        /// 
        /// May not be accurate for Keyboard and Passthrough devices.
        /// </summary>
        int VendorID { get; }

        /// <summary>
        /// The USB PID of this device.
        /// </summary>
        int ProductID { get; }

        /// <summary>
        /// The name of the device.
        /// </summary>
        string DeviceName { get; }

        /// <summary>
        /// A user-friendly label to give to this device.
        /// </summary>
        string FriendlyName { get; }

        /// <summary>
        /// Gets the path to this device instance.
        /// 
        /// On Windows, this usually begins with \\?\HID
        /// whereas on Linux, this usually begins with /dev/input.
        /// </summary>
        string DevicePath { get; }

        /// <summary>
        /// A GUID that identifies the device.
        /// 
        /// In general, this GUID is opaque and arbitrary, but must be consistent with the 
        /// <see cref="IDeviceEnumerator"/> that produced this device.
        /// 
        /// On Windows, this will be the device instance GUID as enumerated from DirectInput, and
        /// on Linux, this GUID will be in SDL2 format. The passthrough and keyboard instances
        /// will always have the GUIDs <see cref="IDeviceEnumerator.PassthroughInstanceGuid"/> and
        /// <see cref="IDeviceEnumerator.KeyboardInstanceGuid"/>
        /// in particular.
        /// </summary>
        Guid InstanceGuid { get; }

        /// <summary>
        /// Gets the device driver instances for which this device implements.
        /// </summary>
        IEnumerable<IInputDeviceInstance> Instances { get; }
    }
}
