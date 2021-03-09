using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Configuration;
using Snowflake.Configuration.Tests;
using Xunit;

namespace Snowflake.Configuration.Tests
{
    public class DynamicProxyTests
    {
        [Fact]
        public void CollectionTest()
        {
            var x = new ConfigurationCollection<IRetroArchConfig>();
            x.Configuration.VideoConfiguration.VideoDriver = VideoDriver.SDL2;
            Console.Write(x.GetEnumerator());
            Assert.Equal(VideoDriver.SDL2, x.Configuration.VideoConfiguration.VideoDriver);
        }
    }
}
