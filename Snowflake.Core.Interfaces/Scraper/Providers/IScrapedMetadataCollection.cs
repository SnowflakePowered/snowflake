using Snowflake.Records.Metadata;

namespace Snowflake.Scraper.Providers
{
    /// <summary>
    /// Represents a metadata collection from scraping
    /// </summary>
    public interface IScrapedMetadataCollection : IMetadataCollection
    {
        /// <summary>
        /// The result sorce
        /// </summary>
        string Source { get; }
        /// <summary>
        /// The accuracy
        /// </summary>
        double Accuracy { get; }
    }
}