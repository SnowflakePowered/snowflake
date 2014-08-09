using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Plugin.Scraper;
using Snowflake.Collections;
using Snowflake.Information.Game;
using System.ComponentModel.Composition;

namespace Snowflake.Plugin.Interface
{
    [InheritedExport(typeof(IScraper))]
    public interface IScraper : IPlugin
    {
        BiDictionary<string, string> ScraperMap { get; }
        IList<GameScrapeResult> GetSearchResults(string searchQuery);
        IList<GameScrapeResult> GetSearchResults(string searchQuery, string platformId);
        Tuple<IDictionary<string, string>, GameImages> GetGameDetails(string id);
      
    }
}
