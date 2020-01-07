using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Snowflake.Model.Records.File;

namespace Snowflake.Filesystem
{
    /// <summary>
    /// Represents a file contained within an <see cref="IDirectory"/> that could have
    /// metadata potentially attached to it by a unique <see cref="Guid"/> that tracks the file
    /// within the file system, as long as the <see cref="Snowflake.Filesystem"/> API methods
    /// are used.
    /// </summary>
    public interface IFile : IReadOnlyFile
    {
        /// <summary>
        /// Opens a read-write stream to the file. If it does not currently exist,
        /// it will be created. 
        /// </summary>
        /// <returns>A read-write stream to the file.</returns>
        Stream OpenStream();

        /// <summary>
        /// Opens a stream to the file with the given <see cref="FileAccess"/>. 
        /// If it does not currently exist, it will be created. 
        /// </summary>
        /// <param name="rw">The <see cref="FileAccess"/> permissions to open the stream with.</param>
        /// <returns>A stream to the file with the given <see cref="FileAccess"/>.</returns>
        Stream OpenStream(FileAccess rw);

        /// <summary>
        /// Opens a stream to the file in the specified mode with read, write, or read/write access, and sharing options.
        /// </summary>
        /// <param name="mode">The mode with which to open the file.</param>
        /// <param name="rw">The <see cref="FileAccess"/> permissions to open the stream with.</param>
        /// <param name="share">The sharing mode with which to open the file.</param>
        /// 
        /// <returns>A stream to the file with the given <see cref="FileAccess"/>.</returns>
        Stream OpenStream(FileMode mode, FileAccess rw, FileShare share = FileShare.None);

        /// <summary>
        /// Renames the file, keeping the same <see cref="IReadOnlyFile.FileGuid"/>.
        /// If the provided name is a path, it will be truncated with <see cref="Path.GetFileName(string)"/>.
        /// 
        /// To move files between directories, use <see cref="IDirectory.MoveFrom(IFile)"/> and
        /// <see cref="IDirectory.CopyFrom(IReadOnlyFile)"/> and friends.
        /// </summary>
        /// <param name="newName">The new name of the <see cref="IFile"/></param>
        void Rename(string newName);

        /// <summary>
        /// Deletes a <see cref="IFile"/> from the underlying file system as well as the
        /// manifest of the <see cref="IReadOnlyFile.ParentDirectory"/>.
        /// 
        /// If <see cref="IReadOnlyFile.Created"/> is false, the file will simply be removed from the
        /// manifest.
        /// </summary>
        void Delete();

        /// <summary>
        /// Gets a read-only view over this <see cref="IFile"/>.
        /// </summary>
        /// <returns>
        /// A read-only view over this <see cref="IFile"/>.
        /// </returns>
        IReadOnlyFile AsReadOnly();
    }
}
