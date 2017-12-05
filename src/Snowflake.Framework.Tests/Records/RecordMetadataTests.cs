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
    public class RecordMetadataTests
    {
        [Fact]
        public void Equality_Test()
        {
            var x = new RecordMetadata("test_key", "one_value", Guid.Empty);
            var y = new RecordMetadata("test_key", "different_value", Guid.Empty);
            Assert.Equal(x, y); // A record metadata with one key and one guid should be equal
        }
    }
}
