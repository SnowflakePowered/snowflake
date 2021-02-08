using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Zio;
using Snowflake.Filesystem;
using Zio.FileSystems;
using Directory = Snowflake.Filesystem.Directory;

namespace Snowflake.Filesystem.Tests
{
    public class ProjectingDirectoryTests
    {
        [Fact]
        public void DirectoryProjectFromFileInfo_Test()
        {
            var fs = new PhysicalFileSystem();
            var fs2 = new PhysicalFileSystem();

            var temp = Path.GetTempPath();
            var pfs = fs.GetOrCreateSubFileSystem(fs.ConvertPathFromInternal(temp));
            using var dir = new Directory(Path.GetRandomFileName(), pfs, pfs.GetDirectoryEntry("/"), false).AsDisposable();

            var dir2 = new Directory(Path.GetRandomFileName(), pfs, pfs.GetDirectoryEntry("/"), true);

            var tempFile = Path.GetTempFileName();
            var file = dir2.OpenFile(tempFile);
            Assert.False(dir.ContainsFile(".manifest"));
            using (var str = file.OpenStream())
            {
                str.WriteByte(255);
            }// safe the file

            
            var projecting = dir.OpenDirectory("test");
            var file2 = projecting.Project(file);
            Assert.Equal(1, file2.Length);

            Assert.Equal(Guid.Empty, file2.FileGuid);

            Assert.Equal(file.FileGuid, file2.FileGuid);

            Assert.Throws<IOException>(() => dir2.CopyFrom(file));
            Assert.Equal(1, dir2.CopyFrom(file, true).Length);
        }

    }
}
