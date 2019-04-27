using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Xunit;
using Zio;
using Zio.FileSystems;
using FS = Snowflake.Filesystem;

namespace Snowflake.FileSystem.Tests
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

            Assert.Equal(NormalizePath(dir.GetPath().FullName),
                NormalizePath(Path.Combine(temp, "test")));

            Assert.Equal(NormalizePath(dir.OpenDirectory("dir1").GetPath().FullName),
                NormalizePath(Path.Combine(temp, "test", "dir1")));

            Assert.Equal(NormalizePath(dir.OpenDirectory("test").OpenDirectory("test").GetPath().FullName),
                NormalizePath(Path.Combine(temp, "test", "test", "test")));
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
            Assert.Equal(file.FileGuid, dir.GetGuid("test.txt"));
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
            Assert.Equal(file.FileGuid, dir.GetGuid("test.txt"));
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
            var dir = new FS.Directory("test", pfs, pfs.GetDirectoryEntry("/"));
            var file = dir.OpenFile("test.txt");
            Assert.True(dir.ContainsFile(".manifest"));
            Assert.Equal(file.FileGuid, dir.GetGuid("test.txt"));
            file.OpenStream().Close();
            dir.OpenDirectory("next_test").OpenFile("test2.txt").OpenStream().Close();
            var iter = dir.EnumerateFilesRecursive();
            Assert.True(iter.Count() >= 2);
        }
    }
}
