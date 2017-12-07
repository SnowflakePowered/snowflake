using Snowflake.Scraping.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowflake.Scraping.Scrapers
{
    [Target("filename_title")] // all ISeed objects will be of type file. that are not culled.
    [Attach(AttachTarget.Root)] // resultant seeds attach to parent
    public class StructuredFilenameTitleLifter : Scraper
    {
        public override IEnumerable<SeedContent> Scrape(ISeed parent, ILookup<string, SeedContent> rootSeeds, ILookup<string, SeedContent> childSeeds)
        {
            yield return ("search_title", parent.Content.Value); // lifts the 'filename_title' key to a 'search' key to the root.
        }
    }
}
