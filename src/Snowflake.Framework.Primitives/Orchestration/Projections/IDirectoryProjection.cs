using Snowflake.Filesystem;
using System;
using System.IO;

namespace Snowflake.Orchestration.Projections
{
    /// <summary>
    /// Represents a symbolic-link based projections for runtime roots.
    /// </summary>
    public interface IDirectoryProjection
    {
        /// <summary>
        /// Enter a new level (creates a directory) in the projection. 
        /// A projected file or directory must not share the same name.
        /// </summary>
        /// <param name="directoryName">The name of the directory to enter.</param>
        /// <returns>A projection rooted at the lower level.</returns>
        /// <exception cref="DirectoryNotFoundException">The provided <paramref name="directoryName"/> is invalid.</exception>
        /// <exception cref="ArgumentException">A projection with the given <paramref name="directoryName"/> already exists.</exception>
        IDirectoryProjection Enter(string directoryName);

        /// <summary>
        /// Exit the current level of the projection, returning the parent level.
        /// </summary>
        /// <returns>The parent of this projection level.</returns>
        /// <exception cref="InvalidOperationException">Can not exit the top level.</exception>
        IDirectoryProjection Exit();

        /// <summary>
        /// Mount a projection (create symlinks) at the root of the given directory.
        /// </summary>
        /// <param name="autoDisposingDirectory">
        /// The <see cref="IDisposableDirectory"/> to mount the projection under. 
        /// Projections can only be mounted to <see cref="IDisposableDirectory"/>.
        /// </param>
        /// <returns>A <see cref="IReadOnlyDirectory"/> at the mounted directory.</returns>
        /// <exception cref="IOException">The given <paramref name="autoDisposingDirectory"/> was not empty.</exception>
        /// <exception cref="InvalidOperationException">Tried to mount projection subtree.</exception>
        IReadOnlyDirectory Mount(IDisposableDirectory autoDisposingDirectory);

        /// <summary>
        /// Mount a projection (create symlinks) in a child directory of the given directory.
        /// </summary>
        /// <param name="autoDisposingDirectory">
        /// The <see cref="IDisposableDirectory"/> to mount the projection under. 
        /// Projections can only be mounted to <see cref="IDisposableDirectory"/>.
        /// </param>
        /// <param name="mountRoot">The child directory to mount the projection under. Longer paths with '/' are accepted.</param>
        /// <returns>A <see cref="IReadOnlyDirectory"/> at the mounted directory.</returns>
        /// <exception cref="IOException">The given <paramref name="mountRoot"/> was not empty.</exception>
        /// <exception cref="InvalidOperationException">Tried to mount projection subtree.</exception>
        /// <exception cref="DirectoryNotFoundException">The provided <paramref name="mountRoot"/> is invalid.</exception>
        IReadOnlyDirectory Mount(IDisposableDirectory autoDisposingDirectory, string mountRoot);

        /// <summary>
        /// Project an existing <see cref="IDirectory"/> to the current level projection.
        /// If an existing projection with the same <paramref name="name"/> exists, it will be replaced.
        /// </summary>
        /// <param name="name">The name of the directory under the projection</param>
        /// <param name="dir">The <see cref="IDirectory"/> to project.</param>
        /// <returns>The current directory projection level.</returns>
        /// <exception cref="DirectoryNotFoundException">The provided <paramref name="name"/> is invalid.</exception>
        /// <exception cref="ArgumentException">A projection level with the given <paramref name="name"/> already exists.</exception>
        IDirectoryProjection Project(string name, IDirectory dir);

        /// <summary>
        /// Project an existing <see cref="IFile"/> to the current level projection.
        /// If an existing projection with the same <paramref name="name"/> exists, it will be replaced.
        /// </summary>
        /// <param name="name">The name of the directory under the projection</param>
        /// <param name="file">The <see cref="IFile"/> to project.</param>
        /// <returns>The current directory projection level.</returns>
        /// <exception cref="DirectoryNotFoundException">The provided <paramref name="name"/> is invalid.</exception>
        /// <exception cref="ArgumentException">A projection level with the given <paramref name="name"/> already exists.</exception>
        IDirectoryProjection Project(string name, IFile file);
    }
}