namespace Snowflake.Scraper
{
    public class GameScrapeResult : IGameScrapeResult
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
