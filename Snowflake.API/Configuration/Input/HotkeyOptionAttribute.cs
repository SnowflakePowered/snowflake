using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Input.Controller;

namespace Snowflake.Configuration.Input
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
        /// <param name="inputOptionType">The type of this input option</param>
        public HotkeyOptionAttribute(string optionName, InputOptionType inputOptionType)
        {
            this.OptionName = optionName;
            this.InputOptionType = inputOptionType;
        }

        /// <summary>
        /// The type of this input option, whether it accepts
        /// keyboard only mappings, controller button mappings, or any type of mapping
        /// </summary>
        public InputOptionType InputOptionType { get; }

        /// <summary>
        /// The name of the option as it appears in configuration
        /// </summary>
        public string OptionName { get; }

        /// <summary>
        /// The display name of this option
        /// </summary>
        public string DisplayName { get; set; }
    }
}
