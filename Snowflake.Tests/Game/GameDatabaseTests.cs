using System;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Moq;
using Xunit;

namespace Snowflake.Game.Tests
{
    public class GameDatabaseTests
    {
        [Fact]
        public void CreateDatabase_Test()
        {
            string filename = Path.GetTempFileName();
            IGameLibrary database = new GameLibrary(filename);
            Assert.NotNull(database);
        }

        [Fact]
        public void AddGame_Test()
        {
            string filename = Path.GetTempFileName();
            IGameLibrary database = new GameLibrary(filename);
            var fakeGameInfo = new GameInfo("TEST", "TEST", "TEST.TEST", "TEST", "TEST",
                new Dictionary<string, string> {{"snowflake_mediastore", "TEST"}});
            
            database.AddGame(fakeGameInfo);
        }

        [Fact]
        public void RemoveGame_Test()
        {
            string filename = Path.GetTempFileName();
            IGameLibrary database = new GameLibrary(filename);
            var fakeGameInfo = new GameInfo("TESTGAME", "TEST", "TEST.TEST", "TEST", "TEST",
             new Dictionary<string, string> { { "snowflake_mediastore", "TEST" } });

            database.AddGame(fakeGameInfo);
            Assert.Equal(fakeGameInfo.UUID, database.GetGameByUUID("TESTGAME").UUID);
            Assert.Equal(fakeGameInfo.Title, database.GetGameByUUID("TESTGAME").Title);
            Assert.Equal(fakeGameInfo.PlatformID, database.GetGameByUUID("TESTGAME").PlatformID);

            database.RemoveGame(fakeGameInfo);
            Assert.Null(database.GetGameByUUID(fakeGameInfo.UUID));
        }

        [Fact]
        public void GetGameByUUID_Test()
        {
            string filename = Path.GetTempFileName();
            IGameLibrary database = new GameLibrary(filename);
            var fakeGameInfo = new GameInfo("TESTGAME", "TEST", "TEST.TEST", "TEST", "TEST",
              new Dictionary<string, string> { { "snowflake_mediastore", "TEST" } });
            database.AddGame(fakeGameInfo);
            Assert.Equal(fakeGameInfo.UUID, database.GetGameByUUID("TESTGAME").UUID);
            Assert.Equal(fakeGameInfo.Title, database.GetGameByUUID("TESTGAME").Title);
            Assert.Equal(fakeGameInfo.PlatformID, database.GetGameByUUID("TESTGAME").PlatformID);
        }

        [Fact]
        public void GetGamesByName_Test()
        {
            string filename = Path.GetTempFileName();
            IGameLibrary database = new GameLibrary(filename);
            var fakeGameInfo = new GameInfo("TEST", "TEST", "TEST.TEST", "TestGame", "TEST",
              new Dictionary<string, string> { { "snowflake_mediastore", "TEST" } });

            database.AddGame(fakeGameInfo);
            Assert.Equal(fakeGameInfo.UUID, database.GetGamesByName("TestGame").First().UUID);
            Assert.Equal(fakeGameInfo.Title, database.GetGamesByName("TestGame").First().Title);
            Assert.Equal(fakeGameInfo.PlatformID, database.GetGamesByName("TestGame").First().PlatformID);
        }
        [Fact]
        public void GetGamesByPlatform_Test()
        {
            string filename = Path.GetTempFileName();
            IGameLibrary database = new GameLibrary(filename);
            var fakeGameInfo = new GameInfo("TEST", "TESTPLATFORM", "TEST.TEST", "TEST", "TEST",
            new Dictionary<string, string> { { "snowflake_mediastore", "TEST" } });

            database.AddGame(fakeGameInfo);
            Assert.Equal(fakeGameInfo.UUID, database.GetGamesByPlatform("TESTPLATFORM").First().UUID);
            Assert.Equal(fakeGameInfo.Title, database.GetGamesByPlatform("TESTPLATFORM").First().Title);
            Assert.Equal(fakeGameInfo.PlatformID, database.GetGamesByPlatform("TESTPLATFORM").First().PlatformID);
        }

        [Fact]
        public void GetAllGames_Test()
        {
            string filename = Path.GetTempFileName();
            IGameLibrary database = new GameLibrary(filename);
            var fakeGameInfo = new GameInfo("TEST", "TEST", "TEST.TEST", "TEST", "TEST",
              new Dictionary<string, string> { { "snowflake_mediastore", "TEST" } });

            database.AddGame(fakeGameInfo);
            Assert.NotEmpty(database.GetAllGames());
        }
    }
}
