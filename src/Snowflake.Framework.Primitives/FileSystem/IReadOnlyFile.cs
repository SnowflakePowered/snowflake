using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Snowflake.Filesystem
{
    /// <summary>
    /// Represents a read only view over a file contained within an <see cref="IDirectory"/> that could have
    /// metadata potentially attached to it by a unique <see cref="Guid"/> that tracks the file
    /// within the file system, as long as the <see cref="Snowflake.Filesystem"/> API methods
    /// are used.
    /// </summary>
    public interface IReadOnlyFile
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
        IReadOnlyDirectory ParentDirectory { get; }

        /// <summary>
        /// Opens a read-only stream to the file. If it does not currently exist, IOException
        /// will be thrown.
        /// </summary>
        /// <returns>A read-write stream to the file.</returns>
        Stream OpenStream();
        
        /// <summary>
        /// The unique ID identifying this file in the directory manifest.
        /// </summary>
        Guid FileGuid { get; }

        /// <summary>
        /// Whether or not this file has been created or currently exists.
        /// 
        /// Keep in mind that <see cref="IDirectory.OpenFile(string)"/> does not actually
        /// create a file on the file system.
        /// </summary>
        bool Created { get; }

        /// <summary>
        /// The path of this file, relative to the root of the directory provider.
        /// </summary>
        string RootedPath { get; }
    }
}
