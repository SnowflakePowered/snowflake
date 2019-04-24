using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Extensibility;
using Snowflake.Scraping.Extensibility;
using static Snowflake.Scraping.Extensibility.SeedBuilder;

namespace Snowflake.Scraping.Tests
{
    [Plugin("AsyncScraper")]
    public class AsyncScraper : Scraper
    {
        public AsyncScraper()
            : base(typeof(AsyncScraper), AttachTarget.Root, SeedContent.RootSeedType)
        {
        }

        public override async IAsyncEnumerable<SeedTree> ScrapeAsync(ISeed parent,
            ILookup<string, SeedContent> rootSeeds, ILookup<string, SeedContent> childSeeds,
            ILookup<string, SeedContent> siblingSeeds)
        {
            yield return await Task.Run(async () =>
            {
                var nestedValue = await Task.FromResult("Nested Value");
                return ("TestAsync", $"Hello from Async Scraper", _(
                    ("TestAsyncNested", nestedValue, _(
                        ("TestAsyncNestedTwo", await Task.FromResult("Nested Value Two"))))));
            });
            yield return ("TestSync", "Synchronous and Async");  
        }
    }
}
