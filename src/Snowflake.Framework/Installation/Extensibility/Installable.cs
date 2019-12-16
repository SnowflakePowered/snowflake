using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Snowflake.Installation.Extensibility
{
    public sealed class Installable : IInstallable
    {
        /// <summary>
        /// Creates an <see cref="IInstallable"/> with the provided artifacts and uses
        /// the file path as a display name.
        /// </summary>
        /// <param name="artifacts">The artifacts to install.</param>
        /// <param name="source">The source file system info of the installable.</param>
        public Installable(IEnumerable<FileSystemInfo> artifacts, FileSystemInfo source)
            : this(artifacts, source.Name) { }

        /// <summary>
        /// Creates an <see cref="IInstallable"/> with the provided artifacts and display name.
        /// </summary>
        /// <param name="artifacts">The artifacts to install.</param>
        /// <param name="displayName">The display name of the installable.</param>
        public Installable(IEnumerable<FileSystemInfo> artifacts, string displayName)
        {
            this.Artifacts = artifacts;
            this.DisplayName = displayName;
        }

        public IEnumerable<FileSystemInfo> Artifacts { get; }

        public string DisplayName { get; }
    }
}
