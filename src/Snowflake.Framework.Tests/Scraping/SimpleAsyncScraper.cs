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
    [Plugin("SimpleAsyncScraper")]
    public class SimpleAsyncScraper : Scraper
    {
        public SimpleAsyncScraper()
            : base(typeof(AsyncScraper), AttachTarget.Root, SeedContent.RootSeedType)
        {
        }

        public override IEnumerable<SeedTreeAwaitable> Scrape(ISeed parent, ILookup<string, SeedContent> rootSeeds, ILookup<string, SeedContent> childSeeds)
        {
            yield return Task.FromResult<SeedTree>(("TestAsync", "Hello World"));
        }
    }
}
