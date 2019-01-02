using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Snowflake.Scraping.Extensibility;

namespace Snowflake.Scraping
{
    /// <summary>
    /// Represents a job to produce a seed tree that can be traversed to yield usable
    /// records or other metadata.
    /// </summary>
    public interface IScrapeJob
    {
        /// <summary>
        /// Gets the seed root context local to this job.
        /// </summary>
        ISeedRootContext Context { get; }

        /// <summary>
        /// Gets the list of <see cref="ICuller"/> that will be used to cull the resultant seed tree.
        /// </summary>
        IEnumerable<ICuller> Cullers { get; }

        /// <summary>
        /// Gets the unique ID identifying this job.
        /// </summary>
        Guid JobGuid { get; }

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
        /// <returns>Whether or not all possible seeds have been added with the given <see cref="Scrapers"/></returns>
        Task<bool> Proceed();

        /// <summary>
        /// Proceed with populating the seed tree using the given <see cref="Scrapers"/>.
        /// </summary>
        /// <param name="seedsToAdd">Adds seeds to the root of the tree before running scrapers.</param>
        /// <returns>Whether or not all possible seeds have been added with the given <see cref="Scrapers"/></returns>
        Task<bool> Proceed(IEnumerable<SeedContent> seedsToAdd);
    }
}
