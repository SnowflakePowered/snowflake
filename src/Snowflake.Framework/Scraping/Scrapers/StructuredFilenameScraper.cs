using Snowflake.Romfile;
using Snowflake.Scraping.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Snowflake.Scraping.Scrapers
{
    [Target("file")] // all ISeed objects will be of type file.
    [Attach(AttachTarget.Target)] // resultant seeds attach to parent
    public class StructuredFilenameScraper : Scraper
    {
        public override IEnumerable<SeedContent> Scrape(ISeed parent,
            ILookup<string, SeedContent> rootSeeds, ILookup<string, SeedContent> childSeeds)
        {
            IStructuredFilename filename = new StructuredFilename(parent.Content.Value);

            if (filename.NamingConvention == NamingConvention.Unknown)
            {
                yield break;
            }

            yield return ("filename_title", filename.Title);
            yield return ("naming_convention", filename.NamingConvention.ToString());
            yield return ("filename_year", filename.Year);
            yield return ("filename_region", filename.RegionCode);
        }
    }
}
