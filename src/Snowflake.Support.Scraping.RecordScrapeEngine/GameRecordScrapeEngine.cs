using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Snowflake.Records.Game;
using Snowflake.Scraping;
using Snowflake.Scraping.Extensibility;
using Snowflake.Support.Scraping.RecordScrapeEngine;

namespace Snowflake.Support.Scraping.RecordScrapeEngine
{
    public class GameRecordScrapeEngine : IScrapeEngine<IGameRecord>
    {
        private ITraverser<IGameRecord> GameTraverser { get; }
        private IImmutableDictionary<Guid, IScrapeJob> ScrapeJobs { get; set; }
        private ResultCuller ResultCuller { get; }
        public GameRecordScrapeEngine(ITraverser<IGameRecord> gameTraverser)
        {
            this.GameTraverser = gameTraverser;
            this.ResultCuller = new ResultCuller();
            this.ScrapeJobs = ImmutableDictionary.Create<Guid, IScrapeJob>();
        }

        public async Task<IGameRecord> Result(Guid scrapeJobId)
        {
            this.ScrapeJobs.TryGetValue(scrapeJobId, out IScrapeJob scrapeJob);
            if (scrapeJob == null)
            {
                return null;
            }

            while (await scrapeJob.Proceed(Enumerable.Empty<SeedContent>())) { }
            scrapeJob.Cull();
            var result = this.GameTraverser.Traverse(scrapeJob.Context.Root, scrapeJob.Context).FirstOrDefault();
            this.ScrapeJobs = this.ScrapeJobs.Remove(scrapeJobId);
            return result;
        }

        public Guid CreateJob(IEnumerable<SeedTree> initialSeeds, IEnumerable<IScraper> scrapers, IEnumerable<ICuller> cullers)
        {
            var job = new ScrapeJob(initialSeeds, scrapers, cullers.Prepend(this.ResultCuller));
            this.ScrapeJobs = this.ScrapeJobs.Add(job.JobGuid, job);
            return job.JobGuid;
        }

        public async Task<bool> ProceedJob(Guid jobGuid)
        {
            this.ScrapeJobs.TryGetValue(jobGuid, out IScrapeJob scrapeJob);
            if (scrapeJob == null)
            {
                return false;
            }

            return await scrapeJob.Proceed();
        }

        public async Task<bool> ProceedJob(Guid jobGuid, IEnumerable<SeedContent> initialSeeds)
        {
            this.ScrapeJobs.TryGetValue(jobGuid, out IScrapeJob scrapeJob);
            if (scrapeJob == null)
            {
                return false;
            }

            return await scrapeJob.Proceed(initialSeeds);
        }

        public IEnumerable<ISeed> GetJobState(Guid jobGuid)
        {
            this.ScrapeJobs.TryGetValue(jobGuid, out IScrapeJob scrapeJob);
            if (scrapeJob == null)
            {
                return Enumerable.Empty<ISeed>();
            }

            return scrapeJob.Context.GetUnculled();
        }

        public void CullJob(Guid jobGuid)
        {
            this.ScrapeJobs.TryGetValue(jobGuid, out IScrapeJob scrapeJob);
            scrapeJob?.Cull();
        }

        public void CullJob(Guid jobGuid, IEnumerable<Guid> manualCull)
        {
            this.ScrapeJobs.TryGetValue(jobGuid, out IScrapeJob scrapeJob);
            scrapeJob?.Cull(manualCull);
        }
    }
}
