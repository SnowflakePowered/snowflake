using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Persistence;
using Snowflake.Records.File;
using Snowflake.Records.Game;
using Snowflake.Records.Metadata;

using Xunit;

namespace Snowflake.Records.Tests
{
    public class SqliteFileLibraryTests
    {
        [Fact]
        public void Set_Test()
        {
            var fileRecord = new FileRecord(@"C:\somefile\test.txt", "text/plain");
            var library = new SqliteFileLibrary(new SqliteDatabase(Path.GetTempFileName()));
            fileRecord.Metadata.Add("test_metadata", new RecordMetadata("test_metadata", "hello world", fileRecord.Guid));
            library.Set(fileRecord);
        }

        [Fact]
        public void SetMultiple_Test()
        {
            var fileRecord = new FileRecord(@"C:\somefile\test.txt", "text/plain");
            var fileRecord2 = new FileRecord(@"C:\somefile\moretest.txt", "text/plain");

            var library = new SqliteFileLibrary(new SqliteDatabase(Path.GetTempFileName()));
            fileRecord.Metadata.Add("test_metadata", new RecordMetadata("test_metadata", "hello world", fileRecord.Guid));
            library.Set(new List<IFileRecord> { fileRecord, fileRecord2 });
        }

        [Fact]
        public void Remove_Test()
        {
            var fileRecord = new FileRecord(@"C:\somefile\test.txt", "text/plain");
            var library = new SqliteFileLibrary(new SqliteDatabase(Path.GetTempFileName()));
            fileRecord.Metadata.Add("test_metadata", new RecordMetadata("test_metadata", "hello world", fileRecord.Guid));
            library.Set(fileRecord);
            Assert.NotNull(library.Get(fileRecord.Guid));
            library.Remove(fileRecord);
            Assert.Null(library.Get(fileRecord.Guid));
        }

        [Fact]
        public void RemoveMultiple_Test()
        {
            var fileRecord = new FileRecord(@"C:\somefile\test.txt", "text/plain");
            var fileRecord2 = new FileRecord(@"C:\somefile\moretest.txt", "text/plain");

            var library = new SqliteFileLibrary(new SqliteDatabase(Path.GetTempFileName()));
            fileRecord.Metadata.Add("test_metadata", new RecordMetadata("test_metadata", "hello world", fileRecord.Guid));
            library.Set(new List<IFileRecord> { fileRecord, fileRecord2 });
            Assert.NotEmpty(library.GetAllRecords());
            library.Remove(new List<IFileRecord> { fileRecord, fileRecord2 });
            Assert.Empty(library.GetAllRecords());
        }

        [Fact]
        public void GetMultiple_Test()
        {
            var fileRecord = new FileRecord(@"C:\somefile\test.txt", "text/plain");
            var fileRecord2 = new FileRecord(@"C:\somefile\moretest.txt", "text/plain");

            var library = new SqliteFileLibrary(new SqliteDatabase(Path.GetTempFileName()));
            fileRecord.Metadata.Add("test_metadata", new RecordMetadata("test_metadata", "hello world", fileRecord.Guid));
            var records = new List<IFileRecord> { fileRecord, fileRecord2 };
            library.Set(records);
            Assert.NotEmpty(library.Get(records.Select(f => f.Guid)));
        }

        [Fact]
        public void GetAllRecords_Test()
        {
            var fileRecord = new FileRecord(@"C:\somefile\test.txt", "text/plain");
            var library = new SqliteFileLibrary(new SqliteDatabase(Path.GetTempFileName()));
            fileRecord.Metadata.Add("test_metadata", new RecordMetadata("test_metadata", "hello world", fileRecord.Guid));
            library.Set(fileRecord);
            Assert.NotEmpty(library.GetAllRecords());
        }

        [Fact]
        public void GetByMetadata_Test()
        {
            var fileRecord = new FileRecord(@"C:\somefile\test.txt", "text/plain");
            var library = new SqliteFileLibrary(new SqliteDatabase(Path.GetTempFileName()));
            fileRecord.Metadata.Add("test_metadata", new RecordMetadata("test_metadata", "hello world", fileRecord.Guid));
            library.Set(fileRecord);
            Assert.NotEmpty(library.GetByMetadata("test_metadata", "hello world"));
        }

        [Fact]
        public void SearchByMetadata_Test()
        {
            var fileRecord = new FileRecord(@"C:\somefile\test.txt", "text/plain");
            var library = new SqliteFileLibrary(new SqliteDatabase(Path.GetTempFileName()));
            fileRecord.Metadata.Add("test_metadata", new RecordMetadata("test_metadata", "hello world", fileRecord.Guid));
            library.Set(fileRecord);
            Assert.NotEmpty(library.SearchByMetadata("test_metadata", "world"));
        }

        [Fact]
        public void GetFilesByPath_Test()
        {
            var fileRecord = new FileRecord(@"C:\somefile\test.txt", "text/plain");
            var library = new SqliteFileLibrary(new SqliteDatabase(Path.GetTempFileName()));
            fileRecord.Metadata.Add("test_metadata", new RecordMetadata("test_metadata", "hello world", fileRecord.Guid));
            library.Set(fileRecord);
            Assert.NotNull(library.Get(@"C:\somefile\test.txt"));
        }

        [Fact]
        public void GetFilesByGuid_Test()
        {
            var fileRecord = new FileRecord(@"C:\somefile\test.txt", "text/plain");
            var library = new SqliteFileLibrary(new SqliteDatabase(Path.GetTempFileName()));
            fileRecord.Metadata.Add("test_metadata", new RecordMetadata("test_metadata", "hello world", fileRecord.Guid));
            library.Set(fileRecord);
            Assert.NotNull(library.Get(fileRecord.Guid));
        }
    }
}
