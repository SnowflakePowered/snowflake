using System;
using System.Collections.Generic;
using Snowflake.Game;
using Snowflake.Scraper;
namespace Snowflake.Service
{
    /// <summary>
    /// This server is called to scrape games for a platform.
    /// It first identifies the game rom.
    /// It then calls the platform's preferred scraper
    /// </summary>
    public interface IScrapeService
    {
        /// <summary>
        /// Gets the game information
        /// </summary>
        /// <param name="fileName">The filename of the game</param>
        /// <returns>The game information</returns>
        IGameInfo GetGameInfo(string fileName);
        /// <summary>
        /// Gets the game information for a result
        /// </summary>
        /// <param name="gameResult">The game scrape result</param>
        /// <param name="fileName">The filename of the game</param>
        /// <returns>The game information</returns>
        IGameInfo GetGameInfo(IGameScrapeResult gameResult, string fileName);
        /// <summary>
        /// Gets the game information given an ID
        /// </summary>
        /// <param name="fileName">The filename of the game</param>
        /// <returns>The game information</returns>
        IGameInfo GetGameInfo(string id, string fileName);
        /// <summary>
        /// Gets the game results given the file name of a game
        /// </summary>
        /// <param name="fileName">The filename of the game</param>
        /// <returns>The game scrape results</returns>
        IList<IGameScrapeResult> GetGameResults(string fileName);

    }
}
