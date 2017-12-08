using Snowflake.Scraping.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Snowflake.Extensibility;
using static Snowflake.Scraping.SeedTree;

namespace Snowflake.Scraping.Tests
{
    [Plugin("GroupScraper")]
    public class GroupScraper : Scraper
    {
        public GroupScraper()
            : base(typeof(GroupScraper), AttachTarget.Root, SeedContent.RootSeedType)
        {
        }

        public override IEnumerable<SeedTreeResult> Scrape(ISeed parent, ILookup<string, SeedContent> rootSeeds, ILookup<string, SeedContent> childSeeds)
        {
            yield return ("MyGroup", "Hello World", With(
                ("Test", "Hello World"),
                ("TestTwo", "Foo Bar")));
        }
    }
}
