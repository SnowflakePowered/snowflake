using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Filesystem;
using Snowflake.Installation;
using Snowflake.Installation.Tasks;
using Snowflake.Tests;
using Xunit;
using Zio;
using Zio.FileSystems;
using FS = Snowflake.Filesystem;

namespace Snowflake.Extensibility.Queueing.Tests
{
    public class AsyncJobQueueCopyFileTaskIntegrationTests
    {
        public static byte[] Data { get; }
        static AsyncJobQueueCopyFileTaskIntegrationTests() {
            Data = new byte[1 * 1024 * 1024];
            Random rng = new Random();
            rng.NextBytes(Data);
        }

        [Fact]
        public async Task CopyFileJobQueue_Test()
        {
            var fs = new PhysicalFileSystem();
            var temp = Path.GetTempPath();
            var pfs = fs.GetOrCreateSubFileSystem(fs.ConvertPathFromInternal(temp));
            var dir = new FS.Directory("test", pfs, pfs.GetDirectoryEntry("/"));
            var temp2 = Path.GetTempFileName();
            await System.IO.File.WriteAllBytesAsync(temp2, Data);
            // Execute the results.
            var tq = new AsyncJobQueue<TaskResult<IFile>>();
            var token = await tq.QueueJob(EmitCopyResult(temp2, dir));

            for ((TaskResult<IFile> val, bool next) = await tq.GetNext(token); next; (val, next) = await tq.GetNext(token))
            {

            }

            Assert.True(dir.ContainsFile(Path.GetFileName(temp2)));

            using (var stream = dir.OpenFile(Path.GetFileName(temp2)).OpenStream())
            {
                Assert.Equal(Data.Length, stream.Length);
            }

            dir.OpenFile(Path.GetFileName(temp2)).Delete();
            Assert.False(dir.ContainsFile(Path.GetFileName(temp2)));
        }

        [Fact]
        public async Task CopyDirectoryJobQueue_Test()
        {
            var fs = new PhysicalFileSystem();
            var temp = Path.GetTempPath();
            var pfs = fs.GetOrCreateSubFileSystem(fs.ConvertPathFromInternal(temp));
            var dir = new FS.Directory("test", pfs, pfs.GetDirectoryEntry("/"));

            var dirToCopy = new DirectoryInfo(temp).CreateSubdirectory(Path.GetRandomFileName());
            var subDirToCopy = dirToCopy.CreateSubdirectory(Path.GetRandomFileName());
            string fileName;
            using (var file = System.IO.File.Create(Path.Combine(subDirToCopy.FullName, Path.GetRandomFileName())))
            {
                fileName = Path.GetFileName(file.Name);
                await file.WriteAsync(Data, 0, Data.Length);
                file.Close();
            }

            var subSubDirToCopy = subDirToCopy
                .CreateSubdirectory(Path.GetRandomFileName());

            var tq = new AsyncJobQueue<TaskResult<IFile>>();
            var token = await tq.QueueJob(EmitCopyDirResult(dirToCopy, dir));
           
            await foreach (var x in  tq.AsEnumerable(token)) { }

            Assert.True(dir.ContainsDirectory(subDirToCopy.Name), "Did not copy parent successfully");
            Assert.True(dir.OpenDirectory(subDirToCopy.Name).ContainsFile(fileName), "Did not copy file successfully");

            using (var file = dir.OpenDirectory(subDirToCopy.Name).OpenFile(fileName).OpenStream())
            {
                Assert.Equal(Data.Length, file.Length);
            }

            Assert.True(dir.OpenDirectory(subDirToCopy.Name).ContainsDirectory(subSubDirToCopy.Name), "Did not copy nested folder successfully");

            dir.OpenDirectory(subDirToCopy.Name).OpenFile(fileName).Delete();
            Assert.False(dir.OpenDirectory(subDirToCopy.Name).ContainsFile(fileName), "Did not copy file successfully");

        }

        [Fact]
        public async Task ExtractZipJobQueue_Test()
        {
            var fs = new PhysicalFileSystem();
            var temp = Path.GetTempPath();
            var pfs = fs.GetOrCreateSubFileSystem(fs.ConvertPathFromInternal(temp));
            var dir = new FS.Directory("test", pfs, pfs.GetDirectoryEntry("/"));

            var dirToCopy = new DirectoryInfo(temp).CreateSubdirectory(Path.GetRandomFileName());
            var subDirToCopy = dirToCopy.CreateSubdirectory(Path.GetRandomFileName());
            string fileName;
            using (var file = System.IO.File.Create(Path.Combine(subDirToCopy.FullName, Path.GetRandomFileName())))
            {
                fileName = Path.GetFileName(file.Name);
                await file.WriteAsync(Data, 0, Data.Length);
            }

            var subSubDirToCopy = subDirToCopy
                .CreateSubdirectory(Path.GetRandomFileName());

            string zipFile = Path.GetTempFileName();
            System.IO.File.Delete(zipFile); // hack to get around file existing.
            ZipFile.CreateFromDirectory(dirToCopy.FullName, zipFile, CompressionLevel.Fastest, true);

            var tq = new AsyncJobQueue<TaskResult<IFile>>();
            var token = await tq.QueueJob(EmitZipResult(zipFile, dir));

            await foreach (var res in tq.AsEnumerable(token)) {
                var val = await res;
                if (res.Error != null) throw res.Error.InnerException;
            }


            Assert.True(dir.ContainsDirectory(dirToCopy.Name), "Did not extract parent successfully");
            Assert.True(dir.OpenDirectory(dirToCopy.Name).OpenDirectory(subDirToCopy.Name).ContainsFile(fileName), "Did not copy file successfully");
            using (var file = dir.OpenDirectory(dirToCopy.Name).OpenDirectory(subDirToCopy.Name).OpenFile(fileName).OpenStream()) {
                Assert.Equal(Data.Length, file.Length);
            }
            Assert.True(dir.OpenDirectory(dirToCopy.Name).OpenDirectory(subDirToCopy.Name).ContainsDirectory(subSubDirToCopy.Name), "Did not copy nested folder successfully");

            dir.OpenDirectory(dirToCopy.Name).OpenDirectory(subDirToCopy.Name).OpenFile(fileName).Delete();
            Assert.False(dir.OpenDirectory(dirToCopy.Name).OpenDirectory(subDirToCopy.Name).ContainsFile(fileName), "Did not cleanup file successfully");

            try
            {
                // nothing to do with tests here if this fails 
                System.IO.File.Delete(zipFile); 
            } catch
            {

            }
        }

        public async IAsyncEnumerable<TaskResult<IFile>> EmitZipResult(string tempFile, IDirectory dir)
        {
            await foreach (var tr in new ExtractZipTask(new FileInfo(tempFile), dir))
            {
                yield return tr;
            }
        }

        public async IAsyncEnumerable<TaskResult<IFile>> EmitCopyResult(string tempFile, IDirectory dir)
        {
            yield return await new CopyFileTask(new FileInfo(tempFile), dir);
        }

        public async IAsyncEnumerable<TaskResult<IFile>> EmitCopyDirResult(DirectoryInfo tempdir, IDirectory dir)
        {
            await foreach (var tr in new CopyDirectoryContentsTask(tempdir, dir)) {
                yield return tr;
            }
        }

    }
}
