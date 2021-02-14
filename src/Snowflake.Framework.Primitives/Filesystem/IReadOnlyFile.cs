using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Snowflake.Filesystem
{
    /// <summary>
    /// Represents a read only view over a file contained within an <see cref="IDeletableDirectory"/> that could have
    /// metadata potentially attached to it by a unique <see cref="Guid"/> that tracks the file
    /// within the file system, as long as the <see cref="Snowflake.Filesystem"/> API methods
    /// are used.
    /// 
    /// Prefer using the <see cref="IReadOnlyFile"/> interface if no writes are guaranteed.
    /// </summary>
    public interface IReadOnlyFile
    {
        /// <summary>
        /// The name of the file.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// The length of the file in bytes. If the file does not yet exist, this value is -1.
        /// </summary>
        long Length { get; }

        /// <summary>
        /// The <see cref="IDeletableDirectory"/> under which this file is located.
        /// </summary>
        IReadOnlyDirectory ParentDirectory { get; }

        /// <summary>
        /// Opens a read-only stream to the file. If it does not currently exist,
        /// <see cref="FileNotFoundException"/> will be thrown.
        /// </summary>
        /// <returns>A read-write stream to the file.</returns>
        Stream OpenReadStream();

        /// <summary>
        /// The unique ID identifying this file in the directory manifest.
        /// </summary>
        Guid FileGuid { get; }

        /// <summary>
        /// Whether or not this file has been created or currently exists.
        /// 
        /// Keep in mind that <see cref="IDeletableDirectory.OpenFile(string)"/> does not actually
        /// create a file on the file system.
        /// </summary>
        bool Created { get; }

        /// <summary>
        /// The path of this file, relative to the root of the directory provider.
        /// </summary>
        string RootedPath { get; }

        /// <summary>
        /// Returns the real file path of this file.
        /// 
        /// This method is obsolete because it is unsafe to use,
        /// without going through the interface methods of 
        /// <see cref="IFile"/>, there is no guarantee that the parent
        /// <see cref="IDeletableDirectory"/> instance will remain consistent.
        /// 
        /// However, access to the underlying filesystem is necessary
        /// for the API to be remotely useful at the barrier.
        /// 
        /// If <see cref="IsLink"/> is true, then this returns the path that is linked to.
        /// 
        /// Restrict usage to read-only unless absolutely necessary.
        /// </summary>
        /// <returns>The real file path of the file.</returns>
        [Obsolete("Avoid accessing the underlying file path, and use the object methods instead.")]
        FileInfo UnsafeGetFilePath();
    }
}
