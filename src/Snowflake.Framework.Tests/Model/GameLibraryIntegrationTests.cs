using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Snowflake.Configuration.Tests;
using Snowflake.Model.Database;
using Snowflake.Model.Database.Models;
using Snowflake.Model.Game;
using Snowflake.Model.Game.LibraryExtensions;
using Xunit;
using Zio;
using Zio.FileSystems;

namespace Snowflake.Model.Tests
{
    public class GameLibraryIntegrationTests
    {
        [Fact]
        public async Task GameLibraryIntegrationCreate_Test()
        {
            var path = new DirectoryInfo(Path.GetTempPath())
                .CreateSubdirectory(Path.GetFileNameWithoutExtension(Path.GetTempFileName()));
            var fs = new PhysicalFileSystem();
            var gfs = fs.GetOrCreateSubFileSystem(fs.ConvertPathFromInternal(path.FullName));

            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            optionsBuilder.UseSqlite($"Data Source={Path.GetTempFileName()}");
            var glib = new GameRecordLibrary(optionsBuilder);
            var gl = new GameLibrary(glib);
            var game = gl.CreateGame("NINTENDO_NES");
        }

        [Fact]
        public async Task GameLibraryIntegrationConfig_Test()
        {
            var path = new DirectoryInfo(Path.GetTempPath())
                .CreateSubdirectory(Path.GetFileNameWithoutExtension(Path.GetTempFileName()));
            var fs = new PhysicalFileSystem();
            var gfs = fs.GetOrCreateSubFileSystem(fs.ConvertPathFromInternal(path.FullName));

            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            optionsBuilder.UseSqlite($"Data Source={Path.GetTempFileName()}");
            var glib = new GameRecordLibrary(optionsBuilder);
            var ccs = new ConfigurationCollectionStore(optionsBuilder);

            var gl = new GameLibrary(glib);
            gl.AddExtension<IGameConfigurationExtensionProvider, IGameConfigurationExtension>
                (new GameConfigurationExtensionProvider(ccs));
            var game = gl.CreateGame("NINTENDO_NES");

            var profile = game.WithConfigurations()
                .CreateNewProfile<ExampleConfigurationCollection>("TestConfiguration", "test");

            Assert.NotNull(game.WithConfigurations()
                .GetProfile<ExampleConfigurationCollection>("TestConfiguration", "test"));

            Assert.NotEmpty(game.WithConfigurations().GetProfileNames());

            profile.Configuration.ExampleConfiguration.FullscreenResolution =
                Configuration.FullscreenResolution.Resolution1600X1050;
            gl.WithConfigurationLibrary().UpdateProfile(profile);
            var newProfile = game.WithConfigurations()
                .GetProfile<ExampleConfigurationCollection>("TestConfiguration", "test");
            Assert.Equal(Configuration.FullscreenResolution.Resolution1600X1050,
                newProfile.Configuration.ExampleConfiguration.FullscreenResolution);

            Assert.ThrowsAny<Exception>(() => game.WithConfigurations()
                .CreateNewProfile<ExampleConfigurationCollection>("TestConfiguration", "test"));

            game.WithConfigurations().DeleteProfile("TestConfiguration", "test");
            Assert.Empty(game.WithConfigurations().GetProfileNames());
        }

        [Fact]
        public async Task GameLibraryUnknownExtension_Test()
        {
            var path = new DirectoryInfo(Path.GetTempPath())
                .CreateSubdirectory(Path.GetFileNameWithoutExtension(Path.GetTempFileName()));
            var fs = new PhysicalFileSystem();
            var gfs = fs.GetOrCreateSubFileSystem(fs.ConvertPathFromInternal(path.FullName));

            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            optionsBuilder.UseSqlite($"Data Source={Path.GetTempFileName()}");
            var glib = new GameRecordLibrary(optionsBuilder);
            var ccs = new ConfigurationCollectionStore(optionsBuilder);

            var gl = new GameLibrary(glib);
            Assert.Throws<KeyNotFoundException>(() => gl.GetExtension<IGameConfigurationExtensionProvider>());
            var game = gl.CreateGame("NINTENDO_NES");
            Assert.Throws<KeyNotFoundException>(() => game.GetExtension<IGameConfigurationExtension>());
        }

        [Fact]
        public async Task GameLibraryIntegrationUpdate_Test()
        {
            var path = new DirectoryInfo(Path.GetTempPath())
                .CreateSubdirectory(Path.GetFileNameWithoutExtension(Path.GetTempFileName()));
            var fs = new PhysicalFileSystem();
            var gfs = fs.GetOrCreateSubFileSystem(fs.ConvertPathFromInternal(path.FullName));

            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            optionsBuilder.UseSqlite($"Data Source={Path.GetTempFileName()}");
            var glib = new GameRecordLibrary(optionsBuilder);
            var flib = new FileRecordLibrary(optionsBuilder);

            var gl = new GameLibrary(glib);
            gl.AddExtension<GameFileExtensionProvider, IGameFileExtension
            >(new GameFileExtensionProvider(flib, gfs));

            var game = gl.CreateGame("NINTENDO_NES");
            game.Record.Title = "My Awesome Game";
            gl.UpdateGameRecord(game.Record);

            var file = game.WithFiles().MiscRoot.OpenFile("Test.txt");
            file.OpenStream().Close();
            game.WithFiles().RegisterFile(file, "application/text");
            var record = game.WithFiles().GetFileInfo(file);
            record.Metadata.Add("file_metadata", "test");
            gl.GetExtension<GameFileExtensionProvider>().UpdateFile(record);

            var newGame = gl.GetGame(game.Record.RecordID);
            Assert.NotEmpty(newGame.WithFiles().FileRecords);
        }


