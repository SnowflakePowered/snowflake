using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Snowflake.Scraping.Extensibility;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;
using Snowflake.Model.Game;
using Snowflake.Model.Game.LibraryExtensions;
using Snowflake.Romfile;

namespace Snowflake.Scraping
{
    public class GameScrapeContext : IScrapeContext
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
        private List<(string, Guid)> Visited { get; }

        /// <inheritdoc />
        public Guid JobGuid { get; }

        /// <inheritdoc />
        public IEnumerable<ICuller> Cullers { get; }

        public GameScrapeContext(IEnumerable<IScraper> scrapers,
            IEnumerable<ICuller> cullers)
            : this(Enumerable.Empty<SeedContent>(), scrapers, cullers)
        {
        }

        public GameScrapeContext(IEnumerable<SeedContent> initialSeeds,
            IEnumerable<IScraper> scrapers,
            IEnumerable<ICuller> cullers)
            : this(initialSeeds, scrapers, cullers, Guid.NewGuid())
        {
        }

        public GameScrapeContext(IEnumerable<SeedTree> initialSeeds,
            IEnumerable<IScraper> scrapers,
            IEnumerable<ICuller> cullers)
            : this(initialSeeds, scrapers, cullers, Guid.NewGuid())
        {
        }

        public GameScrapeContext(IGame game, IEnumerable<IScraper> scrapers,
            IEnumerable<ICuller> cullers)
            : this(GameScrapeContext.MakeGameSeeds(game), scrapers, cullers)
        {

        }

        private static IEnumerable<SeedContent> MakeGameSeeds(IGame game)
        {
            yield return ("platform", game.Record.PlatformId);
            if (game.Record.Title != null) {
                yield return ("search_title", game.Record.Title);
            }

            foreach (var file in game.WithFiles().FileRecords)
            {
                if (file.Metadata.ContainsKey("hash_crc32"))
                {
                    yield return ("search_crc", file.Metadata["hash_crc32"]!);
                }

                if (file.MimeType.StartsWith("application/vnd.stone-romfile"))
                {
                    var structuredFileName = new StructuredFilename(file.File.Name);
                    yield return ("search_title", structuredFileName.Title);
                }
            }
        }

        internal GameScrapeContext(IEnumerable<SeedContent> initialSeeds,
            IEnumerable<IScraper> scrapers, IEnumerable<ICuller> cullers, Guid jobGuid)
        {
            this.Context = new SeedRootContext();
            this.Visited = new List<(string, Guid)>();
            this.Scrapers = scrapers;
            this.Cullers = cullers;
            this.Context.AddRange(initialSeeds.Select(p => (p, this.Context.Root)), GameScrapeContext.ClientSeedSource);
            this.JobGuid = jobGuid;
        }

        internal GameScrapeContext(IEnumerable<SeedTree> initialSeeds,
            IEnumerable<IScraper> scrapers, IEnumerable<ICuller> cullers, Guid jobGuid)
        {
            this.Context = new SeedRootContext();
            this.Visited = new List<(string, Guid)>();
            this.Scrapers = scrapers;
            this.Cullers = cullers;
            this.Context.AddRange(
                initialSeeds.SelectMany(s => s.Collapse(this.Context.Root, GameScrapeContext.ClientSeedSource)));
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

        public ValueTask<bool> Proceed() => this.Proceed(Enumerable.Empty<SeedContent>());

        public async ValueTask<bool> Proceed(IEnumerable<SeedContent> seedsToAdd)
        {
            // Add any client seeds.
            this.Context.AddRange(seedsToAdd.Select(p => (p, this.Context.Root)), GameScrapeContext.ClientSeedSource);

            // Keep track of previously visited seeds.
            int previousCount = this.Visited.Count;
            var results = await this.Scrapers.ToObservable().Select(async (scraper) =>
            {
                var matchingSeeds = this.Context.GetAllOfType(scraper.TargetType)
                    .Where(s => !this.Visited.Contains((scraper.Name, s.Guid))).ToList();

                var scraperResults = new List<ISeed>();
                var scraperVisited = new List<(string, Guid)>();

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

                    await foreach (var task in scraper.ScrapeAsync(matchingSeed, requiredRoots, requiredChildren,
                        requiredSiblings))
                    {
                        try
                        {
                            resultsToAppend.AddRange(task.Collapse(attachSeed, scraper.Name));
                        }
                        catch
                        {
                            continue;
                        }
                    }
                    scraperResults.AddRange(resultsToAppend);
                    scraperVisited.Add((scraper.Name, matchingSeed.Guid)); // mark that matchingSeed was visited.
                                                                           // scraperResults.AddRange(resultsToAppend);
                                                                           // Attach the seeds
                }

                return (scraperVisited, scraperResults);
            }).ToList();

            foreach (var result in results)
            {
                (var scraperVisited, var scraperResults) = await result;
                this.Visited.AddRange(scraperVisited);
                this.Context.AddRange(scraperResults);
            }

            // if there are no new additions to the table, then we know to stop.

            return this.Visited.Count != previousCount;
        }

        public void Cull() => this.Cull(Enumerable.Empty<Guid>());

        public void Cull(IEnumerable<Guid> manualCull)
        {
            foreach (var culledSeed in manualCull.SelectMany(m => this.Context.GetAll().Where(s => s.Guid == m)))
            {
                this.Context.CullSeedTree(culledSeed);
            }

            var seedsToCull = this.Cullers.ToObservable().SelectMany(culler =>
            {
                var seedsToCull = this.Context.GetAllOfType(culler.TargetType);
                var unculledSeeds = culler.Filter(seedsToCull, this.Context);
                var unculledSeedsGuid = unculledSeeds.Select(p => p.Guid);
                return seedsToCull.Where(s => !unculledSeedsGuid.Contains(s.Guid));
            }).ToEnumerable();

            foreach (var culledSeed in seedsToCull)
            {
                this.Context.CullSeedTree(culledSeed);
            }
        }

        private async Task<IEnumerable<ISeed>> RunToEnd()
        {
            while (await this.Proceed()) { }
            this.Cull();
            return this.Context.GetUnculled();
        }

        public IAsyncEnumerator<IEnumerable<ISeed>> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        {
            return new GameScrapeContextAsyncEnumerator(this, cancellationToken);
        }

        public ConfiguredTaskAwaitable<IEnumerable<ISeed>>.ConfiguredTaskAwaiter GetAwaiter()
        {
            return this.RunToEnd().ConfigureAwait(false).GetAwaiter();
        }
    }
}
