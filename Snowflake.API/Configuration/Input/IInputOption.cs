using Snowflake.Input.Controller;

namespace Snowflake.Configuration.Input
{
    /// <summary>
    /// Represents an input configuration option
    /// </summary>
    public interface IInputOption
    {
        /// <summary>
        /// The type of value the input option accepts
        /// </summary>
        InputOptionType InputOptionType { get; }
        /// <summary>
        /// The mapped element to the input option.
        /// </summary>
        ControllerElement TargetElement { get; }
        /// <summary>
        /// The name of the input option as it appears in configuration.
        /// </summary>
        string OptionName { get; }
        /// <summary>
        /// The property name of the input option as it is declared
        /// </summary>
        string KeyName { get; }
    }
}