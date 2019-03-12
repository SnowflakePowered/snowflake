using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Filesystem;
using Snowflake.Installation.Tasks;
using Snowflake.Tests;
using Xunit;
using Zio;
using Zio.FileSystems;
using FS = Snowflake.Filesystem;

namespace Snowflake.Installation.Tests
{
    public class CopyFileTaskTests
    {
        [Fact]
        public async Task CopyFile_Test()
        {
            var fs = new PhysicalFileSystem();
            var temp = Path.GetTempPath();
            var pfs = fs.GetOrCreateSubFileSystem(fs.ConvertPathFromInternal(temp));
            var dir = new FS.Directory("test", pfs, pfs.GetDirectoryEntry("/"));
            var temp2 = Path.GetTempFileName();

            // Execute the results.
            await foreach (var res in EmitCopyResult(temp2, dir));

            Assert.True(dir.ContainsFile(Path.GetFileName(temp2)));
        }

        [Fact]
        public async Task CopyDirectory_Test()
        {
            var fs = new PhysicalFileSystem();
            var temp = Path.GetTempPath();
            var pfs = fs.GetOrCreateSubFileSystem(fs.ConvertPathFromInternal(temp));
            var dir = new FS.Directory("test", pfs, pfs.GetDirectoryEntry("/"));

            var dirToCopy = new DirectoryInfo(temp).CreateSubdirectory(Path.GetRandomFileName());
            var subDirToCopy = dirToCopy.CreateSubdirectory(Path.GetRandomFileName());
            var file = System.IO.File.Create(Path.Combine(subDirToCopy.FullName, Path.GetRandomFileName()));
            var fileName = Path.GetFileName(file.Name);
            file.Close();


            var subSubDirToCopy = subDirToCopy
                .CreateSubdirectory(Path.GetRandomFileName());
            
            // Execute the results.
            await foreach (var res in EmitCopyDirResult(dirToCopy, dir)) ;

            Assert.True(dir.ContainsDirectory(subDirToCopy.Name), "Did not copy parent successfully");
            Assert.True(dir.OpenDirectory(subDirToCopy.Name).ContainsFile(fileName), "Did not copy file successfully");
            Assert.True(dir.OpenDirectory(subDirToCopy.Name).ContainsDirectory(subSubDirToCopy.Name), "Did not copy nested folder successfully");
        }


        public async IAsyncEnumerable<TaskResult> EmitCopyResult(string tempFile, IDirectory dir)
        {
            yield return await new CopyFileTask(new FileInfo(tempFile), dir);
        }

        public async IAsyncEnumerable<TaskResult> EmitCopyDirResult(DirectoryInfo tempdir, IDirectory dir)
        {
            await foreach (var tr in new CopyDirectoryContentsTask(tempdir, dir)) {
                yield return tr;
            }
        }

    }
}
