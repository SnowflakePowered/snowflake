using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Scraper;
using Snowflake.Service;

namespace Snowflake.Events.CoreEvents.GameEvent
{
    public class GameResultsScrapedEventArgs : SnowflakeEventArgs
    {
        public IScraper GameScraper { get; private set; }
        public string GameFileName { get; private set;}
        public IList<IGameScrapeResult> ScrapedResults { get; private set; }
        public GameResultsScrapedEventArgs(ICoreService eventCoreInstance, string gameFileName, IList<IGameScrapeResult> scrapedResults, IScraper scraper) : base(eventCoreInstance)
        {
            this.GameFileName = gameFileName;
            this.ScrapedResults = scrapedResults;
            this.GameScraper = scraper;
        }
       
    }
}
