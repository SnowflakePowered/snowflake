using Snowflake.Platform;
using Snowflake.Service;

namespace Snowflake.Extensions
{
    public static class PlatformExts
    {
        /// <summary>
        /// This method is used as part of a core function, 
        /// and does not serve the pure data role of the PlatformInfo class.
        /// Extension methods are put in when they are unrelated to the class and is not used as part of the class.
        /// </summary>
        /// <param name="platform"></param>
        /// <returns></returns>
        public static ScrapeService GetScrapeEngine(this IPlatformInfo platform) 
        {
            return new ScrapeService(platform);
        }
        /// <summary>
        /// This method is used as part of a core function, 
        /// and does not serve the pure data role of the PlatformInfo class.
        /// Extension methods are put in when they are unrelated to the class and is not used as part of the class.
        /// </summary>
        /// <param name="platform"></param>
        /// <param name="scraperName">The name of the scraper</param>
        /// <returns></returns>s
        public static ScrapeService GetScrapeEngine(this IPlatformInfo platform, string scraperName)
        {
            return new ScrapeService(platform, scraperName);
        }
    }
}
