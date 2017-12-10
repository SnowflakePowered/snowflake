using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Plugin.Scraping.TheGamesDb;
using Snowflake.Scraping;
using Xunit;

namespace Snowflake.Scraper
{
    public class ScraperIntegrationTests
    {
        [Fact]
        public async Task TGDBScraping_Test()
        {
            var tgdb = new TheGamesDbScraper();
            var scrapeJob = new ScrapeJob(new SeedContent[] {
                ("platform", "NINTENDO_NES"),
                ("search_title", "Super Mario Bros."),
            }, new[] { tgdb }, new ICuller[] { });
            while (await scrapeJob.Proceed());
        }
    }
}
