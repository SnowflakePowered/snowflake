using Snowflake.Input.Controller;
using Snowflake.Input.Controller.Mapped;
using Snowflake.Input.Device;

namespace Snowflake.Emulator
{
    /// <summary>
    /// Represents a controller port on the emulated machine
    /// </summary>
    public interface IEmulatedPort
    {
        /// <summary>
        /// The port number index emulated, zero indexed.
        /// eg. (0 -> Player One, 1 -> Player 2)
        /// </summary>
        int EmulatedPortNumber { get; }

        /// <summary>
        /// The physical plugged-in device from the host machine
        /// </summary>
        IInputDevice PluggedDevice { get; }

        /// <summary>
        /// The layout of the emulated controller
        /// </summary>
        IControllerLayout EmulatedController { get; }

        /// <summary>
        /// The mapped controller elements mapping real inputs to the emulated controller
        /// </summary>
        IMappedControllerElementCollection MappedElementCollection { get; }
    }
}