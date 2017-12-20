using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Snowflake.Platform;
using Snowflake.Services;
using Snowflake.Support.ExecutionSupport;
using Xunit;

namespace Snowflake.Execution.SystemFiles
{
    public class SystemFileProviderTests
    {
        [Fact]
        public void AddSystemFile_Test()
        {
            var contentDirMock = new Mock<IContentDirectoryProvider>();
            contentDirMock.Setup(g => g.ApplicationData).Returns(new DirectoryInfo(Path.GetTempPath())
                         .CreateSubdirectory(Path.GetFileNameWithoutExtension(Path.GetTempFileName())));
            var sfp = new SystemFileProvider(contentDirMock.Object);
            var testStream = new MemoryStream(new byte[] { 0, 1, 2, 3, 4 });
            var biosFile = new BiosFile("testFile", "testHash");
            sfp.AddSystemFile(biosFile, testStream);
            var newStream = sfp.GetSystemFile(biosFile);
            Assert.Equal(testStream.Length, newStream.Length);
        }

        [Fact]
        public async Task AddSystemAsyncFile_Test()
        {
            var contentDirMock = new Mock<IContentDirectoryProvider>();
            contentDirMock.Setup(g => g.ApplicationData).Returns(new DirectoryInfo(Path.GetTempPath())
                         .CreateSubdirectory(Path.GetFileNameWithoutExtension(Path.GetTempFileName())));
            var sfp = new SystemFileProvider(contentDirMock.Object);
            var testStream = new MemoryStream(new byte[] { 0, 1, 2, 3, 4 });
            var biosFile = new BiosFile("testFile", "testHash");
            await sfp.AddSystemFileAsync(biosFile, testStream);
            var newStream = sfp.GetSystemFile(biosFile);
            Assert.Equal(testStream.Length, newStream.Length);
        }

        [Fact]
        public void AddSystemFileByPathFile_Test()
        {
            var contentDirMock = new Mock<IContentDirectoryProvider>();
            contentDirMock.Setup(g => g.ApplicationData).Returns(new DirectoryInfo(Path.GetTempPath())
                         .CreateSubdirectory(Path.GetFileNameWithoutExtension(Path.GetTempFileName())));
            var sfp = new SystemFileProvider(contentDirMock.Object);
            var file = Path.GetTempFileName();
            var biosFile = new BiosFile("testFile", "testHash");
            sfp.AddSystemFile(biosFile, new FileInfo(file));
            var filePath = sfp.GetSystemFilePath(biosFile);
            Assert.Equal($"{biosFile.Md5Hash}.{biosFile.FileName}", Path.GetFileName(filePath.FullName));
        }

        [Fact]
        public void ThrowsUnknownFile_Test()
        {
            var contentDirMock = new Mock<IContentDirectoryProvider>();
            contentDirMock.Setup(g => g.ApplicationData).Returns(new DirectoryInfo(Path.GetTempPath())
                         .CreateSubdirectory(Path.GetFileNameWithoutExtension(Path.GetTempFileName())));
            var sfp = new SystemFileProvider(contentDirMock.Object);
            var file = Path.GetTempFileName();
            var biosFile = new BiosFile("testFile", "testHash");
            Assert.Throws<FileNotFoundException>(() => sfp.GetSystemFilePath(biosFile));
        }

        [Fact]
        public void ContainsFile_Test()
        {
            var contentDirMock = new Mock<IContentDirectoryProvider>();
            contentDirMock.Setup(g => g.ApplicationData).Returns(new DirectoryInfo(Path.GetTempPath())
                         .CreateSubdirectory(Path.GetFileNameWithoutExtension(Path.GetTempFileName())));
            var sfp = new SystemFileProvider(contentDirMock.Object);
            var file = Path.GetTempFileName();
            var biosFile = new BiosFile("testFile", "testHash");
            Assert.False(sfp.ContainsSystemFile(biosFile));
        }
    }
}
