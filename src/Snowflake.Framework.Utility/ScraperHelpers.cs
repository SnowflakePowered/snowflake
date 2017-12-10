using System;
using System.Collections.Generic;
using System.Text;
using Snowflake.Scraping;

namespace Snowflake.Utility
{
    public static class ScraperHelpers
    {
        public static IEnumerable<SeedTreeAwaitable> Seeds(params SeedTreeAwaitable[] children) => children;
        public static IEnumerable<SeedTree> WithSeed(params SeedTree[] children) => children;

        /// <summary>
        /// Begins a seed tree result.
        /// Note the double underscore to begin.
        /// </summary>
        /// <param name="children"></param>
        /// <returns></returns>
        public static IEnumerable<SeedTreeAwaitable> _(params SeedTreeAwaitable[] children) => Seeds(children);

        /// <summary>
        /// Continues a seed tree result with nested seeds.
        /// </summary>
        /// <param name="children"></param>
        /// <returns></returns>
        public static IEnumerable<SeedTree> __(params SeedTree[] children) => WithSeed(children);

        public static IEnumerable<SeedTreeAwaitable> _(string type, string value, IEnumerable<SeedTree> children)
            => _((type, value, children));

        public static IEnumerable<SeedTree> __(string type, string value, IEnumerable<SeedTree> children)
           => __((type, value, children));

        public static IEnumerable<SeedTreeAwaitable> _(string type, string value)
            => _((type, value));

    }
}
