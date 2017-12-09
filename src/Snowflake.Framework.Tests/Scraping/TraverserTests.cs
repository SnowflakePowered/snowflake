using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moq;
using Snowflake.Platform;
using Snowflake.Scraping.Tests;
using Snowflake.Services;
using Snowflake.Support.Scraping.RecordTraversers;
using Xunit;

namespace Snowflake.Scraping
{
    public class TraverserTests
    {

        [Fact]
        public void TraverseFile_Test()
        {
            var scraper = new TrivialScraper();
            var scraperTwo = new TrivialScraperTwo();
            var traverser = new FileRecordTraverser();
            var scrapeJob = new ScrapeJob(new SeedContent[] { ("file", "test:\\test_file"), ("file", "test:\\test_file_not_exist") },
                new IScraper[] { },
                new ICuller[] { });
            var fileSeed = scrapeJob.Context.GetAllOfType("file").First();
            Assert.Equal("test:\\test_file", fileSeed.Content.Value);
            scrapeJob.Context.Add(("mimetype", "application/octet-stream"), fileSeed, ScrapeJob.ClientSeedSource);
            scrapeJob.Context.Add(("example_metadata", "metadata_value"), fileSeed, ScrapeJob.ClientSeedSource);
            var nestedSeed = scrapeJob.Context.GetAllOfType("example_metadata").First();
            scrapeJob.Context.Add(("nested_metadata", "nested_value"), nestedSeed, ScrapeJob.ClientSeedSource);
            var records = traverser.Traverse(scrapeJob.Context.Root, scrapeJob.Context).ToList();
            Assert.Single(records);
            Assert.Equal("test:\\test_file", records[0].FilePath);
            Assert.Equal("application/octet-stream", records[0].MimeType);
            Assert.Contains(records[0].Metadata, m => m.Key == "file_example_metadata"
                            && m.Value.Value == "metadata_value"
                            && m.Value.Key == "file_example_metadata");
            Assert.Contains(records[0].Metadata, m => m.Key == "file_nested_metadata"
                  && m.Value.Value == "nested_value"
                  && m.Value.Key == "file_nested_metadata");
        }

        [Fact]
        public void TraverseGame_Test()
        {
            var scraper = new TrivialScraper();
            var scraperTwo = new TrivialScraperTwo();
            var stoneProvider = new Mock<IStoneProvider>();
            var platform = new Mock<IPlatformInfo>();
            platform.SetupGet(p => p.PlatformID).Returns("TEST_PLATFORM");
            var platformDict = new Dictionary<string, IPlatformInfo>()
            {
                { "TEST_PLATFORM", platform.Object },
            };

            stoneProvider.SetupGet(p => p.Platforms)
                .Returns(platformDict);
            var scrapeJob = new ScrapeJob(new SeedContent[] {
                ("file", "test:\\test_file"),
                ("platform", "TEST_PLATFORM"),
                ("result", "Test Game"),
            },
                new IScraper[] { },
                new ICuller[] { });
            var fileTraverser = new FileRecordTraverser();
            var gameTraverser = new GameRecordTraverser(stoneProvider.Object, fileTraverser);
            var fileSeed = scrapeJob.Context.GetAllOfType("file").First();
            var resultSeed = scrapeJob.Context.GetAllOfType("result").First();
            Assert.Equal("test:\\test_file", fileSeed.Content.Value);
            scrapeJob.Context.Add(("mimetype", "application/octet-stream"), fileSeed, ScrapeJob.ClientSeedSource);
            scrapeJob.Context.Add(("example_metadata", "metadata"), resultSeed, ScrapeJob.ClientSeedSource);
            var games = gameTraverser.Traverse(scrapeJob.Context.Root, scrapeJob.Context);
            Assert.Single(games);
            Assert.Equal("Test Game", games.First().Title);
            Assert.Single(games.First().Files);
            Assert.Contains(games.First().Metadata, k => k.Key == "game_example_metadata");
        }
    }
}
