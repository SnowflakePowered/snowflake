using System.Collections.Generic;

namespace Snowflake.Emulator.Input
{
    /// <summary>
    /// An Input Template maps an emulator's input configuration file's keys to be replaced with key combinations.
    /// Every available option in the configuration file is replaced with a unique key that is later replaced with
    /// the Input value during compilation
    /// </summary>
    public interface IInputTemplate
    {
        /// <summary>
        /// The name of the input template used as an identifier
        /// </summary>
        string Name { get; }
        /// <summary>
        /// The gamepad input mappings 
        /// </summary>
        IReadOnlyDictionary<string, IGamepadMapping> GamepadMappings { get; }
        /// <summary>
        /// The keyboard input mappings 
        /// </summary>
        IReadOnlyDictionary<string, IKeyboardMapping> KeyboardMappings { get; }
        /// <summary>
        /// The value to be inputted if no suitable value is available. This is usually blank.
        /// </summary>
        string NoBind { get; }
        /// <summary>
        /// The string template with insertion positions marked
        /// </summary>
        string StringTemplate { get; }
        /// <summary>
        /// A list of all replaceable keys in the string template
        /// </summary>
        IReadOnlyList<string> TemplateKeys { get; }
    }
}
