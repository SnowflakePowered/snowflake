using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Caching;
using Snowflake.Records.File;
using Snowflake.Records.Game;
using Snowflake.Romfile;
using Snowflake.Romfile.FileSignatures;
using Snowflake.Scraper;
using Snowflake.Scraper.Providers;
using Snowflake.Scraper.Shiragame;
using Snowflake.Plugin.Scrapers.TheGamesDb;
using Xunit;
using Snowflake.Services;
using Snowflake.Romfile.FileSignatures.Composers;
using Snowflake.Support.Caching.KeyedImageCache;
using Snowflake.Support.ShiragameProvider;
using Snowflake.Persistence;

namespace Snowflake.Tests.Scraper
{
    public class ScrapingIntegrationTests
    {
        private readonly IStoneProvider stoneProvider;
        private readonly IFileSignatureMatcher fileSignatureMatcher;
        private readonly IQueryProvider<IScrapedMetadataCollection> scrapedProvider;
        private readonly IScrapeEngine scrapeGen;
        public ScrapingIntegrationTests()
        {
            this.stoneProvider = new StoneProvider();
            this.scrapedProvider = new TheGamesDbMetadataProvider();
            this.fileSignatureMatcher = new FileSignatureMatcher(this.stoneProvider);
            IShiragameProvider shiragame = new ShiragameProvider(new SqliteDatabase("shiragame.db"));
            new FileSignaturesComposer().RegisterFileSignatures(this.fileSignatureMatcher);
            var source = new QueryProviderSource();
            source.Register(this.scrapedProvider);
            source.Register(
                new TheGamesDbMediaProvider(
                    new KeyedImageCache(new DirectoryInfo(Path.GetTempPath()))));
            this.scrapeGen = new ScrapeEngine(this.stoneProvider, shiragame,
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
