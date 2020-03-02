using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Snowflake.Installation.Extensibility
{
    /// <summary>
    /// An installable that can be later processed with
    /// <see cref="IGameInstaller.Install(Model.Game.IGame, IEnumerable{System.IO.FileSystemInfo})"/>.
    /// 
    /// Only process <see cref="IInstallable"/>s with the same <see cref="IGameInstaller"/> that produced it.
    /// </summary>
    public interface IInstallable
    {
        /// <summary>
        /// Gets the artifacts to install
        /// </summary>
        IEnumerable<FileSystemInfo> Artifacts { get; }

        /// <summary>
        /// Gets a display name for this installable.
        /// </summary>
        string DisplayName { get; }

        /// <summary>
        /// The name of the <see cref="IGameInstaller"/> that produced this installable.
        /// </summary>
        string Source { get; }
    }
}
