using System;

namespace Snowflake.Scrapers.Metadata.TheGamesDb.TheGamesDbApi
{
    /// <summary>
    /// Represents a search result when searching for games.
    /// </summary>
    internal class ApiGameSearchResult
    {
        /// <summary>
        /// Unique database ID.
        /// </summary>
        public int ID;

        /// <summary>
        /// Title of the game.
        /// </summary>
        public string Title;

        /// <summary>
        /// Date on which the game was released.
        /// </summary>
        public string ReleaseDate;

        /// <summary>
        /// Which platform the game is for.
        /// </summary>
        public string Platform;
    }
}
