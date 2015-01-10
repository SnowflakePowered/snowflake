using System;
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
        IReadOnlyDictionary<string, IGamepadMapping> GamepadMappings { get; }
        IReadOnlyDictionary<string, IKeyboardMapping> KeyboardMappings { get; }
        string NoBind { get; }
        string StringTemplate { get; }
        IReadOnlyList<string> TemplateKeys { get; }
    }
}
