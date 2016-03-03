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
            this.DisposeSqlite();
            File.Delete(filename);
        }
        [Fact]
        public void RemoveGame_Test()
        {
            string filename = Path.GetTempFileName();
            IGameLibrary database = new GameLibrary(filename);
            var fakeGameInfo = new Mock<IGameInfo>();
            fakeGameInfo.SetupGet(game => game.Name).Returns("TestGame");
            fakeGameInfo.SetupGet(game => game.UUID).Returns("TESTGAME");
            fakeGameInfo.SetupGet(game => game.PlatformID).Returns("TESTPLATFORM");
            fakeGameInfo.SetupGet(game => game.Metadata).Returns(new Dictionary<string, string>() { { "TEST", "GAME"} });
            database.AddGame(fakeGameInfo.Object);
            Assert.Equal(fakeGameInfo.Object.UUID, database.GetGameByUUID("TESTGAME").UUID);
            Assert.Equal(fakeGameInfo.Object.Name, database.GetGameByUUID("TESTGAME").Name);
            Assert.Equal(fakeGameInfo.Object.PlatformID, database.GetGameByUUID("TESTGAME").PlatformID);

            database.RemoveGame(fakeGameInfo.Object);
            Assert.Null(database.GetGameByUUID(fakeGameInfo.Object.UUID));
            this.DisposeSqlite();
            File.Delete(filename);
        }

        [Fact]
        public void GetGameByUUID_Test()
        {
            string filename = Path.GetTempFileName();
            IGameLibrary database = new GameLibrary(filename);
            var fakeGameInfo = new Mock<IGameInfo>();
            fakeGameInfo.SetupGet(game => game.Name).Returns("TestGame");
            fakeGameInfo.SetupGet(game => game.UUID).Returns("TESTGAME");
            fakeGameInfo.SetupGet(game => game.PlatformID).Returns("TESTPLATFORM");
            database.AddGame(fakeGameInfo.Object);
            Assert.Equal(fakeGameInfo.Object.UUID, database.GetGameByUUID("TESTGAME").UUID);
            Assert.Equal(fakeGameInfo.Object.Name, database.GetGameByUUID("TESTGAME").Name);
            Assert.Equal(fakeGameInfo.Object.PlatformID, database.GetGameByUUID("TESTGAME").PlatformID);
            this.DisposeSqlite();
            File.Delete(filename);
        }

        [Fact]
        public void GetGamesByName_Test()
        {
            string filename = Path.GetTempFileName();
            IGameLibrary database = new GameLibrary(filename);
            var fakeGameInfo = new Mock<IGameInfo>();
            fakeGameInfo.SetupGet(game => game.Name).Returns("TestGame");
            fakeGameInfo.SetupGet(game => game.UUID).Returns("TESTGAME");
            fakeGameInfo.SetupGet(game => game.PlatformID).Returns("TESTPLATFORM");
            database.AddGame(fakeGameInfo.Object);
            Assert.Equal(fakeGameInfo.Object.UUID, database.GetGamesByName("TestGame").First().UUID);
            Assert.Equal(fakeGameInfo.Object.Name, database.GetGamesByName("TestGame").First().Name);
            Assert.Equal(fakeGameInfo.Object.PlatformID, database.GetGamesByName("TestGame").First().PlatformID);
            this.DisposeSqlite();
            File.Delete(filename);
        }
        [Fact]
        public void GetGamesByPlatform_Test()
        {
            string filename = Path.GetTempFileName();
            IGameLibrary database = new GameLibrary(filename);
            var fakeGameInfo = new Mock<IGameInfo>();
            fakeGameInfo.SetupGet(game => game.Name).Returns("TestGame");
            fakeGameInfo.SetupGet(game => game.UUID).Returns("TESTGAME");
            fakeGameInfo.SetupGet(game => game.PlatformID).Returns("TESTPLATFORM");
            database.AddGame(fakeGameInfo.Object);
            Assert.Equal(fakeGameInfo.Object.UUID, database.GetGamesByPlatform("TESTPLATFORM").First().UUID);
            Assert.Equal(fakeGameInfo.Object.Name, database.GetGamesByPlatform("TESTPLATFORM").First().Name);
            Assert.Equal(fakeGameInfo.Object.PlatformID, database.GetGamesByPlatform("TESTPLATFORM").First().PlatformID);
            this.DisposeSqlite();
            File.Delete(filename);
        }

        [Fact]
        public void GetAllGames_Test()
        {
            string filename = Path.GetTempFileName();
            IGameLibrary database = new GameLibrary(filename);
            var fakeGameInfo = new Mock<IGameInfo>();
            fakeGameInfo.SetupGet(game => game.Name).Returns("TestGame");
            fakeGameInfo.SetupGet(game => game.UUID).Returns("TESTGAME");
            fakeGameInfo.SetupGet(game => game.PlatformID).Returns("TESTPLATFORM");
            database.AddGame(fakeGameInfo.Object);
            Assert.NotEmpty(database.GetAllGames());
            this.DisposeSqlite();
            File.Delete(filename);
        }
        private void DisposeSqlite()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }
}
