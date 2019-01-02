using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using Snowflake.Model.Database;
using Snowflake.Model.Database.Models;
using Snowflake.Model.Records;
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
            var guid = record.RecordId;
            var newRecord = lib.GetAllRecords().First();
            Assert.Equal(guid, newRecord.RecordId);
            Assert.Equal(record.PlatformId, newRecord.PlatformId);
        }


        [Fact]
        public void SetRemoveMetadata_Test()
        {
            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            optionsBuilder.UseSqlite($"Data Source={Path.GetTempFileName()}");

            var lib = new GameRecordLibrary(optionsBuilder);
            var record = lib.CreateRecord("NINTENDO_NES");
            var guid = record.RecordId;

            record.Metadata.Add("is_test", "true");

            lib.UpdateRecord(record);
            var newRecord = lib.GetAllRecords().First();
            Assert.Equal(guid, newRecord.RecordId);
            Assert.Equal(record.PlatformId, newRecord.PlatformId);
            Assert.Contains("is_test", newRecord.Metadata.Keys);
            Assert.Equal("true", newRecord.Metadata["is_test"]);

            newRecord.Metadata.Remove("is_test");
            lib.UpdateRecord(newRecord);

            var newNewRecord = lib.GetAllRecords().First();

            Assert.Equal(guid, newNewRecord.RecordId);
            Assert.Equal(record.PlatformId, newNewRecord.PlatformId);
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
            var guid = record.RecordId;

            record.Metadata.Add("is_test", "true");
            lib.UpdateRecord(record);
            var newRecord = lib.GetAllRecords().First();
            Assert.Equal(guid, newRecord.RecordId);
            Assert.Equal(record.PlatformId, newRecord.PlatformId);
            Assert.Contains("is_test", newRecord.Metadata.Keys);
            Assert.Equal("true", newRecord.Metadata["is_test"]);

            newRecord.Metadata.Add("is_test_two", "true");
            lib.UpdateRecord(newRecord);

            var newNewRecord = lib.GetAllRecords().First();

            Assert.Equal(guid, newNewRecord.RecordId);
            Assert.Equal(record.PlatformId, newNewRecord.PlatformId);
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
            Assert.NotNull(lib.GetRecord(record.RecordId));
            lib.DeleteRecord(record);
            Assert.Null(lib.GetRecord(record.RecordId));
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
            Assert.NotNull(lib.GetRecord(record.RecordId));
        }

        [Fact]
        public void GetGameByPlatforms_Test()
        {
            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            optionsBuilder.UseSqlite($"Data Source={Path.GetTempFileName()}");
            var lib = new GameRecordLibrary(optionsBuilder);

            var record = lib.CreateRecord("TEST_PLATFORM");

            Assert.NotEmpty(lib.GetRecords(r => r.PlatformId == "TEST_PLATFORM"));
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

            Assert.NotEmpty(lib.GetRecords(g => g.Title == "Test"));
        }
    }
}
