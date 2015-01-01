using System;
using System.Collections.Generic;
namespace Snowflake.Emulator.Input
{
    public interface IInputTemplate
    {
        IReadOnlyDictionary<string, IGamepadMapping> GamepadMappings { get; }
        IReadOnlyDictionary<string, IKeyboardMapping> KeyboardMappings { get; }
        string NoBind { get; }
        string StringTemplate { get; }
        IReadOnlyList<string> TemplateKeys { get; }
    }
}
