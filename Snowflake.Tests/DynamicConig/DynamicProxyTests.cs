using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Configuration;
using Snowflake.DynamicConfiguration;
using Xunit;
namespace Snowflake.Tests.DynamicConig
{
    public class DynamicProxyTests
    {
        [Fact]
        public void Test()
        {
            var x = new DynamicConfiguration<IVideoConfiguration>();
            x.Configuration.VideoDriver = VideoDriver.SDL2;
            Assert.Equal(x.Configuration.VideoDriver, VideoDriver.SDL2);
        }
    }
}
