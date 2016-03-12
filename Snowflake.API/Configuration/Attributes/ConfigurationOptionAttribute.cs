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
        /// Whether or not this option is iterable, i.e. the OptionName contains a '{N}' to replace with the iteration
        /// </summary>
        public bool IsIterable { get; set; } = false;

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
