using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Configuration.Hotkey;
using Snowflake.Input.Controller;
using Snowflake.Input.Controller.Mapped;
using Snowflake.Input.Device;

namespace Snowflake.Configuration.Hotkey
{
    /// <summary>
    /// Represents a section of input configuration for a single player, for a single device.
    /// </summary>
    public interface IHotkeyTemplate
    {
        /// <summary>
        /// The name of the section as it appears in the emulator configuration file
        /// </summary>
        string SectionName { get; }
        /// <summary>
        /// The filename of the configuration file the template belongs to
        /// </summary>
        string FileName { get; }
        /// <summary>
        /// The hotkey options of this configuration section, without key names.
        /// </summary>
        IEnumerable<IHotkeyOption> HotkeyOptions { get; }
        /// <summary>
        /// The options of this configuration section, without key names.
        /// </summary>
        IEnumerable<IConfigurationOption> ConfigurationOptions { get; }
        /// <summary>
        /// The modifier key of the template.
        /// </summary>
        HotkeyTrigger ModifierTrigger { get; set; }

    }
}
