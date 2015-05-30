using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Controller
{
    /// <summary>
    /// Types of ControllerProfile
    /// </summary>
    public enum ControllerProfileType
    {
        /// <summary>
        /// A profile that maps to a standard XInput-style gamepad
        /// </summary>
        GAMEPAD_PROFILE,
        /// <summary>
        /// A profile that maps to a standard keyboard
        /// </summary>
        KEYBOARD_PROFILE,
        /// <summary>
        /// A custom (unknown) type of profile. 
        /// <remarks>A controller with type <b>CUSTOM_PROFILE</b> is unhandled, the <see cref="Snowflake.Emulator.IEmulatorBridge"/> must handle this case manually</remarks>
        /// </summary>
        CUSTOM_PROFILE,
        /// <summary>
        /// A stubbed, empty profile
        /// </summary>
        NULL_PROFILE
    }
}
