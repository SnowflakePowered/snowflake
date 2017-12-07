using Snowflake.Scraping.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Snowflake.Scraping.Tests
{
    [Target(SeedContent.RootSeedType)]
    [Attach(AttachTarget.Root)]
    public class TrivialScraper : Scraper
    {
        public override IEnumerable<SeedContent> Scrape(ISeed parent, ILookup<string, SeedContent> rootSeeds, ILookup<string, SeedContent> childSeeds)
        {
            yield return ("Test", "Hello World");
        }
    }

    [Target(SeedContent.RootSeedType)]
    [Attach(AttachTarget.Root)]
    public class TrivialScraperTwo : Scraper
    {
        public override IEnumerable<SeedContent> Scrape(ISeed parent, ILookup<string, SeedContent> rootSeeds, ILookup<string, SeedContent> childSeeds)
        {
            yield return ("Test", "Goodbye World");
        }
    }
}
