using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using Snowflake.Model.Database;
using Snowflake.Model.Database.Models;
using Snowflake.Model.Game;
using Snowflake.Model.Records;
using Snowflake.Model.Records.Game;
using Xunit;

namespace Snowflake.Model.Tests
{
    public class EntityGameRecordLibraryTests
    {
        [Fact]
        public void Set_Test()
        {
            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            optionsBuilder.UseSqlite($"Data Source={Path.GetTempFileName()}");
            var lib = new GameRecordLibrary(optionsBuilder);
            var record = lib.CreateRecord("NINTENDO_NES");
            var guid = record.RecordID;
            var newRecord = lib.GetAllRecords().First();
            Assert.Equal(guid, newRecord.RecordID);
            Assert.Equal(record.PlatformID, newRecord.PlatformID);
        }

        [Fact]
        public void SetRemoveMetadata_Test()
        {
            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            optionsBuilder.UseSqlite($"Data Source={Path.GetTempFileName()}");

            var lib = new GameRecordLibrary(optionsBuilder);
            var record = lib.CreateRecord("NINTENDO_NES");
            var guid = record.RecordID;

            record.Metadata.Add("is_test", "true");

            lib.UpdateRecord(record);
            var newRecord = lib.GetAllRecords().First();
            Assert.Equal(guid, newRecord.RecordID);
            Assert.Equal(record.PlatformID, newRecord.PlatformID);
            Assert.Contains("is_test", newRecord.Metadata.Keys);
            Assert.Equal("true", newRecord.Metadata["is_test"]);

            newRecord.Metadata.Remove("is_test");
            lib.UpdateRecord(newRecord);

            var newNewRecord = lib.GetAllRecords().First();

            Assert.Equal(guid, newNewRecord.RecordID);
            Assert.Equal(record.PlatformID, newNewRecord.PlatformID);
            Assert.DoesNotContain("is_test", newNewRecord.Metadata.Keys);
        }

        [Fact]
        public void SetWithMetadata_Test()
        {
            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            optionsBuilder.UseSqlite($"Data Source={Path.GetTempFileName()}")
                .ConfigureWarnings(warnings => warnings.Throw(RelationalEventId.QueryClientEvaluationWarning));

            var lib = new GameRecordLibrary(optionsBuilder);
            var record = lib.CreateRecord("NINTENDO_NES");
            var guid = record.RecordID;

            record.Metadata.Add("is_test", "true");
            lib.UpdateRecord(record);
            var newRecord = lib.GetAllRecords().First();
            Assert.Equal(guid, newRecord.RecordID);
            Assert.Equal(record.PlatformID, newRecord.PlatformID);
            Assert.Contains("is_test", newRecord.Metadata.Keys);
            Assert.Equal("true", newRecord.Metadata["is_test"]);

            newRecord.Metadata.Add("is_test_two", "true");
            lib.UpdateRecord(newRecord);

            var newNewRecord = lib.GetAllRecords().First();

            Assert.Equal(guid, newNewRecord.RecordID);
            Assert.Equal(record.PlatformID, newNewRecord.PlatformID);
            Assert.Contains("is_test", newNewRecord.Metadata.Keys);
            Assert.Equal("true", newNewRecord.Metadata["is_test"]);
            Assert.Contains("is_test_two", newNewRecord.Metadata.Keys);
            Assert.Equal("true", newNewRecord.Metadata["is_test_two"]);
        }

        [Fact]
        public void SetMultiple_Test()
        {
            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            optionsBuilder.UseSqlite($"Data Source={Path.GetTempFileName()}");
            var lib = new GameRecordLibrary(optionsBuilder);
            var record = lib.CreateRecord("NINTENDO_NES");
            var record2 = lib.CreateRecord("NINTENDO_NES");

            record.Title = "Test Game";
            record2.Title = "Test Game 2";

            lib.UpdateRecord(record);
            lib.UpdateRecord(record2);
        }

        [Fact]
        public void Remove_Test()
        {
            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            optionsBuilder.UseSqlite($"Data Source={Path.GetTempFileName()}");
            var lib = new GameRecordLibrary(optionsBuilder);

            var record = lib.CreateRecord("TEST_PLATFORM");
            Assert.NotNull(lib.GetRecord(record.RecordID));
            lib.DeleteRecord(record);
            Assert.Null(lib.GetRecord(record.RecordID));
        }

