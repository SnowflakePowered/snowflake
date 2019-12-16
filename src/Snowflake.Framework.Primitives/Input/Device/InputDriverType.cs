using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Input.Device
{
    /// <summary>
    /// The type of input driver for a <see cref="IInputDeviceInstance"/>.
    /// </summary>
    public enum InputDriverType
    {
        /// <summary>
        /// No input driver.
        /// 
        /// Reserved for internal use. Do not use for emulator handled input, 
        /// that is represented by <see cref="Passthrough"/>
        /// </summary>
        None,
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
    }
}
