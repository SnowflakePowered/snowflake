using System;
using System.Collections.Generic;

namespace Snowflake.Scraping
{
    /// <summary>
    /// Represents the context in which <see cref="ISeed"/> can have 
    /// parent-child relationships.
    /// </summary>
    public interface ISeedRootContext
    {
        /// <summary>
        /// Returns the seed with the given GUID. If it does not exists, returns null.
        /// </summary>
        /// <param name="seedGuid">The GUID of the seed.</param>
        /// <returns>The seed with the given GUID if it exists within the context, otherwise the root seed.</returns>
        ISeed? this[Guid seedGuid] { get; }

        /// <summary>
        /// Gets the root of the context where all seeds are attached to
        /// </summary>
        ISeed Root { get; }

        /// <summary>
        /// Gets a unique ID identifying this context. The <see cref="Root"/> will have a <see cref="ISeed.Parent"/>
        /// value of <see cref="SeedCollectionGuid"/>
        /// </summary>
        Guid SeedCollectionGuid { get; }

        /// <summary>
        /// Gets all the seeds in this context, whether culled or unculled.
        /// </summary>
        /// <returns>All the seed in this context.</returns>
        IEnumerable<ISeed> GetAll();

        /// <summary>
        /// Gets seeds that have not been marked as culled in this context.
        /// </summary>
        /// <returns>All the seed in this context that have not been marked as culled.</returns>
        IEnumerable<ISeed> GetUnculled();

        /// <summary>
        /// Gets all the unculled seeds of the given type that exist in this context.
        /// </summary>
        /// <param name="type">The semantic type of the seed.</param>
        /// <returns>All seeds with the given type that exist in the context.</returns>
        IEnumerable<ISeed> GetAllOfType(string type);

        /// <summary>
        /// Gets all the unculled children of the given seed relative to this context, not
        /// including the given seed.
        /// </summary>
        /// <param name="seed">The seed of which whose children are returned.</param>
        /// <returns>All the children of the given seed relative to this context.</returns>
        IEnumerable<ISeed> GetChildren(ISeed? seed);

        /// <summary>
        /// Gets all the unculled siblings of the given seed relative to this context,
        /// not including the given seed.
        /// </summary>
        /// <param name="seed">The seed of which whose siblings are returned.</param>
        /// <returns>All the siblings of the given seed relative to this context.</returns>
        IEnumerable<ISeed> GetSiblings(ISeed? seed);

        /// <summary>
        /// Gets all the unculled children of the root.
        /// </summary>
        /// <returns>All the children of the root.</returns>
        IEnumerable<ISeed> GetRootSeeds();

        /// <summary>
        /// Gets all the unculled descendants of a given seed.
        /// </summary>
        /// <param name="seed">The seed whose descendants are returned.</param>
        /// <returns>All the descendants of the given seed.</returns>
        IEnumerable<ISeed> GetDescendants(ISeed? seed);

        /// <summary>
        /// Adds a seed to the context.
        /// </summary>
        /// <param name="value">The seed content of the seed.</param>
        /// <param name="parent">The parent of the seed.</param>
        /// <param name="source">The source of the seed.</param>
        /// <returns>The newly created seed with the given value and a parent relative to this context.</returns>
        ISeed Add(SeedContent value, ISeed parent, string source);

        /// <summary>
        /// Culls the given seed and all of its descendants.
        /// </summary>
        /// <param name="seed">The seed to cull.</param>
        void CullSeedTree(ISeed? seed);

        /// <summary>
        /// Adds a seed directly to the context.
        /// </summary>
        /// <param name="seed">The seed to add to the context.</param>
        void Add(ISeed? seed);

        /// <summary>
        /// Adds multiple seeds directly to the context. 
        /// </summary>
        /// <param name="seeds">The seeds to add directly to the context.</param>
        void AddRange(IEnumerable<ISeed?> seeds);

        /// <summary>
        /// Adds multiple seeds to the context.
        /// </summary>
        /// <param name="seeds">The values of the seeds to add.</param>
        /// <param name="source">The source of the seed.</param>
        /// <returns>The newly created seeds with the given value and a parent relative to this context.</returns>
        IEnumerable<ISeed> AddRange(IEnumerable<(SeedContent value, ISeed parent)> seeds, string source);
    }
}
