namespace Snowflake.Scraper
{
    /// <summary>
    /// Represents a result returned from scraping
    /// This is not the game information itself, only an identifier that can be used to gain the game information
    /// </summary>
    public interface IGameScrapeResult
    {
        string GameTitle { get; }
        string ID { get; }
        string PlatformID { get; }
    }
}
