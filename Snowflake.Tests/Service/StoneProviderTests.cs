using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Service;
using Xunit;
namespace Snowflake.Service.Tests
{
    public class StoneProviderTests
    {
        [Fact]
        public void StoneProvider_Test()
        {
            var provider = new StoneProvider();
            Assert.NotEmpty(provider.Platforms);
            Assert.NotNull(provider.StoneVersion);
        }
    }
}
