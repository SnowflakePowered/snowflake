using Snowflake.Records.File;
using Snowflake.Records.Game;
using Snowflake.Scraping;
using Snowflake.Scraping.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Snowflake.Scraping
{
    public class ScrapeJob
    {
        public IEnumerable<IScraper> Scrapers { get; }
        public ISeedRootContext Context { get; }
        private IList<(string, Guid)> Visited { get; }

        public ScrapeJob(IEnumerable<IScraper> scrapers)
            : this(Enumerable.Empty<SeedContent>(), scrapers)
        {
        }

        public ScrapeJob(IEnumerable<SeedContent> initialSeeds, IEnumerable<IScraper> scrapers)
        {
            this.Context = new SeedRootContext();
            this.Visited = new List<(string, Guid)>();
            this.Scrapers = scrapers;
            foreach (var seed in initialSeeds)
            {
                this.Context.Add(seed, this.Context.Root, "_client");
            }
        }

        public bool Proceed(IEnumerable<SeedContent> seedsToAdd)
        {
            // Add any client seeds.
            foreach (var seed in seedsToAdd)
            {
                this.Context.Add(seed, this.Context.Root, "_client");
            }

            // Keep track of previously visited seeds.
            int previousCount = this.Visited.Count;
            foreach (var scraper in this.Scrapers)
            {
                var matchingSeeds = this.Context.GetAllOfType(scraper.ParentType)
                    .Where(s => !this.Visited.Contains((scraper.Name, s.Guid))).ToList();

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
                    var results = scraper.Scrape(matchingSeed, requiredRoots, requiredChildren);
                    this.Visited.Add((scraper.Name, matchingSeed.Guid)); // mark that matchingSeed was visited.

                    // Attach the seeds.
                    var attachSeed = scraper.Target == AttachTarget.Parent ? matchingSeed : this.Context.Root;

                    if (scraper.IsGroup)
                    {
                        var groupSeed = new Seed((scraper.GroupType,
                                               results.First(r => r.Type == scraper.GroupValueType).Value),
                                               this.Context.Root, scraper.Name);
                        resultsToAppend.Add(groupSeed);
                        resultsToAppend.AddRange(results.Select(r => new Seed(r, groupSeed, scraper.Name)));
                    }
                    else
                    {
                        resultsToAppend.AddRange(results.Select(r => new Seed(r, attachSeed, scraper.Name)));
                    }

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

        public IEnumerable<ISeed> Cull(IDictionary<string, string> cullers)
        {
            /**
             * group seeds by type.
             * run culler for each type (culler mapping? configuration? that makes this require a plugin provision)
             * return culled seeds.
             */
            yield break;
        }

        public IEnumerable<IFileRecord> TraverseFiles()
        {
            /**
             * makes every 'file' type into a FileRecord
             */
            yield break;
        }

        public IEnumerable<IGameRecord> TraverseGames()
        {
            /**
             * makes every 'result' type into a GameRecord with traversed files.
             * theoretically this should only return one if there is one result,
             * but the user can override this because seeds with source __user are untouched when culled.
             */
            yield break;
        }

    }
}
