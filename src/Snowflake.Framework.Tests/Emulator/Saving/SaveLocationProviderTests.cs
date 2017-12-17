using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Snowflake.Records.Game;
using Snowflake.Services;
using Xunit;

namespace Snowflake.Emulator.Saving
{
    public class SaveLocationProviderTests
    {
        [Fact]
        public async Task CreateLocation_Test()
        {
            var gameMock = new Mock<IGameRecord>();
            gameMock.Setup(g => g.Guid).Returns(Guid.NewGuid());
            var contentDirMock = new Mock<IContentDirectoryProvider>();
            contentDirMock.Setup(g => g.ApplicationData).Returns(new DirectoryInfo(Path.GetTempPath())
                .CreateSubdirectory(Path.GetFileNameWithoutExtension(Path.GetTempFileName())));

            var location = new SaveLocationProvider(contentDirMock.Object);
            var save = await location.CreateSaveLocationAsync(gameMock.Object, "test");
            Assert.True(File.Exists(Path.Combine(save.LocationRoot.FullName, SaveLocationProvider.ManifestFileName)));
        }

        [Fact]
        public async Task SerializeLocation_Test()
        {
            var gameMock = new Mock<IGameRecord>();
            gameMock.Setup(g => g.Guid).Returns(Guid.NewGuid());
            var contentDirMock = new Mock<IContentDirectoryProvider>();
            contentDirMock.Setup(g => g.ApplicationData).Returns(new DirectoryInfo(Path.GetTempPath())
                         .CreateSubdirectory(Path.GetFileNameWithoutExtension(Path.GetTempFileName())));
            var location = new SaveLocationProvider(contentDirMock.Object);
            var save = await location.CreateSaveLocationAsync(gameMock.Object, "test");
            Assert.True(File.Exists(Path.Combine(save.LocationRoot.FullName, SaveLocationProvider.ManifestFileName)));

            var retrieved = await location.GetSaveLocationAsync(save.LocationGuid);
            Assert.Equal(save.LocationGuid, retrieved.LocationGuid);
        }

        [Fact]
        public async Task EnumeratorLocation_Test()
        {
            var gameMock = new Mock<IGameRecord>();
            gameMock.Setup(g => g.Guid).Returns(Guid.NewGuid());
            var contentDirMock = new Mock<IContentDirectoryProvider>();
            contentDirMock.Setup(g => g.ApplicationData).Returns(new DirectoryInfo(Path.GetTempPath())
                         .CreateSubdirectory(Path.GetFileNameWithoutExtension(Path.GetTempFileName())));
            var location = new SaveLocationProvider(contentDirMock.Object);
            var save = await location.CreateSaveLocationAsync(gameMock.Object, "test");
            Assert.True(File.Exists(Path.Combine(save.LocationRoot.FullName, SaveLocationProvider.ManifestFileName)));

            Assert.Single(await location.GetAllSaveLocationsAsync());
            Assert.Single(await location.GetSaveLocationsAsync(gameMock.Object));

        }

        [Fact]
        public async Task LoadLocation_Test()
        {
            var gameMock = new Mock<IGameRecord>();
            gameMock.Setup(g => g.Guid).Returns(Guid.NewGuid());
            var contentDirMock = new Mock<IContentDirectoryProvider>();
            contentDirMock.Setup(g => g.ApplicationData).Returns(new DirectoryInfo(Path.GetTempPath())
                         .CreateSubdirectory(Path.GetFileNameWithoutExtension(Path.GetTempFileName())));
            var location = new SaveLocationProvider(contentDirMock.Object);
            var save = await location.CreateSaveLocationAsync(gameMock.Object, "test");
            File.Create(Path.Combine(save.LocationRoot.FullName, "TestSave")).Close();
            var loadlocation = new DirectoryInfo(Path.GetTempPath())
                .CreateSubdirectory(Path.GetFileNameWithoutExtension(Path.GetTempFileName()));
            save.RetrieveTo(loadlocation);
            Assert.True(File.Exists(Path.Combine(loadlocation.FullName, "TestSave")));
        }

        [Fact]
        public async Task PersistLocation_Test()
        {
            var gameMock = new Mock<IGameRecord>();
            gameMock.Setup(g => g.Guid).Returns(Guid.NewGuid());
            var contentDirMock = new Mock<IContentDirectoryProvider>();
            contentDirMock.Setup(g => g.ApplicationData).Returns(new DirectoryInfo(Path.GetTempPath())
                         .CreateSubdirectory(Path.GetFileNameWithoutExtension(Path.GetTempFileName())));
            var location = new SaveLocationProvider(contentDirMock.Object);
            var save = await location.CreateSaveLocationAsync(gameMock.Object, "test");
            var loadlocation = new DirectoryInfo(Path.GetTempPath())
                .CreateSubdirectory(Path.GetFileNameWithoutExtension(Path.GetTempFileName()));
            File.Create(Path.Combine(loadlocation.FullName, "TestSave")).Close();

            save.PersistFrom(loadlocation);
            Assert.True(File.Exists(Path.Combine(save.LocationRoot.FullName, "TestSave")));
        }
    }
}
