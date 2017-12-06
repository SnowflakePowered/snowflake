using Snowflake.Scraping.Attributes;
using Snowflake.Scraping.Attributes.Conditions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowflake.Scraping
{
    public class FileMimetypeScraper
    {
        [Parent("file")] // all ISeed objects will be of type file.
        [Attach(AttachTarget.Parent)] // resultant seeds attach to parent
        [RequiresRoot("platform")]
        public IEnumerable<(string key, string value)>
            DetermineMimetype(ISeed target, ILookup<string, ISeed> rootSeeds, ILookup<string, ISeed> childSeeds)
        {
            var platform = rootSeeds["platform"].First();
            // do something with platform...
            yield return ("mimetype", "mymimetype");
        }
    }
}
