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
using Snowflake.Records.Metadata;
using Xunit;

namespace Snowflake.Records.Tests
{
    public class EntityGameLibraryTests
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
    }
}
