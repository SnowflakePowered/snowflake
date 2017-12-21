using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Scraping;

namespace Snowflake.Scraping.Extensibility
{
    /// <summary>
    /// <para>
    /// Provides helper methods for building <see cref="SeedTree"/> and <see cref="SeedTreeAwaitable"/>
    /// for use in <see cref="IScraper"/> implementations.
    /// </para>
    /// <para>
    /// Enables use of underscore methods for "SeedBuilder syntax".
    /// Import this class with a <code>using static</code> directive.
    /// </para>
    /// <para>
    /// Use <seealso cref="_(SeedTreeAwaitable[])"/> to begin a seed tree, and
    /// <seealso cref="__(SeedTree[])"/> to continue for nested seeds.
    /// </para>
    /// <para>
    /// Remember that top-level seeds can be <seealso cref="Task{SeedTree}"/> and other awaitables.
    /// </para>
    /// </summary>
    public static class SeedBuilder
    {
        /// <summary>
        /// Begins a seed tree result with multiple top-level results.
        /// Top-level seeds can be awaitable.
        /// Use Seed Builder Syntax rather than the full call.
        /// <see cref="_(SeedTreeAwaitable[])"/>
        /// </summary>
        /// <param name="children">The list of seed results to return</param>
        /// <returns>A seed tree result.</returns>
        public static IEnumerable<SeedTreeAwaitable> AwaitableSeeds(params SeedTreeAwaitable[] children) => children;

        /// <summary>
        /// Continues a seed tree result with a list of nested results.
        /// Nested seeds can not be awaitable, but can be returned from an asynchronous context.
        /// Use Seed Builder Syntax rather than the full call.
        /// </summary>
        /// <param name="children">The list of seed results to return</param>
        /// <returns>The nested portion of a seed tree result.</returns>
        public static IEnumerable<SeedTree> WithSeeds(params SeedTree[] children) => children;

        /// <summary>
        /// Begins a seed tree result with multiple top-level results.
        /// Top-level seeds can be awaitable.
        /// </summary>
        /// <param name="children">The list of seed results to return</param>
        /// <returns>A seed tree result.</returns>
        public static IEnumerable<SeedTreeAwaitable> _(params SeedTreeAwaitable[] children) => AwaitableSeeds(children);

        /// <summary>
        /// Continues a seed tree result with multiple nested seeds.
        /// </summary>
        /// <param name="children">The list of seed results to return</param>
        /// <returns>The nested portion of a seed tree result.</returns>
        public static IEnumerable<SeedTree> __(params SeedTree[] children) => WithSeeds(children);

        /// <summary>
        /// Begins a seed tree result with a single top level seed.
        /// Top-level seeds can be awaitable.
        /// </summary>
        /// <param name="type">The type of the seed.</param>
        /// <param name="value">The value of the seed.</param>
        /// <param name="children">The list of seed results to return</param>
        /// <returns>A seed tree result.</returns>
        public static IEnumerable<SeedTreeAwaitable> _(string type, string value, IEnumerable<SeedTree> children)
            => _((type, value, children));

        /// <summary>
        /// Returns a single seed result.
        /// </summary>
        /// <param name="type">The type of the seed.</param>
        /// <param name="value">The value of the seed.</param>
        /// <returns>A single seed result.</returns>
        public static IEnumerable<SeedTreeAwaitable> _(string type, string value)
            => _((type, value));

        /// <summary>
        /// Continues a seed tree result with a single nested seed.
        /// Nested seeds must not be awaitable, but can be returned from an asynchronous context.
        /// </summary>
        /// <param name="type">The type of the seed.</param>
        /// <param name="value">The value of the seed.</param>
        /// <param name="children">The list of seed results to return</param>
        /// <returns>A seed tree result.</returns>
        public static IEnumerable<SeedTree> __(string type, string value, IEnumerable<SeedTree> children)
           => __((type, value, children));

    }
}
