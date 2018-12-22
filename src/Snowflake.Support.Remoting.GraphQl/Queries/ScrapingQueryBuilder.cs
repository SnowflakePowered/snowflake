using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphQL.Types;
using Snowflake.Extensibility;
using Snowflake.Framework.Remoting.GraphQl.Attributes;
using Snowflake.Framework.Remoting.GraphQl.Query;
using Snowflake.Records.Game;
using Snowflake.Scraping;
using Snowflake.Scraping.Extensibility;
using Snowflake.Support.Remoting.GraphQl.Inputs.Scraping;
using Snowflake.Support.Remoting.GraphQl.Types.Record;
using Snowflake.Support.Remoting.GraphQl.Types.Scraping;
using static Snowflake.Scraping.Extensibility.SeedBuilder;
namespace Snowflake.Support.Remoting.GraphQl.Queries
{
    public class ScrapingQueryBuilder : QueryBuilder
    {
        private IPluginCollection<IScraper> Scrapers { get; }
        private IPluginCollection<ICuller> Cullers { get; }
        private IScrapeEngine<IGameRecord> ScrapeEngine { get; }

        public ScrapingQueryBuilder(IPluginCollection<IScraper> scrapers,
            IPluginCollection<ICuller> cullers,
            IScrapeEngine<IGameRecord> scrapeEngine)
        {
            this.Scrapers = scrapers;
            this.Cullers = cullers;
            this.ScrapeEngine = scrapeEngine;
        }

        [Field("autoScrape", "Automatically results scrape to end.", typeof(ListGraphType<SeedGraphType>))]
        [Parameter(typeof(string), typeof(StringGraphType), "platform", "platform")]
        [Parameter(typeof(string), typeof(StringGraphType), "title", "title")]
        [Parameter(typeof(IEnumerable<string>), typeof(ListGraphType<StringGraphType>), "scraperNames", "The scrapers to use for this job.")]
        [Parameter(typeof(IEnumerable<string>), typeof(ListGraphType<StringGraphType>), "cullerNames", "The cullers to use for this job.")]
        public async Task<IList<ISeed>> AutoScrape(string platform, string title,
            IEnumerable<string> scraperNames, IEnumerable<string> cullerNames)
        {
            var job = this.ScrapeEngine.CreateJob(__(("platform", platform), ("search_title", title)),
                this.Scrapers.Where(s => scraperNames.Contains(s.Name, StringComparer.OrdinalIgnoreCase)),
                this.Cullers.Where(s => cullerNames.Contains(s.Name, StringComparer.OrdinalIgnoreCase)));
            while (await this.ScrapeEngine.ProceedJob(job)) { }
            return this.ScrapeEngine.GetJobState(job).ToList();
        }

        [Mutation("createJobWithAllScrapers", "Creates a scrape job using all registered scrapers (testing only)", typeof(GuidGraphType))]
        [Parameter(typeof(SeedTreeInputObjectCollection), typeof(SeedTreeInputObjectCollectionType), "seeds", "input")]
        [Parameter(typeof(IEnumerable<string>), typeof(ListGraphType<StringGraphType>), "scraperNames", "The scrapers to use for this job.")]
        [Parameter(typeof(IEnumerable<string>), typeof(ListGraphType<StringGraphType>), "cullerNames", "The cullers to use for this job.")]
        public Guid CreateJob(SeedTreeInputObjectCollection seeds, IEnumerable<string> scraperNames, IEnumerable<string> cullerNames)
        {
            var job = this.ScrapeEngine.CreateJob(seeds.Seeds.Select(s => s.ToSeedTree()).ToList(),
                this.Scrapers.Where(s => scraperNames.Contains(s.Name, StringComparer.OrdinalIgnoreCase)),
                this.Cullers.Where(s => cullerNames.Contains(s.Name, StringComparer.OrdinalIgnoreCase)));
            return job;
        }

        [Mutation("proceedScrapeJob", "Proceeds with the scrape job", typeof(BooleanGraphType))]
        [Parameter(typeof(Guid), typeof(GuidGraphType), "jobGuid", "The GUID of the job")]
        public async Task<bool> ProceedJob(Guid jobGuid)
        {
            return await this.ScrapeEngine.ProceedJob(jobGuid);
        }

        [Field("getJobSeeds", "Gets the seeds of the given job", typeof(ListGraphType<SeedGraphType>))]
        [Parameter(typeof(Guid), typeof(GuidGraphType), "jobGuid", "The GUID of the job")]
        public IEnumerable<ISeed> GetJobSeeds(Guid jobGuid)
        {
            return this.ScrapeEngine.GetJobState(jobGuid);
        }

        [Mutation("resultJob", "Results the job. Does not add to database!", typeof(GameRecordGraphType))]
        [Parameter(typeof(Guid), typeof(GuidGraphType), "jobGuid", "The GUID of the job")]
        public async Task<IGameRecord> Result(Guid jobGuid)
        {
            return await this.ScrapeEngine.Result(jobGuid);
        }

        [Mutation("cullSeeds", "Cull the seeds", typeof(ListGraphType<SeedGraphType>))]
        [Parameter(typeof(Guid), typeof(GuidGraphType), "jobGuid", "The GUID of the job")]
        [Parameter(typeof(IEnumerable<Guid>), typeof(ListGraphType<GuidGraphType>), "culledSeeds", "Culled Seeds")]
        public IEnumerable<ISeed> CullSeeds(Guid jobGuid, IEnumerable<Guid> culledSeeds)
        {
            this.ScrapeEngine.CullJob(jobGuid, culledSeeds);
            return this.ScrapeEngine.GetJobState(jobGuid);
        }
    }
}
