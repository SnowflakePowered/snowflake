using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Emulator.Input.InputManager
{
    public interface IInputManager
    {
        /// <summary>
        /// Get the currently usable gamepads for this computer
        /// </summary>
        /// <returns>A list of usable gamepad input devices</returns>
        IList<IInputDevice> GetGamepads();
    }
}
