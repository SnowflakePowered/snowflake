namespace Snowflake.Platform
{
    /// <summary>
    /// Represents the default-use plugins for a certain platform
    /// Only used to populate initial <see cref="IPlatformPreferenceDatabase"/>  entry
    /// </summary>
    public interface IPlatformDefaults
    {
        /// <summary>
        /// The default emulator plugin
        /// </summary>
        string Emulator { get; set; }
        /// <summary>
        /// The default scraper plugin
        /// </summary>
        string Scraper { get; set; }
    }
}
