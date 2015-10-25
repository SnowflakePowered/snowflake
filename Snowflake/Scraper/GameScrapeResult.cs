using System.Collections.Generic;

namespace Snowflake.Scraper
{
    public class GameScrapeResult : IGameScrapeResult
    {
        public string ID { get; }
        public string PlatformID { get; }
        public string GameTitle { get; }
        public double Accuracy { get; }
        public string Scraper { get; }

        public GameScrapeResult(string id, string platformid, string gameTitle, double accuracy, string scraper)
        {
            this.ID = id;
            this.PlatformID = platformid;
            this.GameTitle = gameTitle;
            this.Accuracy = accuracy;
            this.Scraper = scraper;
        }
    }
}
