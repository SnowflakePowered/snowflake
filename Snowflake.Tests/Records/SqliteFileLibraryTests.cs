using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Records.File;
using Snowflake.Records.Game;
using Snowflake.Records.Metadata;
using Snowflake.Utility;
using Xunit;

namespace Snowflake.Records.Tests
{
    public class SqliteFileLibraryTests
    {
        [Fact]
        public void GetRecords_Test()
        {
            var fileRecord = new FileRecord(@"C:\somefile\test.txt", "text/plain", Guid.Empty);
            var library = new SqliteFileLibrary(new SqliteDatabase(Path.GetTempFileName()));
            fileRecord.Metadata.Add("test_metadata", new RecordMetadata("test_metadata", "hello world", fileRecord.Guid));
            library.Add(fileRecord);
            Assert.NotEmpty(library.GetRecords());
        }

        [Fact]
        public void GetByMetadata_Test()
        {
            var fileRecord = new FileRecord(@"C:\somefile\test.txt", "text/plain", Guid.Empty);
            var library = new SqliteFileLibrary(new SqliteDatabase(Path.GetTempFileName()));
            fileRecord.Metadata.Add("test_metadata", new RecordMetadata("test_metadata", "hello world", fileRecord.Guid));
            library.Add(fileRecord);
            Assert.NotEmpty(library.GetByMetadata("test_metadata", "hello world"));
        }

        [Fact]
        public void GetByMetadatEmpty_Test()
        {
            var fileRecord = new FileRecord(@"C:\somefile\test.txt", "text/plain", Guid.Empty);
            var library = new SqliteFileLibrary(new SqliteDatabase(Path.GetTempFileName()));
            fileRecord.Metadata.Add("test_metadata", new RecordMetadata("test_metadata", "hello world", fileRecord.Guid));
            library.Add(fileRecord);
            Assert.Empty(library.GetByMetadata(String.Empty, String.Empty));
        }

        [Fact]
        public void SearchByMetadata_Test()
        {
            var fileRecord = new FileRecord(@"C:\somefile\test.txt", "text/plain", Guid.Empty);
            var library = new SqliteFileLibrary(new SqliteDatabase(Path.GetTempFileName()));
            fileRecord.Metadata.Add("test_metadata", new RecordMetadata("test_metadata", "hello world", fileRecord.Guid));
            library.Add(fileRecord);
            Assert.NotEmpty(library.SearchByMetadata("test_metadata", "world"));
        }

        [Fact]
        public void SearchByMetadataEmpty_Test()
        {
            var fileRecord = new FileRecord(@"C:\somefile\test.txt", "text/plain", Guid.Empty);
            var library = new SqliteFileLibrary(new SqliteDatabase(Path.GetTempFileName()));
            fileRecord.Metadata.Add("test_metadata", new RecordMetadata("test_metadata", "hello world", fileRecord.Guid));
            library.Add(fileRecord);
            Assert.Empty(library.SearchByMetadata(String.Empty, String.Empty));
        }

        [Fact]
        public void GetFilesForGame_Test()
        {
            var fileRecord = new FileRecord(@"C:\somefile\test.txt", "text/plain", Guid.Empty);
            var library = new SqliteFileLibrary(new SqliteDatabase(Path.GetTempFileName()));
            fileRecord.Metadata.Add("test_metadata", new RecordMetadata("test_metadata", "hello world", fileRecord.Guid));
            library.Add(fileRecord);
            Assert.NotNull(library.GetFilesForGame(Guid.Empty)
                .FirstOrDefault(g => g.Guid == fileRecord.Guid));
            Assert.NotEmpty(library.GetFilesForGame(Guid.Empty)
                .FirstOrDefault(g => g.Guid == fileRecord.Guid).Metadata);
        }

        [Fact]
        public void GetFilesByPath_Test()
        {
            var fileRecord = new FileRecord(@"C:\somefile\test.txt", "text/plain", Guid.Empty);
            var library = new SqliteFileLibrary(new SqliteDatabase(Path.GetTempFileName()));
            fileRecord.Metadata.Add("test_metadata", new RecordMetadata("test_metadata", "hello world", fileRecord.Guid));
            library.Add(fileRecord);
            Assert.NotNull(library.GetFile(@"C:\somefile\test.txt"));
        }
        [Fact]
        public void GetFilesByPathEmpty_Test()
        {
            var fileRecord = new FileRecord(@"C:\somefile\test.txt", "text/plain", Guid.Empty);
            var library = new SqliteFileLibrary(new SqliteDatabase(Path.GetTempFileName()));
            fileRecord.Metadata.Add("test_metadata", new RecordMetadata("test_metadata", "hello world", fileRecord.Guid));
            library.Add(fileRecord);
            Assert.Null(library.GetFile(String.Empty));
        }


        [Fact]
        public void GetFilesByGuid_Test()
        {
            var fileRecord = new FileRecord(@"C:\somefile\test.txt", "text/plain", Guid.Empty);
            var library = new SqliteFileLibrary(new SqliteDatabase(Path.GetTempFileName()));
            fileRecord.Metadata.Add("test_metadata", new RecordMetadata("test_metadata", "hello world", fileRecord.Guid));
            library.Add(fileRecord);
            Assert.NotNull(library.Get(fileRecord.Guid));
        }

        [Fact]
        public void GetFilesByGuidEmpty_Test()
        {
            var fileRecord = new FileRecord(@"C:\somefile\test.txt", "text/plain", Guid.Empty);
            var library = new SqliteFileLibrary(new SqliteDatabase(Path.GetTempFileName()));
            fileRecord.Metadata.Add("test_metadata", new RecordMetadata("test_metadata", "hello world", fileRecord.Guid));
            library.Add(fileRecord);
            Assert.Null(library.Get(Guid.NewGuid()));
        }
    }
    
}
