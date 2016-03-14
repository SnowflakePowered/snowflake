using System.Collections.Generic;

namespace Snowflake.Input
{
    public interface IInputManager
    {
        /// <summary>
        /// Get the currently usable gamepads for this computer
        /// </summary>
        /// <returns>A list of usable gamepad input devices</returns>
        IEnumerable<IInputDevice> GetAllDevices();
    }
}
