using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Zio;
using Zio.FileSystems;
using FS = Snowflake.Filesystem;

namespace Snowflake.Filesystem.Tests
{
    public class DisposableDirectoryTests
    {
        static string NormalizePath(string path)
        {
            return Path.GetFullPath(new Uri(path).LocalPath)
                .TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
                .ToLowerInvariant();
        }

        [Fact]
        public void DirectoryDeepCreatePath_Test()
        {
            var fs = new PhysicalFileSystem();
            var temp = Path.GetTempPath();
            var pfs = fs.GetOrCreateSubFileSystem(fs.ConvertPathFromInternal(temp));
            string test = Path.GetRandomFileName();
            var dir = new Directory(test, pfs, pfs.GetDirectoryEntry("/"), false);

            var deep = dir.OpenDirectory("test/test");
#pragma warning disable CS0618 // Type or member is obsolete
            Assert.Equal(NormalizePath(deep.UnsafeGetPath().FullName), // lgtm [cs/call-to-obsolete-method]
               NormalizePath(Path.Combine(temp, test, "test", "test")));
#pragma warning restore CS0618 // Type or member is obsolete
        }

        [Fact]
        public void DirectoryChildrenNoManifest_Test()
        {
            var fs = new PhysicalFileSystem();
            var temp = Path.GetTempPath();
            var pfs = fs.GetOrCreateSubFileSystem(fs.ConvertPathFromInternal(temp));
            string test = Path.GetRandomFileName();
            var dir = new Directory(test, pfs, pfs.GetDirectoryEntry("/"), false);

       
            var deep = dir.OpenDirectory("test/test");
            using (var deepDirDispose = deep.AsDisposable())
            {
                Assert.False(deep.ContainsFile(".manifest"));
            }
            Assert.True(dir.ContainsDirectory("test"));
            Assert.False(dir.ContainsDirectory("test/test"));
        }

        [Fact]
        public void NonEmptyDirectoryCanNotBeDisposable_Test()
        {
            var fs = new PhysicalFileSystem();
            var temp = Path.GetTempPath();
            var pfs = fs.GetOrCreateSubFileSystem(fs.ConvertPathFromInternal(temp));
            var dir = new Directory(Path.GetRandomFileName(), pfs, pfs.GetDirectoryEntry("/"), false);
            dir.OpenFile("test.txt").OpenStream().Close();
            Assert.Throws<IOException>(() => dir.AsDisposable());
        }

        [Fact]
        public void DirectoryRecursiveFileOpen_Test()
        {
            var fs = new PhysicalFileSystem();
            var temp = Path.GetTempPath();
            var pfs = fs.GetOrCreateSubFileSystem(fs.ConvertPathFromInternal(temp));
            using var dir = new Directory(Path.GetRandomFileName(), pfs, pfs.GetDirectoryEntry("/"), false).AsDisposable();
            var file = dir.OpenFile("test.txt");
            Assert.False(dir.ContainsFile(".manifest"));
            file.OpenStream().Close();
            dir.OpenFile("test2.txt").OpenStream().Close();
            var iter = dir.EnumerateFilesRecursive();
            Assert.True(iter.Count() >= 2);
        }


        [Fact]
        public void DirectoryCopyFromFileInfo_Test()
        {
            var fs = new PhysicalFileSystem();
            var temp = Path.GetTempPath();
            var pfs = fs.GetOrCreateSubFileSystem(fs.ConvertPathFromInternal(temp));
            using var dir = new Directory(Path.GetRandomFileName(), pfs, pfs.GetDirectoryEntry("/"), false).AsDisposable();

            var tempFile = Path.GetTempFileName();
            var file = dir.OpenFile(tempFile);
            Assert.False(dir.ContainsFile(".manifest"));
            using (var str = file.OpenStream()) {
                str.WriteByte(255);
            }// safe the file

            Assert.Throws<IOException>(() => dir.CopyFrom(new FileInfo(tempFile)));
            Assert.Equal(0, dir.CopyFrom(new FileInfo(tempFile), true).Length);
        }

        [Fact]
        public void DirectoryCopyFromManaged_Test()
        {
            var fs = new PhysicalFileSystem();
            var fs2 = new PhysicalFileSystem();

            var temp = Path.GetTempPath();
            var pfs = fs.GetOrCreateSubFileSystem(fs.ConvertPathFromInternal(temp));
            using var dir = new Directory(Path.GetRandomFileName(), pfs, pfs.GetDirectoryEntry("/"), false).AsDisposable();
            
            var dir2 = new Directory(Path.GetRandomFileName(), pfs, pfs.GetDirectoryEntry("/"), false);

            var tempFile = Path.GetTempFileName();
            var file = dir.OpenFile(tempFile);
            Assert.False(dir.ContainsFile(".manifest"));
            using (var str = file.OpenStream())
            {
                str.WriteByte(255);
            }// safe the file

            var file2 = dir2.CopyFrom(file);

            Assert.Equal(file.FileGuid, file2.FileGuid);
            Assert.Equal(1, file2.Length);

            Assert.Throws<IOException>(() => dir2.CopyFrom(file));
            Assert.Equal(1, dir2.CopyFrom(file, true).Length);
        }

        [Fact]
        public async Task DirectoryCopyFromAsyncManaged_Test()
        {
            var fs = new PhysicalFileSystem();
            var fs2 = new PhysicalFileSystem();

            var temp = Path.GetTempPath();
            var pfs = fs.GetOrCreateSubFileSystem(fs.ConvertPathFromInternal(temp));
            using var dir = new Directory(Path.GetRandomFileName(), pfs, pfs.GetDirectoryEntry("/"), false).AsDisposable();

            var dir2 = new Directory(Path.GetRandomFileName(), pfs, pfs.GetDirectoryEntry("/"), false);

            var tempFile = Path.GetTempFileName();
            var file = dir.OpenFile(tempFile);
            Assert.False(dir.ContainsFile(".manifest"));
            using (var str = file.OpenStream())
            {
                str.WriteByte(255);
            }// safe the file
            
            var file2 = await dir2.CopyFromAsync(file);

            Assert.Equal(file.FileGuid, file2.FileGuid);
            Assert.True(file.Created);
            Assert.Equal(1, file2.Length);

            await Assert.ThrowsAsync<IOException>(async () => await dir2.CopyFromAsync(file));
            Assert.Equal(1, (await dir2.CopyFromAsync(file, true)).Length);
        }

        [Fact]
        public void FileRename_Test()
        {
            var fs = new PhysicalFileSystem();

            var temp = Path.GetTempPath();
            var pfs = fs.GetOrCreateSubFileSystem(fs.ConvertPathFromInternal(temp));
            using var dir = new Directory(Path.GetRandomFileName(), pfs, pfs.GetDirectoryEntry("/"), false).AsDisposable();

            var tempFile = Path.GetTempFileName();
            var tempFile2 = Path.GetTempFileName();

            var file = dir.OpenFile(tempFile);
            Guid oldGuid = file.FileGuid;
            Assert.False(dir.ContainsFile(".manifest"));
            using (var str = file.OpenStream())
            {
                str.WriteByte(255);
            }// safe the file

            file.Rename(tempFile2);
            Assert.Equal(Path.GetFileName(tempFile2), file.Name);
            Assert.Equal(file.FileGuid, dir.OpenFile(tempFile2).FileGuid);
        }
    }
}
