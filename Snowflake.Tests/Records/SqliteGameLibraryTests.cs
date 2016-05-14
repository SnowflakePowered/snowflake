using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Snowflake.Platform;
using Snowflake.Records.File;
using Snowflake.Records.Game;
using Snowflake.Records.Metadata;
using Snowflake.Utility;
using Xunit;

namespace Snowflake.Records.Tests
{
    public class SqliteGameLibraryTests
    {
        [Fact]
        public void Set_Test()
        {
            var platformInfo = new Mock<IPlatformInfo>();
            platformInfo.SetupGet(p => p.PlatformID).Returns("TEST_PLATFORM");
            var library = new SqliteGameLibrary(new SqliteDatabase(Path.GetRandomFileName()));

            var gameRecord = new GameRecord(platformInfo.Object, "Test Game");
            var fileRecord = new FileRecord(@"C:\somefile\test.txt", "text/plain", gameRecord);
            fileRecord.Metadata.Add("test_metadata", new RecordMetadata("test_metadata", "hello world", fileRecord));

            gameRecord.Files.Add(fileRecord);
            library.Set(gameRecord);

        }

        [Fact]
        public void SetMultiple_Test()
        {
            var platformInfo = new Mock<IPlatformInfo>();
            platformInfo.SetupGet(p => p.PlatformID).Returns("TEST_PLATFORM");
            var library = new SqliteGameLibrary(new SqliteDatabase(Path.GetRandomFileName()));

            var gameRecord = new GameRecord(platformInfo.Object, "Test Game");
            var fileRecord = new FileRecord(@"C:\somefile\test.txt", "text/plain", gameRecord);
            fileRecord.Metadata.Add("test_metadata", new RecordMetadata("test_metadata", "hello world", fileRecord));
            gameRecord.Files.Add(fileRecord);

            var gameRecord2 = new GameRecord(platformInfo.Object, "Test Game II");
            var fileRecord2 = new FileRecord(@"C:\somefile\test.txt", "text/plain", gameRecord2);
            gameRecord2.Files.Add(fileRecord2);
            library.Set(new List<IGameRecord> { gameRecord, gameRecord2 });
        }

        [Fact]
        public void Remove_Test()
        {
            var platformInfo = new Mock<IPlatformInfo>();
            platformInfo.SetupGet(p => p.PlatformID).Returns("TEST_PLATFORM");
            var library = new SqliteGameLibrary(new SqliteDatabase(Path.GetRandomFileName()));

            var gameRecord = new GameRecord(platformInfo.Object, "Test Game");
            var fileRecord = new FileRecord(@"C:\somefile\test.txt", "text/plain", gameRecord);
            fileRecord.Metadata.Add("test_metadata", new RecordMetadata("test_metadata", "hello world", fileRecord));
            gameRecord.Files.Add(fileRecord);

            library.Set(gameRecord);
            Assert.NotNull(library.Get(gameRecord.Guid));
            library.Remove(gameRecord.Guid);
            Assert.Null(library.Get(gameRecord.Guid));
        }


        [Fact]
        public void RemoveMultiple_Test()
        {
            var platformInfo = new Mock<IPlatformInfo>();
            platformInfo.SetupGet(p => p.PlatformID).Returns("TEST_PLATFORM");
            var library = new SqliteGameLibrary(new SqliteDatabase(Path.GetRandomFileName()));

            var gameRecord = new GameRecord(platformInfo.Object, "Test Game");
            var fileRecord = new FileRecord(@"C:\somefile\test.txt", "text/plain", gameRecord);
            fileRecord.Metadata.Add("test_metadata", new RecordMetadata("test_metadata", "hello world", fileRecord));
            gameRecord.Files.Add(fileRecord);

            var gameRecord2 = new GameRecord(platformInfo.Object, "Test Game II");
            var fileRecord2 = new FileRecord(@"C:\somefile\test.txt", "text/plain", gameRecord2);
            gameRecord2.Files.Add(fileRecord2);
            library.Set(new List<IGameRecord> { gameRecord, gameRecord2 });
            Assert.NotEmpty(library.GetAllRecords());
            library.Remove(new List<IGameRecord> {gameRecord, gameRecord2});
            Assert.Empty(library.GetAllRecords());
        }

