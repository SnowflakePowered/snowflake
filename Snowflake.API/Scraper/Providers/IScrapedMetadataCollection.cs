using Snowflake.Records.Metadata;

namespace Snowflake.Scraper.Providers
{
    /// <summary>
    /// Represents a metadata collection from scraping
    /// </summary>
    public interface IScrapedMetadataCollection : IMetadataCollection
    {
        /// <summary>
        /// The scraper ID
        /// </summary>
        string ScraperId { get; }
        /// <summary>
        /// The accuracy
        /// </summary>
        double Accuracy { get; }
        /// <summary>
        /// The title
        /// </summary>
        string Title { get; set; }
    }
}