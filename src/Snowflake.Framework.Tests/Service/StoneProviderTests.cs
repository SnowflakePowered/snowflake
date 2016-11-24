using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Services;
using Xunit;
namespace Snowflake.Tests.Service
{
    public class StoneProviderTests
    {
        [Fact]
        public void StoneLoad_Test()
        {
            var stone = new StoneProvider();
            Assert.NotEmpty(stone.Controllers);
            Assert.NotEmpty(stone.Platforms);
            Assert.NotNull(stone.StoneVersion);
        }
    }
}
