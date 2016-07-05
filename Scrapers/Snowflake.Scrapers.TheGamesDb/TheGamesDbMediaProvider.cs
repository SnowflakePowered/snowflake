using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Snowflake.Records.File;
using Snowflake.Records.Game;
using Snowflake.Records.Metadata;
using Snowflake.Utility;
using Snowflake.Scraper.Providers;
using Snowflake.Scrapers.TheGamesDb.TheGamesDbApi;
using Snowflake.Caching;
namespace Snowflake.Scrapers.TheGamesDb
{
    public class TheGamesDbMediaProvider : ScrapeProvider<IList<IFileRecord>>
    {
        public override IEnumerable<IList<IFileRecord>> Query(string searchQuery, string platformId)
        {
            throw new NotImplementedException();
        }

        public override IList<IFileRecord> QueryBestMatch(string searchQuery, string platformId)
        {
            throw new NotImplementedException();
        }

        [Provider]
        [RequiredMetadata("scraper_thegamesdb_id")]
        [ReturnMetadata(ImageMetadataKeys.CacheId)]
        [ReturnMetadata(ImageMetadataKeys.Type)]
        [ReturnMetadata(ImageTypes.MediaBoxartFront)]
        public IList<IFileRecord> GetBoxart(IMetadataCollection collection)
        {
            return this.QueryBestMatch(collection[FileMetadataKeys.RomCanonicalTitle],
                collection[FileMetadataKeys.RomPlatform]);
        }

    }
}
