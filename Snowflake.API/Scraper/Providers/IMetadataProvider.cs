using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Records.Metadata;
using Snowflake.Scraper.Provider;

namespace Snowflake.Scraper.Providers
{
    public interface IMetadataProvider
    {
        IList<IScrapedMetadataCollection> QueryAllResults(string searchQuery, string platformId);
        IScrapedMetadataCollection QueryBestMatch(string searchQuery, string platformId);
        IScrapedMetadataCollection Query(IMetadataCollection metadata);

    }
}
