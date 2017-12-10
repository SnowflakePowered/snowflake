using Snowflake.Extensibility;
using Snowflake.Scraping.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Scraping
{
    [Plugin("TrivialCuller")]
    public class TrivialCuller : Culler
    {
        public TrivialCuller()
            : base(typeof(TrivialCuller), "Test")
        {
        }

        public override IEnumerable<ISeed> Filter(IEnumerable<ISeed> seedsToTrim, ISeedRootContext context)
        {
            foreach (var seed in seedsToTrim)
            {
                if (seed.Content.Value == "Goodbye World")
                {
                    yield return seed;
                }
            }
        }
    }
}
