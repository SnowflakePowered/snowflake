using Snowflake.Records.Metadata;

namespace Snowflake.Scraper.Providers
{
    /// <summary>
    /// Represents a metadata collection from scraping
    /// </summary>
    public interface IScrapedMetadataCollection : IMetadataCollection
    {
        /// <summary>
        /// Gets the result sorce
        /// </summary>
        string Source { get; }

        /// <summary>
        /// Gets the accuracy
        /// </summary>
        double Accuracy { get; }
    }
}