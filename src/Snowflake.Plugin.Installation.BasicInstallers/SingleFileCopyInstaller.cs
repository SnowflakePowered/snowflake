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
    [SupportedPlatform("NINTENDO_NES")]
    [SupportedPlatform("NINTENDO_SNES")]
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
