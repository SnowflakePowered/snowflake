using Snowflake.Input.Device.Extensibility;
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
        /// The instance GUID for the required passthrough instance.
        /// </summary>
        static readonly Guid PassthroughInstanceGuid = new Guid("53465041-0000-5041-5353-504944564944");

        /// <summary>
        /// The instance GUID for the required keyboard instance.
        /// </summary>
        static readonly Guid KeyboardInstanceGuid = new Guid("53464B59-0000-004B-4559-504944564944");

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
        /// In general, this GUID is opaque and arbitrary, but must be consistent with the 
        /// <see cref="IDeviceEnumerator"/> that produced this device.
        /// 
        /// On Windows, this will be the device instance GUID as enumerated from DirectInput, and
        /// on Linux, this GUID will be in SDL2 format. The passthrough and keyboard instances
        /// will always have the GUIDs <see cref="PassthroughInstanceGuid"/> and <see cref="KeyboardInstanceGuid"/>
        /// in particular.
        /// </summary>
        Guid InstanceGuid { get; }

        /// <summary>
        /// Gets the input driver instances for which this device implements.
        /// </summary>
        IEnumerable<IInputDriverInstance> Instances { get; }
    }
}
