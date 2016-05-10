using System;
using System.Collections.Generic;
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
        public void GetFilesForGameTest()
        {
            var fileRecord = new FileRecord(@"C:\somefile\test.txt", "text/plain", Guid.Empty);
            var library = new SqliteFileLibrary(new SqliteDatabase("test.db"));
            fileRecord.Metadata.Add("test_metadata", new RecordMetadata("test_metadata", "hello world", fileRecord.Guid));
            library.Add(fileRecord);
            Assert.NotNull(library.GetFilesForGame(Guid.Empty)
                .FirstOrDefault(g => g.Guid == fileRecord.Guid));
            Assert.NotEmpty(library.GetFilesForGame(Guid.Empty)
                .FirstOrDefault(g => g.Guid == fileRecord.Guid).Metadata);
        }
    }
    
}
