using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Extensibility;
using static Snowflake.Utility.SeedBuilder;
namespace Snowflake.Scraping.Tests
{
    [Plugin("AsyncScraper")]
    public class AsyncScraper : Scraper
    {
        public AsyncScraper()
            : base(typeof(AsyncScraper), AttachTarget.Root, SeedContent.RootSeedType)
        {
        }

        public override async Task<IEnumerable<SeedTreeAwaitable>> ScrapeAsync(ISeed parent, ILookup<string, SeedContent> rootSeeds, ILookup<string, SeedContent> childSeeds)
        {

            return _(
                await Task.Run(async () =>
                    {
                        var nestedValue = await Task.FromResult("Nested Value");
                        return ("TestAsync", $"Hello from Async Scraper", __(
                                ("TestAsyncNested", nestedValue, __(
                                  ("TestAsyncNestedTwo", await Task.FromResult("Nested Value Two"))))));
                    }),
                ("TestSync", "Synchronous and Async")
                );
        }
    }
}
