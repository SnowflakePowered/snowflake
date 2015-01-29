using System;
using System.Collections.Generic;
namespace Snowflake.Controller
{
    /// <summary>
    /// Represents a controller layout.
    /// Every input (button, triggers, analog sticks) is represeted as IControllerInput
    /// <see cref="Snowflake.Controller.IControllerInput"/>
    /// </summary>
    public interface IControllerDefinition
    {
        /// <summary>
        /// The ID of the controller
        /// This ID is used to reference this controller definition
        /// </summary>
        string ControllerID { get; }
        /// <summary>
        /// A dictionary of inputs that comprise the controller. 
        /// Inputs are stored with the input name as the key.
        /// </summary>
        IReadOnlyDictionary<string, IControllerInput> ControllerInputs { get; }
        /// <summary>
        /// Get the profile store associated with this controller
        /// </summary>
        IControllerProfileStore ProfileStore { get; }

    }
}
