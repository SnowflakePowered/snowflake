using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphQL.Types;
using Snowflake.Extensibility;
using Snowflake.Records.Game;
using Snowflake.Scraping;
using Snowflake.Scraping.Extensibility;
using Snowflake.Support.Remoting.GraphQl.Framework.Attributes;
using Snowflake.Support.Remoting.GraphQl.Framework.Query;
using Snowflake.Support.Remoting.GraphQl.Inputs.Scraping;
using static Snowflake.Utility.SeedBuilder;
namespace Snowflake.Support.Remoting.GraphQl.Queries
{
    public class ScrapingQueryBuilder : QueryBuilder
    {
        private IPluginCollection<IScraper> Scrapers { get; }
        private IScrapeEngine<IGameRecord> ScrapeEngine { get; }

        public ScrapingQueryBuilder(IPluginCollection<IScraper> scrapers,
            IScrapeEngine<IGameRecord> scrapeEngine)
        {
            this.Scrapers = scrapers;
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
            return this.ScrapeEngine.GetCurrentState(job).ToList();
        }
    }
}
