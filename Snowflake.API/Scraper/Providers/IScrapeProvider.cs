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
        IList<T> QueryAllResults(string searchQuery, string platformId);
        T QueryBestMatch(string searchQuery, string platformId);
        T Query(IMetadataCollection metadata);
        T Query(IMetadataCollection metadata, params string[] wantedMetadata);

    }
}
