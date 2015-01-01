using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using Snowflake.Utility;
using Snowflake.Plugin;
namespace Snowflake.Scraper
{
    [InheritedExport(typeof(IScraper))]
    public interface IScraper : IBasePlugin
    {
        BiDictionary<string, string> ScraperMap { get; }
        IList<IGameScrapeResult> GetSearchResults(string searchQuery);
        IList<IGameScrapeResult> GetSearchResults(string searchQuery, string platformId);
        Tuple<IDictionary<string, string>, IGameImagesResult> GetGameDetails(string id);
    }
}
