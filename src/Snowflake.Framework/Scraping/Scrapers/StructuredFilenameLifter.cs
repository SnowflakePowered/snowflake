using Snowflake.Romfile;
using Snowflake.Scraping.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Scraping.Scrapers
{
    public class StructuredFilenameLifter
    {
        [Parent("filename_title")] // all ISeed objects will be of type file. that are not culled.
        [Attach(AttachTarget.Root)] // resultant seeds attach to parent
        public IEnumerable<(string key, string value)> GetSeeds(ISeed parent)
        {
            yield return ("search", parent.Value); // lifts the 'filename_title' key to a 'search' key to the root.
        }
    }
}
