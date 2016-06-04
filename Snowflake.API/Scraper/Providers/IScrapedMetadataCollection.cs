namespace Snowflake.Scraper.Provider
{
    public interface IScrapedMetadataCollection
    {
        string ScraperId { get; }
        double Accuracy { get; }
        string Title { get; set; }
    }
}