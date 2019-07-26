using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Snowflake.Filesystem
{
    /// <summary>
    /// Represents a readonly view over the root of a Directory, where each file that is access through a directory is
    /// associated with a GUID in the directory's manifest.
    /// 
    /// When files are moved between IDirectories, the files GUID is preserved. 
    /// Thus, metadata can be preserved throughout.
    /// </summary>
    public interface IReadOnlyDirectory
    {
        /// <summary>
        /// Opens an existing descendant directory with the given name.
        /// If the directory does not exist, throws IOException.
        /// You can open a nested directory using '/' as the path separator, and it 
        /// will be created relative to this current directory.
        /// </summary>
        /// <param name="name">The name of the existing directory</param>
        /// <returns>The directory if it exists, or null if it does not.</returns>
        IReadOnlyDirectory OpenDirectory(string name);

        /// <summary>
        /// Opens a file if it exists, otherwise this will throw IOException.
        /// 
        /// Unlike <see cref="OpenDirectory(string)"/>, you can not use the path separator to
        /// open a nested file. Paths will be truncated with <see cref="Path.GetFileName(string)"/>.
        /// 
        /// Instead, use <see cref="OpenDirectory(string)"/> to open the directory of the desired file,
        /// then call <see cref="OpenFile(string)"/> on the returned instance.
        /// 
        /// Keep in mind this does not actually create a file on the underlying file system. 
        /// However, you can use <see cref="IFile.OpenStream()"/> and friends to create the file
        /// if it does not yet exist.
        /// </summary>
        /// <param name="file">The name of the file. If this is a path, will be truncated with <see cref="Path.GetFileName(string)"/></param>
        /// <returns>An object that associates a given file with a with a unique <see cref="Guid"/></returns>
        IReadOnlyFile OpenFile(string file);


        /// <summary>
        /// Enumerates all direct child directories of this <see cref="IDirectory"/>.
        /// </summary>
        /// <returns>All direct children directories.</returns>
        IEnumerable<IReadOnlyDirectory> EnumerateDirectories();

        /// <summary>
        /// Enumerates all files in this directory,
        /// </summary>
        /// <returns>All direct children files.</returns>
        IEnumerable<IReadOnlyFile> EnumerateFiles();

        /// <summary>
        /// Recursively enumerates all files that are contained in this directory.
        /// 
        /// This method is usually implemented as a Breadth-first search, but no order is guaranteed.
        /// </summary>
        /// <returns>All files contained within this directory, including descendant subfolders.</returns>
        IEnumerable<IReadOnlyFile> EnumerateFilesRecursive();

        /// <summary>
        /// Whether or not this directory contains a file in its manifest. If provided a
        /// full path, this will truncate the path using <see cref="Path.GetFileName(string)"/>
        /// </summary>
        /// <param name="file">The name of the file to check.</param>
        /// <returns>Whether or not this directory contains the given file.</returns>
        bool ContainsFile(string file);

        /// <summary>
        /// Whether or not this directory contains directory as a direct child.
        /// full path, this will truncate the path using <see cref="Path.GetDirectoryName(string)"/>
        /// </summary>
        /// <param name="directory">The name of the directory to check.</param>
        /// <returns>Whether or not this directory contains the given directory.</returns>
        bool ContainsDirectory(string directory);

        /// <summary>
        /// Returns the mutable view over this read only view of the directory.
        /// </summary>
        /// <returns>A mutable view over this read only directory.</returns>
        IDirectory AsMutable();
    }
}
