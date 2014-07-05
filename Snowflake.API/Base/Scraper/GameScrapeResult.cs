using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.API.Base.Scraper
{
    public class GameScrapeResult
    {
        public string ID { get; private set; }
        public string PlatformID { get; private set; }
        public string GameTitle { get; private set; }


        public GameScrapeResult(string id, string platformid, string gameTitle)
        {
            this.ID = id;
            this.PlatformID = platformid;
            this.GameTitle = gameTitle;
        }
    }
}
