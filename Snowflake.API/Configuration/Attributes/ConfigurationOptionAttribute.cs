using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Configuration.Attributes
{
    /// <summary>
    /// Represents one option in an emulator configuration inside a configuration section.
    /// Typically configuration options must be a double, bool, integer or an enum value in order to be safe,
    /// type information may be lost when serializing into a wire format.
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
        /// The maximum value allowable for a number value
        /// </summary>
        public double Max { get; set; } = 0;

        /// <summary>
        /// The minimum value allowable for a number value
        /// </summary>
        public double Min { get; set; } = 0;

        /// <summary>
        /// The increment to increase a numerical value by
        /// </summary>
        public double Increment { get; set; } = 1;

        /// <summary>
        /// Whether or not this string is a file path.
        /// </summary>
        public bool IsPath { get; set; } = false;

        /// <summary>
        /// The name of the option as it appears inside the emulator configuration 
        /// </summary>
        public string OptionName { get; }

        public object Default { get; }

        internal Type Type { get; }

        /// <summary>
        /// Represents one option in an emulator configuration inside a configuration section.
        /// Typically configuration options must be a double, bool, integer or an enum value in order to be safe,
        /// type information may be lost when serializing into a wire format.
        /// </summary>
        /// <param name="optionName">The name of the option as it appears inside the emulator configuration</param>
        /// <see cref="Snowflake.Configuration.IConfigurationSection"></see>
        /// <seealso cref="System.Attribute" />
        [Obsolete]
        public ConfigurationOptionAttribute(string optionName)
        {
            this.OptionName = optionName;
        }

        public ConfigurationOptionAttribute(string optionName, int value) : this(optionName, value, typeof(int))
        {
        }
        public ConfigurationOptionAttribute(string optionName, bool value) : this(optionName, value, typeof(bool))
        {
        }
        public ConfigurationOptionAttribute(string optionName, double value) : this(optionName, value, typeof(double))
        {
        }
        public ConfigurationOptionAttribute(string optionName, object @default) : this(optionName, @default, @default.GetType())
        {
            if (!this.Type.IsEnum) throw new ArgumentException("Configuration options can not be complex objects.");
        }
        public ConfigurationOptionAttribute(string optionName, string value) : this(optionName, value, typeof(string))
        {
        }
        private ConfigurationOptionAttribute(string optionName, object @default, Type valueType)
        {
            
            this.OptionName = optionName;
            this.Default = @default;
            this.Type = valueType;
        }

    }
}
