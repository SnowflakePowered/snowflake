using Moq;
using Snowflake.Model.Game;
using Snowflake.Scraping.Extensibility;
using Snowflake.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Snowflake.Scraping.Tests
{
    public class ScrapeContextTests
    {
        [Fact]
        public async Task Trivial_Test()
        {
            var scraper = new TrivialScraper();
            var scrapeJob = new GameScrapeContext(new[] {scraper}, new ICuller[] { });
            Assert.True(await scrapeJob.Proceed(Enumerable.Empty<SeedContent>()));
            Assert.NotEmpty(scrapeJob.Context.GetAllOfType("Test"));
            Assert.False(await scrapeJob.Proceed(Enumerable.Empty<SeedContent>()));
        }

        [Fact]
        public async Task Dependent_Test()
        {
            var scraper = new TrivialScraper();
            var dependent = new DependentScraper();
            // we make dependent before scraper to allow scrapeJob to resolve
            // all items after 3 iterations.
            var scrapeJob = new GameScrapeContext(new IScraper[] {dependent, scraper}, new ICuller[] { });
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
            var scrapeJob = new GameScrapeContext(new IScraper[] {scraper}, new ICuller[] { });

            Assert.True(await scrapeJob.Proceed(Enumerable.Empty<SeedContent>()));
            Assert.False(await scrapeJob.Proceed(Enumerable.Empty<SeedContent>()));
            Assert.NotEmpty(scrapeJob.Context.GetAllOfType("MyGroup"));
            Assert.NotEmpty(scrapeJob.Context.GetAllOfType("Test"));
            Assert.NotEmpty(scrapeJob.Context.GetAllOfType("TestTwo"));
            Assert.Equal(scrapeJob.Context.GetAllOfType("MyGroup").First().Content.Value,
                scrapeJob.Context.GetAllOfType("Test").First().Content.Value);
            Assert.NotEmpty(scrapeJob.Context.GetChildren(scrapeJob.Context.GetAllOfType("MyGroup").First()));
        }

        [Fact]
        public async Task Exclude_Test()
        {
            var scraper = new TrivialScraper();
            var exclude = new ExcludeScraper();
            var scrapeJob = new GameScrapeContext(new SeedContent[] {},
                new IScraper[] {scraper, exclude}, new ICuller[] { });

            Assert.True(await scrapeJob.Proceed(Enumerable.Empty<SeedContent>()));
            Assert.False(await scrapeJob.Proceed(new SeedContent[] { ("ExcludeTest", "Test") }));
            Assert.NotEmpty(scrapeJob.Context.GetAllOfType("Test"));
            Assert.Empty(scrapeJob.Context.GetAllOfType("ThisShouldNeverAppear"));
            Assert.Equal("Hello World",
                scrapeJob.Context.GetAllOfType("Test").First().Content.Value);
        }

        [Fact]
        public async Task ExcludeRunsWhenExclusionNotPresent_Test()
        {
            var exclude = new ExcludeScraper();
            var scrapeJob = new GameScrapeContext(new SeedContent[] {("ExcludeTest", "Test")},
                new IScraper[] {exclude}, new ICuller[] { });
            Assert.True(await scrapeJob.Proceed(Enumerable.Empty<SeedContent>()));
            Assert.False(await scrapeJob.Proceed(Enumerable.Empty<SeedContent>()));
            Assert.NotEmpty(scrapeJob.Context.GetAllOfType("ThisShouldNeverAppear"));
        }

        [Fact]
        public async Task SimpleAsync_Test()
        {
            var scraper = new SimpleAsyncScraper();
            var scrapeJob = new GameScrapeContext(new IScraper[] {scraper}, new ICuller[] { });

            Assert.True(await scrapeJob.Proceed(Enumerable.Empty<SeedContent>()));
            Assert.False(await scrapeJob.Proceed(Enumerable.Empty<SeedContent>()));
            Assert.NotEmpty(scrapeJob.Context.GetAllOfType("TestAsync"));
            Assert.Equal("Hello World",
                scrapeJob.Context.GetAllOfType("TestAsync").First().Content.Value);
        }

        [Fact]
        public async Task Async_Test()
        {
            var scraper = new AsyncScraper();
            var scrapeJob = new GameScrapeContext(new IScraper[] {scraper}, new ICuller[] { });

            Assert.True(await scrapeJob.Proceed(Enumerable.Empty<SeedContent>()));
            Assert.False(await scrapeJob.Proceed(Enumerable.Empty<SeedContent>()));
            Assert.NotEmpty(scrapeJob.Context.GetAllOfType("TestAsync"));
            Assert.NotEmpty(scrapeJob.Context.GetAllOfType("TestSync"));
            Assert.NotEmpty(scrapeJob.Context.GetAllOfType("TestAsyncNested"));
            Assert.NotEmpty(scrapeJob.Context.GetAllOfType("TestAsyncNestedTwo"));
            Assert.Equal("Nested Value Two",
                scrapeJob.Context.GetAllOfType("TestAsyncNestedTwo").First().Content.Value);
            Assert.Equal("Nested Value",
                scrapeJob.Context.GetAllOfType("TestAsyncNested").First().Content.Value);
        }

        [Fact]
        public async Task Autoscrape_Test()
        {
            var scraper = new AsyncScraper();
            var scrapeJob = new GameScrapeContext(new IScraper[] { scraper }, new ICuller[] { });

            var results = await scrapeJob;

            Assert.NotEmpty(results.Where(s => s.Content.Type == "TestAsync"));
            Assert.NotEmpty(results.Where(s => s.Content.Type == "TestSync"));
            Assert.NotEmpty(results.Where(s => s.Content.Type == "TestAsyncNested"));
            Assert.NotEmpty(results.Where(s => s.Content.Type == "TestAsyncNestedTwo"));

            Assert.NotEmpty(scrapeJob.Context.GetAllOfType("TestAsync"));
            Assert.NotEmpty(scrapeJob.Context.GetAllOfType("TestSync"));
            Assert.NotEmpty(scrapeJob.Context.GetAllOfType("TestAsyncNested"));
            Assert.NotEmpty(scrapeJob.Context.GetAllOfType("TestAsyncNestedTwo"));
            Assert.Equal("Nested Value Two",
                scrapeJob.Context.GetAllOfType("TestAsyncNestedTwo").First().Content.Value);
            Assert.Equal("Nested Value",
                scrapeJob.Context.GetAllOfType("TestAsyncNested").First().Content.Value);
        }



        [Fact]
        public async Task Cull_Test()
        {
            var scraper = new TrivialScraper();
            var scraperTwo = new TrivialScraperTwo();
            var scrapeJob = new GameScrapeContext(new IScraper[] {scraper, scraperTwo}, new ICuller[] {new TrivialCuller()});
            Assert.True(await scrapeJob.Proceed(Enumerable.Empty<SeedContent>()));
            Assert.NotEmpty(scrapeJob.Context.GetAllOfType("Test"));
            Assert.False(await scrapeJob.Proceed(Enumerable.Empty<SeedContent>()));
            Assert.Equal(2, scrapeJob.Context.GetAllOfType("Test").Count());
            scrapeJob.Cull();
            Assert.Single(scrapeJob.Context.GetAllOfType("Test"));
            Assert.Equal("Goodbye World", scrapeJob.Context.GetAllOfType("Test").First().Content.Value);
        }

        [Fact]
        public async Task ManualCull_Test()
        {
            var scraper = new TrivialScraper();
            var scraperTwo = new TrivialScraperTwo();
            var scrapeJob = new GameScrapeContext(new IScraper[] {scraper, scraperTwo}, new ICuller[] { });
            Assert.True(await scrapeJob.Proceed(Enumerable.Empty<SeedContent>()));
            Assert.NotEmpty(scrapeJob.Context.GetAllOfType("Test"));
            Assert.False(await scrapeJob.Proceed(Enumerable.Empty<SeedContent>()));
            Assert.Equal(2, scrapeJob.Context.GetAllOfType("Test").Count());
            scrapeJob.Cull(new[] {scrapeJob.Context.GetAllOfType("Test").First().Guid});
            Assert.Single(scrapeJob.Context.GetAllOfType("Test"));
        }
    }
}
