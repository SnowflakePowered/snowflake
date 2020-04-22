using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Snowflake.Extensibility;
using System.Threading.Tasks;
using Snowflake.Scraping.Extensibility;
using static Snowflake.Scraping.Extensibility.SeedBuilder;
using System.Threading;
using System.Runtime.CompilerServices;

namespace Snowflake.Scraping.Tests
{
    [Plugin("GroupScraper")]
    public class GroupScraper : Scraper
    {
        public GroupScraper()
            : base(typeof(GroupScraper), AttachTarget.Root, SeedContent.RootSeedType)
        {
        }

        public override async IAsyncEnumerable<SeedTree> ScrapeAsync(ISeed parent,
            ILookup<string, SeedContent> rootSeeds, ILookup<string, SeedContent> childSeeds,
            ILookup<string, SeedContent> siblingSeeds, [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            yield return ("MyGroup", "Hello World",
                _(("Test", "Hello World"),
                  ("TestTwo", "Foo Bar")
                 ));
          
        }
    }
}
