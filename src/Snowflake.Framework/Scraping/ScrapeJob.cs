using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Snowflake.Scraping.Extensibility;

namespace Snowflake.Scraping
{
    public class ScrapeJob : IScrapeJob
    {
        /// <summary>
        ///  The default source for a Client-provided seed.
        /// </summary>
        public const string ClientSeedSource = "__client";

        /// <inheritdoc />
        public IEnumerable<IScraper> Scrapers { get; }

        /// <inheritdoc />
        public ISeedRootContext Context { get; }

        /// <inheritdoc />
        private IList<(string, Guid)> Visited { get; }

        /// <inheritdoc />
        public Guid JobGuid { get; }

        /// <inheritdoc />
        public IEnumerable<ICuller> Cullers { get; }

        public ScrapeJob(IEnumerable<IScraper> scrapers,
            IEnumerable<ICuller> cullers)
            : this(Enumerable.Empty<SeedContent>(), scrapers, cullers)
        {
        }

        public ScrapeJob(IEnumerable<SeedContent> initialSeeds,
            IEnumerable<IScraper> scrapers,
            IEnumerable<ICuller> cullers)
            : this(initialSeeds, scrapers, cullers, Guid.NewGuid())
        {
        }

        public ScrapeJob(IEnumerable<SeedTree> initialSeeds,
            IEnumerable<IScraper> scrapers,
            IEnumerable<ICuller> cullers)
            : this(initialSeeds, scrapers, cullers, Guid.NewGuid())
        {
        }

        internal ScrapeJob(IEnumerable<SeedContent> initialSeeds,
            IEnumerable<IScraper> scrapers, IEnumerable<ICuller> cullers, Guid jobGuid)
        {
            this.Context = new SeedRootContext();
            this.Visited = new List<(string, Guid)>();
            this.Scrapers = scrapers;
            this.Cullers = cullers;
            this.Context.AddRange(initialSeeds.Select(p => (p, this.Context.Root)), ScrapeJob.ClientSeedSource);
            this.JobGuid = jobGuid;
        }

        internal ScrapeJob(IEnumerable<SeedTree> initialSeeds,
            IEnumerable<IScraper> scrapers, IEnumerable<ICuller> cullers, Guid jobGuid)
        {
            this.Context = new SeedRootContext();
            this.Visited = new List<(string, Guid)>();
            this.Scrapers = scrapers;
            this.Cullers = cullers;
            this.Context.AddRange(
                initialSeeds.SelectMany(s => s.Collapse(this.Context.Root, ScrapeJob.ClientSeedSource)));
            this.JobGuid = jobGuid;
        }

        private ISeed GetAttachTarget(AttachTarget t, ISeed matchingSeed)
        {
            switch (t)
            {
                case AttachTarget.Root:
                    return this.Context.Root;
                case AttachTarget.Target:
                    return matchingSeed;
                case AttachTarget.TargetParent:
                    return this.Context[matchingSeed.Parent];
                default:
                    return this.Context.Root;
            }
        }

        public Task<bool> Proceed() => this.Proceed(Enumerable.Empty<SeedContent>());

