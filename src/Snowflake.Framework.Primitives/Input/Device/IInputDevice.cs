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
        short VendorID { get; }
        /// <summary>
        /// The USB PID of this device.
        /// </summary>
        short ProductID { get; }

        /// <summary>
        /// The friendly name of the device.
        /// </summary>
        string DisplayName { get; }

        /// <summary>
        /// A GUID that identifies the device.
        /// 
        /// In general, this GUID is opaque and arbitrary; its format is up to the OS input enumerator.
        /// However, on Windows, this will be the device instance GUID as enumerated from DirectInput, and
        /// on Linux, this GUID will be in SDL2 format.
        /// </summary>
        Guid InstanceGuid { get; }

        /// <summary>
        /// Gets the input driver instances for which this device implements.
        /// </summary>
        IEnumerable<IInputDriverInstance> Instances { get; }
    }
}
