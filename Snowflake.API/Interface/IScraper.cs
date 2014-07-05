using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.API.Base.Scraper;
using Snowflake.API.Collections;
using Snowflake.API.Information.Game;

namespace Snowflake.API.Interface
{
    public interface IScraper : IPlugin
    {
        BiDictionary<string, string> ScraperMap { get; }
        List<GameScrapeResult> GetSearchResults(string searchQuery);
        List<GameScrapeResult> GetSearchResults(string searchQuery, string platformId);
        Tuple<Dictionary<string, string>, GameImages> GetGameDetails(string id);
      
    }
}
