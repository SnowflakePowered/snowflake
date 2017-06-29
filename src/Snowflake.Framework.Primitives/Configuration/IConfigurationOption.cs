using System;
using System.Collections.Generic;
using Snowflake.Configuration.Attributes;

namespace Snowflake.Configuration
{
    /// <summary>
    /// Represents a configuration option.
    /// </summary>
    public interface IConfigurationOption
    {
        /// <summary>
        /// The display name for human readable purposes of this option
        /// </summary>
        string DisplayName { get; } 

        /// <summary>
        /// A description of this configuration option
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Whether or not this option is a simple option (displayed in "Simple" configuration mode)
        /// </summary>
        bool Simple { get; }

        /// <summary>
        /// Whether or not this option is private (not ever displayed to the user)
        /// </summary>
        bool Private { get; }

        /// <summary>
        /// A 'flag' property is never serialized into the configuration option, and is instead used to cause
        /// side effects to the configuration during emulator instance creation by the emulator handler.
        /// If a flag affects the configuration, it should be placed in the same section it modifies.
        /// </summary>
        bool Flag { get; }

        /// <summary>
        /// The maximum value allowable for a number value
        /// </summary>
        double Max { get; }
        /// <summary>
        /// The minimum value allowable for a number value
        /// </summary>
        double Min { get; }

        /// <summary>
        /// The increment to increase this by
        /// </summary>
        double Increment { get; }

        /// <summary>
        /// Whether or not this string is a file path.
        /// </summary>
        bool IsPath { get; }

        /// <summary>
        /// The name of the option as it appears inside the emulator configuration 
        /// </summary>
        string OptionName { get; }

        /// <summary>
        /// The key of the configuration option
        /// </summary>
        string KeyName { get; }

        /// <summary>
        /// The default object.
        /// </summary>
        object Default { get; }

        /// <summary>
        /// The CLR type of the option
        /// </summary>
        Type Type { get; }

        /// <summary>
        /// Any custom metadata attached to this option
        /// </summary>
        /// <see cref="CustomMetadataAttribute"/>
        IDictionary<string, object> CustomMetadata { get; }
    }

}
