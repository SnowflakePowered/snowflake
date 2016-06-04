using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Records.File;
using Snowflake.Records.Metadata;
using Snowflake.Scraper.Provider;
using Snowflake.Scraper.Providers;
using Xunit;

namespace Snowflake.Tests.Scraper
{
    public class MetadataProviderTests
    {
        [Fact]
        public void TestQuery()
        {
            var provider = new TestMetadataProvider();
            var collection = new MetadataCollection(Guid.NewGuid()) {{"TestMetadataKey", "Test"}};
            var result = provider.Query(collection);
            Assert.Equal(result.ScraperId, "Test");
        }
    }

    internal class TestMetadataProvider : MetadataProvider
    {
        public override IList<IScrapedMetadataCollection> QueryAllResults(string searchQuery, string platformId)
        {
            throw new NotImplementedException();
        }

        public override IScrapedMetadataCollection QueryBestMatch(string searchQuery, string platformId)
        {
            throw new NotImplementedException();
        }

        [Provider]
        [RequiredMetadata("TestMetadataKey")]
        [ReturnMetadata("TestMetadataReturn")]
        public IScrapedMetadataCollection Test(IMetadataCollection collection)
        {
            IMetadataCollection retmetadata = new ScrapedMetadataCollection("Test", 1.0);
            retmetadata.Add("TestMetadataReturn", "lmao");
            return (IScrapedMetadataCollection) retmetadata;
        }


        [Provider]
        [RequiredMetadata("TestMetadataKey")]
        [RequiredMetadata("TestMetadataKeyTwo")]
        [ReturnMetadata("TestMetadataReturn")]
        public IScrapedMetadataCollection TestTwo(IMetadataCollection collection)
        {
            IMetadataCollection retmetadata = new ScrapedMetadataCollection("TestTwo", 1.0);
            retmetadata.Add("TestMetadataReturn", "lmao");
            return (IScrapedMetadataCollection)retmetadata;
        }

    }
}
