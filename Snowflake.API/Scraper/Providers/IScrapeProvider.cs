using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Records.Metadata;

namespace Snowflake.Scraper.Providers
{
    public interface IScrapeProvider<T>
    {
        T QueryBestMatch(string searchQuery, string platformId);
        IEnumerable<T> Query(string searchQuery, string platformId);
        IEnumerable<T> Query(IMetadataCollection metadata);
        IEnumerable<T> Query(IMetadataCollection metadata, params string[] wantedMetadata);

    }
}
