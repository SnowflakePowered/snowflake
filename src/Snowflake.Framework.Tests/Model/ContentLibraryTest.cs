using Microsoft.EntityFrameworkCore;
using Snowflake.Model.Database;
using Snowflake.Model.Database.Models;
using Snowflake.Model.Game;
using Snowflake.Tests;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Zio.FileSystems;

namespace Snowflake.Model.Records.Tests
{
    public class ContentLibraryTest
    {
        [Fact]
        public void CreateLibraryTest()
        {
            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            optionsBuilder.UseSqlite($"Data Source={Path.GetTempFileName()}");

            var store = new ContentLibraryStore(new PhysicalFileSystem(), optionsBuilder);

            var tempDir = TestUtilities.GetTemporaryDirectory();
#pragma warning disable CS0618 // Type or member is obsolete
            var library = store.CreateLibrary(tempDir.UnsafeGetPath());
#pragma warning restore CS0618 // Type or member is obsolete

            var recordGuid = Guid.NewGuid();
            library.OpenRecordLibrary(recordGuid);

            Assert.True(tempDir.ContainsDirectory(recordGuid.ToString()));
            Assert.Equal(library.LibraryID, store.GetLibrary(library.LibraryID).LibraryID);
        }


        [Fact]
        public void CreateLibraryForGameTest()
        {
            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            optionsBuilder.UseSqlite($"Data Source={Path.GetTempFileName()}");

            var store = new ContentLibraryStore(new PhysicalFileSystem(), optionsBuilder);

            var tempDir = TestUtilities.GetTemporaryDirectory();
#pragma warning disable CS0618 // Type or member is obsolete
            var library = store.CreateLibrary(tempDir.UnsafeGetPath());
#pragma warning restore CS0618 // Type or member is obsolete

            var glib = new GameRecordLibrary(optionsBuilder);
            var gl = new GameLibrary(glib);
            var game = gl.CreateGame("NINTENDO_NES");

            store.SetRecordLibrary(library, game.Record);
            Assert.Equal(library.LibraryID, store.GetRecordLibrary(game.Record).LibraryID);

            var library2 = store.CreateLibrary(TestUtilities.GetTemporaryDirectory().UnsafeGetPath());
            store.SetRecordLibrary(library2, game.Record);
            Assert.Equal(library2.LibraryID, store.GetRecordLibrary(game.Record).LibraryID);
        }
    }
}
