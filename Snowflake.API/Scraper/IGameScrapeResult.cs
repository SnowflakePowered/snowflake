namespace Snowflake.Scraper
{
    /// <summary>
    /// Represents a result returned from scraping
    /// This is not the game information itself, only an identifier that can be used to gain the game information
    /// </summary>
    public interface IGameScrapeResult
    {
        /// <summary>
        /// The title returned from the scrape result
        /// </summary>
        string GameTitle { get; }
        /// <summary>
        /// The search ID that can be used to refer the details of the result
        /// </summary>
        string ID { get; }
        /// <summary>
        /// The plaform ID related to this result
        /// </summary>
        string PlatformID { get; }
        /// <summary>
        /// The accuracy or faith in this result
        /// </summary>
        double Accuracy { get; }
        /// <summary>
        /// The Scraper that produced this result
        /// </summary>
        string Scraper { get; }
    }
}
