using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Configuration
{
    /// <summary>
    /// Represents a repeated section in an emulator configuration.
    /// </summary>
    public interface IIterableConfigurationSection : IConfigurationSection
    {
        /// <summary>
        /// The iteration number of this configuration section
        /// </summary>
        int InterationNumber { get; set; }
    }
}
