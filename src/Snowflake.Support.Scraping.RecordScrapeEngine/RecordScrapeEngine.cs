using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Extensibility;
using Snowflake.Records.Game;
using Snowflake.Scraping.Extensibility;
using Snowflake.Support.Scraping.RecordScrapeEngine;

namespace Snowflake.Scraping
{
    public class RecordScrapeEngine
    {
        private IPluginCollection<IScraper> AvailableScrapers { get; }
        private ITraverser<IGameRecord> GameTraverser { get; }
        private ResultCuller Culler { get; }
        
        public IScrapeJob CreateJob(IEnumerable<string> scraperNames, IEnumerable<SeedTree> initialSeeds)
        {
            // todo: Make cullers.
            var scrapers = this.AvailableScrapers.Where(p => scraperNames.Contains(p.Name)).ToList();
            return new ScrapeJob(initialSeeds, scrapers, new ICuller[] { Culler });
        }

        public IScrapeJob CreateJob(IEnumerable<string> scraperNames, IEnumerable<SeedContent> initialSeeds)
        {
            // todo: Make cullers.
            var scrapers = this.AvailableScrapers.Where(p => scraperNames.Contains(p.Name)).ToList();
            return new ScrapeJob(initialSeeds, scrapers, new ICuller[] { Culler });
        }

        public async Task<IGameRecord> AutoResult(IScrapeJob scrapeJob)
        {
            while (await scrapeJob.Proceed(Enumerable.Empty<SeedContent>()));
            scrapeJob.Cull();
            return this.GameTraverser.Traverse(scrapeJob.Context.Root, scrapeJob.Context).FirstOrDefault();
        }
    }
}
