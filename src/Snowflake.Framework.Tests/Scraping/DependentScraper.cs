using Snowflake.Scraping.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Snowflake.Extensibility;
using System.Threading.Tasks;
using static Snowflake.Utility.SeedBuilder;

namespace Snowflake.Scraping.Tests
{
    [RequiresRoot("Test")]
    [Plugin("DependentScraper")]
    public class DependentScraper : Scraper
    {
        public DependentScraper()
            : base(typeof(DependentScraper), AttachTarget.Root, SeedContent.RootSeedType)
        {
        }

        public override async Task<IEnumerable<SeedTreeAwaitable>> ScrapeAsync(ISeed parent, ILookup<string, SeedContent> rootSeeds, ILookup<string, SeedContent> childSeeds)
        {
            return _("TestDependent", $"{rootSeeds["Test"].First()} from Dependent Scraper");
        }
    }
}
