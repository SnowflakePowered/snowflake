using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Scraping;

namespace Snowflake.Scraping.Extensibility
{
    /// <summary>
    /// <para>
    /// Provides helper methods for building <see cref="SeedTree"/>
    /// for use in <see cref="IScraper"/> implementations.
    /// </para>
    /// <para>
    /// Enables use of underscore methods for "SeedBuilder syntax".
    /// Import this class with a <code>using static</code> directive.
    /// </para>
    /// <para>
    /// <seealso cref="_(SeedTree[])"/> to continue for nested seeds.
    /// </para>
    /// <para>
    /// Remember that top-level seeds can be <seealso cref="Task{SeedTree}"/> and other awaitables.
    /// </para>
    /// </summary>
    public static class SeedBuilder
    {
        
        /// <summary>
        /// Continues a seed tree result with a list of nested results.
        /// Nested seeds can not be awaitable, but can be returned from an asynchronous context.
        /// Use Seed Builder Syntax rather than the full call.
        /// </summary>
        /// <param name="children">The list of seed results to return</param>
        /// <returns>The nested portion of a seed tree result.</returns>
        public static IEnumerable<SeedTree> WithSeeds(params SeedTree[] children) => children;

        /// <summary>
        /// Continues a seed tree result with multiple nested seeds.
        /// </summary>
        /// <param name="children">The list of seed results to return</param>
        /// <returns>The nested portion of a seed tree result.</returns>
        public static IEnumerable<SeedTree> _(params SeedTree[] children) => WithSeeds(children);
    }
}
