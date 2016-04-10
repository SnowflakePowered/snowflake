using System.Collections.Generic;

namespace Snowflake.Input.Device
{
    /// <summary>
    /// Manages input devices from the operating system.
    /// </summary>
    public interface IInputManager
    {
        /// <summary>
        /// Get the currently usable gamepads for this computer
        /// </summary>
        /// <returns>A list of usable gamepad input devices</returns>
        IEnumerable<ILowLevelInputDevice> GetAllDevices();
    }
}
