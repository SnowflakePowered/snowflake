using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Scraping;
using Snowflake.Scraping.Extensibility;

namespace Snowflake.Scrapers
{
    public class RomFileInfoScraper : Scraper
    {
        public override Task<IEnumerable<SeedTreeAwaitable>> ScrapeAsync(ISeed parent, ILookup<string, SeedContent> rootSeeds, ILookup<string, SeedContent> childSeeds, ILookup<string, SeedContent> siblingSeeds)
        {
            throw new NotImplementedException();
        }
    }
}
