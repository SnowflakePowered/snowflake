namespace Snowflake.Input.Controller
{
    /// <summary>
    /// Represents metadata about a controller element
    /// </summary>
    public interface IControllerElementInfo
    {
        /// <summary>
        /// The user friendly label for this element
        /// </summary>
        string Label { get; }

        /// <summary>
        /// The type of element
        /// </summary>
        ControllerElementType Type { get; }
    }
}