        [Fact]
        public async Task GameLibraryIntegrationCreateAsync_Test()
        {
            var path = new DirectoryInfo(Path.GetTempPath())
                .CreateSubdirectory(Path.GetFileNameWithoutExtension(Path.GetTempFileName()));
            var fs = new PhysicalFileSystem();
            var gfs = fs.GetOrCreateSubFileSystem(fs.ConvertPathFromInternal(path.FullName));

            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            optionsBuilder.UseSqlite($"Data Source={Path.GetTempFileName()}");
            var glib = new GameRecordLibrary(optionsBuilder);
            var gl = new GameLibrary(glib);
            var game = gl.CreateGame("NINTENDO_NES");
        }

        [Fact]
        public async Task GameLibraryIntegrationConfigAsync_Test()
        {
            var path = new DirectoryInfo(Path.GetTempPath())
                .CreateSubdirectory(Path.GetFileNameWithoutExtension(Path.GetTempFileName()));
            var fs = new PhysicalFileSystem();
            var gfs = fs.GetOrCreateSubFileSystem(fs.ConvertPathFromInternal(path.FullName));

            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            optionsBuilder.UseSqlite($"Data Source={Path.GetTempFileName()}");
            var glib = new GameRecordLibrary(optionsBuilder);
            var ccs = new ConfigurationCollectionStore(optionsBuilder);

            var gl = new GameLibrary(glib);
            gl.AddExtension<IGameConfigurationExtensionProvider, IGameConfigurationExtension>
                (new GameConfigurationExtensionProvider(ccs));
            var game = gl.CreateGame("NINTENDO_NES");

            var profile = await game.WithConfigurations()
                .CreateNewProfileAsync<ExampleConfigurationCollection>("TestConfiguration", "test");

            Assert.NotNull(await game.WithConfigurations()
                .GetProfileAsync<ExampleConfigurationCollection>("TestConfiguration", "test"));

            Assert.NotEmpty(game.WithConfigurations().GetProfileNames());

            profile.Configuration.ExampleConfiguration.FullscreenResolution =
                Configuration.FullscreenResolution.Resolution1600X1050;
            await gl.WithConfigurationLibrary().UpdateProfileAsync(profile);
            var newProfile = await game.WithConfigurations()
                .GetProfileAsync<ExampleConfigurationCollection>("TestConfiguration", "test");
            Assert.Equal(Configuration.FullscreenResolution.Resolution1600X1050,
                newProfile.Configuration.ExampleConfiguration.FullscreenResolution);

            await Assert.ThrowsAnyAsync<Exception>(async () => await game.WithConfigurations()
                .CreateNewProfileAsync<ExampleConfigurationCollection>("TestConfiguration", "test"));

            await game.WithConfigurations().DeleteProfileAsync("TestConfiguration", "test");
            Assert.Empty(game.WithConfigurations().GetProfileNames());
        }

        [Fact]
        public async Task GameLibraryUnknownExtensionAsync_Test()
        {
            var path = new DirectoryInfo(Path.GetTempPath())
                .CreateSubdirectory(Path.GetFileNameWithoutExtension(Path.GetTempFileName()));
            var fs = new PhysicalFileSystem();
            var gfs = fs.GetOrCreateSubFileSystem(fs.ConvertPathFromInternal(path.FullName));

            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            optionsBuilder.UseSqlite($"Data Source={Path.GetTempFileName()}");
            var glib = new GameRecordLibrary(optionsBuilder);
            var ccs = new ConfigurationCollectionStore(optionsBuilder);

            var gl = new GameLibrary(glib);
            Assert.Throws<KeyNotFoundException>(() => gl.GetExtension<IGameConfigurationExtensionProvider>());
            var game = gl.CreateGame("NINTENDO_NES");
            Assert.Throws<KeyNotFoundException>(() => game.GetExtension<IGameConfigurationExtension>());
        }

        [Fact]
        public async Task GameLibraryIntegrationUpdateAsync_Test()
        {
            var path = new DirectoryInfo(Path.GetTempPath())
                .CreateSubdirectory(Path.GetFileNameWithoutExtension(Path.GetTempFileName()));
            var fs = new PhysicalFileSystem();
            var gfs = fs.GetOrCreateSubFileSystem(fs.ConvertPathFromInternal(path.FullName));

            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            optionsBuilder.UseSqlite($"Data Source={Path.GetTempFileName()}");
            var glib = new GameRecordLibrary(optionsBuilder);
            var flib = new FileRecordLibrary(optionsBuilder);

            var gl = new GameLibrary(glib);
            gl.AddExtension<GameFileExtensionProvider, IGameFileExtension
            >(new GameFileExtensionProvider(flib, gfs));

            var game = gl.CreateGame("NINTENDO_NES");
            game.Record.Title = "My Awesome Game";
            gl.UpdateGameRecord(game.Record);

            var file = game.WithFiles().MiscRoot.OpenFile("Test.txt");
            file.OpenStream().Close();
            game.WithFiles().RegisterFile(file, "application/text");
            var record = game.WithFiles().GetFileInfo(file);
            record.Metadata.Add("file_metadata", "test");
            gl.GetExtension<GameFileExtensionProvider>().UpdateFile(record);

            var newGame = gl.GetGame(game.Record.RecordID);
            Assert.NotEmpty(newGame.WithFiles().FileRecords);
        }
    }
}