        [Fact]
        public void GetGameByGuid_Test()
        {
            var platformInfo = new Mock<IPlatformInfo>();
            platformInfo.SetupGet(p => p.PlatformID).Returns("TEST_PLATFORM");
            var library = new SqliteGameLibrary(new SqliteDatabase(Path.GetRandomFileName()));

            var gameRecord = new GameRecord(platformInfo.Object, "Test Game");
            var fileRecord = new FileRecord(@"C:\somefile\test.txt", "text/plain", gameRecord);
            fileRecord.Metadata.Add("test_metadata", new RecordMetadata("test_metadata", "hello world", fileRecord));

            gameRecord.Files.Add(fileRecord);
            library.Set(gameRecord);
            Assert.NotNull(library.Get(gameRecord.Guid));

        }

        [Fact]
        public void GetGameByPlatforms_Test()
        {
            var platformInfo = new Mock<IPlatformInfo>();
            platformInfo.SetupGet(p => p.PlatformID).Returns("TEST_PLATFORM");
            var library = new SqliteGameLibrary(new SqliteDatabase(Path.GetRandomFileName()));

            var gameRecord = new GameRecord(platformInfo.Object, "Test Game");
            var fileRecord = new FileRecord(@"C:\somefile\test.txt", "text/plain", gameRecord);
            fileRecord.Metadata.Add("test_metadata", new RecordMetadata("test_metadata", "hello world", fileRecord));

            gameRecord.Files.Add(fileRecord);
            library.Set(gameRecord);
            Assert.NotEmpty(library.GetGamesByPlatform("TEST_PLATFORM"));
            Assert.Contains("TEST_PLATFORM", library.GetGamesByPlatform("TEST_PLATFORM").Select(g => g.PlatformId));
        }

        [Fact]
        public void GetAllRecords_Test()
        {
            var platformInfo = new Mock<IPlatformInfo>();
            platformInfo.SetupGet(p => p.PlatformID).Returns("TEST_PLATFORM");
            var library = new SqliteGameLibrary(new SqliteDatabase(Path.GetRandomFileName()));

            var gameRecord = new GameRecord(platformInfo.Object, "Test Game");
            var fileRecord = new FileRecord(@"C:\somefile\test.txt", "text/plain", gameRecord);
            fileRecord.Metadata.Add("test_metadata", new RecordMetadata("test_metadata", "hello world", fileRecord));

            gameRecord.Files.Add(fileRecord);
            library.Set(gameRecord);
            Assert.NotEmpty(library.GetAllRecords());
        }

        [Fact]
        public void GetGamesByTitle_Test()
        {
            var platformInfo = new Mock<IPlatformInfo>();
            platformInfo.SetupGet(p => p.PlatformID).Returns("TEST_PLATFORM");
            var library = new SqliteGameLibrary(new SqliteDatabase(Path.GetRandomFileName()));

            var gameRecord = new GameRecord(platformInfo.Object, "Test Game");
            var fileRecord = new FileRecord(@"C:\somefile\test.txt", "text/plain", gameRecord);
            fileRecord.Metadata.Add("test_metadata", new RecordMetadata("test_metadata", "hello world", fileRecord));

            gameRecord.Files.Add(fileRecord);
            library.Set(gameRecord);
            Assert.NotEmpty(library.GetGamesByTitle("Test"));
        }

        [Fact]
        public void GetGamesByMetadata_Test()
        {
            var platformInfo = new Mock<IPlatformInfo>();
            platformInfo.SetupGet(p => p.PlatformID).Returns("TEST_PLATFORM");
            var library = new SqliteGameLibrary(new SqliteDatabase(Path.GetRandomFileName()));

            var gameRecord = new GameRecord(platformInfo.Object, "Test Game");
            var fileRecord = new FileRecord(@"C:\somefile\test.txt", "text/plain", gameRecord);
            fileRecord.Metadata.Add("test_metadata", new RecordMetadata("test_metadata", "hello world", fileRecord));

            gameRecord.Files.Add(fileRecord);
            gameRecord.Metadata.Add("test_metadata", "game_test_value");
            library.Set(gameRecord);
            Assert.NotEmpty(library.GetByMetadata("test_metadata", "game_test_value"));
        }

    }
    
}
