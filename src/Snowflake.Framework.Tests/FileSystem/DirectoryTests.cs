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
    public class DirectoryTests
    {
        static string NormalizePath(string path)
        {
            return Path.GetFullPath(new Uri(path).LocalPath)
                .TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
                .ToLowerInvariant();
        }

        [Fact]
        public void DirectoryPath_Impl_Test()
        {
            var fs = new PhysicalFileSystem();
            var temp = Path.GetTempPath();
            var pfs = fs.GetOrCreateSubFileSystem(fs.ConvertPathFromInternal(temp));
            Assert.Equal(NormalizePath(pfs.ConvertPathToInternal("/")),
                NormalizePath(temp));

            var pfs_sub = pfs.GetOrCreateSubFileSystem("/test");
            Assert.Equal(NormalizePath(pfs_sub.ConvertPathToInternal("/")),
                NormalizePath(Path.Combine(temp, "test")));

            var pfs_sub_sub = pfs_sub.GetOrCreateSubFileSystem("/test");
            Assert.Equal(NormalizePath(pfs_sub_sub.ConvertPathToInternal("/")),
                NormalizePath(Path.Combine(temp, "test", "test")));
        }

        [Fact]
        public void DirectoryPath_Test()
        {
            var fs = new PhysicalFileSystem();
            var temp = Path.GetTempPath();
            var pfs = fs.GetOrCreateSubFileSystem(fs.ConvertPathFromInternal(temp));
            var dir = new FS.Directory("test", pfs, pfs.GetDirectoryEntry("/"));

            Assert.Equal(NormalizePath(dir.UnsafeGetPath().FullName), // lgtm [cs/call-to-obsolete-method]
                NormalizePath(Path.Combine(temp, "test")));

#pragma warning disable CS0618 // Type or member is obsolete
            Assert.Equal(NormalizePath(dir.OpenDirectory("dir1").UnsafeGetPath().FullName), // lgtm [cs/call-to-obsolete-method]
                NormalizePath(Path.Combine(temp, "test", "dir1")));

            Assert.Equal(NormalizePath(dir.OpenDirectory("test").OpenDirectory("test").UnsafeGetPath().FullName), // lgtm [cs/call-to-obsolete-method]
                NormalizePath(Path.Combine(temp, "test", "test", "test")));
#pragma warning restore CS0618 // Type or member is obsolete

        }

        [Fact]
        public void DirectoryDeepCreatePath_Test()
        {
            var fs = new PhysicalFileSystem();
            var temp = Path.GetTempPath();
            var pfs = fs.GetOrCreateSubFileSystem(fs.ConvertPathFromInternal(temp));
            var dir = new FS.Directory("test", pfs, pfs.GetDirectoryEntry("/"));

            var deep = dir.OpenDirectory("test/test");
#pragma warning disable CS0618 // Type or member is obsolete
            Assert.Equal(NormalizePath(deep.UnsafeGetPath().FullName), // lgtm [cs/call-to-obsolete-method]
               NormalizePath(Path.Combine(temp, "test", "test", "test")));
#pragma warning restore CS0618 // Type or member is obsolete

        }


        [Fact]
        public void DirectoryManifestCreated_Test()
        {
            var fs = new PhysicalFileSystem();
            var temp = Path.GetTempPath();
            var pfs = fs.GetOrCreateSubFileSystem(fs.ConvertPathFromInternal(temp));
            var dir = new FS.Directory("test", pfs, pfs.GetDirectoryEntry("/"));
            dir.OpenFile("test.txt");
            Assert.True(dir.ContainsFile(".manifest"));
        }

        [Fact]
        public void DirectoryManifestPersist_Test()
        {
            var fs = new PhysicalFileSystem();
            var temp = Path.GetTempPath();
            var pfs = fs.GetOrCreateSubFileSystem(fs.ConvertPathFromInternal(temp));
            var dir = new FS.Directory("test", pfs, pfs.GetDirectoryEntry("/"));
            var file = dir.OpenFile("test.txt");
            Assert.True(dir.ContainsFile(".manifest"));
            Assert.Equal(file.FileGuid, dir.RetrieveManifestRecord(dir.ThisDirectory.Path / Path.GetFileName("test.txt"), false).guid);
        }

        [Fact]
        public void DirectoryManifestRemove_Test()
        {
            var fs = new PhysicalFileSystem();
            var temp = Path.GetTempPath();
            var pfs = fs.GetOrCreateSubFileSystem(fs.ConvertPathFromInternal(temp));
            var dir = new FS.Directory("test", pfs, pfs.GetDirectoryEntry("/"));
            var file = dir.OpenFile("test.txt");
            Assert.True(dir.ContainsFile(".manifest"));
            Assert.Equal(file.FileGuid, dir.RetrieveManifestRecord(dir.ThisDirectory.Path / Path.GetFileName("test.txt"), false).guid);
            file.Delete();
            var newFile = dir.OpenFile("test.txt");
            Assert.NotEqual(newFile.FileGuid, file.FileGuid);
            //  Assert.True(dir.Manifest.ContainsKey("test.txt"));
        }

        [Fact]
        public void DirectoryRecursiveFileOpen_Test()
        {
            var fs = new PhysicalFileSystem();
            var temp = Path.GetTempPath();
            var pfs = fs.GetOrCreateSubFileSystem(fs.ConvertPathFromInternal(temp));
            var dir = new FS.Directory("testrr", pfs, pfs.GetDirectoryEntry("/"));
            var file = dir.OpenFile("test.txt");
            Assert.True(dir.ContainsFile(".manifest"));
            Assert.Equal(file.FileGuid, dir.RetrieveManifestRecord(dir.ThisDirectory.Path / Path.GetFileName("test.txt"), false).guid);
            file.OpenStream().Close();
            dir.OpenDirectory("next_test").OpenFile("test2.txt").OpenStream().Close();
            var iter = dir.EnumerateFilesRecursive();
            Assert.True(iter.Count() >= 2);
        }


        [Fact]
        public void DirectoryCopyFromFileInfo_Test()
        {
            var fs = new PhysicalFileSystem();
            var temp = Path.GetTempPath();
            var pfs = fs.GetOrCreateSubFileSystem(fs.ConvertPathFromInternal(temp));
            var dir = new FS.Directory("test", pfs, pfs.GetDirectoryEntry("/"));

            var tempFile = Path.GetTempFileName();
            var file = dir.OpenFile(tempFile);
            Assert.True(dir.ContainsFile(".manifest"));
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
            var dir = new FS.Directory("test", pfs, pfs.GetDirectoryEntry("/"));
            
            var dir2 = new FS.Directory("test2", pfs, pfs.GetDirectoryEntry("/"));

            var tempFile = Path.GetTempFileName();
            var file = dir.OpenFile(tempFile);
            Assert.True(dir.ContainsFile(".manifest"));
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
            var dir = new FS.Directory("test", pfs, pfs.GetDirectoryEntry("/"));

            var dir2 = new FS.Directory("test2", pfs, pfs.GetDirectoryEntry("/"));

            var tempFile = Path.GetTempFileName();
            var file = dir.OpenFile(tempFile);
            Assert.True(dir.ContainsFile(".manifest"));
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
        public void DirectoryMoveFromManaged_Test()
        {
            var fs = new PhysicalFileSystem();
            var fs2 = new PhysicalFileSystem();

            var temp = Path.GetTempPath();
            var pfs = fs.GetOrCreateSubFileSystem(fs.ConvertPathFromInternal(temp));
            var dir = new FS.Directory("test", pfs, pfs.GetDirectoryEntry("/"));

            var dir2 = new FS.Directory("test2", pfs, pfs.GetDirectoryEntry("/"));

            var tempFile = Path.GetTempFileName();
            var file = dir.OpenFile(tempFile);
            Guid oldGuid = file.FileGuid;
            Assert.True(dir.ContainsFile(".manifest"));
            using (var str = file.OpenStream())
            {
                str.WriteByte(255);
            }// safe the file

            var file2 = dir2.CopyFrom(file);

            Assert.Throws<IOException>(() => dir2.MoveFrom(file));
            Assert.Equal(1, dir2.MoveFrom(file, true).Length);

            Assert.Equal(oldGuid, file2.FileGuid);
            Assert.Equal(1, file2.Length);

            Assert.False(file.Created);
            Assert.False(dir.ContainsFile(file.Name));
        }

        [Fact]
        public void FileRename_Test()
        {
            var fs = new PhysicalFileSystem();

            var temp = Path.GetTempPath();
            var pfs = fs.GetOrCreateSubFileSystem(fs.ConvertPathFromInternal(temp));
            var dir = new FS.Directory("test", pfs, pfs.GetDirectoryEntry("/"));

            var tempFile = Path.GetTempFileName();
            var tempFile2 = Path.GetTempFileName();

            var file = dir.OpenFile(tempFile);
            Guid oldGuid = file.FileGuid;
            Assert.True(dir.ContainsFile(".manifest"));
            using (var str = file.OpenStream())
            {
                str.WriteByte(255);
            }// safe the file

            file.Rename(tempFile2);
            Assert.Equal(Path.GetFileName(tempFile2), file.Name);
            Assert.Equal(file.FileGuid, dir.OpenFile(tempFile2).FileGuid);
        }

        [Fact]
        public void DirectoryDelete_Test()
        {
            var fs = new PhysicalFileSystem();

            var temp = Path.GetTempPath();
            var pfs = fs.GetOrCreateSubFileSystem(fs.ConvertPathFromInternal(temp));
            var dir = new FS.Directory("test", pfs, pfs.GetDirectoryEntry("/"));
            var nested = dir.OpenDirectory("nested");
            var nested2 = nested.OpenDirectory("nested2");
            nested.Delete();
            
            Assert.Throws<InvalidOperationException>(() => nested.EnumerateFiles());
            Assert.Throws<InvalidOperationException>(() => nested2.EnumerateFiles());
        }
    }
}
