using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
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


        [Fact]
        public void LinkDelete_Test()
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
            link.Delete();
            Assert.True(new FileInfo(tempFile).Exists);
            using (var str = new FileInfo(tempFile).OpenRead())
            {
                Assert.Equal(255, str.ReadByte());
            }
        }

        [Fact]
        public void LinkOverwritesNonCreatedFile_Test()
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

            var file = dir.OpenFile(tempFile);

            var link = dir.LinkFrom(new FileInfo(tempFile));
            Assert.IsAssignableFrom<Link>(link);
            using (var str = link.OpenReadStream())
            {
                Assert.Equal(255, str.ReadByte());
            }
            Assert.Equal(file.FileGuid, link.FileGuid);
        }

        [Fact]
        public void LinkOverwritesCreatedFile_Test()
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

            var file = dir.OpenFile(tempFile);
            using (var f = file.OpenStream())
            {
                f.WriteByte(254);
            }

            Assert.Throws<IOException>(() => dir.LinkFrom(new FileInfo(tempFile)));
            var link = dir.LinkFrom(new FileInfo(tempFile), true);
            Assert.IsAssignableFrom<Link>(link);
            using (var str = link.OpenReadStream())
            {
                Assert.Equal(255, str.ReadByte());
            }
            Assert.Equal(file.FileGuid, link.FileGuid);
        }

        [Fact]
        public void FileOverwritesLink_Test()
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

            var link = dir.LinkFrom(new FileInfo(tempFile), true);
            using (var str = link.OpenReadStream())
            {
                Assert.Equal(255, str.ReadByte());
            }

            Assert.Throws<IOException>(() => dir.CopyFrom(new FileInfo(tempFile)));
            var file = dir.CopyFrom(new FileInfo(tempFile), true);
            using (var f = file.OpenStream(FileMode.Create, FileAccess.Write))
            {
                f.WriteByte(254);
            }

            using (var f = file.OpenReadStream())
            using (var s = new FileInfo(tempFile).OpenRead())
            {
                var fbyte = f.ReadByte();
                var abyte = s.ReadByte();
                Assert.NotEqual(fbyte, abyte);
                Assert.Equal(254, fbyte);
                Assert.Equal(255, abyte);
            }
            Assert.Equal(file.FileGuid, link.FileGuid);
        }

        [Fact]
        public async Task FileOverwritesLinkAsync_Test()
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

            var link = dir.LinkFrom(new FileInfo(tempFile), true);
            using (var str = link.OpenReadStream())
            {
                Assert.Equal(255, str.ReadByte());
            }

            Assert.Throws<IOException>(() => dir.CopyFrom(new FileInfo(tempFile)));
            var file = await dir.CopyFromAsync(new FileInfo(tempFile), true);
            using (var f = file.OpenStream(FileMode.Create, FileAccess.Write))
            {
                f.WriteByte(254);
            }

            using (var f = file.OpenReadStream())
            using (var s = new FileInfo(tempFile).OpenRead())
            {
                var fbyte = f.ReadByte();
                var abyte = s.ReadByte();
                Assert.NotEqual(fbyte, abyte);
                Assert.Equal(254, fbyte);
                Assert.Equal(255, abyte);
            }
            Assert.Equal(file.FileGuid, link.FileGuid);
        }

        [Fact]
        public void LinkRename_Test()
        {
            var fs = new PhysicalFileSystem();

            var temp = Path.GetTempPath();
            var pfs = fs.GetOrCreateSubFileSystem(fs.ConvertPathFromInternal(temp));
            var dir = new FS.Directory("test", pfs, pfs.GetDirectoryEntry("/"));

            var tempFile = Path.GetTempFileName();
            var tempFile2 = Path.GetTempFileName();

            using (var str = new FileInfo(tempFile).OpenWrite())
            {
                str.WriteByte(255);
            }

            var link = dir.LinkFrom(new FileInfo(tempFile));

            Guid oldGuid = link.FileGuid;
            Assert.True(dir.ContainsFile(".manifest"));
            

            link.Rename(tempFile2);
            Assert.Equal(Path.GetFileName(tempFile2), link.Name);
            Assert.Equal(link.FileGuid, dir.OpenFile(tempFile2).FileGuid);
            Assert.True(new FileInfo(tempFile).Exists);
        }

    }
}