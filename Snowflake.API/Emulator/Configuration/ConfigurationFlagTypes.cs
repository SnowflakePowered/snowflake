using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Emulator.Configuration
{
    /// <summary>
    /// Types of Configuration Flags
    /// Part of the Configuration Flag API
    /// </summary>
    public enum ConfigurationFlagTypes
    {
        /// <summary>
        /// The flag is either true or false
        /// </summary>
        BOOLEAN_FLAG,
        /// <summary>
        /// The flag is an integer
        /// </summary>
        INTEGER_FLAG,
        /// <summary>
        /// The flag is a string selectable from a choice of other selectable strings
        /// </summary>
        SELECT_FLAG
    }
}
