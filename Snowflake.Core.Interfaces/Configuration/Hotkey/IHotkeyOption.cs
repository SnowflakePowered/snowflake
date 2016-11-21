using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Configuration.Input;
using Snowflake.Input.Controller;

namespace Snowflake.Configuration.Hotkey

{  /// <summary>
   /// Represents a configuration option.
   /// </summary>
    public interface IHotkeyOption
    {
        /// <summary>
        /// The display name for human readable purposes of this option
        /// </summary>  
        string DisplayName { get; }

        /// <summary>
        /// Whether or not this option is private (not ever displayed to the user)
        /// </summary>
        bool Private { get; }

        /// <summary>
        /// The name of the option as it appears inside the emulator configuration 
        /// </summary>
        string HotkeyName { get; }

        /// <summary>
        /// The key of the configuration option
        /// </summary>
        string KeyName { get; }

        /// <summary>
        /// The type of input this hotkey option accepts
        /// </summary>
        InputOptionType InputType { get; }

        /// <summary>
        /// The name of the option if serializing specifically for a <see cref="KeyboardKey"/> value
        /// If this is null, it is treated as identical to <see cref="HotkeyName"/>
        /// </summary>
        string KeyboardConfigurationKey { get; }

        /// <summary>
        /// The name of the option if serializing specifically for a <see cref="ControllerElement"/> value
        /// If this is null, it is treated as identical to <see cref="HotkeyName"/>
        /// </summary>
        string ControllerConfigurationKey { get; }

        /// <summary>
        /// The name of the option if serializing for a <see cref="ControllerElement"/> with an element type
        /// of <see cref="ControllerElementType.AxisPositive"/> or <see cref="ControllerElementType.AxisNegative"/>.
        /// If this is null, it is treated as identical to <see cref="ControllerConfigurationKey"/>
        /// </summary>
        string ControllerAxisConfigurationKey { get; }

        /// <summary>
        /// The trigger for this hotkey
        /// </summary>
        HotkeyTrigger Value { get; set; }
    }
}
