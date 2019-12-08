using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Input.Device
{
    public enum InputDriverType
    {
        /// <summary>
        /// Microsoft DirectInput
        /// </summary>
        DirectInput,
        /// <summary>
        /// Microsoft XInput
        /// </summary>
        XInput,
        /// <summary>
        /// Linux evdev API using libevdev
        /// </summary>
        Libevdev,
        /// <summary>
        /// Passthrough driver delegates input configuration
        /// to the emulator. Generally used for native solutions
        /// involving libUSB or other non-standard peripherals.
        /// </summary>
        Passthrough,
        /// <summary>
        /// API agnostic keyboard driver. 
        /// </summary>
        Keyboard,
    }
}
