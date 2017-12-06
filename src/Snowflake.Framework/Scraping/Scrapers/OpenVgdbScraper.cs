using Snowflake.Scraping.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowflake.Scraping.Scrapers
{
    class OpenVgdbScraper
    {
        [Parent("search_crc32")]
        [Attach(AttachTarget.Root)]
        [Group("result", "game_title")] // groups all return values of this scraper under a result key with value game_title.
        [RequiresRoot("platform")]
        public IEnumerable<(string key, string value)> Scrape(ISeed parent, ILookup<string, ISeed> rootSeeds,
     ILookup<string, ISeed> childSeeds)
        {
            var crc32 = parent.Value;
            // do something here
            yield return ("result", "game_title");
        }
    }
}
