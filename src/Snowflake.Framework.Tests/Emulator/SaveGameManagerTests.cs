using Snowflake.Orchestration.Saving;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Zio;
using Zio.FileSystems;
using FS = Snowflake.Filesystem;

namespace Snowflake.Orchestration.Saving.Tests
{
    public class SaveGameManagerTests
    {
        [Fact]
        public void CreateSave_Test()
        {
            var fs = new PhysicalFileSystem();
            var temp = Path.GetTempPath();
            var pfs = fs.GetOrCreateSubFileSystem(fs.ConvertPathFromInternal(temp));
            var dir = new FS.Directory("test", pfs, pfs.GetDirectoryEntry("/"));

            var manager =  new SaveGameManager(dir);
            var save = manager.CreateSave("test", directory => directory.OpenFile("test.save").OpenStream().Close());
            var retrievedSave = manager.GetSave(save.Guid);
            Assert.NotNull(retrievedSave);
            Assert.Equal(save.Guid, retrievedSave.Guid);
            Assert.NotEmpty(retrievedSave.SaveContents.EnumerateFiles());
            // todo: confirm
        }

        [Fact]
        public async Task RetrieveLatestSave_Test()
        {
            var fs = new PhysicalFileSystem();
            var temp = Path.GetTempPath();
            var pfs = fs.GetOrCreateSubFileSystem(fs.ConvertPathFromInternal(temp));
            var dir = new FS.Directory("test", pfs, pfs.GetDirectoryEntry("/"));

            var manager = new SaveGameManager(dir);
            var oldSave = manager.CreateSave("test", directory => directory.OpenFile("test.save").OpenStream().Close());

            await Task.Delay(2000);

            var newSave = manager.CreateSave("test", directory => directory.OpenFile("test.save").OpenStream().Close());
            Assert.Equal(newSave.Guid, manager.GetLatestSave("test").Guid);
        }
    }
}
