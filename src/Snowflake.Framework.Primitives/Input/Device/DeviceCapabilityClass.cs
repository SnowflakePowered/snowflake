using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Input.Device
{
    /// <summary>
    /// Classes of device capabilities
    /// </summary>
    [Flags]
    public enum DeviceCapabilityClass
    {
        /// <summary>
        /// This input doe not accept any types.
        /// </summary>
        None = 1 << 0,

        /// <summary>
        /// Controller capabilities, excluding rumble.
        /// </summary>
        Controller = ControllerButton | ControllerAxis,

        /// <summary>
        /// Controller Axes
        /// </summary>
        ControllerAxis = 1 << 1,

        /// <summary>
        /// Controller buttons, including both directional and face buttons.
        /// </summary>
        ControllerButton = ControllerFaceButton | ControllerDirectional,

        /// <summary>
        /// Controller face buttons.
        /// </summary>
        ControllerFaceButton = 1 << 2,

        /// <summary>
        /// Controller face buttons.
        /// </summary>
        ControllerDirectional = 1 << 3,

        /// <summary>
        /// Keyboard and mouse inputs.
        /// </summary>
        KeyboardMouse = Keyboard | Mouse,

        /// <summary>
        /// Only keyboard inputs.
        /// </summary>
        Keyboard = 1 << 4,

        /// <summary>
        /// Only mouse inputs.
        /// </summary>
        Mouse = MouseButton | MouseCursor,

        /// <summary>
        /// Only mouse button inputs.
        /// </summary>
        MouseButton = 1 << 5,

        /// <summary>
        /// Only mouse cursors.
        /// </summary>
        MouseCursor = 1 << 6,

        /// <summary>
        /// Rumble motors
        /// </summary>
        Rumble = 1 << 7,
    }
}
