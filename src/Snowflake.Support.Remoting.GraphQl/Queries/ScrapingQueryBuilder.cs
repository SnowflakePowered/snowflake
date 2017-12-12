using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphQL.Conventions.Adapters.Types;
using GraphQL.Types;
using Snowflake.Extensibility;
using Snowflake.Records.Game;
using Snowflake.Scraping;
using Snowflake.Scraping.Extensibility;
using Snowflake.Support.Remoting.GraphQl.Framework.Attributes;
using Snowflake.Support.Remoting.GraphQl.Framework.Query;
using Snowflake.Support.Remoting.GraphQl.Inputs.Scraping;
using Snowflake.Support.Remoting.GraphQl.Types.Record;
using Snowflake.Support.Remoting.GraphQl.Types.Scraping;
using static Snowflake.Utility.SeedBuilder;
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

        [Field("seedTest", "seed test", typeof(ListGraphType<SeedGraphType>))]
        [Parameter(typeof(string), typeof(StringGraphType), "platform", "platform")]
        [Parameter(typeof(string), typeof(StringGraphType), "title", "title")]
        public async Task<IList<ISeed>> SeedTest(string platform, string title)
        {
            var job = this.ScrapeEngine.CreateJob(__(("platform", platform), ("search_title", title)),
                this.Scrapers,
                Enumerable.Empty<ICuller>()
                );
            while (await this.ScrapeEngine.ProceedJob(job)) { }
            return this.ScrapeEngine.GetJobState(job).ToList();
        }

        [Field("createJobWithAllScrapers", "Creates a scrape job using all registered scrapers (testing only)", typeof(GuidGraphType))]
        [Parameter(typeof(SeedTreeInputObjectCollection), typeof(SeedTreeInputObjectCollectionType), "seeds", "input")]
        public Guid CreateJob(SeedTreeInputObjectCollection seeds)
        {
            var job = this.ScrapeEngine.CreateJob(seeds.Seeds.Select(s => s.ToSeedTree()).ToList(),
               this.Scrapers,
               this.Cullers
             );
            return job;
        }

        [Field("proceedScrapeJob", "Proceeds with the scrape job", typeof(BooleanGraphType))]
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

        [Field("resultJob", "Results the job. Does not add to database!!", typeof(GameRecordGraphType))]
        [Parameter(typeof(Guid), typeof(GuidGraphType), "jobGuid", "The GUID of the job")]
        public async Task<IGameRecord> Result(Guid jobGuid)
        {
            return await this.ScrapeEngine.Result(jobGuid);
        }

        [Field("cullSeeds", "Cull the seeds", typeof(ListGraphType<SeedGraphType>))]
        [Parameter(typeof(Guid), typeof(GuidGraphType), "jobGuid", "The GUID of the job")]
        [Parameter(typeof(IEnumerable<Guid>), typeof(ListGraphType<GuidGraphType>), "culledSeeds", "Culled Seeds")]
        public IEnumerable<ISeed> CullSeeds(Guid jobGuid, IEnumerable<Guid> culledSeeds)
        {
            this.ScrapeEngine.CullJob(jobGuid, culledSeeds);
            return this.ScrapeEngine.GetJobState(jobGuid);
        }
    }
}
