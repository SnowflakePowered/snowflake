using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Input.Device
{
    /// <summary>
    /// The input API of a device
    /// </summary>
    public enum InputApi
    {
        /// <summary>
        /// XInput (Windows)
        /// </summary>
        XInput,

        /// <summary>
        /// DirectInput (Windows)
        /// </summary>
        DirectInput,

        /// <summary>
        /// RawInput (Windows)
        /// </summary>
        RawInput,

        /// <summary>
        /// Udev (Linux)
        /// </summary>
        Udev,

        /// <summary>
        /// WndProc messaging (Windows)
        /// </summary>
        WndProc,

        /// <summary>
        /// Human Interface Device interface (Windows)
        /// </summary>
        HID,

        /// <summary>
        /// Other
        /// </summary>
        Other,
    }
}
