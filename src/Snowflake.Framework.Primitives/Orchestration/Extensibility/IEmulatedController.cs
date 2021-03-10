using Snowflake.Input.Controller;
using Snowflake.Input.Device;

namespace Snowflake.Orchestration.Extensibility
{
    /// <summary>
    /// Represents a controller port on the emulated machine
    /// </summary>
    public interface IEmulatedController
    {
        /// <summary>
        /// Gets the emulated port number index, zero indexed.
        /// eg. (0 -> Player One, 1 -> Player 2)
        /// </summary>
        int PortIndex { get; }

        /// <summary>
        /// Gets the physical plugged-in device from the host machine
        /// </summary>
        IInputDevice PhysicalDevice { get; }

        /// <summary>
        /// Gets the driver instance of the physical device plugged into this emulated port.
        /// </summary>
        IInputDeviceInstance PhysicalDeviceInstance { get; }

        /// <summary>
        /// Gets the layout of the emulated controller
        /// </summary>
        IControllerLayout TargetLayout { get; }

        /// <summary>
        /// Gets the mapped controller elements mapping real inputs to the emulated controller
        /// </summary>
        IControllerElementMappingProfile LayoutMapping { get; }
    }
}
