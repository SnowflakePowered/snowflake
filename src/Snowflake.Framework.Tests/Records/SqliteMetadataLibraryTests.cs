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
using Snowflake.Persistence;

namespace Snowflake.Records.Tests
{
    public class SqliteMetadataLibraryTests
    {
        [Fact]
        public void Set_Test()
        {
            var metadata = new RecordMetadata("test", "value", Guid.Empty);
            var library = new SqliteMetadataLibrary(new SqliteDatabase(Path.GetTempFileName()));
            library.Set(metadata);
        }

        [Fact]
        public void SetMultiple_Test()
        {
            var metadata = new List<IRecordMetadata>
            {
                new RecordMetadata("test", "value", Guid.Empty),
                new RecordMetadata("test2", "value", Guid.Empty)
            };
            var library = new SqliteMetadataLibrary(new SqliteDatabase(Path.GetTempFileName()));
            library.Set(metadata);
        }

        [Fact]
        public void Remove_Test()
        {
            var metadata = new RecordMetadata("test", "value", Guid.Empty);
            var library = new SqliteMetadataLibrary(new SqliteDatabase(Path.GetTempFileName()));
            library.Set(metadata);
            Assert.NotNull(library.Get(metadata.Guid));
            library.Remove(metadata);
            Assert.Null(library.Get(metadata.Guid));
        }

        [Fact]
        public void RemoveMultiple_Test()
        {
            var metadata = new List<IRecordMetadata>
            {
                new RecordMetadata("test", "value", Guid.Empty),
                new RecordMetadata("test2", "value", Guid.Empty)
            };
            var library = new SqliteMetadataLibrary(new SqliteDatabase(Path.GetTempFileName()));
            library.Set(metadata);
            Assert.NotEmpty(library.GetAllRecords());
            library.Remove(metadata);
            Assert.Empty(library.GetAllRecords());
        }

        [Fact]
        public void GetMultiple_Test()
        {
            var metadata = new List<IRecordMetadata>
            {
                new RecordMetadata("test", "value", Guid.Empty),
                new RecordMetadata("test2", "value", Guid.Empty)
            };
            var library = new SqliteMetadataLibrary(new SqliteDatabase(Path.GetTempFileName()));
            library.Set(metadata);
            Assert.NotEmpty(library.Get(metadata.Select(f => f.Guid)));
        }

        [Fact]
        public void GetByKey_Test()
        {
            var metadata = new RecordMetadata("test", "value", Guid.Empty);
            var library = new SqliteMetadataLibrary(new SqliteDatabase(Path.GetTempFileName()));
            library.Set(metadata);
            Assert.NotNull(library.Get("test", Guid.Empty));
        }

        [Fact]
        public void GetByValue_Test()
        {
            var metadata = new RecordMetadata("test", "value", Guid.Empty);
            var library = new SqliteMetadataLibrary(new SqliteDatabase(Path.GetTempFileName()));
            library.Set(metadata);
            Assert.NotEmpty(library.Get("test", "value"));
        }

        [Fact]
        public void GetAllRecords_Test()
        {
            var metadata = new RecordMetadata("test", "value", Guid.Empty);
            var library = new SqliteMetadataLibrary(new SqliteDatabase(Path.GetTempFileName()));
            library.Set(metadata);
            Assert.NotEmpty(library.GetAllRecords());
        }
    }
    
}
