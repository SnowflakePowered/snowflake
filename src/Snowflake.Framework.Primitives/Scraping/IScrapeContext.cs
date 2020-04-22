using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Snowflake.Scraping.Extensibility;

namespace Snowflake.Scraping
{
    /// <summary>
    /// Represents a job to produce a seed tree that can be traversed to yield usable
    /// records or other metadata.
    /// </summary>
    public interface IScrapeContext : IAsyncEnumerable<IEnumerable<ISeed>>
    {
        /// <summary>
        ///  The default source for a Client-provided seed.
        /// </summary>
        static readonly string ClientSeedSource = "__client";

        /// <summary>
        /// Gets the seed root context local to this job.
        /// </summary>
        ISeedRootContext Context { get; }

        /// <summary>
        /// Gets the list of <see cref="ICuller"/> that will be used to cull the resultant seed tree.
        /// </summary>
        IEnumerable<ICuller> Cullers { get; }

        /// <summary>
        /// Gets the list of <see cref="IScraper"/> that will be used to produce the resultant seed tree.
        /// </summary>s
        IEnumerable<IScraper> Scrapers { get; }

        /// <summary>
        /// Culls the seed tree with the given <see cref="Cullers"/>.
        /// </summary>
        void Cull();

        /// <summary>
        /// Culls the given seeds before additionally culling the seed tree with the given <see cref="Cullers"/>
        /// </summary>
        /// <param name="manualCull">The seeds to manually cull</param>
        void Cull(IEnumerable<Guid> manualCull);

        /// <summary>
        /// Proceed with populating the seed tree using the given <see cref="Scrapers"/>.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token used to halt this step.</param>
        /// <returns>Whether or not all possible seeds have been added with the given <see cref="Scrapers"/></returns>
        ValueTask<bool> Proceed(CancellationToken cancellationToken = default);

        /// <summary>
        /// Proceed with populating the seed tree using the given <see cref="Scrapers"/>.
        /// </summary>
        /// <param name="seedsToAdd">Adds seeds to the root of the tree before running scrapers.</param>
        /// <param name="cancellationToken">The cancellation token used to halt this step.</param>
        /// <returns>Whether or not all possible seeds have been added with the given <see cref="Scrapers"/></returns>
        ValueTask<bool> Proceed(IEnumerable<SeedContent> seedsToAdd, CancellationToken cancellationToken = default);

        /// <summary>
        /// Automatically gets the awaiter for the final, automatically culled, flattened seed tree state.
        /// Use the <see cref="IAsyncEnumerable{T}"/> API if you want finer control over the seed tree state, or
        /// directly call the <see cref="Proceed()"/> methods for even finer control over the tree state.
        /// </summary>
        /// <returns>The final, automatically culled, flattened seed tree state.</returns>
        ConfiguredTaskAwaitable<IEnumerable<ISeed>>.ConfiguredTaskAwaiter GetAwaiter();
    }
}
