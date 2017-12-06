using Snowflake.Scraping.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowflake.Scraping.Scrapers
{
    public class TheGamesDBSearchScraper
    {
        [Parent("search_title")]
        [Attach(AttachTarget.Parent)]
        [RequiresRoot("platform")]
        public IEnumerable<(string key, string value)> Scrape(ISeed parent, ILookup<string, ISeed> rootSeeds,
            ILookup<string, ISeed> childSeeds)
        {
            var platform = rootSeeds["platform"].First();
            var search = parent.Value;
            // do something 
            yield return ("result_tgdb", "key");
        }

        [Parent("result_tgdb")]
        [Attach(AttachTarget.Root)]
        [Group("result", "game_title")] // groups all return values of this scraper under a result key with value game_title.
        public IEnumerable<(string key, string value)> Scrape_Result(ISeed parent, ILookup<string, ISeed> rootSeeds,
            ILookup<string, ISeed> childSeeds)
        {
            parent.Cull(); // Cull only your custom results.
            var lookup = parent.Value;
            //restore from cache here
            yield return ("blah", "blah");
        }
    }
}
