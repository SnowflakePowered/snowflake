using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Filesystem
{
    /// <summary>
    /// Describes the real path from which to root a managed file system from.
    /// </summary>
    public interface IDirectoryRoot
    {
        /// <summary>
        /// The managed <see cref="IDirectory"/> that represents this directory on the real file system.
        /// </summary>
        IDirectory Root { get; }
        /// <summary>
        /// Resolves a relative path to a real file system path that is OS-specific.
        /// </summary>
        /// <param name="relativePath">
        /// The relative path from the root <see cref="IDirectory"/>. This path
        /// always uses '/' as the file separator.
        /// </param>
        /// <returns>The real path on the file system that the relative path maps to.</returns>
        string ResolveRealPath(string relativePath);
    }
}
