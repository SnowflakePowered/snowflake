using System;
using Snowflake.Game;
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
    }
}
