using Snowflake.Scraping.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Snowflake.Extensibility;

namespace Snowflake.Scraping.Tests
{
    [Group("MyGroup", "Test")]
    [Plugin("GroupScraper")]
    public class GroupScraper : Scraper
    {
        public GroupScraper()
            : base(typeof(GroupScraper), AttachTarget.Root, SeedContent.RootSeedType)
        {
        }

        public override IEnumerable<ReturnSeedContent> Scrape(ISeed parent, ILookup<string, SeedContent> rootSeeds, ILookup<string, SeedContent> childSeeds)
        {
            yield return ("Test", "Hello World");
            yield return ("TestTwo", "Foo Bar");
        }
    }
}
