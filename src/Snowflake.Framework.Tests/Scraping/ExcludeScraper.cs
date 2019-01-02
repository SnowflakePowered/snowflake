using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Extensibility;
using Snowflake.Scraping.Extensibility;
using static Snowflake.Scraping.Extensibility.SeedBuilder;

namespace Snowflake.Scraping
{
    [Plugin("ExcludeScraper")]
    [Directive(AttachTarget.Root, Directive.Excludes, "Test")]
    [Directive(AttachTarget.Root, Directive.Requires, "ExcludeTest")]
    public class ExcludeScraper : Scraper
    {
        public ExcludeScraper()
            : base(typeof(ExcludeScraper), AttachTarget.Root, SeedContent.RootSeedType)
        {
        }

        public override async Task<IEnumerable<SeedTreeAwaitable>> ScrapeAsync(ISeed parent,
            ILookup<string, SeedContent> rootSeeds, ILookup<string, SeedContent> childSeeds,
            ILookup<string, SeedContent> siblingSeeds)
        {
            return _("ThisShouldNeverAppear", "Hello World");
        }
    }
}
