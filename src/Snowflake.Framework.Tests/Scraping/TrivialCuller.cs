using Snowflake.Scraping.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Scraping
{
    [Target("Test")]
    public class TrivialCuller : Culler
    {
        public override IEnumerable<ISeed> Filter(IEnumerable<ISeed> seedsToTrim)
        {
            foreach (var seed in seedsToTrim)
            {
                if (seed.Content.Value == "Goodbye World") yield return seed;
            }
        }
    }
}
