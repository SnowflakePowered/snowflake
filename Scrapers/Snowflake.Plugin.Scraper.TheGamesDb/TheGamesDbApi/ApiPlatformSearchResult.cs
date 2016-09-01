using System;

namespace Snowflake.Plugin.Scraper.TheGamesDb.TheGamesDbApi
{
    /// <summary>
    /// Represents a search result when listing games.
    /// </summary>
    internal class ApiPlatformSearchResult
    {
        /// <summary>
        /// Unique database ID.
        /// </summary>
        public int ID;

        /// <summary>
        /// Name of the platform.
        /// </summary>
        public string Name;

        /// <summary>
        /// URL alias
        /// </summary>
        public string Alias;
    }
}
