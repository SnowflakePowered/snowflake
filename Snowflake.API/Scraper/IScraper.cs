using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using Snowflake.Utility;
using Snowflake.Plugin;
using Snowflake.Identifier;
namespace Snowflake.Scraper
{
    /// <summary>
    /// Represents a scraper plugin that is able to grab information from a source
    /// </summary>
    [InheritedExport(typeof(IScraper))]
    public interface IScraper : IBasePlugin
    {
        /// <summary>
        /// A map of Snowflake's platform IDs to how the scraper source identifies platforms
        /// BiDictionary allows for backwards mapping from how the scraper source identifiers platforms to Snowflake's platform IDs
        /// </summary>
        BiDictionary<string, string> ScraperMap { get; }
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
        /// Gets the search results given a string query
        /// </summary>
        /// <param name="identifiedMetadata">The identified metadata gleaned from the ROM file</param>
        /// <param name="searchQuery">The string to search</param>
        /// <returns>A list of search results</returns>
        IList<IGameScrapeResult> GetSearchResults(IList<IIdentifiedMetadata> identifiedMetadata, string platformId);
        /// <summary>
        /// Gets the search results given a string query and the Snowflake platform ID of a platform
        /// </summary>
        /// <param name="identifiedMetadata">The identified metadata gleaned from the ROM file</param>
        /// <param name="searchQuery">The string to search</param>
        /// <param name="platformId">The Snowflake platform ID</param>
        /// <returns>A list of search results</returns>
        IList<IGameScrapeResult> GetSearchResults(IList<IIdentifiedMetadata> identifiedMetadata, string searchQuery, string platformId);
        /// <summary>
        /// Sorts the list of results from best match to worst match given the available metadata from
        /// the game scrape results and the identified metadata for the game
        /// </summary>
        /// <param name="identifiedMetadata">The identified metadata gleaned from the ROM file</param>
        /// <param name="searchResults">The search results to sort</param>
        /// <returns>A sorted list of search results ordered from best to worst</returns>
        IList<IGameScrapeResult> SortBestResults(IList<IIdentifiedMetadata> identifiedMetadata, IList<IGameScrapeResult> searchResults);
        /// <summary>
        /// Gets the details of the games from the result id
        /// </summary>
        /// <param name="id">Search Result ID</param>
        /// <returns>Tuple containing game metadata and game images</returns>
        Tuple<IDictionary<string, string>, IGameImagesResult> GetGameDetails(string id);
    }
}
