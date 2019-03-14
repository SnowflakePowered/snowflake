using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Snowflake.Extensibility;
using Snowflake.Model.Game;

namespace Snowflake.Installation.Extensibility
{
    public interface IGameInstaller
        : IPlugin
    {
        IAsyncEnumerable<ITaskResult> Install(IGame game, IEnumerable<FileSystemInfo> files);
        IEnumerable<PlatformId> SupportedPlatforms { get; }
    }
}
