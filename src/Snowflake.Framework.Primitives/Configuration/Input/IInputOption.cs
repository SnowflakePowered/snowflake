using Snowflake.Input.Controller;

namespace Snowflake.Configuration.Input
{
    /// <summary>
    /// Represents an input configuration option
    /// </summary>
    public interface IInputOption
    {
        /// <summary>
        /// Gets the type of value the input option accepts
        /// </summary>
        InputOptionType InputOptionType { get; }

        /// <summary>
        /// Gets the mapped element to the input option.
        /// </summary>
        ControllerElement TargetElement { get; }

        /// <summary>
        /// Gets the name of the input option as it appears in configuration.
        /// </summary>
        string OptionName { get; }

        /// <summary>
        /// Gets the property name of the input option as it is declared
        /// </summary>
        string KeyName { get; }
    }
}
