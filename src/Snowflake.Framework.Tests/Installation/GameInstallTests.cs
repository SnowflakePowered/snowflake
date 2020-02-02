using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Snowflake.Model.Database;
using Snowflake.Model.Database.Models;
using Snowflake.Model.Game;
using Snowflake.Model.Game.LibraryExtensions;
using Snowflake.Plugin.Installation.BasicInstallers;
using Snowflake.Services;
using Snowflake.Tests;
using Xunit;
using Zio;
using Zio.FileSystems;

namespace Snowflake.Installation.Tests
{
    public class GameInstallTests
    {
        [Fact]
        public async Task TestCopyInstaller_IntegrationTest()
        {
            using var testStream = TestUtilities.GetResource("TestRoms.test.nes");
            using var fileStream = File.Create(Path.GetTempFileName() + ".nes");
            await testStream.CopyToAsync(fileStream);
            string fname = fileStream.Name;
            fileStream.Dispose();

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

            var stone = new StoneProvider();
            var install = new SingleFileCopyInstaller(stone);

            var installables = install.GetInstallables("NINTENDO_NES", new List<FileSystemInfo>() { new FileInfo(fname) });
            
            foreach (var i in installables)
            {
                await foreach (var res in install.Install(game, i.Artifacts))
                {

                }
            }
            
            Assert.NotEmpty(game.WithFiles().GetFileRecords());
        }
    }
}
