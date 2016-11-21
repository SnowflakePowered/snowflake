using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Configuration.Input;
using Snowflake.Input.Controller;

namespace Snowflake.Configuration.Hotkey
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
        /// <param name="hotkeyName">The name of the option as it appears in configuration</param>
        public HotkeyOptionAttribute(string hotkeyName)
        {
            this.HotkeyName = hotkeyName;
        }

        /// <summary>
        /// The type of this input option, whether it accepts
        /// keyboard only mappings, controller button mappings, or any type of mapping
        /// </summary>
        public InputOptionType InputType { get; }  = InputOptionType.Controller | InputOptionType.Keyboard;

        /// <summary>
        /// The default name of the option as it appears in configuration
        /// </summary>
        public string HotkeyName { get; }

        /// <summary>
        /// The name of the option if serializing specifically for a <see cref="KeyboardKey"/> value
        /// If this is null, it is treated as identical to <see cref="HotkeyName"/>
        /// </summary>
        public string KeyboardConfigurationKey { get; set; } 

        /// <summary>
        /// The name of the option if serializing specifically for a <see cref="ControllerElement"/> value
        /// If this is null, it is treated as identical to <see cref="HotkeyName"/>
        /// </summary>
        public string ControllerConfigurationKey { get; set; }

        /// <summary>
        /// The name of the option if serializing for a <see cref="ControllerElement"/> with an element type
        /// of <see cref="ControllerElementType.AxisPositive"/> or <see cref="ControllerElementType.AxisNegative"/>.
        /// If this is null, it is treated as identical to <see cref="ControllerConfigurationKey"/>
        /// </summary>
        public string AxisConfigurationKey { get; set; }

        /// <summary>
        /// The display name of this option.
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
