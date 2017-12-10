using Snowflake.Scraping.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Snowflake.Extensibility;
using static Snowflake.Utility.SeedBuilder;
using System.Threading.Tasks;

namespace Snowflake.Scraping.Tests
{
    [Plugin("TrivialScraper")]
    public class TrivialScraper : Scraper
    {
        public TrivialScraper()
            : base(typeof(TrivialScraper), AttachTarget.Root, SeedContent.RootSeedType)
        {
        }

        public override async Task<IEnumerable<SeedTreeAwaitable>> ScrapeAsync(ISeed parent,
            ILookup<string, SeedContent> rootSeeds, ILookup<string, SeedContent> childSeeds, ILookup<string, SeedContent> siblingSeeds)
        {
            return _("Test", "Hello World");
        }
    }

    [Plugin("TrivialScraperTwo")]
    public class TrivialScraperTwo : Scraper
    {
        public TrivialScraperTwo()
            : base(typeof(TrivialScraperTwo), AttachTarget.Root, SeedContent.RootSeedType)
        {
        }

        public override async Task<IEnumerable<SeedTreeAwaitable>> ScrapeAsync(ISeed parent,
            ILookup<string, SeedContent> rootSeeds, ILookup<string, SeedContent> childSeeds, ILookup<string, SeedContent> siblingSeeds)
        {
            return _("Test", "Goodbye World");
        }
    }
}
