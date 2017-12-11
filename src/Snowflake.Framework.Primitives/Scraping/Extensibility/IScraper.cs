using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Snowflake.Extensibility;

namespace Snowflake.Scraping.Extensibility
{
    /// <summary>
    /// A <see cref="IScraper"/> specifies a type of <see cref="ISeed"/>
    /// and some restrictions on the seed, and uses the value of the seed to
    /// return a resultant tree of new seeds.
    /// </summary>
    public interface IScraper : IPlugin
    {
        /// <summary>
        /// Gets the seed node where the results of this scraper will attach to the
        /// scrape context tree.
        /// </summary>
        AttachTarget AttachPoint { get; }

        /// <summary>
        /// Gets the type of seed this <see cref="IScraper"/> will examine to produce
        /// new seeds.
        /// </summary>
        string TargetType { get; }

        /// <summary>
        /// Gets the list of directives or restrictions that must be fulfilled if this
        /// <see cref="IScraper"/> is to be run.
        /// </summary>
        IEnumerable<IScraperDirective> Directives { get; }

        /// <summary>
        /// Once all <see cref="Directives"/> have been fulfilled, returns
        /// a new tree of seeds that are attached to the specified attach point on
        /// the job <see cref="ISeedRootContext"/>.
        /// <para>
        /// While scraping must be an asynchronous action, there is no requirement that
        /// the actual processing must be asynchronous. You may not need to use
        /// async/await at all and return a seed tree as normal using
        /// SeedBuilder syntax.
        /// </para>
        /// </summary>
        /// <param name="target">An instance of the seed with the specified <see cref="TargetType"/>.</param>
        /// <param name="rootSeeds">Any seeds, keyed on their type, that must exist as children of the root as specified in <see cref="Directives"/>.</param>
        /// <param name="childSeeds">Any seeds, keyed on their type, that must exist as children of the target seed as specified in <see cref="Directives"/>.</param>
        /// <param name="siblingSeeds">Any seeds, keyed on their type, that must exist as siblings of the target seed as specified in <see cref="Directives"/></param>
        /// <returns>A tree of seeds based on information available in the given target. <seealso cref="SeedTreeAwaitable"/></returns>
        Task<IEnumerable<SeedTreeAwaitable>> ScrapeAsync(ISeed target,
            ILookup<string, SeedContent> rootSeeds,
            ILookup<string, SeedContent> childSeeds,
            ILookup<string, SeedContent> siblingSeeds);
    }
}
