using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Snowflake.Extensibility;
using Snowflake.Filesystem;
using Snowflake.Model.Game;

namespace Snowflake.Installation.Extensibility
{
    /// <summary>
    /// Installs files to an <see cref="IGame"/> by yielding <see cref="TaskResult{IFile}"/>.
    /// </summary>
    public interface IGameInstaller
        : IPlugin
    {
        /// <summary>
        /// Installs files to an <see cref="IGame"/> by yielding instances of <see cref="TaskResult{IFile}"/>.
        /// Instead of doing manual file manipulation, use installation tasks. <see cref="AsyncInstallTask{T}"/>
        /// and <see cref="AsyncInstallTaskEnumerable{T}"/>.
        /// </summary>
        /// <param name="game"></param>
        /// <param name="files"></param>
        /// <returns>The files that were installed to the game.</returns>
        IAsyncEnumerable<TaskResult<IFile>> Install(IGame game, IEnumerable<FileSystemInfo> files);
        /// <summary>
        /// Platforms this <see cref="IGameInstaller"/> supports. There can be at most one instance of
        /// a <see cref="IGameInstaller"/> per platform, and they must support installing all forms of 
        /// ROM files for that platform.
        /// </summary>
        IEnumerable<PlatformId> SupportedPlatforms { get; }
    }
}
