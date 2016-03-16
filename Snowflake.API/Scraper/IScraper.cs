using System;
using System.Collections.Generic;
using Snowflake.Platform;
using Snowflake.Extensibility;
using Snowflake.Romfile;

namespace Snowflake.Scraper
{
    /// <summary>
    /// Represents a scraper plugin that is able to grab information from a source
    /// </summary>
    public interface IScraper : IPlugin
    {
        /// <summary>
        /// The estimated accuracy of this scraper
        /// </summary>
        double ScraperAccuracy { get; }
        /// <summary>
        /// A map of Snowflake's platform IDs to how the scraper source identifies platforms
        /// </summary>
        IDictionary<string, string> ScraperMap { get; }
        /// <summary>
        /// Gets the search results given a string query
        /// </summary>
        /// <param name="searchQuery">The string to search</param>
        /// <returns></returns>
        IList<IGameScrapeResult> GetSearchResults(string searchQuery);
        /// <summary>
        /// Gets the search results given a string query and the Snowflake platform ID of a platform
        /// </summary>
        /// <param name="searchQuery">The string to search</param>
        /// <param name="platformId">The Snowflake platform ID</param>
        /// <returns>A list of search results</returns>
        IList<IGameScrapeResult> GetSearchResults(string searchQuery, string platformId);
        /// <summary>
        /// Gets the search results 
        /// </summary>
        /// <returns>A list of search results</returns>
        IList<IGameScrapeResult> GetSearchResults(IScrapableInfo scrapableInfo);
        /// <summary>
        /// Gets the details of the games from the result id
        /// </summary>
        /// <param name="id">Search Result ID</param>
        /// <returns>Tuple containing game metadata and game images</returns>
        Tuple<IDictionary<string, string>, IGameImagesResult> GetGameDetails(string id);
        /// <summary>
        /// Gets the details of the games from the result id
        /// </summary>
        /// <param name="scrapeResult">Search Result object</param>
        /// <returns>Tuple containing game metadata and game images</returns>
        Tuple<IDictionary<string, string>, IGameImagesResult> GetGameDetails(IGameScrapeResult scrapeResult);
    }
}
