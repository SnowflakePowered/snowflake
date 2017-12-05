using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Input.Controller;

namespace Snowflake.Input.Device
{
    /// <summary>
    /// A more filtered, usable version of an ILowLevelInputDevice
    /// </summary>
    public interface IInputDevice
    {
        /// <summary>
        /// Gets a unique name assigned to this type of device
        /// </summary>
        string DeviceId { get; }

        /// <summary>
        /// Gets the device this wrapper belongs to
        /// </summary>
        ILowLevelInputDevice DeviceInfo { get; }

        /// <summary>
        /// Gets the device index differs from the enumeration number that it is the index in a set
        /// of the device rather than all devices.
        /// For example, if the device is an Xinput device and is the 3rd input device, this index would be
        /// 2 in the set of input devices.
        /// </summary>
        int? DeviceIndex { get; }

        /// <summary>
        /// Gets a friendly name for the controller (Xbox Controller, or Wii Remote, etc etc)
        /// </summary>
        string ControllerName { get; }

        /// <summary>
        /// Gets the device layout this input device implements
        /// </summary>
        IControllerLayout DeviceLayout { get; }

        /// <summary>
        /// Gets the API this controller uses
        /// </summary>
        InputApi DeviceApi { get; }
    }
}
