using Snowflake.Execution.Saving;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;
using Zio;
using Zio.FileSystems;
using FS = Snowflake.Filesystem;

namespace Snowflake.Emulator.Execution.Saving
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
            manager.CreateSave("sram");
            // todo: confirm
        }
    }
}
