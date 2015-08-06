namespace Snowflake.Scraper
{
    public class GameScrapeResult : IGameScrapeResult
    {
        public string ID { get; }
        public string PlatformID { get; }
        public string GameTitle { get; }


        public GameScrapeResult(string id, string platformid, string gameTitle)
        {
            this.ID = id;
            this.PlatformID = platformid;
            this.GameTitle = gameTitle;
        }
    }
}
