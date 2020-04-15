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
        /// Queries all installable file system entries in a given parent directory.
        /// 
        /// This method should not perform any file write operations.
        /// </summary>
        /// <param name="platformId">The platform to lookup installables for.</param>
        /// <param name="fileEntries">
        /// A list of file system entries to search. Typically this may be
        /// a list of file entries in a directory.
        /// </param>
        /// <returns>
        /// A list of installables that can be used with 
        /// <see cref="Install(IGame, IEnumerable{FileSystemInfo})"/>
        /// </returns>
        IEnumerable<IInstallable> GetInstallables(PlatformId platformId, IEnumerable<FileSystemInfo> fileEntries);

        /// <summary>
        /// Installs files to an <see cref="IGame"/> by yielding instances of <see cref="TaskResult{IFile}"/>.
        /// Instead of doing manual file manipulation, use installation tasks. <see cref="AsyncInstallTask{T}"/>
        /// and <see cref="AsyncInstallTaskEnumerable{T}"/>.
        /// </summary>
        /// <param name="game">The game to install the files to.</param>
        /// <param name="files">
        /// The list of files to install. Use <see cref="GetInstallables(PlatformId, IEnumerable{FileSystemInfo})"/>
        /// to ensure that this installer can properly install the given files. Theoretically 
        /// </param>
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
