using Moq;
using Snowflake.Platform;
using Snowflake.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Snowflake.Scraping.Tests
{
    public class ScrapeJobTests
    {
        [Fact]
        public async Task Trivial_Test()
        {
            var scraper = new TrivialScraper();
            var stoneProvider = new Mock<IStoneProvider>();
            var scrapeJob = new ScrapeJob(stoneProvider.Object, new[] { scraper }, new ICuller[] { });
            Assert.True(await scrapeJob.Proceed(Enumerable.Empty<SeedContent>()));
            Assert.NotEmpty(scrapeJob.Context.GetAllOfType("Test"));
            Assert.False(await scrapeJob.Proceed(Enumerable.Empty<SeedContent>()));
        }

        [Fact]
        public async Task Dependent_Test()
        {
            var scraper = new TrivialScraper();
            var dependent = new DependentScraper();
            var stoneProvider = new Mock<IStoneProvider>();
            // we make dependent before scraper to allow scrapeJob to resolve
            // all items after 3 iterations.
            var scrapeJob = new ScrapeJob(stoneProvider.Object, new IScraper[] { dependent, scraper }, new ICuller[] { });
            Assert.True(await scrapeJob.Proceed(Enumerable.Empty<SeedContent>()));
            Assert.NotEmpty(scrapeJob.Context.GetAllOfType("Test"));
            Assert.True(await scrapeJob.Proceed(Enumerable.Empty<SeedContent>()));
            Assert.NotEmpty(scrapeJob.Context.GetAllOfType("TestDependent"));
            Assert.False(await scrapeJob.Proceed(Enumerable.Empty<SeedContent>()));
        }

        [Fact]
        public async Task Group_Test()
        {
            var scraper = new GroupScraper();
            var stoneProvider = new Mock<IStoneProvider>();
            var scrapeJob = new ScrapeJob(stoneProvider.Object, new IScraper[] { scraper }, new ICuller[] { });

            Assert.True(await scrapeJob.Proceed(Enumerable.Empty<SeedContent>()));
            Assert.False(await scrapeJob.Proceed(Enumerable.Empty<SeedContent>()));
            Assert.NotEmpty(scrapeJob.Context.GetAllOfType("MyGroup"));
            Assert.NotEmpty(scrapeJob.Context.GetAllOfType("Test"));
            Assert.NotEmpty(scrapeJob.Context.GetAllOfType("TestTwo"));
            Assert.NotEmpty(scrapeJob.Context.GetChildren(scrapeJob.Context.GetAllOfType("MyGroup").First()));

        }

        [Fact]
        public async Task Cull_Test()
        {
            var scraper = new TrivialScraper();
            var scraperTwo = new TrivialScraperTwo();
            var stoneProvider = new Mock<IStoneProvider>();
            var scrapeJob = new ScrapeJob(stoneProvider.Object, new IScraper[] { scraper, scraperTwo }, new ICuller[] { new TrivialCuller() });
            Assert.True(await scrapeJob.Proceed(Enumerable.Empty<SeedContent>()));
            Assert.NotEmpty(scrapeJob.Context.GetAllOfType("Test"));
            Assert.False(await scrapeJob.Proceed(Enumerable.Empty<SeedContent>()));
            Assert.Equal(2, scrapeJob.Context.GetAllOfType("Test").Count());
            scrapeJob.Cull();
            Assert.Single(scrapeJob.Context.GetAllOfType("Test"));
            Assert.Equal("Goodbye World", scrapeJob.Context.GetAllOfType("Test").First().Content.Value);

        }

        [Fact]
        public void TraverseFile_Test()
        {
            var scraper = new TrivialScraper();
            var scraperTwo = new TrivialScraperTwo();
            var stoneProvider = new Mock<IStoneProvider>();
            var scrapeJob = new ScrapeJob(new SeedContent[] { ("file", "test:\\test_file"), ("file", "test:\\test_file_not_exist") },
                stoneProvider.Object,
                new IScraper[] { },
                new ICuller[] { });
            var fileSeed = scrapeJob.Context.GetAllOfType("file").First();
            Assert.Equal("test:\\test_file", fileSeed.Content.Value);
            scrapeJob.Context.Add(("mimetype", "application/octet-stream"), fileSeed, ScrapeJob.ClientSeedSource);
            scrapeJob.Context.Add(("example_metadata", "metadata_value"), fileSeed, ScrapeJob.ClientSeedSource);
            var nestedSeed = scrapeJob.Context.GetAllOfType("example_metadata").First();
            scrapeJob.Context.Add(("nested_metadata", "nested_value"), nestedSeed, ScrapeJob.ClientSeedSource);
            var records = scrapeJob.TraverseFiles().ToList();
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
                stoneProvider.Object,
                new IScraper[] { },
                new ICuller[] { });
            var fileSeed = scrapeJob.Context.GetAllOfType("file").First();
            var resultSeed = scrapeJob.Context.GetAllOfType("result").First();
            Assert.Equal("test:\\test_file", fileSeed.Content.Value);
            scrapeJob.Context.Add(("mimetype", "application/octet-stream"), fileSeed, ScrapeJob.ClientSeedSource);
            scrapeJob.Context.Add(("example_metadata", "metadata"), resultSeed, ScrapeJob.ClientSeedSource);
            var games = scrapeJob.TraverseGames().ToList();
            Assert.Single(games);
            Assert.Equal("Test Game", games.First().Title);
            Assert.Single(games.First().Files);
            Assert.Contains(games.First().Metadata, k => k.Key == "game_example_metadata");

        }
    }
}
