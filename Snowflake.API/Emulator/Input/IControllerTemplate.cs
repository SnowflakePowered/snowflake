using System;
using System.Collections.Generic;
namespace Snowflake.Emulator.Input
{
    /// <summary>
    /// A Controller Mapping maps the buttons of a controller definition to the specific locations specified by an
    /// input template. For example, given an input template of 
    /// <code>input_player{N}_select = "{RETROARCH_KEYBOARD_INPUT_SELECT}"</code> 
    /// A mapping of <code>BTN_SELECT: RETROARCH_KEYBOARD_INPUT_SELECT</code> would make whatever keyboard input entered 
    /// for a controller's Select button to be set as <code>input_player{N}_select</code>, where {N} is replaced with the player index
    /// </summary>
    public interface IControllerTemplate
    {
        string ControllerID { get; }
        string EmulatorID { get; }
        IReadOnlyDictionary<string, IControllerMapping> GamepadControllerMappings { get; }
        IReadOnlyDictionary<string, IControllerMapping> KeyboardControllerMappings { get; }
        string PlatformID { get; }
    }
}
