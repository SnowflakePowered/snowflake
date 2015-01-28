using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Game;
using Snowflake.Information.MediaStore;
using Xunit;
namespace Snowflake.Game.Tests
{
    public class GameInfoTests
    {
        [Fact]
        public void GameInfoCreation_Test()
        {
            Assert.NotNull(new GameInfo("TEST", "TEST", new FakeMediaStore(), new Dictionary<string, string>(), "TEST", "TEST.TEST", "TEST"));
        }

    }

    internal class FakeMediaStore : IMediaStore
    {
        public IMediaStoreSection Resources { get; set; }
        public IMediaStoreSection Video { get; set; }
        public IMediaStoreSection Audio { get; set; }
        public IMediaStoreSection Images { get; set; }
        public string MediaStoreKey { get; set; }


    }
}
