namespace Snowflake.Input.Controller
{
    /// <summary>
    /// Represents metadata about a controller element
    /// </summary>
    public interface IControllerElementInfo
    {
        /// <summary>
        /// Gets the user friendly label for this element
        /// </summary>
        string Label { get; }

        /// <summary>
        /// Gets the type of element
        /// </summary>
        ControllerElementType Type { get; }
    }
}