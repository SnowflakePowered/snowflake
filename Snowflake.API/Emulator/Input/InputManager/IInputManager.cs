using System.Collections.Generic;

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
