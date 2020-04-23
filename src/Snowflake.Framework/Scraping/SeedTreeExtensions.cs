using System;
using System.Collections.Generic;
using System.Text;
using Snowflake.Scraping.Extensibility;

namespace Snowflake.Scraping
{
    internal static class SeedTreeExtensions
    {
        public static IEnumerable<ISeed> Collapse(this SeedTree @this, ISeed parent, string source)
        {
            var parentSeed = new Seed(@this.Content, parent, source);
            yield return parentSeed;
            var seedsToProcess = new Stack<SeedTree>(@this.Children);
            while (seedsToProcess.Count != 0)
            {
                var childSeed = seedsToProcess.Pop();
                foreach (SeedTree childSeedTree in childSeed.Children)
                {
                    seedsToProcess.Push(childSeedTree);
                }
                yield return new Seed(childSeed.Content, parentSeed, source);
            }
        }
    }
}
