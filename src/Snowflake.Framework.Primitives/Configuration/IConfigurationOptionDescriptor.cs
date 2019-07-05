﻿using System;
using System.Collections.Generic;
using Snowflake.Configuration.Attributes;

namespace Snowflake.Configuration
{
    /// <summary>
    /// Describes a configuration option.
    /// </summary>
    public interface IConfigurationOptionDescriptor
    {
        /// <summary>
        /// Gets the display name for human readable purposes of this option
        /// </summary>
        string DisplayName { get; }

        /// <summary>
        /// Gets a description of this configuration option
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Gets a value indicating whether whether or not this option is a simple option (displayed in "Simple" configuration mode)
        /// </summary>
        bool Simple { get; }

        /// <summary>
        /// Gets a value indicating whether whether or not this option is private (not ever displayed to the user)
        /// </summary>
        bool Private { get; }

        /// <summary>
        /// Gets a value indicating whether a 'flag' property is never serialized into the configuration option, and is instead used to cause
        /// side effects to the configuration during emulator instance creation by the emulator handler.
        /// If a flag affects the configuration, it should be placed in the same section it modifies.
        /// </summary>
        bool Flag { get; }

        /// <summary>
        /// Gets the maximum value allowable for a number value
        /// </summary>
        double Max { get; }

        /// <summary>
        /// Gets the minimum value allowable for a number value
        /// </summary>
        double Min { get; }

        /// <summary>
        /// Gets the increment to increase this by
        /// </summary>
        double Increment { get; }

        /// <summary>
        /// Gets a value indicating whether whether or not this string is a file path.
        /// </summary>
        bool IsPath { get; }

        /// <summary>
        /// If <see cref="IsPath"/> is true, the type of the path.
        /// </summary>
        PathType PathType { get; }

        /// <summary>
        /// Gets the name of the option as it appears inside the emulator configuration
        /// </summary>
        string OptionName { get; }

        /// <summary>
        /// Gets the key of the configuration option
        /// </summary>
        string OptionKey { get; }

        /// <summary>
        /// Gets the default object.
        /// </summary>
        object Default { get; }

        /// <summary>
        /// Gets the CLR type of the option
        /// </summary>
        Type Type { get; }

        /// <summary>
        /// Gets the option type of the option
        /// </summary>
        ConfigurationOptionType OptionType { get; }

        /// <summary>
        /// Gets any custom metadata attached to this option
        /// </summary>
        /// <see cref="CustomMetadataAttribute"/>
        IDictionary<string, object> CustomMetadata { get; }

        /// <summary>
        /// Gets a list of selection option descriptors if this option is an enum.
        /// </summary>
        IEnumerable<ISelectionOptionDescriptor> SelectionOptions { get; }

        /// <summary>
        /// Gets a value indicating whether whether or not this option is an enum.
        /// </summary>
        bool IsSelection { get; }

        /// <summary>
        /// For string options, when the option is set to null, it is serialized as
        /// this value.
        /// </summary>
        string Unset { get; }
    }
}
