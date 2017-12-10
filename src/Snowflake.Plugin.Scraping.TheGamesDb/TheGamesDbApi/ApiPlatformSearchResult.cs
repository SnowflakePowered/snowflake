using System;

namespace Snowflake.Plugin.Scrapers.TheGamesDb.TheGamesDbApi
{
    /// <summary>
    /// Represents a search result when listing games.
    /// </summary>
    internal class ApiPlatformSearchResult
    {
        /// <summary>
        /// Gets or sets unique database ID.
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Gets or sets name of the platform.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets uRL alias
        /// </summary>
        public string Alias { get; set; }
    }
}
