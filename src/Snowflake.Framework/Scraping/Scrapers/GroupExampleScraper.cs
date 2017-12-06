using Snowflake.Scraping.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowflake.Scraping.Scrapers
{
    [Attach(AttachTarget.Root)] //no parent specifies means ISeed will be the root.
    [Group("result", "group_name")] // groups all return values of this scraper under a result key with value game_title.
    public class GroupExampleScraper : Scraper
    {
        public override IEnumerable<SeedContent> Scrape(ISeed parent, ILookup<string, SeedContent> rootSeeds, ILookup<string, SeedContent> childSeeds)
        {
            yield return ("group_name", "GroupExample");
            yield return ("group_data", "Some data that should be grouped");
        }
    }
}
