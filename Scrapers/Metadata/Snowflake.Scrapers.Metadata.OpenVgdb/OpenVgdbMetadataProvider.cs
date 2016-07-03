using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Records.File;
using Snowflake.Records.Game;
using Snowflake.Records.Metadata;
using Snowflake.Scraper.Providers;

namespace Snowflake.Scrapers.Metadata.OpenVgdb
{
    class OpenVgdbMetadataProvider : ScrapeProvider<IScrapeResult>
    {
        public override IEnumerable<IScrapeResult> QueryAllResults(string searchQuery, string platformId)
        {
            throw new NotImplementedException();
        }

        public override IScrapeResult QueryBestMatch(string searchQuery, string platformId)
        {
            throw new NotImplementedException();
        }

        [Provider]
        [RequiredMetadata(FileMetadataKeys.RomCanonicalTitle)]
        [ReturnMetadata("TestMetadataReturn")]
        public IScrapeResult Test(IMetadataCollection collection)
        {
            IMetadataCollection retmetadata = new ScrapedMetadataCollection("Test", 1.0);
            retmetadata.Add("TestMetadataReturn", "lmao");
            return (IScrapeResult)retmetadata;
        }

    }
}
