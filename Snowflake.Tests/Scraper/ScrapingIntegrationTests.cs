using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Records.File;
using Snowflake.Records.Game;
using Snowflake.Romfile;
using Snowflake.Romfile.FileSignatures;
using Snowflake.Scraper;
using Snowflake.Scraper.Providers;
using Snowflake.Scraper.Shiragame;
using Snowflake.Service;
using Snowflake.Scrapers.Metadata.TheGamesDb;
using Xunit;
namespace Snowflake.Tests.Scraper
{
    public class ScrapingIntegrationTests
    {
        private IStoneProvider stoneProvider;
        private readonly IFileSignatureMatcher fileSignatureMatcher;
        private readonly IScrapeProvider<IScrapedMetadataCollection> scrapedProvider;
        private readonly ScraperGenerator scrapeGen;
        public ScrapingIntegrationTests()
        {
            this.stoneProvider = new StoneProvider();
            this.scrapedProvider = new TheGamesDbMetadataProvider();
            this.fileSignatureMatcher = new FileSignatureMatcher(this.stoneProvider);
            new FileSignaturesContainer().RegisterFileSignatures(this.fileSignatureMatcher);
            this.scrapeGen = new ScraperGenerator(this.stoneProvider, new ShiragameProvider("shiragame.db"),
                new List<IScrapeProvider<IScrapedMetadataCollection>>() { this.scrapedProvider }, this.fileSignatureMatcher);
        }

        public IRomFileInfo GetInformation(string fileName)
        {
            try
            {
                return this.fileSignatureMatcher.GetInfo(fileName, File.OpenRead(fileName));
            }
            catch
            {
                return null;
            }
        }

        public IGameRecord ScrapeTgdb(string fileName)
        {
            var fr = this.scrapeGen.GetFileInformation(fileName);
            return this.scrapeGen.GetGameRecordFromFiles(fr);
        }

    }
}
