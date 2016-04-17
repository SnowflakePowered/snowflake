using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Configuration.Attributes
{
    /// <summary>
    /// Represents one option in an emulator configuration inside a configuration section.
    /// </summary>
    /// <see cref="Snowflake.Configuration.IConfigurationSection"></see>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class ConfigurationOptionAttribute : Attribute
    {
        /// <summary>
        /// The display name for human readable purposes of this option
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// A description of this configuration option
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Whether or not this option is iterable, i.e. the OptionName contains a '{N}' to replace with the iteration
        /// </summary>
        public bool Iterable { get; set; } = false;

        /// <summary>
        /// Whether or not this option maps to an input device
        /// </summary>
        public bool IsInput { get; set; } = false;

        /// <summary>
        /// Whether or not this option is a simple option (displayed in "Simple" configuration mode)
        /// </summary>
        public bool Simple { get; set; } = false;

        /// <summary>
        /// Whether or not this option is private (not ever displayed to the user)
        /// </summary>
        public bool Private { get; set; } = false;

        /// <summary>
        /// A 'flag' property is never serialized into the configuration option, and is instead used to cause
        /// side effects to the configuration during emulator instance creation by the emulator handler.
        /// If a flag affects the configuration, it should be placed in the same section it modifies.
        /// </summary>
        public bool Flag { get; set; } = false;

        /// <summary>
        /// The name of the option as it appears inside the emulator configuration 
        /// </summary>
        public string OptionName { get; }

        public ConfigurationOptionAttribute(string optionName)
        {
            this.OptionName = optionName;
        }

    }
}
