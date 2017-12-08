using Snowflake.Scraping.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Snowflake.Extensibility;

namespace Snowflake.Scraping.Tests
{
    [Plugin("TrivialScraper")]
    public class TrivialScraper : SynchronousScraper
    {
        public TrivialScraper()
            : base(typeof(TrivialScraper), AttachTarget.Root, SeedContent.RootSeedType)
        {
        }

        public override IEnumerable<SeedContent> Scrape(ISeed parent, ILookup<string, SeedContent> rootSeeds, ILookup<string, SeedContent> childSeeds)
        {
            yield return ("Test", "Hello World");
        }
    }

    [Plugin("TrivialScraperTwo")]
    public class TrivialScraperTwo : SynchronousScraper
    {
        public TrivialScraperTwo()
            : base(typeof(TrivialScraperTwo), AttachTarget.Root, SeedContent.RootSeedType)
        {
        }

        public override IEnumerable<SeedContent> Scrape(ISeed parent, ILookup<string, SeedContent> rootSeeds, ILookup<string, SeedContent> childSeeds)
        {
            yield return ("Test", "Goodbye World");
        }
    }
}
