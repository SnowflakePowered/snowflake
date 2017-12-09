using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Snowflake.Scraping.Attributes;

namespace Snowflake.Scraping
{
    public class ScrapeJob : IScrapeJob
    {
        public const string ClientSeedSource = "__client";
        public IEnumerable<IScraper> Scrapers { get; }
        public ISeedRootContext Context { get; }
        private IList<(string, Guid)> Visited { get; }
        public Guid JobGuid { get; }
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
                    var requiredChildren = scraper.RequiredChildSeeds.SelectMany(seed => this.Context.GetChildren(matchingSeed)
                                .Where(child => child.Content.Type == seed)).ToLookup(x => x.Content.Type, x => x.Content); ;
                    var requiredRoots = scraper.RequiredRootSeeds.SelectMany(seed => this.Context.GetChildren(this.Context.Root)
                              .Where(child => child.Content.Type == seed)).ToLookup(x => x.Content.Type, x => x.Content);

                    // We need to ensure that the number of keys match before continuing.
                    // If the keys match then we are guaranteed that every key is successfully fulfilled.
                    if (requiredChildren.GroupBy(p => p.Key).Count() != scraper.RequiredChildSeeds.Count()
                        || requiredRoots.GroupBy(p => p.Key).Count() != scraper.RequiredRootSeeds.Count())
                    {
                        continue;
                    }

                    var resultsToAppend = new List<ISeed>();

                    // Collect the results.
                    var results = new List<SeedContent>();
                    var attachSeed = this.GetAttachTarget(scraper.AttachPoint, matchingSeed);

                    foreach (var task in scraper.Scrape(matchingSeed, requiredRoots, requiredChildren))
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

            return this.Visited.Count != previousCount; // if there are no new additions to the table, then we know to stop.
            /*
             * Add seeds to add.
             * Check if we fulfill the requirements, and if so, do not continue.
             * 
             * foreach scraper in scrapers:
             *   if seed exists with the given key
             *     if seed fulfills the requirements of the scraper
             *       get the results of scraper(seed)
             *       make list of seed values to attach
             *       is the scraper group? if so group into one seed value and add to the list.
             *       where does the seed attach?
             *       if it attaches to root, attach all to root.
             *       if it attaches to parent, attach all to parent.
             *       if it doesn't attach, drop it.
             *   keep track of visited.!
             *  return the currently seeds, and true or false if we want to progresss.
             */
        }

        public void Cull()
        {
            /**
             * group seeds by type.
             * run culler for each type (culler mapping? configuration? that makes this require a plugin provision)
             * return culled seeds.
             */
            foreach (var culler in this.Cullers)
            {
                var seedsToCull = this.Context.GetAllOfType(culler.TargetType).Where(s => s.Source != ScrapeJob.ClientSeedSource);
                var unculledSeeds = culler.Filter(seedsToCull);
                var unculledSeedsGuid = unculledSeeds.Select(p => p.Guid).ToList();
                foreach (var culledSeed in seedsToCull.Where(s => !unculledSeedsGuid.Contains(s.Guid)))
                {
                    culledSeed.Cull();
                }
            }
        }
    }
}
