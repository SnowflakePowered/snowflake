using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Emulator.Configuration
{
    /// <summary>
    /// Represents a selectable value for a flag with a value and a description
    /// </summary>
    public interface IConfigurationFlagSelectValue
    {
        /// <summary>
        /// The value name for the select value
        /// </summary>
        string Value { get; }
        /// <summary>
        /// The description for the select value
        /// </summary>
        string Description { get; set; }
    }
}
