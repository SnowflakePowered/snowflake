using Moq;
using Snowflake.Scraping.Scrapers;
using Snowflake.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Snowflake.Scraping.Tests
{
    public class ScrapeJobTests
    {
        [Fact]
        public void Trivial_Test()
        {
            var scraper = new TrivialScraper();
            var stoneProvider = new Mock<IStoneProvider>();
            var scrapeJob = new ScrapeJob(stoneProvider.Object, new[] { scraper }, new ICuller[] { });
            Assert.True(scrapeJob.Proceed(Enumerable.Empty<SeedContent>()));
            Assert.NotEmpty(scrapeJob.Context.GetAllOfType("Test"));
            Assert.False(scrapeJob.Proceed(Enumerable.Empty<SeedContent>()));
        }

        [Fact]
        public void Dependent_Test()
        {
            var scraper = new TrivialScraper();
            var dependent = new DependentScraper();
            var stoneProvider = new Mock<IStoneProvider>();
            // we make dependent before scraper to allow scrapeJob to resolve
            // all items after 3 iterations.
            var scrapeJob = new ScrapeJob(stoneProvider.Object, new IScraper[] { dependent, scraper }, new ICuller[] { });
            Assert.True(scrapeJob.Proceed(Enumerable.Empty<SeedContent>()));
            Assert.NotEmpty(scrapeJob.Context.GetAllOfType("Test"));
            Assert.True(scrapeJob.Proceed(Enumerable.Empty<SeedContent>()));
            Assert.NotEmpty(scrapeJob.Context.GetAllOfType("TestDependent"));
            Assert.False(scrapeJob.Proceed(Enumerable.Empty<SeedContent>()));
        }

        [Fact]
        public void Group_Test()
        {
            var scraper = new GroupScraper();
            var stoneProvider = new Mock<IStoneProvider>();
            var scrapeJob = new ScrapeJob(stoneProvider.Object, new IScraper[] { scraper }, new ICuller[] { });

            Assert.True(scrapeJob.Proceed(Enumerable.Empty<SeedContent>()));
            Assert.False(scrapeJob.Proceed(Enumerable.Empty<SeedContent>()));
            Assert.NotEmpty(scrapeJob.Context.GetAllOfType("MyGroup"));
            Assert.NotEmpty(scrapeJob.Context.GetAllOfType("Test"));
            Assert.NotEmpty(scrapeJob.Context.GetAllOfType("TestTwo"));
            Assert.NotEmpty(scrapeJob.Context.GetChildren(scrapeJob.Context.GetAllOfType("MyGroup").First()));

        }
    }
}
