﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        public void GameLibraryIntegrationCreate_Test()
        {
            var path = new DirectoryInfo(Path.GetTempPath())
                .CreateSubdirectory(Path.GetFileNameWithoutExtension(Path.GetTempFileName()));
            var fs = new PhysicalFileSystem();
            var gfs = fs.GetOrCreateSubFileSystem(fs.ConvertPathFromInternal(path.FullName));

            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            optionsBuilder.UseSqlite($"Data Source={Path.GetTempFileName()}");
            var glib = new GameRecordLibrary(optionsBuilder);
            var gl = new GameLibrary(glib);
            gl.CreateGame("NINTENDO_NES");
        }

        [Fact]
        public async Task GameLibraryIntegrationQuery_Test()
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
            Assert.NotEmpty(gl.QueryGames(g => g.PlatformID == "NINTENDO_NES"));
            Assert.NotEmpty(gl.GetGames(g => g.PlatformID == "NINTENDO_NES"));
            Assert.NotEmpty(gl.GetAllGames());
            Assert.NotNull(gl.GetGame(game.Record.RecordID));
        }

        [Fact]
        public async Task GameLibraryIntegrationQueryAsync_Test()
        {
            var path = new DirectoryInfo(Path.GetTempPath())
                .CreateSubdirectory(Path.GetFileNameWithoutExtension(Path.GetTempFileName()));
            var fs = new PhysicalFileSystem();
            var gfs = fs.GetOrCreateSubFileSystem(fs.ConvertPathFromInternal(path.FullName));

            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            optionsBuilder.UseSqlite($"Data Source={Path.GetTempFileName()}");
            var glib = new GameRecordLibrary(optionsBuilder);
            var gl = new GameLibrary(glib);
            var game = await gl.CreateGameAsync("NINTENDO_NES");
            Assert.False(await gl.QueryGamesAsync(g => g.PlatformID == "NINTENDO_NES").IsEmptyAsync());
            Assert.False(await gl.GetAllGamesAsync().IsEmptyAsync());
            Assert.NotNull(await gl.GetGameAsync(game.Record.RecordID));
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

            var profileGuid = profile.CollectionGuid;

            Assert.NotNull(game.WithConfigurations()
                .GetProfile<ExampleConfigurationCollection>("TestConfiguration", profileGuid));

            Assert.NotEmpty(game.WithConfigurations().GetProfileNames());

            profile.Configuration.ExampleConfiguration.FullscreenResolution =
                Configuration.FullscreenResolution.Resolution1600X1050;
            gl.WithConfigurationLibrary().UpdateProfile(profile);
            var newProfile = game.WithConfigurations()
                .GetProfile<ExampleConfigurationCollection>("TestConfiguration", profileGuid);
            Assert.Equal(Configuration.FullscreenResolution.Resolution1600X1050,
                newProfile.Configuration.ExampleConfiguration.FullscreenResolution);

            game.WithConfigurations().DeleteProfile("TestConfiguration", profileGuid);
            Assert.Empty(game.WithConfigurations().GetProfileNames());
        }

        [Fact]
        public async Task GameLibraryIntegrationConfigGuid_Test()
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
                .GetProfile<ExampleConfigurationCollection>("TestConfiguration", profile.ValueCollection.Guid));

            Assert.NotEmpty(game.WithConfigurations().GetProfileNames());

            profile.Configuration.ExampleConfiguration.FullscreenResolution =
                Configuration.FullscreenResolution.Resolution1600X1050;
            gl.WithConfigurationLibrary().UpdateProfile(profile);
            var newProfile = game.WithConfigurations()
                .GetProfile<ExampleConfigurationCollection>("TestConfiguration", profile.ValueCollection.Guid);
            Assert.Equal(Configuration.FullscreenResolution.Resolution1600X1050,
                newProfile.Configuration.ExampleConfiguration.FullscreenResolution);

            game.WithConfigurations().DeleteProfile("TestConfiguration", profile.ValueCollection.Guid);
            Assert.Empty(game.WithConfigurations().GetProfileNames());
        }

        [Fact]
        public void GameLibraryUnknownExtension_Test()
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
        public void GameLibraryIntegrationUpdate_Test()
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
            Assert.NotEmpty(newGame.WithFiles().GetFileRecords());
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
            await gl.CreateGameAsync("NINTENDO_NES");
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
            var game = await gl.CreateGameAsync("NINTENDO_NES");

            var profile = await game.WithConfigurations()
                .CreateNewProfileAsync<ExampleConfigurationCollection>("TestConfiguration", "test");
            var profileGuid = profile.CollectionGuid;

            Assert.NotNull(await game.WithConfigurations()
                .GetProfileAsync<ExampleConfigurationCollection>("TestConfiguration", profileGuid));

            Assert.NotEmpty(game.WithConfigurations().GetProfileNames());

            profile.Configuration.ExampleConfiguration.FullscreenResolution =
                Configuration.FullscreenResolution.Resolution1600X1050;
            await gl.WithConfigurationLibrary().UpdateProfileAsync(profile);
            var newProfile = await game.WithConfigurations()
                .GetProfileAsync<ExampleConfigurationCollection>("TestConfiguration", profileGuid);
            Assert.Equal(Configuration.FullscreenResolution.Resolution1600X1050,
                newProfile.Configuration.ExampleConfiguration.FullscreenResolution);

            await game.WithConfigurations().DeleteProfileAsync("TestConfiguration", profileGuid);
            Assert.Empty(game.WithConfigurations().GetProfileNames());
        }

        [Fact]
        public async Task GameLibraryIntegrationConfigAsyncGuid_Test()
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
            var game = await gl.CreateGameAsync("NINTENDO_NES");

            var profile = await game.WithConfigurations()
                .CreateNewProfileAsync<ExampleConfigurationCollection>("TestConfiguration", "test");

            Assert.NotNull(await game.WithConfigurations()
                .GetProfileAsync<ExampleConfigurationCollection>("TestConfiguration", profile.CollectionGuid));

            Assert.NotEmpty(game.WithConfigurations().GetProfileNames());

            profile.Configuration.ExampleConfiguration.FullscreenResolution =
                Configuration.FullscreenResolution.Resolution1600X1050;
            await gl.WithConfigurationLibrary().UpdateProfileAsync(profile);
            var newProfile = await game.WithConfigurations()
                .GetProfileAsync<ExampleConfigurationCollection>("TestConfiguration", profile.CollectionGuid);
            Assert.Equal(Configuration.FullscreenResolution.Resolution1600X1050,
                newProfile.Configuration.ExampleConfiguration.FullscreenResolution);

            await game.WithConfigurations().DeleteProfileAsync("TestConfiguration", profile.CollectionGuid);
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
            var game = await gl.CreateGameAsync("NINTENDO_NES");
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

            var game = await gl.CreateGameAsync("NINTENDO_NES");
            game.Record.Title = "My Awesome Game";
            await gl.UpdateGameRecordAsync(game.Record);

            var file = game.WithFiles().MiscRoot.OpenFile("Test.txt");
            file.OpenStream().Close();
            await game.WithFiles().RegisterFileAsync(file, "application/text");
            var record = await game.WithFiles().GetFileInfoAsync(file);
            record.Metadata.Add("file_metadata", "test");
            await gl.GetExtension<GameFileExtensionProvider>().UpdateFileAsync(record);

            var newGame = await gl.GetGameAsync(game.Record.RecordID);
            Assert.False(await newGame.WithFiles().GetFileRecordsAsync().IsEmptyAsync());
        }
    }
}
