using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Configuration;
using Xunit;
namespace Snowflake.Tests.Configuration
{
    public class ConfigurationCollectionTests
    {
        [Fact]
        public void Collection_MakeTest()
        {
            var collection = ConfigurationCollection.MakeDefault<ExampleConfigurationCollection>();
            Assert.NotNull(collection);
        }
    }
}
