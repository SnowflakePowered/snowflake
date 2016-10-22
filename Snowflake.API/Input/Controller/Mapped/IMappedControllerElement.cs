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
        ControllerElement LayoutElement { get; }

        /// <summary>
        /// The real element.
        /// </summary>
        ControllerElement DeviceElement { get; set; }

    }
}