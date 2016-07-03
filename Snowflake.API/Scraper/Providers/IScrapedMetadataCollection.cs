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
        string MetadataSource { get; }
        /// <summary>
        /// The accuracy
        /// </summary>
        double Accuracy { get; }
    }
}