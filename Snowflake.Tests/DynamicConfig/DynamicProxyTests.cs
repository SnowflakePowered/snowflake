using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Configuration;
using Snowflake.DynamicConfiguration;
using Xunit;
namespace Snowflake.Tests.DynamicConfig
{
    public class DynamicProxyTests
    {
        [Fact]
        public void Test()
        {
            var x = new DynamicConfiguration<IVideoConfiguration>("", "", "", "");
            x.Configuration.VideoDriver = VideoDriver.SDL2;
            Assert.Equal(x.Configuration.VideoDriver, VideoDriver.SDL2);
            Assert.Equal(x.Configuration, x.Configuration.Configuration);
        }

        [Fact]
        public void CollectionTest()
        {
            var x = new ConfigurationCollection<IRetroArchConfig>();
            x.Configuration.VideoConfiguration.VideoDriver = VideoDriver.SDL2;
            Console.Write(x.GetEnumerator());
            Assert.Equal(x.Configuration.VideoConfiguration.VideoDriver, VideoDriver.SDL2);
        }
    }
}
