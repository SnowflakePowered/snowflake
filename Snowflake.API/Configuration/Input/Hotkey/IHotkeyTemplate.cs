using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Input.Controller;
using Snowflake.Input.Controller.Mapped;
using Snowflake.Input.Device;

namespace Snowflake.Configuration.Input.Hotkey
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
        /// The display name of the section for human-readable purposes
        /// </summary>
        string DisplayName { get; }
        /// <summary>
        /// A description of what this section does
        /// </summary>
        string Description { get; }
        /// <summary>
        /// The hotkey options of this configuration section, without key names.
        /// </summary>
        IEnumerable<IHotkeyOption> HotkeyOptions { get; }
        /// <summary>
        /// The options of this configuration section, without key names.
        /// </summary>
        IEnumerable<IConfigurationOption> ConfigurationOptions { get; }
    }
}
