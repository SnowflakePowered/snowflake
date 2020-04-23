using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
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
        /// <see cref="Install(IGame, IEnumerable{FileSystemInfo}, CancellationToken)"/>
        /// </returns>
        IEnumerable<IInstallable> GetInstallables(PlatformId platformId, IEnumerable<FileSystemInfo> fileEntries);

        /// <summary>
        /// Installs files to an <see cref="IGame"/> by yielding instances of <see cref="TaskResult{IFile}"/>.
        /// Instead of doing manual file manipulation, use installation tasks. <see cref="AsyncInstallTask{T}"/>
        /// and <see cref="AsyncInstallTaskEnumerable{T}"/>.
        /// 
        /// This method <strong>is not allowed to throw.</strong> Exceptions may only occur within <see cref="AsyncInstallTask{T}"/>
        /// and <see cref="AsyncInstallTaskEnumerable{T}"/> awaited by this <see cref="IAsyncEnumerable{T}"/> to yield
        /// <see cref="TaskResult{T}"/> instances.
        /// </summary>
        /// <param name="game">The game to install the files to.</param>
        /// <param name="files">
        /// The list of files to install. Use <see cref="GetInstallables(PlatformId, IEnumerable{FileSystemInfo})"/>
        /// to ensure that this installer can properly install the given files.
        /// </param>
        /// <param name="cancellationToken">The cancellation token to pass to the installer.</param>
        /// <returns>The files that were installed to the game.</returns>
        IAsyncEnumerable<TaskResult<IFile>> Install(IGame game, IEnumerable<FileSystemInfo> files, CancellationToken cancellationToken = default);

        /// <summary>
        /// Platforms this <see cref="IGameInstaller"/> supports. There can be at most one instance of
        /// a <see cref="IGameInstaller"/> per platform, and they must support installing all forms of 
        /// ROM files for that platform.
        /// </summary>
        IEnumerable<PlatformId> SupportedPlatforms { get; }
    }
}
