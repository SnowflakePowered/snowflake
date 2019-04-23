using System;
using System.Collections.Generic;
using System.IO;
using Snowflake.Installation;
using Snowflake.Model.Game;
using System.Linq;
using Snowflake.Installation.Tasks;
using Snowflake.Model.Game.LibraryExtensions;
using Snowflake.Installation.Extensibility;
using Snowflake.Extensibility.Provisioning;
using Snowflake.Extensibility;
using Snowflake.Services;

namespace Snowflake.Plugin.Installation.BasicInstallers
{
    [Plugin("BasicCopyInstaller")]
    [SupportedPlatform("ATARI_2600")]
    [SupportedPlatform("ATARI_5200")]
    [SupportedPlatform("ATARI_7800")]
    [SupportedPlatform("NINTENDO_GB")]
    [SupportedPlatform("NINTENDO_GBC")]
    [SupportedPlatform("NINTENDO_GBA")]
    [SupportedPlatform("NINTENDO_NDS")]
    [SupportedPlatform("NINTENDO_NES")]
    [SupportedPlatform("NINTENDO_FDS")]
    [SupportedPlatform("NINTENDO_SNES")]
    [SupportedPlatform("NINTENDO_N64")]
    [SupportedPlatform("NINTENDO_N64DD")]
    [SupportedPlatform("SEGA_GEN")]
    [SupportedPlatform("SEGA_GG")]
    [SupportedPlatform("SEGA_SMS")]
    public class SingleFileCopyInstaller
        : GameInstaller
    {
        public SingleFileCopyInstaller(IStoneProvider stone) : base(typeof(SingleFileCopyInstaller))
        {
            this.StoneProvider = stone;
        }

        public SingleFileCopyInstaller(IPluginProvision provision, IStoneProvider stone) : base(provision)
        {
            this.StoneProvider = stone;
        }

        public IStoneProvider StoneProvider { get; }

        public override async IAsyncEnumerable<ITaskResult> Install(IGame game, IEnumerable<FileSystemInfo> files)
        {
            var platform =  game.Record.PlatformId;
            foreach (var file in files.Select(f => f as FileInfo))
            {
                if (file == null) continue;
                using var stream = file.OpenRead();
                string mimetype = this.StoneProvider.GetStoneMimetype(platform, stream, file.Extension);
                if (mimetype == String.Empty) continue;

                var copiedFile = await new CopyFileTask(file, game.WithFiles().ProgramRoot);
                yield return copiedFile;
                game.WithFiles().RegisterFile(await copiedFile, mimetype);
            }
        }
    }
}
