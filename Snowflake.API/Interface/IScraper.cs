using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.API.Base.Scraper;
using Snowflake.API.Collections;

namespace Snowflake.API.Interface
{
    public interface IScraper : IPlugin
    {
        string ScraperSource { get; }
        BiDictionary<string, string> ScraperMap { get; }
        List<GameScrapeResult> GetSearchResults(string searchQuery);
        List<GameScrapeResult> GetSearchResults(string searchQuery, string platformId);
        Tuple<Dictionary<string, string>, Dictionary<string, string>> GetGameDetails(string id);
      
    }
}
