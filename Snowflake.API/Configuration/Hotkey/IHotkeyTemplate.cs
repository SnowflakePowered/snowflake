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
        /// The hotkey options of this hotkey template, without key names.
        /// </summary>
        IEnumerable<IHotkeyOption> HotkeyOptions { get; }
        /// <summary>
        /// The hidden configuration options of this hotkey template, without key names.
        /// These options will never be stored or shown to the user, and are for manipulation by the serializing instance.
        /// </summary>
        IEnumerable<IConfigurationOption> ConfigurationOptions { get; }
        /// <summary>
        /// The modifier key of the template.
        /// </summary>
        HotkeyTrigger ModifierTrigger { get; set; }

    }
}
