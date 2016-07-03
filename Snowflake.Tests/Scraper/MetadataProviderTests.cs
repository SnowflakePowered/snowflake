using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Records.File;
using Snowflake.Records.Metadata;
using Snowflake.Scraper.Providers;
using Xunit;

namespace Snowflake.Scraper.MetadataProvider.Tests
{
    public class MetadataProviderTests
    {
        //test single query
        [Fact]
        public void TestQuery()
        {
            var provider = new TestMetadataProvider();
            var collection = new MetadataCollection(Guid.NewGuid()) {{"TestMetadataKey", "Test"}};
            var result = provider.Query(collection);
            Assert.Equal(result.Source, "Test");
        }

        [Fact]
        public void TestQueryReturnMetadata()
        {
            var provider = new TestMetadataProvider();
            var collection = new MetadataCollection(Guid.NewGuid())
            {
                { "TestMetadataKey", "Test" },
                { "TestMetadataKeyTwo", "Test"}
            };
            var result = provider.Query(collection, "TestMetadataReturnTwo");
            Assert.Equal(result.Source, "TestTwo");
        }

        [Fact]
        public void TestQueryNoSuitable()
        {
            var provider = new TestMetadataProvider();
            var collection = new MetadataCollection(Guid.NewGuid()) { { "TestMetadataKey", "Test" } };
            var result = provider.Query(collection, "TestMetadataReturnTwo");
            Assert.Null(result);
        }
    }

    internal class TestMetadataProvider : ScrapeProvider<IScrapeResult>
    {
        public override IEnumerable<IScrapeResult> QueryAllResults(string searchQuery, string platformId)
        {
            throw new NotImplementedException();
        }

        public override IScrapeResult QueryBestMatch(string searchQuery, string platformId)
        {
            throw new NotImplementedException();
        }

        [Provider]
        [RequiredMetadata("TestMetadataKey")]
        [ReturnMetadata("TestMetadataReturn")]
        public IScrapeResult Test(IMetadataCollection collection)
        {
            IMetadataCollection retmetadata = new ScrapedMetadataCollection("Test", 1.0);
            retmetadata.Add("TestMetadataReturn", "lmao");
            return (IScrapeResult) retmetadata;
        }


        [Provider]
        [RequiredMetadata("TestMetadataKey")]
        [RequiredMetadata("TestMetadataKeyTwo")]
        [ReturnMetadata("TestMetadataReturn")]
        [ReturnMetadata("TestMetadataReturnTwo")]
        public IScrapeResult TestTwo(IMetadataCollection collection)
        {
            IMetadataCollection retmetadata = new ScrapedMetadataCollection("TestTwo", 1.0);
            retmetadata.Add("TestMetadataReturn", "lmao");
            return (IScrapeResult)retmetadata;
        }

    }
}
