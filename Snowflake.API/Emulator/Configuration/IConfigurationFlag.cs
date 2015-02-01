using System;
using System.Collections.Generic;
namespace Snowflake.Emulator.Configuration
{
    /// <summary>
    /// The configuration flag API allows an emulator bridge to define simple per-game flags that can affect
    /// the output of a compiled configuration. This abstracts away the need for a user to understand 
    /// complex configuration options and expose only the ones needed in either a selectable choice, a boolean or an integer switch.
    /// </summary>
    public interface IConfigurationFlag
    {
        /// <summary>
        /// The default value of the flag
        /// </summary>
        string DefaultValue { get; }
        /// <summary>
        /// The description of the flag
        /// </summary>
        string Description { get; }
        /// <summary>
        /// The flag key
        /// </summary>
        string Key { get; }
        /// <summary>
        /// The available choices if this is a SELECT_FLAG type
        /// </summary>
        IReadOnlyDictionary<string, string> SelectValues { get; }
        /// <summary>
        /// The minimum value permitted if this is an INT_FLAG type
        /// 0 if no minimum or not INT_FLAG
        /// </summary>
        int RangeMin { get; }
        /// <summary>
        /// The maximum value permitted if this is an INT_FLAG type
        /// 0 if no maximum or not INT_FLAG
        /// </summary>
        int RangeMax { get; }
        /// <summary>
        /// The type of configuration flag
        /// </summary>
        ConfigurationFlagTypes Type { get; }
    }
}
