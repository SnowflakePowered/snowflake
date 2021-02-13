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
        public void Test()
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

            var p = new DirectoryProjection();
            p.N("SomeDirectory")
                .P("project1", file1)
                .N("DeeperDirectory")
                    .P("project2", file2)
                .X()
            .X();


        }
    }
}
