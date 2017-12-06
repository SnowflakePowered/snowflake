using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowflake.Scraping
{
    public interface IScraper
    {
        IEnumerable<SeedContent> Scrape(ISeed parent, ILookup<string, SeedContent> rootSeeds, ILookup<string, SeedContent> childSeeds);
    }
}
