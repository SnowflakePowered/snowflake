using System;
namespace Snowflake.Controller
{
    /// <summary>
    /// Represents a single input on the controller.
    /// The recommended naming convention for ControllerInputs are 
    /// For Buttons and Triggers: BTN_
    /// For DPad: BTN_DPAD_
    /// For movement of analog sticks, motion tracking and joysticks: ANALOG_
    /// </summary>
    public interface IControllerInput
    {
        /// <summary>
        /// The default profile mapping on a Gamepad
        /// </summary>
        string GamepadAbstraction{ get; }
        /// <summary>
        /// The name of the input
        /// </summary>
        string InputName { get; }
        /// <summary>
        /// Whether the input is analog or not. 
        /// This includes analog triggers.
        /// </summary>
        bool IsAnalog { get; }
    }
}
