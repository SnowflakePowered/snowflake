using Snowflake.Orchestration.Projections;
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

namespace Snowflake.Filesystem
{
    public class DirectoryProjectionTests
    {
        [Fact]
        public void ProjectionSuccess_Test()
        {
            var fs = new PhysicalFileSystem();
            var temp = Path.GetTempPath();
            var pfs = fs.GetOrCreateSubFileSystem(fs.ConvertPathFromInternal(temp));
            string test = Path.GetRandomFileName();
            var dir = new FS.Directory(test, pfs, pfs.GetDirectoryEntry("/"));

            var file1 = dir.OpenFile("MyFirstFile");
            file1.OpenStream().Close();
            var file2 = dir.OpenFile("MySecondFile");
            file2.OpenStream().Close();
            var dirProj = dir.OpenDirectory("MyDirectory");
            dirProj
                .OpenFile("MyThirdFile")
                .OpenStream().Close();

            var p = new DirectoryProjection();
            p.Project("RootFile", file1)
             .Enter("SomeDirectory")
                    .Project("project1", file1)
                    .Enter("DeeperDirectory")
                        .Project("project2", file2)
                        .Project("deeperThree", dirProj)
                    .Exit()
                    .Project("project3", file2)
                .Exit();

            using var mountdir = dir.OpenDirectory("mountPoint")
                .AsDisposable();
            var mountedDirectory = p.Mount(mountdir);
            Assert.True(mountedDirectory.ContainsDirectory("SomeDirectory"));
            Assert.True(mountedDirectory.ContainsFile("RootFile"));
            var someDir = mountedDirectory.OpenDirectory("SomeDirectory");
            Assert.True(someDir.ContainsFile("project1"));
            Assert.True(someDir.ContainsDirectory("DeeperDirectory"));
            Assert.True(someDir.ContainsFile("project3"));

            var deeper = someDir.OpenDirectory("DeeperDirectory");
            Assert.True(deeper.ContainsDirectory("deeperThree"));
            Assert.True(deeper.ContainsFile("project2"));

            var deeperLink = deeper.OpenDirectory("deeperThree");
            Assert.True(deeperLink.ContainsFile("MyThirdFile"));
        }

        [Fact]
        public void DirectoryMustBeEmptyToMount_Test()
        {
            var fs = new PhysicalFileSystem();
            var temp = Path.GetTempPath();
            var pfs = fs.GetOrCreateSubFileSystem(fs.ConvertPathFromInternal(temp));
            string test = Path.GetRandomFileName();
            var dir = new FS.Directory(test, pfs, pfs.GetDirectoryEntry("/"));
            using var mountdir = dir.OpenDirectory("mountPoint")
                .AsDisposable();
            mountdir.OpenDirectory("dummydir");
            Assert.Throws<IOException>(() => new DirectoryProjection().Mount(mountdir));
        }

        [Fact]
        public void DirectoryCanNotMountSubprojection_Test()
        {
            var fs = new PhysicalFileSystem();
            var temp = Path.GetTempPath();
            var pfs = fs.GetOrCreateSubFileSystem(fs.ConvertPathFromInternal(temp));
            string test = Path.GetRandomFileName();
            var dir = new FS.Directory(test, pfs, pfs.GetDirectoryEntry("/"));
            using var mountdir = dir.OpenDirectory("mountPoint")
                .AsDisposable();
            Assert.Throws<InvalidOperationException>(() => new DirectoryProjection().Enter("no").Mount(mountdir));
        }
    }
}
