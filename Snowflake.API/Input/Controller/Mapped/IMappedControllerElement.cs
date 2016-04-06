namespace Snowflake.Input.Controller.Mapped
{
    /// <summary>
    /// Represents a real device controller element mapped onto a virtual device element
    /// </summary>
    public interface IMappedControllerElement
    {
        /// <summary>
        /// The virtual element.
        /// </summary>
        ControllerElement ControllerElement { get; }

        /// <summary>
        /// The real element.
        /// </summary>
        ControllerElement DeviceElement { get; set; }

        /// <summary>
        /// The real device key if deviceelement is keyboard
        /// </summary>
        KeyboardKey DeviceKeyboardKey { get; set; }
    }
}