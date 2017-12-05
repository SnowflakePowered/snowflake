using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Records.File;
using Snowflake.Records.Game;
using Snowflake.Records.Metadata;
using Snowflake.Utility;
using Xunit;

namespace Snowflake.Records.Tests
{
    public class MetadataCollectionTests
    {
        [Fact]
        public void AddWithKeyValue_Test()
        {
            var recordMetadata = new RecordMetadata("TestKey", "TestValue", Guid.Empty);
            var collection = new MetadataCollection(Guid.Empty) { { "TestKey", "TestValue" } };
            Assert.Equal(recordMetadata, collection["TestKey"]);
        }

        [Fact]
        public void GetByGuid_Test()
        {
            var recordMetadata = new RecordMetadata("TestKey", "TestValue", Guid.Empty);
            var collection = new MetadataCollection(Guid.Empty) { { "TestKey", "TestValue" } };
            Assert.Equal(collection[recordMetadata.Guid], recordMetadata);
        }

        [Fact]
        public void SetByKey_Test()
        {
            IMetadataCollection collection = new MetadataCollection(Guid.Empty);
            collection["TestKey"] = "TestValue";
            Assert.Equal("TestValue", collection["TestKey"]);
        }
    }
}
