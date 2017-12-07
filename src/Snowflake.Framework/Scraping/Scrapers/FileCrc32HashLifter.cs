using Snowflake.Scraping.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Snowflake.Scraping.Scrapers
{

    [Target("files")] // all ISeed objects will be of type file. that are not culled.
    [RequiresChild("crc32")]
    [Attach(AttachTarget.Root)] // resultant seeds attach to parent
    public class FileCrc32HashLifter : Scraper
    {
        public override IEnumerable<SeedContent> Scrape(ISeed parent, ILookup<string, SeedContent> rootSeeds, ILookup<string, SeedContent> childSeeds)
        {
            yield return ("search_crc32", childSeeds["crc32"].First().Value);
                    // lifts the 'crc32' key to a 'search_crc32' key to the root.
        }
    }
}
