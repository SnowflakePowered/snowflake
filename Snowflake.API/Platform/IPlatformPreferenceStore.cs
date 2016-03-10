namespace Snowflake.Platform
{
    /// <summary>
    /// This database keeps the preferred plugins of a platform.
    /// Usually is populated with IPlatformDefaults, can be changed. 
    /// </summary>
    public interface IPlatformPreferenceStore
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="platformInfo">The platform</param>
        /// <param name="value">The preferred emulator for a platform</param>
        void SetEmulator(IPlatformInfo platformInfo, string value);
        /// <summary>
        /// Sets the preferred scraper for a platform
        /// </summary>
        /// <param name="platformInfo">The platform</param>
        /// <param name="value">The preferred scraper for a platform</param>
        void SetScraper(IPlatformInfo platformInfo, string value);
    }
}
