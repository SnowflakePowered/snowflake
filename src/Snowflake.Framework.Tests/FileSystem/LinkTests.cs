using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;
using Zio;
using Zio.FileSystems;
using FS = Snowflake.Filesystem;

namespace Snowflake.Filesystem.Tests
{
    public class LinkTests
    {

        [Fact]
        public void DirectoryLinkFromFileInfo_Test()
        {
            var fs = new PhysicalFileSystem();
            var temp = Path.GetTempPath();
            var pfs = fs.GetOrCreateSubFileSystem(fs.ConvertPathFromInternal(temp));
            var dir = new FS.Directory("test", pfs, pfs.GetDirectoryEntry("/"));

            var tempFile = Path.GetTempFileName();
            using (var str = new FileInfo(tempFile).OpenWrite())
            {
                str.WriteByte(255);
            }

            var link = dir.LinkFrom(new FileInfo(tempFile));
            using (var str = link.OpenReadStream())
            {
                Assert.Equal(255, str.ReadByte());
            }

            Assert.Throws<IOException>(() => dir.CopyFrom(new FileInfo(tempFile)));
            Assert.True(dir.ContainsFile(Path.GetFileName(tempFile)));
        }

        [Fact]
        public void DirectoryMoveLink_Test()
        {
            var fs = new PhysicalFileSystem();
            var temp = Path.GetTempPath();
            var pfs = fs.GetOrCreateSubFileSystem(fs.ConvertPathFromInternal(temp));
            var dir = new FS.Directory("test", pfs, pfs.GetDirectoryEntry("/"));

            var tempFile = Path.GetTempFileName();
            using (var str = new FileInfo(tempFile).OpenWrite())
            {
                str.WriteByte(255);
            }

            var link = dir.LinkFrom(new FileInfo(tempFile));

            var newLink = dir.OpenDirectory("new").MoveFrom(link);

            using (var str = newLink.OpenReadStream())
            {
                Assert.Equal(255, str.ReadByte());
            }
        }

        [Fact]
        public void DirectoryCopyLink_Test()
        {
            var fs = new PhysicalFileSystem();
            var temp = Path.GetTempPath();
            var pfs = fs.GetOrCreateSubFileSystem(fs.ConvertPathFromInternal(temp));
            var dir = new FS.Directory("test", pfs, pfs.GetDirectoryEntry("/"));

            var tempFile = Path.GetTempFileName();
            using (var str = new FileInfo(tempFile).OpenWrite())
            {
                str.WriteByte(255);
            }

            var link = dir.LinkFrom(new FileInfo(tempFile));

            var newLink = dir.OpenDirectory("new").CopyFrom(link);

            using (var str = newLink.OpenReadStream())
            {
                Assert.Equal(255, str.ReadByte());
            }
        }
    }
}
