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
        /// <summary>
        /// The unique ID of the controller definition
        /// </summary>
        string ControllerID { get; }
        /// <summary>
        /// The Input Template this controller template corresponds to
        /// </summary>
        string InputTemplateName { get; }
        /// <summary>
        /// The keymappings for gamepad devices
        /// Multiple mappings may be defined for multiple types of gamepads. 
        /// Most common are definitions for an XInput gamepad
        /// </summary>
        IReadOnlyDictionary<string, IControllerMapping> GamepadControllerMappings { get; }
        /// <summary>
        /// The keymappings for keyboard devices.
        /// Multiple mappings may be defined for multiple types of keyboards.
        /// </summary>
        IReadOnlyDictionary<string, IControllerMapping> KeyboardControllerMappings { get; }
        /// <summary>
        /// The platform ID of the controller
        /// </summary>
        string PlatformID { get; }
    }
}
