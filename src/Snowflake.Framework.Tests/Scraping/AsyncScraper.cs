using Snowflake.Scraping.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Snowflake.Extensibility;
using System.Threading.Tasks;
using static Snowflake.Scraping.SeedTree;
namespace Snowflake.Scraping.Tests
{
    [Plugin("AsyncScraper")]
    public class AsyncScraper : Scraper
    {
        public AsyncScraper()
            : base(typeof(AsyncScraper), AttachTarget.Root, SeedContent.RootSeedType)
        {
        }

        public override IEnumerable<SeedTreeAwaitable> Scrape(ISeed parent, ILookup<string, SeedContent> rootSeeds, ILookup<string, SeedContent> childSeeds)
        {
            yield return Task.Run(async () => {
                var nestedValue = await Task.FromResult("Nested Value");
                return ("TestAsync", $"Hello from Async Scraper", _(
                        ("TestAsyncNested", nestedValue, _(
                          ("TestAsyncNestedTwo", await Task.FromResult("Nested Value Two"))))));
                });
            yield return ("TestSync", "Synchronous and Async");
        }
    }
}
