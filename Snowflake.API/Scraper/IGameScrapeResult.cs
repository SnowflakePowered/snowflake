using System;
namespace Snowflake.Scraper
{
    public interface IGameScrapeResult
    {
        string GameTitle { get; }
        string ID { get; }
        string PlatformID { get; }
    }
}
