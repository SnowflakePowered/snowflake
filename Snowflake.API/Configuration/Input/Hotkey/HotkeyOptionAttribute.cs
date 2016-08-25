using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Input.Controller;

namespace Snowflake.Configuration.Input.Hotkey
{
    /// <summary>
    /// Represents a hotkey option that relates a device input to a function within the emulator, but
    /// outside of the scope of the controller layout and game.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class HotkeyOptionAttribute : Attribute
    {

        /// <summary>
        /// Represents a hotkey option that relates a device input to a function within the emulator, but
        /// outside of the scope of the controller layout and game.
        /// </summary>
        /// <param name="optionName">The name of the option as it appears in configuration</param>
        /// <param name="inputType">The type of this input option</param>
        public HotkeyOptionAttribute(string optionName, InputOptionType inputType)
        {
            this.OptionName = optionName;
            this.InputType = inputType;
        }

        /// <summary>
        /// The type of this input option, whether it accepts
        /// keyboard only mappings, controller button mappings, or any type of mapping
        /// </summary>
        public InputOptionType InputType { get; }

        /// <summary>
        /// The name of the option as it appears in configuration
        /// </summary>
        public string OptionName { get; }

        /// <summary>
        /// The display name of this option
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Whether or not this option is ever shown to the user.
        /// Hotkey options should be displayed in 'simple' configuration mode
        /// unless marked private.
        /// </summary>
        public bool Private { get; set; }
    }
}