        [Fact]
        public void RemoveMultiple_Test()
        {
            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            optionsBuilder.UseSqlite($"Data Source={Path.GetTempFileName()}");
            var lib = new GameRecordLibrary(optionsBuilder);

            var record = lib.CreateRecord("TEST_PLATFORM");
            var record2 = lib.CreateRecord("TEST_PLATFORM");

            Assert.NotEmpty(lib.GetAllRecords());
            lib.DeleteRecord(record);
            lib.DeleteRecord(record2);
            Assert.Empty(lib.GetAllRecords());
        }

        [Fact]
        public void GetGameByGuid_Test()
        {
            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            optionsBuilder.UseSqlite($"Data Source={Path.GetTempFileName()}");
            var lib = new GameRecordLibrary(optionsBuilder);

            var record = lib.CreateRecord("TEST_PLATFORM");
            Assert.NotNull(lib.GetRecord(record.RecordID));
        }

        [Fact]
        public void GetGameByPlatforms_Test()
        {
            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            optionsBuilder.UseSqlite($"Data Source={Path.GetTempFileName()}");
            var lib = new GameRecordLibrary(optionsBuilder);

            var record = lib.CreateRecord("TEST_PLATFORM");

            Assert.NotEmpty(lib.GetRecords(r => r.PlatformID == "TEST_PLATFORM"));
        }

        [Fact]
        public void QueryGameByPlatforms_Test()
        {
            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            optionsBuilder.UseSqlite($"Data Source={Path.GetTempFileName()}");
            var lib = new GameRecordLibrary(optionsBuilder);

            var record = lib.CreateRecord("TEST_PLATFORM");

            Assert.NotEmpty(lib.QueryRecords(r => r.PlatformID == "TEST_PLATFORM"));
        }

        [Fact]
        public async Task QueryGamesByPlatformsAsync_Test()
        {
            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            optionsBuilder.UseSqlite($"Data Source={Path.GetTempFileName()}");
            var lib = new GameRecordLibrary(optionsBuilder);

            var record = lib.CreateRecord("TEST_PLATFORM");

            Assert.NotEmpty(await lib.QueryRecordsAsync(r => r.PlatformID == "TEST_PLATFORM"));
        }

        [Fact]
        public void GetAllRecords_Test()
        {
            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            optionsBuilder.UseSqlite($"Data Source={Path.GetTempFileName()}");
            var lib = new GameRecordLibrary(optionsBuilder);

            var record = lib.CreateRecord("TEST_PLATFORM");

            Assert.NotEmpty(lib.GetAllRecords());
        }

        [Fact]
        public void GetGamesByTitle_Test()
        {
            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            optionsBuilder.UseSqlite($"Data Source={Path.GetTempFileName()}");
            var lib = new GameRecordLibrary(optionsBuilder);

            var record = lib.CreateRecord("TEST_PLATFORM");
            record.Title = "Test";
            lib.UpdateRecord(record);

            Assert.NotEmpty(lib.GetRecords(g => g.Metadata[GameMetadataKeys.Title] == "Test"));
        }

        [Fact]
        public void QueryGamesByTitle_Test()
        {
            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            optionsBuilder.UseSqlite($"Data Source={Path.GetTempFileName()}");
            var lib = new GameRecordLibrary(optionsBuilder);

            var record = lib.CreateRecord("TEST_PLATFORM");
            record.Title = "Test";
            lib.UpdateRecord(record);

            Assert.NotEmpty(lib.QueryRecords(g => g.Metadata.First(m => m.MetadataKey == GameMetadataKeys.Title).MetadataValue == "Test"));
        }

        [Fact]
        public async Task QueryGamesByTitleAsync_Test()
        {
            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            optionsBuilder.UseSqlite($"Data Source={Path.GetTempFileName()}");
            var lib = new GameRecordLibrary(optionsBuilder);

            var record = lib.CreateRecord("TEST_PLATFORM");
            record.Title = "Test";
            lib.UpdateRecord(record);

            Assert.NotEmpty(await lib.QueryRecordsAsync(g => g.Metadata.First(m => m.MetadataKey == GameMetadataKeys.Title).MetadataValue == "Test"));
        }
    }
}
