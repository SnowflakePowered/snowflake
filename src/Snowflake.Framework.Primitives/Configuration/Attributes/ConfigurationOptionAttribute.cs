﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
        /// Gets or sets the display name for human readable purposes of this option
        /// </summary>
        public string DisplayName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a description of this configuration option
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether whether or not this option is a simple option (displayed in "Simple" configuration mode)
        /// </summary>
        public bool Simple { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether whether or not this option is private (not ever displayed to the user)
        /// </summary>
        public bool Private { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether a 'flag' property is never serialized into the configuration option, and is instead used to cause
        /// side effects to the configuration during emulator instance creation by the emulator handler.
        /// If a flag affects the configuration, it should be placed in the same section it modifies.
        /// </summary>
        public bool Flag { get; set; } = false;

        /// <summary>
        /// Gets or sets the maximum value allowable for a number value
        /// </summary>
        public double Max { get; set; } = 0;

        /// <summary>
        /// Gets or sets the minimum value allowable for a number value
        /// </summary>
        public double Min { get; set; } = 0;

        /// <summary>
        /// Gets or sets the increment to increase a numerical value by
        /// </summary>
        public double Increment { get; set; } = 1;

        /// <summary>
        /// If <see cref="IsPath"/> is true, specifies the type of the path.
        /// </summary>
        public PathType PathType { get; } = PathType.NotPath;

        /// <summary>
        /// Gets the name of the option as it appears inside the emulator configuration
        /// </summary>
        public string OptionName { get; }

        /// <summary>
        /// Gets the default value of this option.
        /// </summary>
        public object Default { get; }

        /// <summary>
        /// Gets the CLR type of this option.
        /// </summary>
        private Type Type { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationOptionAttribute"/> class.
        /// Represents one option in an emulator configuration inside a configuration section.
        /// Typically configuration options must be a double, bool, integer or an enum value in order to be safe,
        /// type information may be lost when serializing into a wire format.
        /// </summary>
        public ConfigurationOptionAttribute(string optionName, int @default)
            : this(optionName, @default, typeof(int))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationOptionAttribute"/> class.
        /// Represents one option in an emulator configuration inside a configuration section.
        /// Typically configuration options must be a double, bool, integer or an enum value in order to be safe,
        /// type information may be lost when serializing into a wire format.
        /// </summary>
        public ConfigurationOptionAttribute(string optionName, bool @default)
            : this(optionName, @default, typeof(bool))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationOptionAttribute"/> class.
        /// Represents one option in an emulator configuration inside a configuration section.
        /// Typically configuration options must be a double, bool, integer or an enum value in order to be safe,
        /// type information may be lost when serializing into a wire format.
        /// </summary>
        /// <param name="optionName">The name of the option</param>
        /// <param name="default">The default value of the option. Note that only strings, enums and primitive types are supported.</param>
        public ConfigurationOptionAttribute(string optionName, double @default)
            : this(optionName, @default, typeof(double))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationOptionAttribute"/> class.
        /// Represents one option in an emulator configuration inside a configuration section.
        /// Typically configuration options must be a double, bool, integer or an enum value in order to be safe,
        /// type information may be lost when serializing into a wire format.
        /// </summary>
        /// <param name="optionName">The name of the option</param>
        /// <param name="default">The default value of the option. Note that only strings, enums and primitive types are supported.</param>
        public ConfigurationOptionAttribute(string optionName, object @default)
            : this(optionName, @default, @default.GetType())
        {
            if (!this.Type.GetTypeInfo().IsEnum)
            {
                throw new ArgumentException("Configuration options can not be complex objects.");
            }
        }

        /// <summary>
        /// Initializes a new <see cref="ConfigurationOptionAttribute"/>.
        /// Represents one option in an emulator configuration inside a configuration section.
        /// Typically configuration options must be a double, bool, integer or an enum value in order to be safe,
        /// type information may be lost when serializing into a wire format.
        /// </summary>
        /// <param name="optionName">The name of the option</param>
        /// <param name="default">The default value of the option. Note that only strings, enums and primitive types are supported.</param>
        public ConfigurationOptionAttribute(string optionName, string @default)
            : this(optionName, @default ?? string.Empty, typeof(string))
        {
        }

        /// <summary>
        /// Initializes a new <see cref="ConfigurationOptionAttribute"/> that represents a path.
        /// Represents one option in an emulator configuration inside a configuration section.
        /// Typically configuration options must be a double, bool, integer or an enum value in order to be safe,
        /// type information may be lost when serializing into a wire format.
        /// </summary>
        /// <param name="optionName">The name of the option</param>
        /// <param name="default">The default value of the option. Note that only strings, enums and primitive types are supported.</param>
        /// <param name="pathType">The type of the path this points to.</param>
        public ConfigurationOptionAttribute(string optionName, string @default, PathType pathType)
            : this(optionName, @default, typeof(string))
        {
            this.PathType = pathType;
        }


        private ConfigurationOptionAttribute(string optionName, object @default, Type valueType)
        {
            this.OptionName = optionName;
            this.Default = @default;
            this.Type = valueType;
        }
    }
}
