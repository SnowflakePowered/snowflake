using Snowflake.Input.Controller;
using Snowflake.Input.Device;
using Snowflake.Model.Game;
using Snowflake.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Orchestration.Extensibility
{
    /// <summary>
    /// Represents a device plugged into a emulated port.
    /// 
    /// A disconnected device can be "plugged in" to an emulated port, in which case
    /// the corresponding <see cref="IInputDeviceInstance"/> will not be able to be found.
    /// </summary>
    public interface IEmulatedPortDeviceEntry
    {
        /// <summary>
        /// The driver type of the real device used for this port.
        /// </summary>
        InputDriver Driver { get; }

        /// <summary>
        /// The instance GUID of the real device used for this port.
        /// </summary>
        Guid InstanceGuid { get; }

        /// <summary>
        /// The Stone <see cref="ControllerId"/> for the virtual controller used for this port.
        /// </summary>
        ControllerId ControllerID { get; }

        /// <summary>
        /// The Stone <see cref="PlatformId"/> that implements the set of virtual ports this port device belongs to.
        /// </summary>
        PlatformId PlatformID { get; }

        /// <summary>
        /// The GUID of the input mappings profile that maps the real <see cref="DeviceCapability"/> of the
        /// real device to the <see cref="ControllerElement"/> of the virtual device for this port.
        /// </summary>
        Guid ProfileGuid { get; }

        /// <summary>
        /// The 0-indexed port index of the port.
        /// </summary>
        int PortIndex { get; }
    }
}
