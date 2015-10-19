using System.Collections.Generic;
using Snowflake.Game;
using Snowflake.Platform;
using Snowflake.Romfile;
using Snowflake.Scraper;

namespace Snowflake.Service
{
    /// <summary>
    /// Performs intellisent scraping given only the known filename
    /// </summary>
    public interface IScrapeEngine
    {
        /// <summary>
        /// Get an object used to scrape a game given the filename, and an optional known platform.
        /// The known platform will only be used if it can not be determined by file signature alone.
        /// </summary>
        /// <param name="fileName">The filename of the game file</param>
        /// <param name="knownPlatform">That platform that will be used if this is not able to be determined by file signature</param>
        /// <returns>Scrapable information</returns>
        IScrapableInfo GetScrapableInfo(string fileName, IPlatformInfo knownPlatform = null);
        /// <summary>
        /// Gets game data from a ScrapableInfo object.
        /// </summary>
        /// <param name="information">The scrapable information representing a ROM file</param>
        /// <param name="acceptableAccuracy">An acceptable accuracy value from 0 to 1.0. It will return the match that reaches this accuracy value</param>
        /// <returns>A GameInfo object that can be added to the database</returns>
        IGameInfo GetGameData(IScrapableInfo information, double acceptableAccuracy);
        /// <summary>
        /// Gets game data from a ScrapableInfo object with a default acceptableAccuracy of 1.0 (Max)
        /// </summary>
        /// <param name="information">The scrapable information representing a ROM file</param>
        /// <returns>A GameInfo object that can be added to the database</returns>
        IGameInfo GetGameData(IScrapableInfo information);
        /// <summary>
        /// Gets game data from a certain scraper
        /// </summary>
        /// <param name="information">The scrapable information representing a ROM file</param>
        /// <param name="scraper">The scraper to use</param>
        /// <returns>A GameInfo object that can be added to the database</returns>
        IGameInfo GetGameData(IScrapableInfo information, IScraper scraper);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="information"></param>
        /// <param name="scrapeResult"></param>
        /// <returns></returns>
        IGameInfo GetGameData(IScrapableInfo information, IGameScrapeResult scrapeResult);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="information"></param>
        /// <param name="scrapeResult"></param>
        /// <param name="scraper"></param>
        /// <returns></returns>
        IGameInfo GetGameData(IScrapableInfo information, IGameScrapeResult scrapeResult, IScraper scraper);

        /// <summary>
        /// Gets filtered scrape results
        /// </summary>
        /// <param name="information"></param>
        /// <returns></returns>
        IList<IGameScrapeResult> GetScrapeResults(IScrapableInfo information);
        /// <summary>
        /// Gets scrape resutls with a given scraper
        /// </summary>
        /// <param name="information"></param>
        /// <param name="scraper"></param>
        /// <returns></returns>
        IList<IGameScrapeResult> GetScrapeResults(IScrapableInfo information, IScraper scraper);
    }
}