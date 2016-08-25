using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Input.Controller;

namespace Snowflake.Configuration.Input.Hotkey

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
        string OptionName { get; }

        /// <summary>
        /// The key of the configuration option
        /// </summary>
        string KeyName { get; }

        /// <summary>
        /// The type of input this hotkey option accepts
        /// </summary>
        InputOptionType InputType { get; }

        /// <summary>
        /// The trigger for this hotkey
        /// </summary>
        IHotkeyTrigger Value { get; set; }
    }
}
