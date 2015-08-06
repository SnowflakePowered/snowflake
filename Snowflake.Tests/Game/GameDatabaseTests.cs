using System;
using System.IO;
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
            IGameDatabase database = new GameDatabase(filename);
            Assert.NotNull(database);
            this.DisposeSqlite();
            File.Delete(filename);
        }
        [Fact]
        public void RemoveGame_Test()
        {
            string filename = Path.GetTempFileName();
            IGameDatabase database = new GameDatabase(filename);
            var fakeGameInfo = new Mock<IGameInfo>();
            fakeGameInfo.SetupGet(game => game.Name).Returns("TestGame");
            fakeGameInfo.SetupGet(game => game.UUID).Returns("TESTGAME");
            fakeGameInfo.SetupGet(game => game.PlatformID).Returns("TESTPLATFORM");
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
            IGameDatabase database = new GameDatabase(filename);
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
            IGameDatabase database = new GameDatabase(filename);
            var fakeGameInfo = new Mock<IGameInfo>();
            fakeGameInfo.SetupGet(game => game.Name).Returns("TestGame");
            fakeGameInfo.SetupGet(game => game.UUID).Returns("TESTGAME");
            fakeGameInfo.SetupGet(game => game.PlatformID).Returns("TESTPLATFORM");
            database.AddGame(fakeGameInfo.Object);
            Assert.Equal(fakeGameInfo.Object.UUID, database.GetGamesByName("TestGame")[0].UUID);
            Assert.Equal(fakeGameInfo.Object.Name, database.GetGamesByName("TestGame")[0].Name);
            Assert.Equal(fakeGameInfo.Object.PlatformID, database.GetGamesByName("TestGame")[0].PlatformID);
            this.DisposeSqlite();
            File.Delete(filename);
        }
        [Fact]
        public void GetGamesByPlatform_Test()
        {
            string filename = Path.GetTempFileName();
            IGameDatabase database = new GameDatabase(filename);
            var fakeGameInfo = new Mock<IGameInfo>();
            fakeGameInfo.SetupGet(game => game.Name).Returns("TestGame");
            fakeGameInfo.SetupGet(game => game.UUID).Returns("TESTGAME");
            fakeGameInfo.SetupGet(game => game.PlatformID).Returns("TESTPLATFORM");
            database.AddGame(fakeGameInfo.Object);
            Assert.Equal(fakeGameInfo.Object.UUID, database.GetGamesByPlatform("TESTPLATFORM")[0].UUID);
            Assert.Equal(fakeGameInfo.Object.Name, database.GetGamesByPlatform("TESTPLATFORM")[0].Name);
            Assert.Equal(fakeGameInfo.Object.PlatformID, database.GetGamesByPlatform("TESTPLATFORM")[0].PlatformID);
            this.DisposeSqlite();
            File.Delete(filename);
        }

        [Fact]
        public void GetAllGames_Test()
        {
            string filename = Path.GetTempFileName();
            IGameDatabase database = new GameDatabase(filename);
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
