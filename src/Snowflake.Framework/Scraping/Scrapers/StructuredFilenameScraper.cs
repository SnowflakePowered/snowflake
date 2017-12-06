using Snowflake.Romfile;
using Snowflake.Scraping.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Scraping.Scrapers
{
    public class StructuredFilenameScraper
    {
        [Parent("file")] // all ISeed objects will be of type file.
        [Attach(AttachTarget.Parent)] // resultant seeds attach to parent
        public IEnumerable<(string key, string value)> GetSeeds(ISeed parent)
        {
            IStructuredFilename filename = new StructuredFilename(parent.Value);
            yield return ("filename_title", filename.Title);
            yield return ("naming_convention", filename.NamingConvention.ToString());
            yield return ("filename_year", filename.Year);
            yield return ("filename_region", filename.RegionCode);
        }
    }
}
