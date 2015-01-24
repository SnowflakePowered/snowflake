using System;
using System.Collections.Generic;
namespace Snowflake.Controller
{
    /// <summary>
    /// Represents a mapping from the controller definition to the gamepad or keyboard.
    /// <see cref="Snowflake.Emulator.Input.KeyboardMapping"/> For Snowflake's keyboard abstraction.
    /// <seealso cref="Snowflake.Emulator.Input.GamepadMapping"/> For Snowflake's gamepad abstracion.
    /// </summary>
    public interface IControllerProfile
    {
        /// <summary>
        /// The ID of the controller definition in which this profile is for.
        /// </summary>
        string ControllerID { get; }
        /// <summary>
        /// The mapping of inputs to keyboard or gamepad.
        /// <see cref="Snowflake.Controller.IControllerInput"/>
        /// </summary>
        IReadOnlyDictionary<string, string> InputConfiguration { get; }
        /// <summary>
        /// Whether this profile is for a gamepad, keyboard, or unhandled (custom)
        /// </summary>
        ControllerProfileType ProfileType { get; }
        /// <summary>
        /// Generates an IDictionary that can be serialized to json or yaml
        /// </summary>
        /// <returns>An IDictionary that is to be serialized to json or yaml</returns>
        IDictionary<string, dynamic> ToSerializable();
    }
}
