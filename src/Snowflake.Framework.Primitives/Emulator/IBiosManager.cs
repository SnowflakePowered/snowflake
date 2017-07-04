using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Platform;

namespace Snowflake.Emulator
{
    /// <summary>
    /// Manages BIOS files for multiple platforms.
    /// todo: Validate MD5?
    /// </summary>
    public interface IBiosManager
    {
        /// <summary>
        /// Gets the BIOS directory for a certain platform
        /// </summary>
        /// <param name="platformInfo">The platform to look for</param>
        /// <returns>The BIOS directory for the platform</returns>
        string GetBiosDirectory(IPlatformInfo platformInfo);

        /// <summary>
        /// Gets an enumerable of available BIOS files in the platform's bios directory
        /// </summary>
        /// <param name="platformInfo">The platform to look for</param>
        /// <returns>The list of available BIOS files</returns>
        IEnumerable<string> GetAvailableBios(IPlatformInfo platformInfo);

        /// <summary>
        /// Gets an enumerable of missing BIOS files according to the Stone provider
        /// </summary>
        /// <param name="platformInfo">The platform to look for</param>
        /// <returns>The list of missing BIOS files according to the Stone provider</returns>
        IEnumerable<string> GetMissingBios(IPlatformInfo platformInfo);

        /// <summary>
        /// Checks whether or not a certain BIOS is installed in the user's system.
        /// </summary>
        /// <param name="platformInfo">The platform to look for</param>
        /// <param name="biosName">The name of the BIOS to check</param>
        /// <returns>Whether or not the bios is available </returns>
        bool IsBiosAvailable(IPlatformInfo platformInfo, string biosName);
    }
}
