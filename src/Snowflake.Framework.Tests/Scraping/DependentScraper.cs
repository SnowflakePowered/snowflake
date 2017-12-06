using Snowflake.Scraping.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Snowflake.Scraping.Tests
{
    [Parent(SeedContent.RootSeedType)]
    [Attach(AttachTarget.Root)]
    [RequiresRoot("Test")]
    public class DependentScraper : Scraper
    {
        public override IEnumerable<SeedContent> Scrape(ISeed parent, ILookup<string, SeedContent> rootSeeds, ILookup<string, SeedContent> childSeeds)
        {
            yield return ("TestDependent", $"{rootSeeds["Test"].First()} from Dependent Scraper");
        }
    }
}
