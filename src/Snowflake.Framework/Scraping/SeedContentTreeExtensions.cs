using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Scraping
{
    internal static class SeedContentTreeExtensions
    {
        public static IEnumerable<ISeed> Collapse(this SeedTree @this, ISeed parent, string source)
        {
            var parentSeed = new Seed(@this.Content, parent, source);
            yield return parentSeed;
            foreach (var child in @this.Children)
            {
                foreach (var childSeed in child.Collapse(parentSeed, source))
                {
                    yield return childSeed;
                }
            }
        }
    }
}