        public async Task<bool> Proceed(IEnumerable<SeedContent> seedsToAdd)
        {
            // Add any client seeds.
            this.Context.AddRange(seedsToAdd.Select(p => (p, this.Context.Root)), ScrapeJob.ClientSeedSource);

            // Keep track of previously visited seeds.
            int previousCount = this.Visited.Count;
            foreach (var scraper in this.Scrapers)
            {
                var matchingSeeds = this.Context.GetAllOfType(scraper.TargetType)
                    .Where(s => !this.Visited.Contains((scraper.Name, s.Guid))).ToList();

                // todo: Parallelize this.
                foreach (var matchingSeed in matchingSeeds)
                {
                    // todo: Refactor this out into child/sibling/root 

                    var requiredChildrenDirectives = scraper.Directives
                        .Where(p => p.Target == AttachTarget.Target && p.Directive == Directive.Requires).ToList();
                    var excludedChildrenDirectives = scraper.Directives
                        .Where(p => p.Target == AttachTarget.Target && p.Directive == Directive.Excludes)
                        .Select(p => p.Type).ToList();

                    var requiredRootDirectives = scraper.Directives
                        .Where(p => p.Target == AttachTarget.Root && p.Directive == Directive.Requires).ToList();
                    var excludedRootDirectives = scraper.Directives
                        .Where(p => p.Target == AttachTarget.Root && p.Directive == Directive.Excludes)
                        .Select(p => p.Type).ToList();

                    var requiredSiblingDirectives = scraper.Directives
                        .Where(p => p.Target == AttachTarget.TargetParent && p.Directive == Directive.Requires)
                        .ToList();
                    var excludedSiblingDirectives = scraper.Directives
                        .Where(p => p.Target == AttachTarget.TargetParent && p.Directive == Directive.Excludes)
                        .Select(p => p.Type).ToList();

                    var requiredChildren = requiredChildrenDirectives
                        .SelectMany(seed => this.Context.GetChildren(matchingSeed)
                            .Where(child => child.Content.Type == seed.Type))
                        .ToLookup(x => x.Content.Type, x => x.Content);

                    var requiredRoots = requiredRootDirectives
                        .SelectMany(seed => this.Context.GetChildren(this.Context.Root)
                            .Where(child => child.Content.Type == seed.Type))
                        .ToLookup(x => x.Content.Type, x => x.Content);

                    var requiredSiblings = requiredSiblingDirectives
                        .SelectMany(seed => this.Context.GetSiblings(matchingSeed)
                            .Where(child => child.Content.Type == seed.Type))
                        .ToLookup(x => x.Content.Type, x => x.Content);

                    // We need to ensure that the number of keys match before continuing.
                    // If the keys match then we are guaranteed that every key is successfully fulfilled.
                    if (requiredChildren.GroupBy(p => p.Key).Count() != requiredChildrenDirectives.Count()
                        || requiredRoots.GroupBy(p => p.Key).Count() != requiredRootDirectives.Count()
                        || requiredSiblings.GroupBy(p => p.Key).Count() != requiredSiblingDirectives.Count()
                        || this.Context.GetChildren(matchingSeed).Select(p => p.Content.Type)
                            .Intersect(excludedChildrenDirectives).Any()
                        || this.Context.GetRootSeeds().Select(p => p.Content.Type)
                            .Intersect(excludedRootDirectives).Any()
                        || this.Context.GetSiblings(matchingSeed).Select(p => p.Content.Type)
                            .Intersect(excludedSiblingDirectives).Any())
                    {
                        continue;
                    }

                    var resultsToAppend = new List<ISeed>();

                    // Collect the results.
                    var results = new List<SeedContent>();
                    var attachSeed = this.GetAttachTarget(scraper.AttachPoint, matchingSeed);

                    foreach (var task in await scraper.ScrapeAsync(matchingSeed, requiredRoots, requiredChildren,
                        requiredSiblings))
                    {
                        try
                        {
                            resultsToAppend.AddRange((await task).Collapse(attachSeed, scraper.Name));
                        }
                        catch
                        {
                            continue;
                        }
                    }

                    this.Visited.Add((scraper.Name, matchingSeed.Guid)); // mark that matchingSeed was visited.

                    // Attach the seeds.
                    this.Context.AddRange(resultsToAppend);
                }
            }

            return
                this.Visited.Count !=
                previousCount; // if there are no new additions to the table, then we know to stop.
        }

        public void Cull() => this.Cull(Enumerable.Empty<Guid>());

        public void Cull(IEnumerable<Guid> manualCull)
        {
            foreach (var culledSeed in manualCull.SelectMany(m => this.Context.GetAll().Where(s => s.Guid == m)))
            {
                this.Context.CullSeedTree(culledSeed);
            }

            foreach (var culler in this.Cullers)
            {
                var seedsToCull = this.Context.GetAllOfType(culler.TargetType);
                var unculledSeeds = culler.Filter(seedsToCull, this.Context);
                var unculledSeedsGuid = unculledSeeds.Select(p => p.Guid).ToList();
                foreach (var culledSeed in seedsToCull.Where(s => !unculledSeedsGuid.Contains(s.Guid)))
                {
                    this.Context.CullSeedTree(culledSeed);
                }
            }
        }
    }
}
