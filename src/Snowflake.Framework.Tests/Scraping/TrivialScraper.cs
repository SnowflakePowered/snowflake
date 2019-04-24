using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Snowflake.Extensibility;
using System.Threading.Tasks;
using Snowflake.Scraping.Extensibility;
using static Snowflake.Scraping.Extensibility.SeedBuilder;

namespace Snowflake.Scraping.Tests
{
    [Plugin("TrivialScraper")]
    public class TrivialScraper : Scraper
    {
        public TrivialScraper()
            : base(typeof(TrivialScraper), AttachTarget.Root, SeedContent.RootSeedType)
        {
        }

        public override async IAsyncEnumerable<SeedTree> ScrapeAsync(ISeed parent,
            ILookup<string, SeedContent> rootSeeds, ILookup<string, SeedContent> childSeeds,
            ILookup<string, SeedContent> siblingSeeds)
        {
            yield return ("Test", "Hello World");
        }
    }

    [Plugin("TrivialScraperTwo")]
    public class TrivialScraperTwo : Scraper
    {
        public TrivialScraperTwo()
            : base(typeof(TrivialScraperTwo), AttachTarget.Root, SeedContent.RootSeedType)
        {
        }

        public override async IAsyncEnumerable<SeedTree> ScrapeAsync(ISeed parent,
            ILookup<string, SeedContent> rootSeeds, ILookup<string, SeedContent> childSeeds,
            ILookup<string, SeedContent> siblingSeeds)
        {
            yield return ("Test", "Goodbye World");
        }
    }
}
