using System;

namespace Snowflake.Plugin.Scrapers.TheGamesDb.TheGamesDbApi
{
    /// <summary>
    /// Represents a search result when searching for games.
    /// </summary>
    internal class ApiGameSearchResult
    {
        /// <summary>
        /// Unique database ID.
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Title of the game.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Date on which the game was released.
        /// </summary>
        public string ReleaseDate { get; set; }

        /// <summary>
        /// Which platform the game is for.
        /// </summary>
        public string Platform { get; set; }
    }
}
