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
using Snowflake.Scrapers.TheGamesDb;
using Xunit;
namespace Snowflake.Tests.Scraper
{
    public class ScrapingIntegrationTests
    {
        private IStoneProvider stoneProvider;
        private readonly IFileSignatureMatcher fileSignatureMatcher;
        private readonly IQueryProvider<IScrapedMetadataCollection> scrapedProvider;
        private readonly ScrapeEngine scrapeGen;
        public ScrapingIntegrationTests()
        {
            this.stoneProvider = new StoneProvider();
            this.scrapedProvider = new TheGamesDbMetadataProvider();
            this.fileSignatureMatcher = new FileSignatureMatcher(this.stoneProvider);
            new FileSignaturesContainer().RegisterFileSignatures(this.fileSignatureMatcher);
            var source = new QueryProviderSource();
            source.Register(this.scrapedProvider);
            this.scrapeGen = new ScrapeEngine(this.stoneProvider, new ShiragameProvider("shiragame.db"),
                source, this.fileSignatureMatcher);
        }

        public IRomFileInfo GetInformation(string fileName)
        {
            using (FileStream fs = File.OpenRead(fileName))
            {
                return this.fileSignatureMatcher.GetInfo(fileName, fs);
            }
        }

        public IGameRecord ScrapeTgdb(string fileName)
        {
            var fr = this.scrapeGen.GetFileInformation(fileName);
            return this.scrapeGen.GetGameRecordFromFile(fr);
        }

    }
}
