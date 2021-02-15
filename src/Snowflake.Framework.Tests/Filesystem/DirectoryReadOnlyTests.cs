using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Zio;
using Zio.FileSystems;

namespace Snowflake.Filesystem
{
    public class DirectoryReadOnlyTests
    {
        [Fact]
        public void DirectoryReopenAsReadOnly_Test()
        {
            var fs = new PhysicalFileSystem();
            var temp = Path.GetTempPath();
            var pfs = fs.GetOrCreateSubFileSystem(fs.ConvertPathFromInternal(temp));
            string test = Path.GetRandomFileName();

            IDirectory dir = new Directory(test, pfs, pfs.GetDirectoryEntry("/"));
            var file = dir.OpenDirectory("TestInnerDirectory").OpenFile("test.txt");
            file.OpenStream().Close();
            Assert.Single(dir.AsReadOnly().EnumerateFilesRecursive());
            Assert.Throws<FileNotFoundException>(() => dir.AsReadOnly().OpenFile("NonExistentFile"));
        }
    }
}
