using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Platform
{
    /// <summary>
    /// This database keeps the preferred plugins of a platform.
    /// Usually is populated with IPlatformDefaults, can be changed. 
    /// </summary>
    public interface IPlatformPreferenceDatabase
    {
        /// <summary>
        /// Add a platform to database
        /// </summary>
        /// <param name="platformInfo"></param>
        void AddPlatform(IPlatformInfo platformInfo);
        /// <summary>
        /// Get the preferrences for a certain platform in the form of an IPlatformDefaults object
        /// </summary>
        /// <param name="platformInfo">The platform</param>
        /// <returns>The preferences for a certain platform</returns>
        IPlatformDefaults GetPreferences(IPlatformInfo platformInfo);
        void SetEmulator(IPlatformInfo platformInfo);
        /// <summary>
        /// Sets the preferred scraper for a platform
        /// </summary>
        /// <param name="platformInfo">The preferred scraper for a platform</param>
        void SetScraper(IPlatformInfo platformInfo);
        /// <summary>
        /// Sets the preferred identifier for a platform
        /// </summary>
        /// <param name="platformInfo">The preferred identifier for a platform</param>
        void SetIdentifier(IPlatformInfo platformInfo);
    }
}
