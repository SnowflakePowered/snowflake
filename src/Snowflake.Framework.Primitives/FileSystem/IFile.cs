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
    public interface IFile
    {
        /// <summary>
        /// The name of the file.
        /// </summary>
        string Name { get; }
        /// <summary>
        /// The length of the file in bytes.
        /// </summary>
        long Length { get; }
        /// <summary>
        /// The <see cref="IDirectory"/> under which this file is located.
        /// </summary>
        IDirectory ParentDirectory { get; }
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
        /// Renames the file, keeping the same <see cref="FileGuid"/>.
        /// If the provided name is a path, it will be truncated with <see cref="Path.GetFileName(string)"/>.
        /// 
        /// To move files between directories, use <see cref="IDirectory.MoveFrom(IFile)"/> and
        /// <see cref="IDirectory.CopyFrom(IFile)"/> and friends.
        /// </summary>
        /// <param name="newName">The new name of the <see cref="IFile"/></param>
        void Rename(string newName);

        /// <summary>
        /// Deletes a <see cref="IFile"/> from the underlying file system as well as the
        /// manifest of the <see cref="ParentDirectory"/>.
        /// 
        /// If <see cref="IFile.Created"/> is false, the file will simply be removed from the
        /// manifest.
        /// </summary>
        void Delete();

        /// <summary>
        /// The unique ID identifying this file in the directory manifest.
        /// </summary>
        Guid FileGuid { get; }

        /// <summary>
        /// Whether or not this file has been created or currently exists.
        /// 
        /// Keep in mind that <see cref="IDirectory.OpenFile(string)"/> does not actually
        /// create a file on the file system, merely
        /// </summary>
        bool Created { get; }

        /// <summary>
        /// Returns the real file path of this file.
        /// 
        /// This method is obsolete because it is unsafe to use,
        /// without going through the interface methods of 
        /// <see cref="IFile"/>, there is no guarantee that the parent
        /// <see cref="IDirectory"/> instance will remain consistent.
        /// 
        /// Restrict usage to read-only unless absolutely necessary.
        /// </summary>
        /// <returns>The real file path of the file.</returns>
        [Obsolete("Avoid accessing the underlying file path, and use the object methods instead.")]
        FileInfo GetFilePath();

        /// <summary>
        /// The path of this file, relative to the root of the directory provider.
        /// </summary>
        string RootedPath { get; }
    }
}
