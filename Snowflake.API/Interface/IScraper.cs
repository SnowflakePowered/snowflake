using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.API.Base.Scraper;
using Snowflake.API.Collections;
using Snowflake.API.Information.Game;
using System.ComponentModel.Composition;

namespace Snowflake.API.Interface
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
