using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Platform;

namespace Snowflake.Service
{
    /// <summary>
    /// Provides Stone platform and controller layout data
    /// </summary>
    public interface IStoneProvider
    {
        /// <summary>
        /// The list of platforms loaded for this core service
        /// </summary>
        IDictionary<string, IPlatformInfo> Platforms { get; }

        /// <summary>
        /// The version of stone definitions loaded
        /// </summary>
        Version StoneVersion { get; }
    }
}
