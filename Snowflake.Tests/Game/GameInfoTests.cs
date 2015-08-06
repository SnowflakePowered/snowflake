using System.Collections.Generic;
using Xunit;

namespace Snowflake.Game.Tests
{
    public class GameInfoTests
    {
        [Fact]
        public void GameInfoCreation_Test()
        {
            Assert.NotNull(new GameInfo("TEST", "TEST", new Dictionary<string, string> {{"snowflake_mediastore", "TEST"}}, "TEST", "TEST.TEST", "TEST"));
        }

    }
}
